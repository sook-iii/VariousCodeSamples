// Fill out your copyright notice in the Description page of Project Settings.


#include "TerrainGenerator.h"

// Sets default values for this component's properties
UTerrainGenerator::UTerrainGenerator()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;

	// ...
}

void UTerrainGenerator::BeginPlay()
{
	Super::BeginPlay();

	// ...

}


// Called every frame
void UTerrainGenerator::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	// ...

}

//Initial Terrain Pass
void UTerrainGenerator::GenerateTerrain(FMeshData& generatedMesh, FMeshData& subMesh)
{
	//Empty and populate our perlin maps for the main mountain, and the pillars upon it 
	if (meshPerlinMap.IsEmpty()) {
		PopulatePerlinMap(meshPerlinMap, meshPerlinMapSize);
	}

	if (porosityPerlinMap.IsEmpty()) {
		PopulatePerlinMap(porosityPerlinMap, porosityPerlinMapSize);
	}

	//GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, TEXT("Generating Terrain"));

	//is this needed? !!!!!
	generatedMesh.vertices.Empty();
	generatedMesh.triangles.Empty();
	generatedMesh.normals.Empty();
	generatedMesh.uvs.Empty();
	subMesh.vertices.Empty();
	subMesh.triangles.Empty();
	subMesh.normals.Empty();
	subMesh.uvs.Empty();

	//would it be more efficient to just add into generatedmesh directly? !!!!!
	TArray<FVector> generatingVertices;
	TArray<int> generatingTriangles;
	TArray<int> missingTriangles;
	TArray<FVector> generatingNormals;
	TArray<FVector2D> generatingUVs;

	//right-angle triangle with A=?, O=0.5, T=1.	? = 0.75^0.5 = ~0.866
	const float unitTriangleHeight = 0.866025403784;
			
	//add vertices to triangular grid mesh
	for (int row = 0; row < gridLength; row++) {

		for (int col = 0; col < gridLength - row; col++) {

			generatingVertices.Add(FVector((col + (row * 0.5f)) * (globalSize / (gridLength - 1)), (row * unitTriangleHeight) * (globalSize / (gridLength - 1)), FetchValueFromPerlinMap(FVector2D((float)(col + (row * 0.5f)) / (float)(gridLength - 1), (float)row / (float)(gridLength - 1)), meshPerlinMap, meshPerlinMapSize) * meshPerlinAmplitude));
			generatingNormals.Add(FVector(0.f, 1.f, 0.f));

			if (uvMode == 0) { generatingUVs.Add(FVector2D(((float)col / (float)(gridLength - 1)) * uvScale.X, ((float)row / (float)(gridLength - 1))) * uvScale.Y); }
			else if (uvMode == 1) { generatingUVs.Add(FVector2D(((float)col / (float)(gridLength - 1 - row)) * uvScale.X, ((float)row / (float)(gridLength - 1))) * uvScale.Y); }
			else if (uvMode == 2) { generatingUVs.Add(FVector2D((((float)col / (float)(gridLength - 1)) + (((float)row / (float)(gridLength - 1)) / 2.f)) * uvScale.X, ((float)row / (float)(gridLength - 1))) * uvScale.Y); }
			
		}

	}

	//add standard triangles to triangular grid mesh
	for (int row = 0; row < (gridLength - 1); row++) {

		for (int col = 0; col < (gridLength - 1) - row; col++) {

			float porosity = FetchValueFromPerlinMap(FVector2D((float)(col + (row * 0.5f)) / (float)(gridLength - 1), (float)row / (float)(gridLength - 1)), porosityPerlinMap, porosityPerlinMapSize);

			if (porosityPerlinLimit.X < porosity && porosity < porosityPerlinLimit.Y) {

				generatingTriangles.Add(col + (row * gridLength) - (row * (row - 1) / 2.f));
				generatingTriangles.Add(col + ((row + 1) * gridLength) - ((row + 1) * row) / 2.f);
				generatingTriangles.Add(col + (row * gridLength) - (row * (row - 1) / 2.f) + 1);

			}

			else {

				missingTriangles.Add(col + (row * gridLength) - (row * (row - 1) / 2.f));
				missingTriangles.Add(col + ((row + 1) * gridLength) - ((row + 1) * row) / 2.f);
				missingTriangles.Add(col + (row * gridLength) - (row * (row - 1) / 2.f) + 1);

			}

		}

	}

	//add inverted triangles to triangular grid mesh
	for (int row = 1; row < gridLength; row++) {

		for (int col = 1; col < gridLength - row; col++) {

			float porosity = FetchValueFromPerlinMap(FVector2D((float)(col + (row * 0.5f)) / (float)(gridLength - 1), (float)row / (float)(gridLength - 1)), porosityPerlinMap, porosityPerlinMapSize);

			if (porosityPerlinLimit.X < porosity && porosity < porosityPerlinLimit.Y) {

				generatingTriangles.Add(col + ((row - 1) * gridLength) - (((row - 1) * row) / 2.f) + (row - 1));
				generatingTriangles.Add(col + (row * gridLength) - (row * (row - 1) / 2.f) - 1);
				generatingTriangles.Add(col + (row * gridLength) - (row * (row - 1) / 2.f));

			}

			else {

				missingTriangles.Add(col + ((row - 1) * gridLength) - (((row - 1) * row) / 2.f) + (row - 1));
				missingTriangles.Add(col + (row * gridLength) - (row * (row - 1) / 2.f) - 1);
				missingTriangles.Add(col + (row * gridLength) - (row * (row - 1) / 2.f));

			}

		}

	}
	
	generatedMesh.vertices = generatingVertices;
	generatedMesh.triangles = generatingTriangles;
	generatedMesh.normals = generatingNormals;
	generatedMesh.uvs = generatingUVs;

	/*FString debugtritext = "";

	for (int debugCounter = 0; debugCounter < generatedMesh.uvs.Num(); debugCounter++) {
		debugtritext.Append("(");
		debugtritext.Append(FString::SanitizeFloat(generatedMesh.uvs[debugCounter].X));
		debugtritext.Append(", ");
		debugtritext.Append(FString::SanitizeFloat(generatedMesh.uvs[debugCounter].Y));
		debugtritext.Append(") ");
	}

	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, debugtritext);*/

	PopulateNaviGraph(generatedMesh);

	//sharpen the mesh to show off the edge geometry with more distinction
	if (sharpenEdges) {
		SharpenMeshEdges(generatedMesh, subMesh, missingTriangles);
	}

	//generate vertical pillars if we're not going to be sharpening the mesh afterwards (because if we're doing that, we'll have access to triangle normals, and the pillars won't have to be jarringly vertical)
	else {
		CreateRigidPillars(generatedMesh, subMesh, missingTriangles);
	}

}

