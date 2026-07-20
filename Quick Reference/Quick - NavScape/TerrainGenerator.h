// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "TerrainGenerator.generated.h"

USTRUCT(Blueprintable)
struct FMeshData
{
	GENERATED_BODY();

	UPROPERTY(BlueprintReadOnly, VisibleAnywhere)
	TArray<FVector> vertices;

	UPROPERTY(BlueprintReadOnly, VisibleAnywhere)
	TArray<int> triangles;

	UPROPERTY(BlueprintReadOnly, VisibleAnywhere)
	TArray<FVector> normals;

	UPROPERTY(BlueprintReadOnly, VisibleAnywhere)
	TArray<FVector2D> uvs;

};

//thought about having an edge class but since in my model all edges are laterally equisdistant, 
// it seems needless to store edge length, which calls into question the reason behind the entire class (also, edge length can just be added later in each node, and i feel like this would be more efficient !!!!!). 
// i'm just gonna have adjacent nodes i think

USTRUCT()
struct FTemp_NaviNode
{
	GENERATED_BODY();

public:

	//could be used to calc euclidean distance between nodes rather than map it out pure, since the graph "edges" don't overlap themselves (thus, safe to assume any step towards the destination is among the best choices)
	FVector worldPosition;
	
	TArray<FTemp_NaviNode*> adjacentNodes = TArray<FTemp_NaviNode*>();
};

USTRUCT()
struct FTemp_NaviGraph
{
	GENERATED_BODY();

public:

	TArray<FTemp_NaviNode> graphNodes = TArray<FTemp_NaviNode>();

};


//pathfinding pseudo-code
//the AI will have a short-term goal and a long-term goal
//each frame, it moves along its edge to its short-term goal until it's at that node's location. 
//at that point, it'll check to see which of the newly-adjacent nodes is best to reach it's long-term goal.
//
//this'll require a function that gets the triangle a position resides in (in case the start or destination is in the middle of a tri) to be treated like a graph edge (perhaps edge distance is needed after all, but stretch goal)
//this'll require a function that moves the AI each frame to it's short-term goal
//this'll require an A* (probably) search algorithm that takes an existing starting node and finds the destination with efficiency


UCLASS( ClassGroup=(Custom), meta=(BlueprintSpawnableComponent) )
class PSGAMEABOUTNAV_API UTerrainGenerator : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UTerrainGenerator();

	//how big one side of the final mesh should be in world co-ords
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	float globalSize = 10000.f;

	//how many triangles make up one side of the mesh; more like a resolution than a size (potential for name improvement)
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	int gridLength = 150;




	//scale of which the perlin amplitude influences the vertex's height
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	float meshPerlinAmplitude = 10000.f;

	//how many nodes to generate on the perlin map. Keep as int!
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	FVector2D meshPerlinMapSize = FVector2D(10, 10);



	//if perlin value is not within arbitrary amount: create holes in terrain
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	FVector2D porosityPerlinLimit = FVector2D(-0.05f, 0.05f);

	//how many nodes to generate on the perlin map. Keep as int!
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	FVector2D porosityPerlinMapSize = FVector2D(10, 10); 

	//results are tempermental on the edges of the perlin map; how much do we zoom the co-ords in? (as a percentage)
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	float perlinZoomIn = 0.1f;

	//how big cutout mesh triangles should be (can form as pillars or holes)
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	float pillarHeight = 1.f;

	//alternative system; if we're sharpening the mesh later, we have access to the triangle's normals, so do above cutouts directly into the terrain rather than straight down
	//UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	//bool useOrientablePillars = true;
	


	//scale for the uv map
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	FVector2D uvScale = FVector2D(1.f, 1.f);

	//a few options for generating mesh UVs - ranges 0-3, currently
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	int uvMode = 2;

	//a second pass for the mesh to separate all the vertices so that they have their own normals. makes it far easier to see mesh geometry
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Mesh Configurables")
	bool sharpenEdges = true;
	
protected:
	// Called when the game starts
	virtual void BeginPlay() override;
	void CreateRigidPillars(FMeshData& generatedMesh, FMeshData& subMesh, TArray<int> missingTriangles);
	void SharpenMeshEdges(FMeshData& generatedMesh, FMeshData& subMesh, TArray<int> missingTriangles);
	
	void PopulatePerlinMap(TArray<TArray<FVector2D>> &perlinMap, FVector2D &mapSize);
	float FetchValueFromPerlinMap(FVector2D pointCoords, TArray<TArray<FVector2D>> &perlinMap, FVector2D& mapSize);

	void PopulateNaviGraph(FMeshData& generatedMesh);

	float InverseLerp(float x, float y, float value);

	TArray<TArray<FVector2D>> meshPerlinMap;
	TArray<TArray<FVector2D>> porosityPerlinMap;

public:	
	// Called every frame
	virtual void TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction) override;

	FTemp_NaviGraph naviMap;

	UFUNCTION(BlueprintCallable)
	void GenerateTerrain(FMeshData& generatedMesh, FMeshData& subMesh);

	UFUNCTION(BlueprintCallable)
	FVector GetRandomNavGraphPosition();
		

};

