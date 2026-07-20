using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CustomLightComponent : MonoBehaviour
{
    public enum PossibleLightTypes { Directional, Point, Spot };
    public PossibleLightTypes lightType;

    public float lightIntensity;
    public float lightSpotlightLimit;

    public Color ambientLightColour;
    public Color diffuseLightColour;
    public Color specularLightColour;

    public bool lookAtOrigin;

    //Should just face the centre of the scene every time it moves
    void Update()
    {
        //GetComponent<Material>().SetVectorArray("testVar", new List<Vector4>() { new Vector4(1, 0, 0, 1), new Vector4(0, 1, 0, 1) });

        if (lookAtOrigin)
        {

            transform.forward = Vector3.Normalize(-transform.position);

        }

    }
}
