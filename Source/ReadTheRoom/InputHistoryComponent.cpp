#include "InputHistoryComponent.h"
#include "GameFramework/Actor.h"

UInputHistoryComponent::UInputHistoryComponent()
{
	PrimaryComponentTick.bCanEverTick = true;
	// Set a default history size (e.g., 30 frames)
	HistorySize = 30;
}

void UInputHistoryComponent::BeginPlay()
{
	Super::BeginPlay();
	// Optionally initialize or clear the history here.
	InputHistory.Empty();
}

void UInputHistoryComponent::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	// Depending on your design, you could sample inputs here.
	// Alternatively, bind input events in your character/controller and call AddInputFrame() accordingly.
}

void UInputHistoryComponent::AddInputFrame(const FInputFrameData& NewFrameData)
{
	// Maintain a fixed-size history (FIFO buffer)
	if (InputHistory.Num() >= HistorySize)
	{
		// Remove the oldest frame.
		InputHistory.RemoveAt(0);
	}
	InputHistory.Add(NewFrameData);
}

