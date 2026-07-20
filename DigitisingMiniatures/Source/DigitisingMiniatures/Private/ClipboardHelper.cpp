// Fill out your copyright notice in the Description page of Project Settings.


#include "ClipboardHelper.h"
#include "HAL/PlatformApplicationMisc.h"

// Sets default values for this component's properties
UClipboardHelper::UClipboardHelper()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;

	// ...
}


// Called when the game starts
void UClipboardHelper::BeginPlay()
{
	Super::BeginPlay();

	// ...
	
}


// Called every frame
void UClipboardHelper::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	// ...
}

void UClipboardHelper::CopyToClipboard(FString receivingText) {

	FString tempString = receivingText;
	FPlatformApplicationMisc::ClipboardCopy(*tempString);

}

FString UClipboardHelper::PasteFromClipboard() {

	FString tempString = "";
	FPlatformApplicationMisc::ClipboardPaste(tempString);

	return tempString;

}