//TRIANGLE BULK SETUP

void UTerrainGenerator::CreateRigidPillars(FMeshData& generatedMesh, FMeshData& subMesh, TArray<int> missingTriangles) {

	TArray<FVector> addingVertices;
	TArray<int> addingTriangles;
	TArray<FVector> addingNormals;
	TArray<FVector2D> addingUVs;

	for (int triset = 0; triset < missingTriangles.Num(); triset += 3) {

		addingVertices.Add(generatedMesh.vertices[missingTriangles[triset]] + FVector(0.f, 0.f, (globalSize / (gridLength - 1) * pillarHeight)));
		addingVertices.Add(generatedMesh.vertices[missingTriangles[triset + 1]] + FVector(0.f, 0.f, (globalSize / (gridLength - 1) * pillarHeight)));
		addingVertices.Add(generatedMesh.vertices[missingTriangles[triset + 2]] + FVector(0.f, 0.f, (globalSize / (gridLength - 1) * pillarHeight)));
		addingVertices.Add(generatedMesh.vertices[missingTriangles[triset]]);
		addingVertices.Add(generatedMesh.vertices[missingTriangles[triset + 1]]);
		addingVertices.Add(generatedMesh.vertices[missingTriangles[triset + 2]]);

		addingTriangles.Add(addingVertices.Num() - 6);
		addingTriangles.Add(addingVertices.Num() - 5);
		addingTriangles.Add(addingVertices.Num() - 4);

		addingNormals.Add(FVector(0.f, 0.f, 1.f));
		addingNormals.Add(FVector(0.f, 0.f, 1.f));
		addingNormals.Add(FVector(0.f, 0.f, 1.f));
		addingNormals.Add(FVector(0.f, 0.f, 1.f));
		addingNormals.Add(FVector(0.f, 0.f, 1.f));
		addingNormals.Add(FVector(0.f, 0.f, 1.f));

		//not about to pass in "missingUVs" so we're going forward with the uvMode==3 interpretation for these. sorry. a call of performance and readability.
		addingUVs.Add(FVector2D(0.f, 0.f));
		addingUVs.Add(FVector2D(1.f * uvScale.X, 0.f));
		addingUVs.Add(FVector2D(0.5f * uvScale.X, 1.f * uvScale.Y));
		addingUVs.Add(FVector2D(0.f, 0.f));
		addingUVs.Add(FVector2D(1.f * uvScale.X, 0.f));
		addingUVs.Add(FVector2D(0.5f * uvScale.X, 1.f * uvScale.Y));

		//brute code for generating each side of a triangle pillar
		addingTriangles.Add(addingVertices.Num() - 6);
		addingTriangles.Add(addingVertices.Num() - 3);
		addingTriangles.Add(addingVertices.Num() - 5);

		addingTriangles.Add(addingVertices.Num() - 6);
		addingTriangles.Add(addingVertices.Num() - 1);
		addingTriangles.Add(addingVertices.Num() - 3);

		addingTriangles.Add(addingVertices.Num() - 5);
		addingTriangles.Add(addingVertices.Num() - 2);
		addingTriangles.Add(addingVertices.Num() - 4);

		addingTriangles.Add(addingVertices.Num() - 5);
		addingTriangles.Add(addingVertices.Num() - 3);
		addingTriangles.Add(addingVertices.Num() - 2);

		addingTriangles.Add(addingVertices.Num() - 4);
		addingTriangles.Add(addingVertices.Num() - 1);
		addingTriangles.Add(addingVertices.Num() - 6);

		addingTriangles.Add(addingVertices.Num() - 4);
		addingTriangles.Add(addingVertices.Num() - 2);
		addingTriangles.Add(addingVertices.Num() - 1);

	}
	
	subMesh.vertices.Append(addingVertices);
	subMesh.triangles.Append(addingTriangles);
	subMesh.normals.Append(addingNormals);
	subMesh.uvs.Append(addingUVs);

}

