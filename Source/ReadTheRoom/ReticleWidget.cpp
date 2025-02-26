#include "ReticleWidget.h"
#include "Components/Image.h"
#include "Components/CanvasPanelSlot.h"
#include "Kismet/GameplayStatics.h"
#include "GameFramework/PlayerController.h"
// Include your player character and attribute set headers
#include "AbilityCharacter.h"
#include "AbilityAttributeSet.h"
#include "AbilitySystemComponent.h"

void UReticleWidget::NativeTick(const FGeometry& MyGeometry, float InDeltaTime)
{
	Super::NativeTick(MyGeometry, InDeltaTime);

	// Auto-assign the player character if not set
	if (!PlayerCharacter)
	{
		PlayerCharacter = Cast<AAbilityCharacter>(UGameplayStatics::GetPlayerCharacter(GetWorld(), 0));
	}

	// 1) Retrieve the current spread angle from the player's attribute set
	float SpreadAngle = GetSpreadAngleFromPlayer();

	// 2) Compute target gap based on spread angle
	float TargetGap = CalculateGap(SpreadAngle);

	// 3) Combine with any active fire kick offset
	float CombinedGap = TargetGap + FireKickOffset;

	// 4) Smoothly interpolate CurrentGap toward CombinedGap
	CurrentGap = FMath::Lerp(CurrentGap, CombinedGap, InDeltaTime * SmoothSpeed);
	CurrentGap = FMath::Clamp(CurrentGap, 0.f, MaxGap);

	// 5) Apply computed gap to reticle line positions
	ApplyReticlePositions(CurrentGap);

	// 6) Update fire kick effect if active
	if (bIsKicking)
	{
		float Elapsed = GetWorld()->GetTimeSeconds() - KickStartTime;
		float t = FMath::Clamp(Elapsed / FireKickDuration, 0.f, 1.f);
		// Ease-out quadratic: ease-out value from 0 to 1
		float EasedT = 1.f - FMath::Pow(1.f - t, 2.f);
		FireKickOffset = FireKickMaxValue * (1.f - EasedT);

		if (t >= 1.f)
		{
			FireKickOffset = 0.f;
			bIsKicking = false;
		}
	}
}

float UReticleWidget::GetSpreadAngleFromPlayer() const
{
	float SpreadAngle = 0.0f;

	if (!PlayerCharacter)
	{
		UE_LOG(LogTemp, Warning, TEXT("PlayerCharacter not set in ReticleWidget!"));
		return SpreadAngle;
	}

	UAbilitySystemComponent* ASC = PlayerCharacter->GetAbilitySystemComponent();
	if (!ASC)
	{
		UE_LOG(LogTemp, Warning, TEXT("AbilitySystemComponent not found on PlayerCharacter!"));
		return SpreadAngle;
	}

	// Retrieve the gameplay attribute from the static accessor
	static FGameplayAttribute SpreadAngleAttribute = UAbilityAttributeSet::GetBaseSpreadAngleAttribute();
	if (!SpreadAngleAttribute.IsValid())
	{
		UE_LOG(LogTemp, Error, TEXT("BaseSpreadAngle attribute is not valid!"));
		return SpreadAngle;
	}

	// Check if the ASC actually has this attribute
	if (!ASC->HasAttributeSetForAttribute(SpreadAngleAttribute))
	{
		UE_LOG(LogTemp, Warning, TEXT("Player ASC does not have the required AttributeSet for BaseSpreadAngle!"));
		return SpreadAngle;
	}

	// Get the current value of the attribute (includes gameplay effect modifications)
	SpreadAngle = ASC->GetNumericAttribute(SpreadAngleAttribute);

	// Optional: Use GetGameplayAttributeValue for more detailed information
	// const FGameplayAttributeValue AttributeValue = ASC->GetGameplayAttributeValue(SpreadAngleAttribute);
	// SpreadAngle = AttributeValue.CurrentValue;

	return SpreadAngle;
}


float UReticleWidget::CalculateGap(float SpreadAngleDegrees) const
{
	if (bUseLinearMapping)
	{
		return BaseGap + (SpreadAngleDegrees * GapScale);
	}
	else
	{
		float HalfRad = (SpreadAngleDegrees * 0.5f) * (PI / 180.f);
		return GapScale * FMath::Tan(HalfRad);
	}
}

void UReticleWidget::ApplyReticlePositions(float Gap)
{
	// Works with ANY widget type that has a CanvasPanelSlot parent
	auto UpdatePosition = [Gap](UWidget* Widget, FVector2D Offset) {
		if (Widget && Widget->Slot && Widget->Slot->IsA<UCanvasPanelSlot>())
		{
			UCanvasPanelSlot* CanvasSlot = Cast<UCanvasPanelSlot>(Widget->Slot);
			CanvasSlot->SetPosition(Offset * Gap);
		}
		};

	UpdatePosition(TopLine, FVector2D(0, -1));   // (0, +Gap)
	UpdatePosition(BottomLine, FVector2D(0, 1));  // (0, -Gap)
	UpdatePosition(LeftLine, FVector2D(-1, 0));  // (-Gap, 0)
	UpdatePosition(RightLine, FVector2D(1, 0));   // (+Gap, 0)
}

void UReticleWidget::OnWeaponFired()
{
	// Trigger or reset the fire kick effect when weapon is fired
	bIsKicking = true;
	KickStartTime = GetWorld()->GetTimeSeconds();
}
