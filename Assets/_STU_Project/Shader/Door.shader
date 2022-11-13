Shader "Unlit/Door"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex("Main Texture", 2D) = "white" {}
        _AlphaTex("Alpha Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white"  {}
        _Mask("no Noise Mask",2D) = "white"{}
        _Brightness("Brightness", Range(0,20)) = 1
        _SPeedX("SPeedX",FLOAT) = 0.5
        _SPeedY("SPeedY",FLOAT) = 0.5
        _GridentTex("Color Grident Map",2D) = "white"{}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "DisableBatching" = "True" }
        LOD 100

        Pass
        {
            Cull Off
            Lighting Off
            ZWrite Off
            Blend One OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _AlphaTex;
                float4 _AlphaTex_ST;
                sampler2D _NoiseTex;
                float4 _NoiseTex_ST;
                sampler2D _Mask;
                float4 _Mask_ST;
                sampler2D _GridentTex;
                float _SPeedX;
                float _SPeedY;
                float _Brightness;
                fixed4 _Color;



                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    fixed4 color : COLOR;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float2 uv2 : TEXCOORD2;
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;

                    UNITY_VERTEX_OUTPUT_STEREO
                };


                v2f o;

                v2f vert(appdata v)
                {

                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.uv2 = TRANSFORM_TEX(v.uv, _NoiseTex);


                    return o;
                }


                fixed4 frag(v2f IN) : SV_Target
                {
                    fixed4 maintex = tex2D(_MainTex, IN.uv);

                float2 noiseUV = IN.uv2;
                noiseUV += float2(-(_Time.y * _SPeedX * 0.2), -(_Time.y * _SPeedY * 0.2));
                float noise = tex2D(_NoiseTex, noiseUV).r;
                 noise -= 0.5;


                
                fixed mask = tex2D(_Mask, IN.uv).r; 
                float discontinueMask = lerp(noise, 0, mask);
                                
                fixed4 alphatex = tex2D(_AlphaTex, float2 (IN.uv.x + discontinueMask , IN.uv.y + discontinueMask));
                
                float MaskPart = mask.r * alphatex;

                float4 gridenttex = tex2D(_GridentTex, float2(MaskPart, IN.uv.y));

                
                maintex.rgb *= MaskPart * alphatex * _Color;
                fixed4 mulColor = MaskPart * _Color * _Brightness * (alphatex.r + IN.color.a * _Color.a) * gridenttex + MaskPart * _Brightness * 0.5;
                half4 col = saturate((maintex * (1 - saturate(mulColor.a)) * IN.color * IN.color.a) + mulColor * 0.5);


                return col;
            }
            ENDCG
        }
    }
}