void UTerrainGenerator::SharpenMeshEdges(FMeshData& generatedMesh, FMeshData& subMesh, TArray<int> missingTriangles) {

	//for each triangle, get the vertex, copy it into a new vertex list and give that one a new normal based on the tri's angle. 
	//then clear vertices, triangles, and normals; replace with that new vertex list, a crescendoing triangle list (012345678), and the given norms.

	TArray<FVector> sharpVertices;
	TArray<int> sharpTriangles;
	TArray<FVector> sharpNormals;
	TArray<FVector2D> sharpUVs;

	TArray<FVector> pillarVertices;
	TArray<int> pillarTriangles;
	TArray<FVector> pillarNormals;
	TArray<FVector2D> pillarUVs;

	for (int triset = 0; triset < generatedMesh.triangles.Num(); triset += 3) {

		FVector firstDir = (generatedMesh.vertices[generatedMesh.triangles[triset + 1]] - generatedMesh.vertices[generatedMesh.triangles[triset]]);
		FVector secondDir = (generatedMesh.vertices[generatedMesh.triangles[triset + 2]] - generatedMesh.vertices[generatedMesh.triangles[triset]]);
		firstDir.Normalize();
		secondDir.Normalize();
		FVector triangleNormal = FVector::CrossProduct(secondDir, firstDir);
		triangleNormal.Normalize();

		sharpVertices.Add(generatedMesh.vertices[generatedMesh.triangles[triset]]);
		sharpVertices.Add(generatedMesh.vertices[generatedMesh.triangles[triset + 1]]);
		sharpVertices.Add(generatedMesh.vertices[generatedMesh.triangles[triset + 2]]);

		sharpTriangles.Add(triset);
		sharpTriangles.Add(triset + 1);
		sharpTriangles.Add(triset + 2);

		sharpNormals.Add(triangleNormal);
		sharpNormals.Add(triangleNormal);
		sharpNormals.Add(triangleNormal);

		if (uvMode == 3) { 
			sharpUVs.Add(FVector2D(0.f, 0.f));
			sharpUVs.Add(FVector2D(1.f * uvScale.X, 0.f));
			sharpUVs.Add(FVector2D(0.5f * uvScale.X, 1.f * uvScale.Y));
		}
		else {
			sharpUVs.Add(generatedMesh.uvs[generatedMesh.triangles[triset]]);
			sharpUVs.Add(generatedMesh.uvs[generatedMesh.triangles[triset + 1]]);
			sharpUVs.Add(generatedMesh.uvs[generatedMesh.triangles[triset + 2]]);
		}

	}

	for (int triset = 0; triset < missingTriangles.Num(); triset += 3) {

		FVector firstDir = (generatedMesh.vertices[missingTriangles[triset + 1]] - generatedMesh.vertices[missingTriangles[triset]]);
		FVector secondDir = (generatedMesh.vertices[missingTriangles[triset + 2]] - generatedMesh.vertices[missingTriangles[triset]]);
		firstDir.Normalize();
		secondDir.Normalize();
		FVector triangleNormal = FVector::CrossProduct(secondDir, firstDir);
		triangleNormal.Normalize();

		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 1]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 2]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));

		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());

		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);

		if (uvMode == 3) {
			pillarUVs.Add(FVector2D(0.f, 0.f));
			pillarUVs.Add(FVector2D(1.f * uvScale.X, 0.f));
			pillarUVs.Add(FVector2D(0.5f * uvScale.X, 1.f * uvScale.Y));
		}

		else {
			pillarUVs.Add(generatedMesh.uvs[missingTriangles[triset]]);
			pillarUVs.Add(generatedMesh.uvs[missingTriangles[triset + 1]]);
			pillarUVs.Add(generatedMesh.uvs[missingTriangles[triset + 2]]);
		}

		//brute code for generating each side of a triangle pillar
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset]]);
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 1]]);
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarUVs.Add(FVector2D(0.f, 0.f));
		pillarUVs.Add(FVector2D(1.f, 0.f));
		pillarUVs.Add(FVector2D(0.f, 1.f));

		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset]]);
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 2]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarUVs.Add(FVector2D(1.f, 0.f));
		pillarUVs.Add(FVector2D(1.f, 1.f));
		pillarUVs.Add(FVector2D(0.f, 1.f));

		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 1]]);
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 2]]);
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 1]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarUVs.Add(FVector2D(0.f, 0.f));
		pillarUVs.Add(FVector2D(1.f, 0.f));
		pillarUVs.Add(FVector2D(0.f, 1.f));

		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 1]]);
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 1]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarUVs.Add(FVector2D(1.f, 0.f));
		pillarUVs.Add(FVector2D(1.f, 1.f));
		pillarUVs.Add(FVector2D(0.f, 1.f));

		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 2]]);
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset]]);
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 2]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarUVs.Add(FVector2D(0.f, 0.f));
		pillarUVs.Add(FVector2D(1.f, 0.f));
		pillarUVs.Add(FVector2D(0.f, 1.f));

		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 2]]);
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 2]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarVertices.Add(generatedMesh.vertices[missingTriangles[triset + 1]] + (triangleNormal * (globalSize / (gridLength - 1) * pillarHeight)));
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarTriangles.Add(pillarTriangles.Num());
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarNormals.Add(triangleNormal);
		pillarUVs.Add(FVector2D(1.f, 0.f));
		pillarUVs.Add(FVector2D(1.f, 1.f));
		pillarUVs.Add(FVector2D(0.f, 1.f));

	}

	generatedMesh.vertices = sharpVertices;
	generatedMesh.triangles = sharpTriangles;
	generatedMesh.normals = sharpNormals;
	generatedMesh.uvs = sharpUVs;

	subMesh.vertices = pillarVertices;
	subMesh.triangles = pillarTriangles;
	subMesh.normals = pillarNormals;
	subMesh.uvs = pillarUVs;

}

