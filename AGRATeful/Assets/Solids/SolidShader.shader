Shader "Unlit/SolidShader"
{


    //initial pseudocode of design goals

    //Albedo Map
    //Normal Map
    //Roughness Map
    //Occlusion Map

    //Base Textures
    //Detail Textures
    
    //Directional Lights
    //Point Lights
    //Spot Lights

    //Ambient Lighting - off, flat, cubemap
    //Diffuse Lighting - off, blinn, orennayar
    //Specular Lighting - off, blinn, phong, cooktorrence

    //Offset Mapping? 
    //Reflection, Refraction, Fresnel Term?
    //Vertex Painting, Animation?

    //UNITY SETUP


    Properties
    {

        //This is a bulky list, but it shouldn't be exposed directly. See "CustomSolidHelper", "CustomLightComponent", and "CustomSphereComponent" for how to break this up to the developer
        
        //BASIC

        /*[HideInInspector]*/ _AlbedoTexture ("[EXTERNALISED] Albedo Texture", 2D) = "white" {}
        /*[HideInInspector]*/ [Normal] _NormalTexture ("[EXTERNALISED] Normal Texture", 2D) = "white" {}
        /*[HideInInspector]*/ _RoughnessTexture ("[EXTERNALISED] Roughness Texture", 2D) = "white" {}
        /*[HideInInspector]*/ _OcclusionTexture ("[EXTERNALISED] Occlusion Texture", 2D) = "white" {}
        /*[HideInInspector]*/ [Cube] _SkyboxTexture ("[EXTERNALISED] Skybox Texture", Cube) = "white" {}

        /*[HideInInspector]*/ _SolidColour ("[EXTERNALISED] Solid Colour", Color) = (0, 0, 1, 1)
        /*[HideInInspector]*/ _MainScale ("[EXTERNALISED] Main Scale", float) = 1
          
        /*[HideInInspector]*/ _SpecularPower ("[EXTERNALISED] Specular Power", float) = 1 
        /*[HideInInspector]*/ _FresnelPower ("[EXTERNALISED] Fresnel Power", float) = 1
        /*[HideInInspector]*/ _IndexOfRefraction ("[EXTERNALISED] Index of Refraction", float) = 1

        //LIGHTS 

        /*[HideInInspector]*/ _AmbientLightingStyle ("[EXTERNALISED] Ambient Lighting Style", Range(0,3)) = 0       //None, Flat, CubeMap
        /*[HideInInspector]*/ _DiffuseLightingStyle ("[EXTERNALISED] Diffuse Lighting Style", Range(0,3)) = 0       //None, Blinn, OrenNayar
        /*[HideInInspector]*/ _SpecularLightingStyle ("[EXTERNALISED] Specular Lighting Style", Range(0,3)) = 0     //None, Blinn, Phong, CookTorrence
        /*[HideInInspector]*/ _LightRedirectionStyle ("[EXTERNALISED] Light Redirection Style", Range(0,3)) = 0     //None, Reflect, Refract, Both

        /*[HideInInspector]*/ _Light1Position ("[EXTERNALISED] Light 1 Position", Vector) = (0,0,0,0) 
        /*[HideInInspector]*/ _Light1Direction ("[EXTERNALISED] Light 1 Direction", Vector) = (0,0,1,0) 
        /*[HideInInspector]*/ _Light1Type ("[EXTERNALISED] Light 1 Type", int) = 0                                  //Directional, Point, Spot
        /*[HideInInspector]*/ _Light1Intensity ("[EXTERNALISED] Light 1 Intensity", float) = 1
        /*[HideInInspector]*/ _Light1SpotlightLimit ("[EXTERNALISED] Light 1 Spotlight Limit", float) = 1
        /*[HideInInspector]*/ _AmbientLight1Colour ("[EXTERNALISED] Ambient Light 1 Colour", Color) = (1,1,1,1) 
        /*[HideInInspector]*/ _DiffuseLight1Colour ("[EXTERNALISED] Diffuse Light 1 Colour", Color) = (1,1,1,1) 
        /*[HideInInspector]*/ _SpecularLight1Colour ("[EXTERNALISED] Specular Light 1 Colour", Color) = (1,1,1,1) 

        /*[HideInInspector]*/ _Light2Position ("[EXTERNALISED] Light 2 Position", Vector) = (0,0,0,0) 
        /*[HideInInspector]*/ _Light2Direction ("[EXTERNALISED] Light 2 Direction", Vector) = (0,0,1,0) 
        /*[HideInInspector]*/ _Light2Type ("[EXTERNALISED] Light 2 Type", int) = 0                                  //Directional, Point, Spot
        /*[HideInInspector]*/ _Light2Intensity ("[EXTERNALISED] Light 2 Intensity", float) = 1
        /*[HideInInspector]*/ _Light2SpotlightLimit ("[EXTERNALISED] Light 2 Spotlight Limit", float) = 1
        /*[HideInInspector]*/ _AmbientLight2Colour ("[EXTERNALISED] Ambient Light 2 Colour", Color) = (1,1,1,1) 
        /*[HideInInspector]*/ _DiffuseLight2Colour ("[EXTERNALISED] Diffuse Light 2 Colour", Color) = (1,1,1,1) 
        /*[HideInInspector]*/ _SpecularLight2Colour ("[EXTERNALISED] Specular Light 2 Colour", Color) = (1,1,1,1) 

        /*[HideInInspector]*/ _Light3Position ("[EXTERNALISED] Light 3 Position", Vector) = (0,0,0,0) 
        /*[HideInInspector]*/ _Light3Direction ("[EXTERNALISED] Light 3 Direction", Vector) = (0,0,1,0) 
        /*[HideInInspector]*/ _Light3Type ("[EXTERNALISED] Light 3 Type", int) = 0                                  //Directional, Point, Spot
        /*[HideInInspector]*/ _Light3Intensity ("[EXTERNALISED] Light 3 Intensity", float) = 1
        /*[HideInInspector]*/ _Light3SpotlightLimit ("[EXTERNALISED] Light 3 Spotlight Limit", float) = 1
        /*[HideInInspector]*/ _AmbientLight3Colour ("[EXTERNALISED] Ambient Light 3 Colour", Color) = (1,1,1,1) 
        /*[HideInInspector]*/ _DiffuseLight3Colour ("[EXTERNALISED] Diffuse Light 3 Colour", Color) = (1,1,1,1) 
        /*[HideInInspector]*/ _SpecularLight3Colour ("[EXTERNALISED] Specular Light 3 Colour", Color) = (1,1,1,1) 
        
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
             
        Pass
        {                      

            //SHADER INTERNALS
        

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"


            //CUSTOM VARIABLES


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3x3 tangentSpace : TEXCOORD2; //& TEXCOORD3, & TEXCOORD4
                float3 worldPos : TEXCOORD5;
                float3 cameraDir : TEXCOORD6;
            };

            struct GerstnerWaveData
            {
                float3 position;
                float3 tangent;
                float3 binormal;

            };

            sampler2D _AlbedoTexture;
            sampler2D _NormalTexture;
            sampler2D _RoughnessTexture;
            sampler2D _OcclusionTexture;
            samplerCUBE _SkyboxTexture;

            float4 _AlbedoTexture_ST;
            float4 _NormalTexture_ST;
            float4 _RoughnessTexture_ST;
            float4 _OcclusionTexture_ST;
            float4 _SkyboxTexture_ST;

            float4 _SolidColour;

            int _AmbientLightingStyle;
            int _DiffuseLightingStyle;
            int _SpecularLightingStyle;
            int _LightRedirectionStyle;

            float3 _Light1Position;  
            float3 _Light1Direction; 
            int _Light1Type; 
            float _Light1Intensity; 
            float _Light1SpotlightLimit; 
            float4 _AmbientLight1Colour; 
            float4 _DiffuseLight1Colour; 
            float4 _SpecularLight1Colour; 
            
            float3 _Light2Position;  
            float3 _Light2Direction; 
            int _Light2Type; 
            float _Light2Intensity; 
            float _Light2SpotlightLimit; 
            float4 _AmbientLight2Colour; 
            float4 _DiffuseLight2Colour; 
            float4 _SpecularLight2Colour; 
            
            float3 _Light3Position;  
            float3 _Light3Direction; 
            int _Light3Type; 
            float _Light3Intensity; 
            float _Light3SpotlightLimit; 
            float4 _AmbientLight3Colour; 
            float4 _DiffuseLight3Colour; 
            float4 _SpecularLight3Colour; 
                        
            float _SpecularPower; 
            float _FresnelPower; 
            float _IndexOfRefraction;


            //MATHS API FUNCTIONS


            //AMBIENT LIGHTING


            //standard ambient model - very simple
            float3 CalculateLighting_AmbientFlat(float4 usingLightColour) { 

                return (usingLightColour.xyz * usingLightColour.w);

            }

            float3 CalculateLighting_AmbientCubeMap(v2f VertexIn, float4 usingLightColour) { 

	            float3 reflectVector = reflect(VertexIn.cameraDir, VertexIn.normal);
	            float3 reflectColor = texCUBE(_SkyboxTexture, -reflectVector).xyz; 

	            float3 refractVector = refract(VertexIn.cameraDir, -VertexIn.normal, (1.0f / _IndexOfRefraction)).xyz;
                float3 refractColor = texCUBE(_SkyboxTexture, -refractVector).xyz;

	            float fresnelTerm = pow(1 - dot(VertexIn.cameraDir, VertexIn.normal), _FresnelPower);
                                
                switch(_LightRedirectionStyle) { 
                    case 1: return reflectColor * (usingLightColour.xyz * usingLightColour.w);
                    case 2: return refractColor * (usingLightColour.xyz * usingLightColour.w); 
                    case 3: return lerp(reflectColor, refractColor, fresnelTerm) * (usingLightColour.xyz * usingLightColour.w); 
                }
                
                return fresnelTerm; 

            }


            //DIFFUSE LIGHTING

                    
            //standard diffuse model - lit only with the similarity in direction between the normal and the light
            float3 CalculateLighting_DiffuseBlinn(v2f VertexIn, float4 usingLightColour, float3 usingNormal, float3 usingLightDir) { 
            
                float dotProduct = dot(usingNormal, usingLightDir); //1 when identical, 0 when tangent, -1 when opposite

                float diffuseBrightness = (dotProduct + 1) / 2; //1 when identical, 0.5 when tangent, 0 when opposite

                return diffuseBrightness * (usingLightColour.xyz * usingLightColour.w); 

            }

            //alternative diffuse model prioritising reflection on rough surfaces, code credit largely owed to ben cloward's HLSL tutorials
            float3 CalculateLighting_DiffuseOrenNayar(v2f VertexIn, float4 usingLightColour, float3 usingLightDir) { 

	            float pi = 3.14159265;

	            float roughness_g = 0;
	            float albedo_p = _SolidColour;
	            float intensity_e = 1.0f;


	            //float phiTerm = cos(usingLightDir - VertexIn.cameraDirection); //cos doesn't work with float3s and it needs to!
                float phiTerm = dot(usingLightDir, VertexIn.cameraDir) / (normalize(usingLightDir) * normalize(VertexIn.cameraDir)); //https://www.geeksforgeeks.org/python/how-to-calculate-cosine-similarity-in-python/
                
	            float NdotV = dot(VertexIn.normal, VertexIn.cameraDir);
	            float NdotL = dot(VertexIn.normal, usingLightDir);

	            float theta_r = acos(NdotV);
	            float theta_i = -acos(NdotL);

	            float alpha = max(theta_i, theta_r);
	            float beta = min(theta_i, theta_r);

	            float C1 = 1.0f - (0.5f * (roughness_g / (roughness_g + 0.33f)));
	            float C2; 

	            if (phiTerm >= 0) 
	            {
		            C2 = 0.45f * (roughness_g / (roughness_g + 0.09f)) * sin(alpha); //if cos (qi - qr) >= 0
	            }

	            else 
	            {
		            C2 = 0.45f * (roughness_g / (roughness_g + 0.09f)) * (sin(alpha) - ((2.0f * beta / pi) * (2.0f * beta / pi) * (2.0f * beta / pi))); //else
	            }

	            float C3 = 0.125f *  (roughness_g / (roughness_g + 0.09f)) * (((4 * alpha * beta) / (pi * pi)) * ((4.0f * alpha * beta) / (pi * pi)));
	            float L1 = (albedo_p/pi) * intensity_e * cos(theta_i) * (C1 + (C2 * phiTerm * tan(beta)) + (C3 * (1.0f - abs(phiTerm)) * tan((alpha + beta) / 2.0f)));
	            float L2 = 0.17f * ((albedo_p * albedo_p) / pi) * intensity_e * cos(theta_i) * (roughness_g / (roughness_g + 0.13f)) * (1 - phiTerm * ((2.0f * beta / pi) * (2.0f * beta / pi)));
	            float LR = L1 + L2;

	            return LR * (usingLightColour.xyz * usingLightColour.w);
	
            }


            //SPECULAR LIGHTING


            //standard specular model - paint a lit, gradiented circle when the camera looks at the light against the object
            float3 CalculateLighting_SpecularBlinn(v2f VertexIn, float4 usingLightColour, float3 usingNormal, float3 usingLightDir) { 
                
                float3 halfDir = normalize(usingLightDir + VertexIn.cameraDir);

                float dotProduct = saturate(dot(usingNormal, halfDir)); 

                float specularBrightness = pow(dotProduct, _SpecularPower);

                return specularBrightness * (usingLightColour.xyz * usingLightColour.w);

            }

            //alternative lighting model implementing reflection to simulate metallic surfaces
            float3 CalculateLighting_SpecularPhong(v2f VertexIn, float4 usingLightColour) { 
                
                //calculate the direction that light would more realistically reflect off the surface
	            float3 reflectVector = reflect(usingLightColour, VertexIn.normal);
	
	            //use the dot product to see how similar this is to the camera's looking angle
	            float directionalSimilarity = saturate(dot(-reflectVector, VertexIn.cameraDir));
	
	            //use an exponent to simulate a more realistic falloff, then use this colour for whatever is needed!	
	            return pow(directionalSimilarity, _SpecularPower) * (usingLightColour.xyz * usingLightColour.w);

            }

            float3 CalculateLighting_SpecularCookTorrence(float4 usingLightColour) { 
                
                return usingLightColour.xyz * usingLightColour.w; //not implementing

            }


            //LIGHTING HELPER FUNCTIONS


            float3 CalculateLighting_Overall(v2f VertexIn, float3 usingNormal, float3 inputPosition, float3 inputDirection, int lightType, float lightIntensity, float lightSpotlightLimit, float4 usingAmbientLightColour, float4 usingDiffuseLightColour, float4 usingSpecularLightColour) {

                float3 usingDirection = inputDirection;

                if (lightType > 0) 
	            { 
		            usingDirection = normalize(inputPosition - VertexIn.worldPos);
                }

                float3 lightColour = float3(0,0,0);

                //light via selected models
                switch(_AmbientLightingStyle) { 
                    case 1: lightColour += CalculateLighting_AmbientFlat(usingAmbientLightColour); break;
                    case 2: lightColour += CalculateLighting_AmbientCubeMap(VertexIn, usingAmbientLightColour); break;   
                }

                switch(_DiffuseLightingStyle) { 
                    case 1: lightColour += CalculateLighting_DiffuseBlinn(VertexIn, usingDiffuseLightColour, usingNormal, usingDirection); break;
                    case 2: lightColour += CalculateLighting_DiffuseOrenNayar(VertexIn, usingDiffuseLightColour, usingDirection); break;   
                }

                switch(_SpecularLightingStyle) { 
                    case 1: lightColour += CalculateLighting_SpecularBlinn(VertexIn, usingSpecularLightColour, usingNormal, usingDirection); break;
                    case 2: lightColour += CalculateLighting_SpecularPhong(VertexIn, usingSpecularLightColour); break;   
                    case 3: lightColour += CalculateLighting_SpecularCookTorrence(usingSpecularLightColour); break;   
                }                

                //attenuation for omni and spot lights
	            if (lightType > 0) {

		            lightColour *= (1.0f / pow(length(inputPosition - VertexIn.worldPos), 2.0f)) * lightIntensity;

	            }	

                else {
                    lightColour *= clamp(lightIntensity, 0, 1);
                }

	            //cone limiters for spot lights
	            if (lightType == 2) {

		            float coneLimiter = dot(usingDirection, inputDirection);
		            coneLimiter = saturate(coneLimiter);
		            coneLimiter = coneLimiter - (1.0f - (lightSpotlightLimit / 180.0f));
		            coneLimiter = saturate(coneLimiter);
		            coneLimiter /= (lightSpotlightLimit / 180.0f);
		            lightColour *= coneLimiter;

	            }

                return float3(clamp(lightColour.x, 0, 1), clamp(lightColour.y, 0, 1), clamp(lightColour.z, 0, 1));

            }

            //use multiple lights instead of just one by putting a sum of the three into the relevant maths!
            float3 CombineMultipleLights(v2f VertexIn, float3 usingNormal) { 

                float3 returningColour = float3(0,0,0);

                returningColour += CalculateLighting_Overall(VertexIn, usingNormal, _Light1Position.xyz, _Light1Direction.xyz, _Light1Type, _Light1Intensity, _Light1SpotlightLimit, _AmbientLight1Colour, _DiffuseLight1Colour, _SpecularLight1Colour);
                returningColour += CalculateLighting_Overall(VertexIn, usingNormal, _Light2Position.xyz, _Light2Direction.xyz, _Light2Type, _Light2Intensity, _Light2SpotlightLimit, _AmbientLight2Colour, _DiffuseLight2Colour, _SpecularLight2Colour);
                returningColour += CalculateLighting_Overall(VertexIn, usingNormal, _Light3Position.xyz, _Light3Direction.xyz, _Light3Type, _Light3Intensity, _Light3SpotlightLimit, _AmbientLight3Colour, _DiffuseLight3Colour, _SpecularLight3Colour);

                return returningColour;

            }


            //VERTEX CORE PROGRAM


            v2f vert (appdata AppIn)
            {
                v2f VertexOut;
                VertexOut.vertex = UnityObjectToClipPos(AppIn.vertex);

                float3 worldPos = mul(unity_ObjectToWorld, AppIn.vertex);                                                           //https://halisavakis.com/shader-bits-world-and-screen-space-position/
                VertexOut.worldPos = worldPos;

                //VertexOut.cameraDir = WorldSpaceViewDir(AppIn.vertex);
	            VertexOut.cameraDir = normalize(WorldSpaceViewDir(AppIn.vertex) - worldPos);

                VertexOut.uv = TRANSFORM_TEX(AppIn.uv, _AlbedoTexture);

                VertexOut.normal = UnityObjectToWorldNormal(AppIn.normal);
                float3 tangent = UnityObjectToWorldDir(AppIn.tangent.xyz);
                float3 tangentSign = AppIn.tangent.w * unity_WorldTransformParams.w;    //https://docs.unity3d.com/Manual/built-in-shader-examples-environment-reflections.html
                float3 biSpace = cross(VertexOut.normal, tangent) * tangentSign;

                VertexOut.tangentSpace = float3x3(float3(tangent.x, biSpace.x, VertexOut.normal.x),float3(tangent.y, biSpace.y, VertexOut.normal.y),float3(tangent.z, biSpace.z, VertexOut.normal.z));

                UNITY_TRANSFER_FOG(VertexOut, VertexOut.vertex);
                return VertexOut;
            }
                                 

            //FRAGMENT CORE PROGRAM


            fixed4 frag (v2f VertexIn) : SV_Target
            {

                // sample the texture
                float4 albedoMapValue = tex2D(_AlbedoTexture, VertexIn.uv);
                float3 normalMapValue = UnpackNormal(tex2D(_NormalTexture, VertexIn.uv));
                float4 roughnessMapValue = tex2D(_RoughnessTexture, VertexIn.uv);
                float4 occlusionMapValue = tex2D(_OcclusionTexture, VertexIn.uv);

                //fixed4 col = tex2D(_MainTex, i.uv);
                float4 baseColour = _SolidColour * albedoMapValue;
                
                //float3 usingNormal = VertexIn.normal;
                float3 usingNormal = float3(dot(VertexIn.tangentSpace[0], normalMapValue), dot(VertexIn.tangentSpace[1], normalMapValue), dot(VertexIn.tangentSpace[2], normalMapValue));
              

                float3 lightColour = CombineMultipleLights(VertexIn, usingNormal);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                //return float4((usingNormal * 2) - 1, 1);
                return baseColour * float4(lightColour, 1);
            }
            ENDCG
        }
    }
}
