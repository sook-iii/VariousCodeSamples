using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]

public class CustomOceanHelper : MonoBehaviour
{

    public bool constantUpdates;
    public Material usingSkyboxMaterial;


    public List<CustomLightComponent> sceneLights;
    public List<CustomSphereComponent> sceneSolids;

    public Texture albedoTexture;
    public Texture normalTexture;
    public Texture roughnessTexture;
    public Texture occlusionTexture;
    public Texture foamTexture;
    public Texture skyboxTexture;

    public Color mainColour;

    public Color foamColour;
    public float foamFalloff;

    public float gerstnerCircleRadius;
    public float specularPower;
    public float fresnelPower;
    public float indexOfRefraction;

    public Vector4 wave1AmplitudeLengthSpeedAngle;
    public Vector4 wave2AmplitudeLengthSpeedAngle;
    public Vector4 wave3AmplitudeLengthSpeedAngle;
    public Vector4 oceanTextureScaleOpacitySpeedAngle;
    public Vector4 foamTextureScaleOpacitySpeedAngle;

    public enum AmbientLightingStyles { None, Flat, CubeMap };
    public AmbientLightingStyles ambientLightingStyle;

    public enum DiffuseLightingStyles { None, Blinn, OrenNayar };
    public DiffuseLightingStyles diffuseLightingStyle;

    public enum SpecularLightingStyles { None, Blinn, Phong, CookTorrence };
    public SpecularLightingStyles specularLightingStyle;

    public enum LightRedirectionStyles { Fresnel, Reflection, Refraction, Both };
    public LightRedirectionStyles lightRedirectionStyle;






    // Update is called once per frame
    void Update()
    {

        if (constantUpdates)
        {

            usingSkyboxMaterial.mainTexture = skyboxTexture;

            MeshRenderer[] shaderMeshes = GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer shaderMesh in shaderMeshes)
            {

                //Transfer Textures
                shaderMesh.sharedMaterial.SetTexture("_AlbedoTexture", albedoTexture);
                shaderMesh.sharedMaterial.SetTexture("_NormalTexture", normalTexture);
                shaderMesh.sharedMaterial.SetTexture("_RoughnessTexture", roughnessTexture);
                shaderMesh.sharedMaterial.SetTexture("_OcclusionTexture", occlusionTexture);
                shaderMesh.sharedMaterial.SetTexture("_FoamTexture", foamTexture);
                shaderMesh.sharedMaterial.SetTexture("_SkyboxTexture", skyboxTexture);

                //Wave Stuff
                shaderMesh.sharedMaterial.SetColor("_OceanColour", mainColour);
                shaderMesh.sharedMaterial.SetInt("_MainScale", (int) transform.localScale.x / 10);

                shaderMesh.sharedMaterial.SetColor("_FoamColour", foamColour);
                shaderMesh.sharedMaterial.SetFloat("_FoamFalloff", foamFalloff);

                shaderMesh.sharedMaterial.SetFloat("_GerstnerCircleRadius", gerstnerCircleRadius);
                shaderMesh.sharedMaterial.SetFloat("_SpecularPower", specularPower);
                shaderMesh.sharedMaterial.SetFloat("_FresnelPower", fresnelPower);
                shaderMesh.sharedMaterial.SetFloat("_IndexOfRefraction", indexOfRefraction);

                //Vector4 Helper Functions
                shaderMesh.sharedMaterial.SetVector("_Wave1Details", wave1AmplitudeLengthSpeedAngle);
                shaderMesh.sharedMaterial.SetVector("_Wave2Details", wave2AmplitudeLengthSpeedAngle);
                shaderMesh.sharedMaterial.SetVector("_Wave3Details", wave3AmplitudeLengthSpeedAngle);
                shaderMesh.sharedMaterial.SetVector("_OceanTextureDetails", oceanTextureScaleOpacitySpeedAngle);
                shaderMesh.sharedMaterial.SetVector("_FoamTextureDetails", foamTextureScaleOpacitySpeedAngle);

                //Transfer Lighting Style Choices
                shaderMesh.sharedMaterial.SetInt("_AmbientLightingStyle", (int)ambientLightingStyle);
                shaderMesh.sharedMaterial.SetInt("_DiffuseLightingStyle", (int)diffuseLightingStyle);
                shaderMesh.sharedMaterial.SetInt("_SpecularLightingStyle", (int)specularLightingStyle);
                shaderMesh.sharedMaterial.SetInt("_LightRedirectionStyle", (int)lightRedirectionStyle);


            }

            for (int i = 0; i < sceneLights.Count; i++)
            {

                foreach (MeshRenderer shaderMesh in shaderMeshes)
                {

                    //Automatic Scene Transformations. Shouldn't be exposed to editor
                    shaderMesh.sharedMaterial.SetVector("_Light" + (i + 1).ToString() + "Position", new Vector4(sceneLights[i].transform.position.x, sceneLights[i].transform.position.y, sceneLights[i].transform.position.z, 0));
                    shaderMesh.sharedMaterial.SetVector("_Light" + (i + 1).ToString() + "Direction", new Vector4(-sceneLights[i].transform.forward.x, -sceneLights[i].transform.forward.y, -sceneLights[i].transform.forward.z, 0));
                    shaderMesh.sharedMaterial.SetInt("_Light" + (i + 1).ToString() + "Type", (int)sceneLights[i].lightType);
                    shaderMesh.sharedMaterial.SetFloat("_Light" + (i + 1).ToString() + "Intensity", (int)sceneLights[i].lightIntensity);
                    shaderMesh.sharedMaterial.SetFloat("_Light" + (i + 1).ToString() + "SpotlightLimit", (int)sceneLights[i].lightSpotlightLimit);
                    shaderMesh.sharedMaterial.SetColor("_AmbientLight" + (i + 1).ToString() + "Colour", sceneLights[i].ambientLightColour);
                    shaderMesh.sharedMaterial.SetColor("_DiffuseLight" + (i + 1).ToString() + "Colour", sceneLights[i].diffuseLightColour);
                    shaderMesh.sharedMaterial.SetColor("_SpecularLight" + (i + 1).ToString() + "Colour", sceneLights[i].specularLightColour);

                }

            }

            for (int i = 0; i < sceneSolids.Count; i++) {

                foreach (MeshRenderer shaderMesh in shaderMeshes)
                {

                    //Ditto, but for spheres
                    shaderMesh.sharedMaterial.SetVector("_Sphere" + (i + 1).ToString() + "Position", new Vector4(sceneSolids[i].transform.position.x, sceneSolids[i].transform.position.y, sceneSolids[i].transform.position.z, 0));
                    shaderMesh.sharedMaterial.SetFloat("_Sphere" + (i + 1).ToString() + "Radius", sceneSolids[i].sphereRadius / 2);

                }

            }

        }

    }

}