//RANDOM GENERATION

//Populate Perlin Map with RNG-seeded values
void UTerrainGenerator::PopulatePerlinMap(TArray<TArray<FVector2D>>& perlinMap, FVector2D& mapSize) {

	perlinMap.Empty();

	//better way to do this? !!!!!

	//FString debugtritext = "";

	for (int row = 0; row < mapSize.X; row++) {

		perlinMap.Add(TArray<FVector2D>());

		for (int col = 0; col < mapSize.Y; col++) {

			//needs normalisation; could we generate a random point on the PERIMETER of a circle, saving a step and being able to assign directly? !!!!!
			//generate rand value between 0 and 2pi, representing 360 degrees in radians, then use Sin() and Cos() to convert to a unit vector

			FVector2D randomDir = FMath::RandPointInCircle(1.f);
			randomDir.Normalize();

			perlinMap[row].Add(randomDir);

			/*debugtritext.Append("(");
			debugtritext.Append(FString::SanitizeFloat(randomDir.X));
			debugtritext.Append(", ");
			debugtritext.Append(FString::SanitizeFloat(perlinMap[row][col].X));
			debugtritext.Append(", ");
			debugtritext.Append(FString::SanitizeFloat(perlinMap[row][col].Y));
			debugtritext.Append(") ");*/

		}


	}

	//GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, debugtritext);

}

