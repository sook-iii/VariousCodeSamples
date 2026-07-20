// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "TerrainGenerator.h"
#include "WalkerCharacter.generated.h"


UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class PSGAMEABOUTNAV_API UWalkerCharacter : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UWalkerCharacter();

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	float characterSpeed = 100.f;


protected:
	// Called when the game starts
	virtual void BeginPlay() override;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Internal Values")
	UTerrainGenerator* parentTerrain;;
	FVector shortTermGoal;
	FVector longTermGoal;



public:	
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	UFUNCTION(BlueprintCallable)
	void TeachTerrain(UTerrainGenerator* terrain);

	FVector PickNewShortTermGoal();
	FVector PickNewLongTermGoal();

};
