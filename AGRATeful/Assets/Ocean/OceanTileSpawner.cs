using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class OceanTileSpawner : MonoBehaviour
{

    public GameObject oceanTilePrefab;
    public float oceanOverallScale;
    public int oceanGridSize;
    List<GameObject> childrenToDestroy;

    //Spawn in as many ocean child objects as needed to granularize mesh
    //Needs to happen every time a variable is edited, but it's an expensive function so ideally not more
    private void OnValidate()
    {
                
        childrenToDestroy = new List<GameObject>();

        foreach (Transform childTransform in transform)
        {
            childrenToDestroy.Add(childTransform.gameObject);
        }

        transform.localScale = Vector3.one * 10 / oceanGridSize * oceanOverallScale;

        for (int i = 0; i < oceanGridSize; i++)
        {
            for (int j = 0; j < oceanGridSize; j++)
            {

                GameObject oceanTileObject = Instantiate(oceanTilePrefab, transform);
                oceanTileObject.name += new Vector2Int(j,i).ToString();
                oceanTileObject.transform.localPosition = 10 * (new Vector3(j + 0.5f, 0, i + 0.5f));
                oceanTileObject.transform.localScale = Vector3.one;


            }
        }


    }

    private void Update()
    {

        if (childrenToDestroy.Count > 0)
        {

            foreach (GameObject child in childrenToDestroy)
            {
                DestroyImmediate(child);
            }

            childrenToDestroy = new List<GameObject>();

        }
    }

}