float UTerrainGenerator::InverseLerp(float x, float y, float value) { //https://forums.unrealengine.com/t/looking-for-something-like-inverselerp/112983/4
	return (value - x) / (y - x);
}

//Load our RNG perlinMap and follow the process to turn an input position into an output float.
float UTerrainGenerator::FetchValueFromPerlinMap(FVector2D pointCoords, TArray<TArray<FVector2D>>& perlinMap, FVector2D& mapSize) {

	//if (clampPerlinCoords) { pointCoords = FMath::Clamp(pointCoords, FVector2D(0.00001f, 0.1f), FVector2D(0.9f, 0.9f)); }
	pointCoords = (pointCoords * (1.f - perlinZoomIn)) + (perlinZoomIn / 2.f);

	TArray<int> perlinIndices = TArray<int> {
		(int)FMath::FloorToInt(pointCoords.X * (mapSize.X - 1)),	//rounded-down horizontal
		(int)FMath::CeilToInt(pointCoords.X * (mapSize.X - 1)),		//rounded-up horizontal
		(int)FMath::FloorToInt(pointCoords.Y * (mapSize.Y - 1)),	//rounded-down vertical
		(int)FMath::CeilToInt(pointCoords.Y * (mapSize.Y - 1))		//rounded-up vertical
	};

	TArray<float> perlinCoords = TArray<float>{
		(float)perlinIndices[0] / (float)(mapSize.X - 1),		//rounded-down horizontal
		(float)perlinIndices[1] / (float)(mapSize.X - 1),		//rounded-up horizontal
		(float)perlinIndices[2] / (float)(mapSize.Y - 1),		//rounded-down vertical
		(float)perlinIndices[3] / (float)(mapSize.Y - 1)		//rounded-up vertical
	};

	TArray<float> pointDots = TArray<float>{
		(float)(perlinMap[perlinIndices[0]][perlinIndices[2]] | (pointCoords - FVector2D(perlinCoords[0], perlinCoords[2]))),	//lower-left
		(float)(perlinMap[perlinIndices[1]][perlinIndices[2]] | (pointCoords - FVector2D(perlinCoords[1], perlinCoords[2]))),	//lower-right
		(float)(perlinMap[perlinIndices[0]][perlinIndices[3]] | (pointCoords - FVector2D(perlinCoords[0], perlinCoords[3]))),	//upper-left
		(float)(perlinMap[perlinIndices[1]][perlinIndices[3]] | (pointCoords - FVector2D(perlinCoords[1], perlinCoords[3])))	//upper-right
	};

	//float pointDots = (pointCoords - ) | perlinMap[][];

	//float perlinValue = FMath::(pointCoords, pointCoords, 0.1f);

	float perlinValue = 0;
	perlinValue = FMath::Lerp(
		FMath::Lerp(
			pointDots[0], 
			pointDots[1], 
			InverseLerp(perlinCoords[0], perlinCoords[1], pointCoords.X)
		), 
		FMath::Lerp(
			pointDots[2], 
			pointDots[3], 
			InverseLerp(perlinCoords[0], perlinCoords[1], pointCoords.X)
		), 
		InverseLerp(perlinCoords[2], perlinCoords[3], pointCoords.Y)
	);

	/*FString debugtritext = "";
	debugtritext.Append("(");
	debugtritext.Append(FString::SanitizeFloat(perlinValue));
	debugtritext.Append(") ");
	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, debugtritext);*/


	return perlinValue;


}

