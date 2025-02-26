#pragma once

#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "ReticleWidget.generated.h"

class UImage;
class AAbilityCharacter;
class UCanvasPanelSlot;

/**
 * Reticle widget that updates its gap based on player's spread and fire kick effect.
 * This widget is fully Blueprintable so you can create Blueprint subclasses,
 * bind its properties, and call its functions directly in Blueprints.
 */
UCLASS(Blueprintable)
class READTHEROOM_API UReticleWidget : public UUserWidget
{
	GENERATED_BODY()

public:
	// Bindable widget references (must match your UMG design names)
	UPROPERTY(BlueprintReadWrite, meta = (BindWidget), Category = "Reticle")
	UWidget* TopLine;

	UPROPERTY(BlueprintReadWrite, meta = (BindWidget), Category = "Reticle")
	UWidget* BottomLine;

	UPROPERTY(BlueprintReadWrite, meta = (BindWidget), Category = "Reticle")
	UWidget* LeftLine;

	UPROPERTY(BlueprintReadWrite, meta = (BindWidget), Category = "Reticle")
	UWidget* RightLine;

	// Gap settings
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Reticle")
	bool bUseLinearMapping = true;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Reticle")
	float BaseGap = 10.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Reticle")
	float GapScale = 3.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Reticle")
	float MaxGap = 200.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Reticle")
	float SmoothSpeed = 10.f;

	// Fire kick settings: additional gap when firing
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Reticle")
	float FireKickMaxValue = 10.f;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Reticle")
	float FireKickDuration = 0.2f;

	// Optional reference to the player character (auto-assigned if left null)
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Reticle")
	AAbilityCharacter* PlayerCharacter;

	/** Blueprint-callable function to trigger the weapon fired effect */
	UFUNCTION(BlueprintCallable, Category = "Reticle")
	void OnWeaponFired();

protected:
	// Override NativeTick for per-frame updates (automatically called)
	virtual void NativeTick(const FGeometry& MyGeometry, float InDeltaTime) override;

private:
	// Internal state for gap and fire kick effect
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Reticle", meta = (AllowPrivateAccess = "true"))
	float CurrentGap = 0.f;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Reticle", meta = (AllowPrivateAccess = "true"))
	float FireKickOffset = 0.f;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Reticle", meta = (AllowPrivateAccess = "true"))
	bool bIsKicking = false;

	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Reticle", meta = (AllowPrivateAccess = "true"))
	float KickStartTime = 0.f;

	/** Retrieves the current spread angle from the player's AbilityAttributeSet */
	float GetSpreadAngleFromPlayer() const;

	/** Calculates the target gap given a spread angle in degrees */
	float CalculateGap(float SpreadAngleDegrees) const;

	/** Updates the positions of reticle lines based on the computed gap */
	void ApplyReticlePositions(float Gap);
};
