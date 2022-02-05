// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Screen Space Shadow AA Correction"
{
	Properties
	{
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags {"LightMode"="ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				SHADOW_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D_float _CameraDepthTexture;
			float4 _CameraDepthTexture_TexelSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				TRANSFER_SHADOW(o)
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				#ifdef SHADOWS_SCREEN

				float2 screenUV = i._ShadowCoord.xy / i._ShadowCoord.w;
				fixed shadow = tex2D(_ShadowMapTexture, screenUV).r;

				// early out, shows off "standard" screen space shadows
				if(frac(_Time.x) > 0.5)
					return shadow;

				float fragDepth = i._ShadowCoord.z / i._ShadowCoord.w;
				float depth_raw = tex2D(_CameraDepthTexture, screenUV).r;

				float depthDiff = abs(fragDepth - depth_raw);
				float diffTest = 1.0 / 100000.0;

				if (depthDiff > diffTest)
				{
					float2 texelSize = _CameraDepthTexture_TexelSize.xy;
					float4 offsetDepths = 0;

					float2 uvOffsets[5] = {
						float2(1.0, 0.0) * texelSize,
						float2(-1.0, 0.0) * texelSize,
						float2(0.0, 1.0) * texelSize,
						float2(0.0, -1.0) * texelSize,
						float2(0.0, 0.0)
					};

					offsetDepths.x = tex2D(_CameraDepthTexture, screenUV + uvOffsets[0]).r;
					offsetDepths.y = tex2D(_CameraDepthTexture, screenUV + uvOffsets[1]).r;
					offsetDepths.z = tex2D(_CameraDepthTexture, screenUV + uvOffsets[2]).r;
					offsetDepths.w = tex2D(_CameraDepthTexture, screenUV + uvOffsets[3]).r;

					float4 offsetDiffs = abs(fragDepth - offsetDepths);

					float diffs[4] = {offsetDiffs.x, offsetDiffs.y, offsetDiffs.z, offsetDiffs.w};

					int lowest = 4;
					float tempDiff = depthDiff;
					for (int i=0; i<4; i++)
					{
						if(diffs[i] < tempDiff)
						{
							tempDiff = diffs[i];
							lowest = i;
						}
					}

					shadow = tex2D(_ShadowMapTexture, screenUV + uvOffsets[lowest]).r;
				}

				return shadow;

				#endif //SHADOWS_SCREEN

				return 1;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