//Old Version, had worse pointers worth mentioning in devlog
/*void UTerrainGenerator::PopulateNaviGraph(FMeshData& generatedMesh) {

	naviMap.graphNodes.Empty();

	for (int triset = 0; triset < generatedMesh.triangles.Num(); triset += 3) {

		TArray<FTemp_NaviNode*> triangleNodes;

		for (int iteratingVertex = 0; iteratingVertex < 3; iteratingVertex++) {

			if (naviMap.graphNodes.ContainsByPredicate([&](const FTemp_NaviNode& iteratingNode) { return iteratingNode.worldPosition == generatedMesh.vertices[generatedMesh.triangles[triset + iteratingVertex]]; })) {

				triangleNodes.Add(naviMap.graphNodes.FindByPredicate([&](const FTemp_NaviNode& iteratingNode) { return iteratingNode.worldPosition == generatedMesh.vertices[generatedMesh.triangles[triset + iteratingVertex]]; }));
				
			}

			else {

				//!!!!! initial instinct was: "naviMap.graphNodes.Add(FNaviNode { worldPosition = generatedMesh.vertices[generatedMesh.triangles[iteratingVertex]] } );"
				//have a constructor for the struct, or FTemp_NaviNode newNode;

				FTemp_NaviNode newNode;
				newNode.worldPosition = generatedMesh.vertices[generatedMesh.triangles[triset + iteratingVertex]];
				int returnedIndex = naviMap.graphNodes.Add(newNode);

				triangleNodes.Add(&naviMap.graphNodes[returnedIndex]);
				
			}

		}

		//missing from mystery
		if (!triangleNodes[0]->adjacentNodes.ContainsByPredicate([&](FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == triangleNodes[1]->worldPosition; })) {
			triangleNodes[0]->adjacentNodes.Add(triangleNodes[1]);
		}

		//missing from mystery
		if (!triangleNodes[0]->adjacentNodes.ContainsByPredicate([&](FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == triangleNodes[2]->worldPosition; })) {
			triangleNodes[0]->adjacentNodes.Add(triangleNodes[2]);
		}

		if (!triangleNodes[1]->adjacentNodes.ContainsByPredicate([&](FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == triangleNodes[0]->worldPosition; })) {
			triangleNodes[1]->adjacentNodes.Add(triangleNodes[0]);
		}

		if (!triangleNodes[1]->adjacentNodes.ContainsByPredicate([&](FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == triangleNodes[2]->worldPosition; })) {
			triangleNodes[1]->adjacentNodes.Add(triangleNodes[2]);
		}

		if (!triangleNodes[2]->adjacentNodes.ContainsByPredicate([&](FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == triangleNodes[0]->worldPosition; })) {
			triangleNodes[2]->adjacentNodes.Add(triangleNodes[0]);
		}

		if (!triangleNodes[2]->adjacentNodes.ContainsByPredicate([&](FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == triangleNodes[1]->worldPosition; })) {
			triangleNodes[2]->adjacentNodes.Add(triangleNodes[1]);
		}

	}

}*/

