#pragma once

#include "CoreMinimal.h"
#include "AttributeSet.h"
#include "AbilitySystemComponent.h"
#include "AbilityAttributeSet.generated.h"

// Macro to simplify the creation of attribute accessor functions
#define ATTRIBUTE_ACCESSORS(ClassName, PropertyName) \
    GAMEPLAYATTRIBUTE_PROPERTY_GETTER(ClassName, PropertyName) \
    GAMEPLAYATTRIBUTE_VALUE_GETTER(PropertyName) \
    GAMEPLAYATTRIBUTE_VALUE_SETTER(PropertyName) \
    GAMEPLAYATTRIBUTE_VALUE_INITTER(PropertyName)

UCLASS()
class READTHEROOM_API UAbilityAttributeSet : public UAttributeSet
{
	GENERATED_BODY()

public:
	UAbilityAttributeSet();

	// Health attribute
	UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData Health;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, Health);

		// Maximum Health attribute
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData MaxHealth;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, MaxHealth);

		// Damage attribute
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData Damage;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, Damage);

		// Stagger Damage attribute (damage applied to the stagger/balance meter)
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData StaggerDamage;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, StaggerDamage);

		// Stagger attribute (can also be called Balance; industry terms vary)
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData Stagger;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, Stagger);

		// Maximum Stagger attribute
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData MaxStagger;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, MaxStagger);

		// Base Weapon Damage attribute
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData BaseWeaponDamage;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, BaseWeaponDamage);

		// Attack Speed attribute
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData AttackSpeed;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, AttackSpeed);

		// Max Ammo attribute
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData MaxAmmo;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, MaxAmmo);

		// Bullets Per Shot attribute
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData BulletsPerShot;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, BulletsPerShot);

		// Accuracy attribute
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData Accuracy;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, Accuracy);

		// Base Spread Angle attribute
		UPROPERTY(BlueprintReadOnly, Category = "Attributes")
	FGameplayAttributeData BaseSpreadAngle;
	ATTRIBUTE_ACCESSORS(UAbilityAttributeSet, BaseSpreadAngle);


};
