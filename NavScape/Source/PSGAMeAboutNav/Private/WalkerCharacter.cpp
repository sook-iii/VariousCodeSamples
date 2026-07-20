// Fill out your copyright notice in the Description page of Project Settings.


#include "WalkerCharacter.h"

// Sets default values for this component's properties
UWalkerCharacter::UWalkerCharacter()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;

	// ...
}


// Called when the game starts
void UWalkerCharacter::BeginPlay()
{
	Super::BeginPlay();

	// ...

}


// Called every frame
void UWalkerCharacter::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	// ...


}

void UWalkerCharacter::TeachTerrain(UTerrainGenerator* terrain)
{

	parentTerrain = terrain;

}

FVector UWalkerCharacter::PickNewShortTermGoal()
{

	return FVector(0, 0, 0);


}

FVector UWalkerCharacter::PickNewLongTermGoal()
{

	return FVector(0, 0, 0);


}