//Inform every NavGraph node about which nodes lie adjacent to it.
void UTerrainGenerator::PopulateNaviGraph(FMeshData& generatedMesh) {

	/*FString vertexText = "Mesh Vertices: ";
	for (int i = 0; i < generatedMesh.vertices.Num(); i++) {

		vertexText.Append("(");
		vertexText.Append(FString::FromInt(generatedMesh.vertices[i].X));
		vertexText.Append(", ");
		vertexText.Append(FString::FromInt(generatedMesh.vertices[i].Y));
		vertexText.Append(", ");
		vertexText.Append(FString::FromInt(generatedMesh.vertices[i].Z));
		vertexText.Append("), ");

	}
	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, vertexText);*/

	/*FString triangleText = "Mesh Triangles: ";
	for (int i = 0; i < generatedMesh.triangles.Num(); i += 3) {

		triangleText.Append("(");
		triangleText.Append(FString::FromInt(generatedMesh.triangles[i]));
		triangleText.Append(", ");
		triangleText.Append(FString::FromInt(generatedMesh.triangles[i + 1]));
		triangleText.Append(", ");
		triangleText.Append(FString::FromInt(generatedMesh.triangles[i + 2]));
		triangleText.Append("), ");

	}
	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, triangleText);*/
	//GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, "");


	//!!!!!still has a problem that can be replicated by the second vertex always having five vertices.
	//on the first triangle, the first vertex always has it's first position replaced by zero, getting fixed a second later.
	//that is to say - when iteratingVertex is 0, and triset is 0, the newNode created will record it's own position perfectly fine, but when added to the nodes of graphNodeIndices[1] and graphNodeIndices[2], it's X coordinate will be replaced with 0.
	//can maybe be temporarily fixed by just ensuring the first vertex in the system always has a position of 0 - currently the first tri is (4,1,0) and the first vertex is always at (0,0,?). if we just changed the tri to be (0,4,1) or equivalent ... maybe fine?
	//otherwise i give up and have no idea why this happens.
	//addendum: yeah the tri being (0,4,1) worked completely fuck my life #ProfessionalCodeComments. right, i'm done for the day


	naviMap.graphNodes.Empty();

	for (int triset = 0; triset < generatedMesh.triangles.Num(); triset += 3) {

		TArray<int> graphNodeIndices;

		for (int iteratingVertex = 0; iteratingVertex < 3; iteratingVertex++) {

			if (naviMap.graphNodes.ContainsByPredicate([&](const FTemp_NaviNode& iteratingNode) { return iteratingNode.worldPosition == generatedMesh.vertices[generatedMesh.triangles[triset + iteratingVertex]]; })) {

				graphNodeIndices.Add(naviMap.graphNodes.IndexOfByPredicate([&](const FTemp_NaviNode& iteratingNode) { return iteratingNode.worldPosition == generatedMesh.vertices[generatedMesh.triangles[triset + iteratingVertex]]; }));

			}

			else {

				//!!!!! initial instinct was: "naviMap.graphNodes.Add(FNaviNode { worldPosition = generatedMesh.vertices[generatedMesh.triangles[iteratingVertex]] } );"
				//have a constructor for the struct, or FTemp_NaviNode newNode;

				FTemp_NaviNode newNode;
				newNode.worldPosition = generatedMesh.vertices[generatedMesh.triangles[triset + iteratingVertex]];
				graphNodeIndices.Add(naviMap.graphNodes.Add(newNode));

			}

		}

		if (!naviMap.graphNodes[graphNodeIndices[0]].adjacentNodes.ContainsByPredicate([&](const FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == naviMap.graphNodes[graphNodeIndices[1]].worldPosition; })) {
			naviMap.graphNodes[graphNodeIndices[0]].adjacentNodes.Add(&naviMap.graphNodes[graphNodeIndices[1]]);
		}

		if (!naviMap.graphNodes[graphNodeIndices[0]].adjacentNodes.ContainsByPredicate([&](const FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == naviMap.graphNodes[graphNodeIndices[2]].worldPosition; })) {
			naviMap.graphNodes[graphNodeIndices[0]].adjacentNodes.Add(&naviMap.graphNodes[graphNodeIndices[2]]);
		}

		if (!naviMap.graphNodes[graphNodeIndices[1]].adjacentNodes.ContainsByPredicate([&](const FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == naviMap.graphNodes[graphNodeIndices[0]].worldPosition; })) {
			naviMap.graphNodes[graphNodeIndices[1]].adjacentNodes.Add(&naviMap.graphNodes[graphNodeIndices[0]]);
		}

		if (!naviMap.graphNodes[graphNodeIndices[1]].adjacentNodes.ContainsByPredicate([&](const FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == naviMap.graphNodes[graphNodeIndices[2]].worldPosition; })) {
			naviMap.graphNodes[graphNodeIndices[1]].adjacentNodes.Add(&naviMap.graphNodes[graphNodeIndices[2]]);
		}

		if (!naviMap.graphNodes[graphNodeIndices[2]].adjacentNodes.ContainsByPredicate([&](const FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == naviMap.graphNodes[graphNodeIndices[0]].worldPosition; })) {
			naviMap.graphNodes[graphNodeIndices[2]].adjacentNodes.Add(&naviMap.graphNodes[graphNodeIndices[0]]);
		}

		if (!naviMap.graphNodes[graphNodeIndices[2]].adjacentNodes.ContainsByPredicate([&](const FTemp_NaviNode* iteratingNode) { return iteratingNode->worldPosition == naviMap.graphNodes[graphNodeIndices[1]].worldPosition; })) {
			naviMap.graphNodes[graphNodeIndices[2]].adjacentNodes.Add(&naviMap.graphNodes[graphNodeIndices[1]]);
		}

	}

}

