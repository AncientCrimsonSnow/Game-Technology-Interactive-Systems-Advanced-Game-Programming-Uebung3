Shader "Unlit/HandWritten"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Lightsource ("LightSource", Vector) = (1,1,1,1)
        _LightColor ("LightColor", Color) = (1,1,1,1)
        _WorldPos("WorldPos", Vector) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            float4 _Color;
            float4 _Lightsource;
            float4 _LightColor;
            float4 _WorldPos;
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normals;
                return o;                
            }

            float4 frag (v2f i) : SV_Target
            {
                float3 lightDir = normalize(_Lightsource.xyz - _WorldPos.xyz);
                float3 lightColor = _LightColor.rgb;
                float lightFalloff = saturate(dot(lightDir, i.normal));
                float3 diffuseLight = lightColor * lightFalloff;
                float3 ambientLight = 0.1 * _LightColor;
                
                float4 result = float4((saturate(diffuseLight+ambientLight)+_Color.rgb)/2, 1);
                return float4(result);
            }
            ENDCG
        }
    }
}
