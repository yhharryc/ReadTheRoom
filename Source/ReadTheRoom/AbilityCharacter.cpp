#include "AbilityCharacter.h"
#include "AbilitySystemComponent.h"
#include "AbilityAttributeSet.h"

AAbilityCharacter::AAbilityCharacter()
{
	// Enable ticking
	PrimaryActorTick.bCanEverTick = true;

	// Create the Ability System Component
	AbilitySystemComponent = CreateDefaultSubobject<UAbilitySystemComponent>(TEXT("AbilitySystemComponent"));

	// Create and attach the Attribute Set
	AbilityAttributeSet = CreateDefaultSubobject<UAbilityAttributeSet>(TEXT("AbilityAttributeSet"));
}

void AAbilityCharacter::BeginPlay()
{
	Super::BeginPlay();

	// (Optional) Initialize abilities and attributes here.
}

void AAbilityCharacter::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);
}

void AAbilityCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
	Super::SetupPlayerInputComponent(PlayerInputComponent);

	// Bind your ability activation inputs here if needed.
}

UAbilitySystemComponent* AAbilityCharacter::GetAbilitySystemComponent() const
{
	return AbilitySystemComponent;
}
