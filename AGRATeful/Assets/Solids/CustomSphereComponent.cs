using UnityEngine;

[ExecuteAlways]
public class CustomSphereComponent : MonoBehaviour
{

    public float sphereRadius;
    public Color sphereColour;

    //Render Spheres - just primitives for now. Potential expansion to adapt for pixel shader 
    void Update()
    {

        transform.localScale = Vector3.one * sphereRadius;

        MeshRenderer[] shaderMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer shaderMesh in shaderMeshes)
        {
            shaderMesh.sharedMaterial.SetColor("_SolidColour", sphereColour);

        }

    }
}
