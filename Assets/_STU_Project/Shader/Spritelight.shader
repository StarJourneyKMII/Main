Shader "Sprites/Spritelight"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex("MainTex", 2D) = "white" {}
        _AlphaTex("Alpha Texture(R)", 2D) = "white" {}
        _GridentTex("Color Grident Map",2D) = "white"{}
        _Brightness("Brightness", Range(0,20)) = 1
        _ColorSpeed("Color Cange Speed", Float) = 1.0
        _MaskTex("Mask Texture", 2D) = "black" {}
        _MaskSpeedX("Shift Speed X", Float) = 0.0
        _MaskSpeedY("Shift Speed Y", Float) = 1.0


    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "DisableBatching" = "True"
            }

            Pass
            {
                Cull Off
                Lighting Off
                ZWrite Off
                Fog { Mode Off }
                Blend One OneMinusSrcAlpha

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag         
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _AlphaTex;
                float4 _AlphaTex_ST;
                sampler2D _GridentTex;
                float4 _GridentTex_ST;
                sampler2D _MaskTex;
                float4 _MaskTex_ST;
                float _Brightness;
                float _ColorSpeed;
                float _MaskSpeedX;
                float _MaskSpeedY;
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
                    float2 uv2 : TEXCOORD1;
                    float2 uv3 : TEXCOORD2;
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

                    //平移 uv2
                    o.uv2 = TRANSFORM_TEX(v.uv, _MaskTex);
                    o.uv3 = TRANSFORM_TEX(v.uv, _GridentTex);
                    o.color = v.color;
                    return o;
                }


                fixed4 frag(v2f IN) : SV_Target
                {

                fixed4 maintex = tex2D(_MainTex, IN.uv);

                //漸層的uv
                float2 gridentUV = IN.uv3;
                gridentUV += float2(0, -(_Time.y * _ColorSpeed * 0.2));
                fixed4 gridenttex = tex2D(_GridentTex, gridentUV);                                               
                            
                //平移的uv
                float2 maskUV = IN.uv2;                
                maskUV += float2(-(_Time.y * _MaskSpeedX * 0.2), -(_Time.y * _MaskSpeedY * 0.2));                
                float mask = tex2D(_MaskTex, maskUV).r;//平移

                //外框線圖*MASK圖
                fixed alphatex = tex2D(_AlphaTex, IN.uv).r * mask.r;
                maintex.a = alphatex;
                maintex *= maintex.a;
                fixed MaskPart = mask.r * alphatex;

                //染色
                maintex.rgb *= MaskPart * alphatex * _Color;
                fixed4 mulColor = MaskPart * _Color * _Brightness * 2 * (alphatex.r + IN.color.a * _Color.a) * gridenttex + MaskPart * _Brightness;
                half4 col = saturate((maintex * (1 - mulColor.a) * IN.color * IN.color.a * 0.2) + mulColor);
                //*0.2可以消除白光中的那一點點顏色

                return  col;

            }
            ENDCG
        }
        }
}