//Should return a random walkable position on the NavGraph.
FVector UTerrainGenerator::GetRandomNavGraphPosition() {


	FTemp_NaviNode* randomNode = &naviMap.graphNodes[FMath::RandRange(0, naviMap.graphNodes.Num() - 1)];


	FString worldPosText = "Node Position: (";
	worldPosText.Append(FString::FromInt(randomNode->worldPosition.X));
	worldPosText.Append(", ");
	worldPosText.Append(FString::FromInt(randomNode->worldPosition.Y));
	worldPosText.Append(", ");
	worldPosText.Append(FString::FromInt(randomNode->worldPosition.Z));
	worldPosText.Append(")");
	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, worldPosText);
	
	FString adjacentNumberText = "Adjacent Nodes: ";
	adjacentNumberText.Append(FString::FromInt(randomNode->adjacentNodes.Num()));
	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, adjacentNumberText);

	FString adjacentNodesText = "Adjacent Node Positions: ";
	for (int i = 0; i < randomNode->adjacentNodes.Num(); i++) {

		adjacentNodesText.Append("(");
		adjacentNodesText.Append(FString::FromInt(randomNode->adjacentNodes[i]->worldPosition.X));
		adjacentNodesText.Append(", ");
		adjacentNodesText.Append(FString::FromInt(randomNode->adjacentNodes[i]->worldPosition.Y));
		adjacentNodesText.Append(", ");
		adjacentNodesText.Append(FString::FromInt(randomNode->adjacentNodes[i]->worldPosition.Z));
		adjacentNodesText.Append("), ");

	}
	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, adjacentNodesText);

	FString fullGraphText = "Full Graph: ";
	for (int i = 0; i < naviMap.graphNodes.Num(); i++) {

		fullGraphText.Append("(");
		fullGraphText.Append(FString::FromInt(naviMap.graphNodes[i].worldPosition.X));
		fullGraphText.Append(", ");
		fullGraphText.Append(FString::FromInt(naviMap.graphNodes[i].worldPosition.Y));
		fullGraphText.Append(", ");
		fullGraphText.Append(FString::FromInt(naviMap.graphNodes[i].worldPosition.Z));
		fullGraphText.Append("), ");

	}
	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, fullGraphText);
	GEngine->AddOnScreenDebugMessage(-1, 15.0f, FColor::Yellow, "");

	//get random nav node
	//find at least one triangle using it's adjacent nodes
	//lerp randomly within triangle

	return randomNode->worldPosition;

}

