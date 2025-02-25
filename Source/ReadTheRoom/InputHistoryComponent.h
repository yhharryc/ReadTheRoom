// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "Templates/Function.h"
#include "InputHistoryComponent.generated.h"

// Define an enum for player action types.
UENUM(BlueprintType)
enum class EPlayerActionType : uint8
{
	None,
	Shoot,
	Sprint,
	Movement,
	Interact
	// etc.
};

// Structure to hold one frameÅfs worth of input data.
// You can expand this struct with additional flags or a timestamp if desired.
USTRUCT(BlueprintType)
struct FInputFrameData
{
	GENERATED_BODY()

public:

	UPROPERTY(BlueprintReadWrite, Category = "Input")
	bool bShootPressed;

	UPROPERTY(BlueprintReadWrite, Category = "Input")
	bool bSprintPressed;

	// Movement input, representing the directional control (e.g., analog stick/d-pad)
	UPROPERTY(BlueprintReadWrite, Category = "Input")
	FVector2D MovementInput;

	// Interact input (e.g., a trigger or specific button for interactions)
	UPROPERTY(BlueprintReadWrite, Category = "Input")
	float InteractInput;

	// Default constructor initializes all inputs to false/zero.
	FInputFrameData()
		: bShootPressed(false)
		, bSprintPressed(false)
		, MovementInput(FVector2D::ZeroVector)
		, InteractInput(0.f)
	{
	}
};

UCLASS(Blueprintable, ClassGroup = (Custom), meta = (BlueprintSpawnableComponent))
class READTHEROOM_API UInputHistoryComponent : public UActorComponent
{
	GENERATED_BODY()

public:
	// Sets default values for this component's properties
	UInputHistoryComponent();

protected:
	// Called when the game starts
	virtual void BeginPlay() override;

public:
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	// Maximum number of frames to store in history
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Input History")
	int32 HistorySize;

	// Array holding the recent input frames. WeÅfll use a simple FIFO buffer.
	UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Input History")
	TArray<FInputFrameData> InputHistory;

	// Call this to add a new frameÅfs input data.
	UFUNCTION(BlueprintCallable, Category = "Input History")
	void AddInputFrame(const FInputFrameData& NewFrameData);


};
