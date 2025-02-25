#include "AbilityAttributeSet.h"
#include "Net/UnrealNetwork.h"

UAbilityAttributeSet::UAbilityAttributeSet()
{
}

void UAbilityAttributeSet::OnRep_Health(const FGameplayAttributeData& OldHealth)
{
	// Macro to help notify clients of attribute changes.
	GAMEPLAYATTRIBUTE_REPNOTIFY(UAbilityAttributeSet, Health, OldHealth);
}

void UAbilityAttributeSet::OnRep_MaxHealth(const FGameplayAttributeData& OldMaxHealth)
{
	GAMEPLAYATTRIBUTE_REPNOTIFY(UAbilityAttributeSet, MaxHealth, OldMaxHealth);
}

void UAbilityAttributeSet::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
	Super::GetLifetimeReplicatedProps(OutLifetimeProps);

	DOREPLIFETIME_CONDITION_NOTIFY(UAbilityAttributeSet, Health, COND_None, REPNOTIFY_Always);
	DOREPLIFETIME_CONDITION_NOTIFY(UAbilityAttributeSet, MaxHealth, COND_None, REPNOTIFY_Always);
}
