using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteAlways]

public class CustomSolidHelper : MonoBehaviour
{
    public bool constantUpdates;
    public Material usingSkyboxMaterial;


    public List<CustomLightComponent> sceneLights;

    public Texture albedoTexture;
    public Texture normalTexture;
    public Texture roughnessTexture;
    public Texture occlusionTexture;
    public Texture skyboxTexture;

    public float specularPower;
    public float fresnelPower;
    public float indexOfRefraction;
    public float bobbingMagnitude;
    public float bobbingConstant;
    public float bobbingSpeed;

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

            transform.localPosition = new Vector3(transform.localPosition.x, bobbingMagnitude * Mathf.Sin(Time.time * bobbingSpeed + bobbingConstant), transform.localPosition.z);

            usingSkyboxMaterial.mainTexture = skyboxTexture;

            MeshRenderer shaderMesh = GetComponent<MeshRenderer>();

            //Transfer Textures 
            shaderMesh.sharedMaterial.SetTexture("_AlbedoTexture", albedoTexture);
            shaderMesh.sharedMaterial.SetTexture("_NormalTexture", normalTexture);
            shaderMesh.sharedMaterial.SetTexture("_RoughnessTexture", roughnessTexture);
            shaderMesh.sharedMaterial.SetTexture("_OcclusionTexture", occlusionTexture);
            shaderMesh.sharedMaterial.SetTexture("_SkyboxTexture", skyboxTexture);

            shaderMesh.sharedMaterial.SetInt("_MainScale", (int)transform.localScale.x / 10);

            //Transfer Lighting Extras 
            shaderMesh.sharedMaterial.SetFloat("_SpecularPower", specularPower);
            shaderMesh.sharedMaterial.SetFloat("_FresnelPower", fresnelPower);
            shaderMesh.sharedMaterial.SetFloat("_IndexOfRefraction", indexOfRefraction);

            //Transfer Lighting Style Choices
            shaderMesh.sharedMaterial.SetInt("_AmbientLightingStyle", (int)ambientLightingStyle);
            shaderMesh.sharedMaterial.SetInt("_DiffuseLightingStyle", (int)diffuseLightingStyle);
            shaderMesh.sharedMaterial.SetInt("_SpecularLightingStyle", (int)specularLightingStyle);
            shaderMesh.sharedMaterial.SetInt("_LightRedirectionStyle", (int)lightRedirectionStyle);


            for (int i = 0; i < sceneLights.Count; i++)
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

    }
}
