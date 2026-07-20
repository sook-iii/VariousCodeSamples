// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/FFTShader"
{

    //UNITY SETUP


    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _WaveAmplitude ("Wave Amplitude", Float) = 1
        _WaveSpeed ("Wave Speed", Float) = 1
        _OceanSize ("Ocean Size", Float) = 1000
        _WindDirection ("Wind Direction", Vector) = (0, 0, 0, 0)
        _WindSpeed ("Wind Speed", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {


            //SHADER SETUP


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"


            //VARIABLE SETUP


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float4 test : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float _WaveAmplitude; 
            float _WaveSpeed; 
            float _OceanSize;
            float4 _WindDirection;
            float _WindSpeed;


            //FUNCTIONS


            float2 normalize(float2 InVector) { 

                float magnitude = pow(pow(InVector.x, 2.f) + pow(InVector.y, 2.f), 0.5f);
                return float2(InVector.x / magnitude, InVector.y / magnitude);

            }

            float magnitude(float3 InVector) { 

                return pow(pow(InVector.x, 2.f) + pow(InVector.y, 2.f) + pow(InVector.z, 2.f), 0.5f);

            }

            float magnitude(float4 InVector) { 

                return pow(pow(InVector.x, 2.f) + pow(InVector.y, 2.f) + pow(InVector.z, 2.f) + pow(InVector.w, 2.f), 0.5f);

            }


            //formulae from https://barthpaleologue.github.io/Blog/posts/ocean-simulation-webgpu/, implemented myself
            float2 PhillipsSpectrum (float initialVariables)
            {
                
                //k = Wdirection X Wfrequency
                //P(k) = A * ((exp(-1/(kL)^2) / (k^4))*|k? . w?|^2
                //k? = k/|k|
                //w? = w/|w|

                float2 usingWindDir = normalize(float2(_WindDirection.x, _WindDirection.y));

                float2 k = usingWindDir * initialVariables;

                usingWindDir *= _WindSpeed;

                return _WaveAmplitude * ((exp(-1.f / pow(k * _OceanSize, 2.f)) / pow(k, 4.f))) * pow(dot(k / abs(k), usingWindDir / abs(usingWindDir)), 2.f);
                
                
            }

            //VERTEX PROGRAM


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex) + _WaveAmplitude * (float4(0, sin(_Time.w * _WaveSpeed + (v.vertex.x + v.vertex.z)), 0, 0));
                //o.vertex = UnityObjectToClipPos(v.vertex); //+ PhillipsSpectrum(v.vertex)
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.test = float4(PhillipsSpectrum(magnitude(o.worldPos)), 0, 1);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


            //FRAGMENT PROGRAM


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col = i.test;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
