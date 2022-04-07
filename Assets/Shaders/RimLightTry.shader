Shader "Custom/RimLightTry"
{
	Properties{
		_Color("MainColor",Color) = (1,1,1,1)
		_MainTex("Main texture", 2D) = "white" {}
		[Header(Normal)]
		_NormalMap("Normal map",2D) = "white" {}
		[KeywordEnum(Off,On)] _UseNormal("Use Normal Map",Float) = 0
		[Header(Diffuse)]
		_Diffuse("Diffuse",Range(0,5)) = 1
		[KeywordEnum(Off,Vert,Frag)] _Lighting("Lighting Mode",Float) = 0
		[Header(Specular)]
		_Specular("Specular",Range(0,1)) = 1
		_SpecularPower("SpecularPower",Range(0,200)) = 1
		_SpecularMap("Specular map",2D) = "white"{}
		[header(Ambient)]
		_Ambient("Ambient",Range(0,1)) = 1

	}

	Subshader
	{
		Tags{
			"RenderPipeline"="UniversalRenderPipeline" "Queue"="Transparent" "RenderType" = "Transparent"
		}
		Pass
		{
			Tags{"LightMode"="UniversalForward"}
			//alphaBlending
			Blend SrcAlpha OneMinusSrcAlpha
			BlendOp Add
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0;

			#pragma shader_feature _USENORMAL_ON _USENORMAL_OFF
			#pragma shader_feature _LIGHTING_OFF _LIGHTING_VERT _LIGHTING_FRAG

			#include "HLSLSupport.cginc"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			
			uniform half4 _Color;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _NormalMap;
			uniform float4 _NormalMap_ST;
			uniform float _Diffuse;
			uniform sampler2D _SpecularMap;
			uniform float _Specular;
			uniform float _SpecularPower;
			uniform float _Ambient;


			
			
			struct vertexInput
			{
				float4 vertex : POSITION; //object Space 
				float4 texcoord : TEXCOORD0;
				float4 normal : NORMAL;
				#if _USENORMAL_ON
					float4 tangent : TANGENT;
				#endif
			};
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				float4 normalWorld : TEXCOORD1;
				#if _USENORMAL_ON
					float4 tangentWorld : TEXCOORD2;
					float3 binormalWorld : TEXCOORD3;
					float4 normalTexCoord : TEXCOORD4;
				#endif
				#if _LIGHTING_VERT
					float4 surfaceColor : COLOR0;
				#endif
				#if _LIGHTING_FRAG
					float4 posWorld : TEXCOORD5;
					float3 ambientColor : COLOR1;
				#endif
			};

			half3 DiffuseLambert(float3 normalVal,float3 lightDir,half3 lightColor,float diffuseFactor,float attenuation)
			{
				return lightColor * diffuseFactor * attenuation * max(0,dot(normalVal,lightDir));
			}

			half3 RimLight(float N,float3 L,float3 V,float3 specularColor, float specular,
			float specularPower,float attenuation)
			{
				
				return specularColor * specular * attenuation * pow(abs(dot(N,V)),specularPower);

			}
			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);  //projectionSpace
				o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.normalWorld = float4(TransformObjectToWorldNormal(v.normal.xyz),v.normal.w);

				#if _USENORMAL_ON
					o.normalTexCoord.xy = TRANSFORM_TEX(v.texcoord,_NormalMap);
					o.tangentWorld = float4(normalize(mul((float3x3)unity_ObjectToWorld, v.tangent.xyz)),v.tangent.w);
					o.binormalWorld = float3(normalize(cross(o.normalWorld, o.tangentWorld)* v.tangent.w));
					o.binormalWorld *= unity_WorldTransformParams.w; //Negative scale
				#endif

				half3 ambientColor = half3(0,0,0);
				#if _LIGHTING_VERT
					half4 albedoColor = half4(1,1,1,1);
					half3 specularColor, diffuseColor;
					specularColor = diffuseColor = half3(0,0,0);

					Light light = GetMainLight();
					float3 lightDir = normalize(light.direction.xyz);
					float3 lightColor = light.color;
					float attenuation = 1;

					diffuseColor = DiffuseLambert(o.normalWorld,lightDir,lightColor,_Diffuse,attenuation);

					o.surfaceColor = float4(diffuseColor * albedoColor.rgb * _Color.rgb + specularColor + ambientColor,
					albedoColor.a * _Color.a);
				#elif _LIGHTING_FRAG

					o.posWorld = mul(unity_ObjectToWorld,v.vertex);

					o.ambientColor = _Ambient * half3(unity_SHAr.w,unity_SHAg.w,unity_SHAb.w);
					
				#endif

				return o;
				
			}
			
			
			
			half3 SpecularBlinnPhong(float N,float3 L,float3 V,float3 specularColor, float specular,
			float specularPower,float attenuation)
			{
				float3 H = normalize(L+V);
				return specularColor * specular * attenuation * pow(max(0,dot(N,H)),specularPower);

			}

			half4 frag(vertexOutput i): COLOR
			{
				half4 finalColor = half4(1,1,1,1);
				float3 normalWorldAtPixel;
				//half4 texColor = tex2D(_MainTex, i.texcoord);
				#if _USENORMAL_ON
					half4 normalColor = tex2D(_NormalMap, i.normalTexCoord);
					float3 TSNormal = normalFromColor(normalColor);
					float3x3 TBNWorld = float3x3(i.tangentWorld.x,i.binormalWorld.x,i.normalWorld.x,
					i.tangentWorld.y,i.binormalWorld.y,i.normalWorld.y,
					i.tangentWorld.z,i.binormalWorld.z,i.normalWorld.z);

					float3 WSNormalAtPixel = normalize(mul(TBNWorld,TSNormal)); 						 
					normalWorldAtPixel = WSNormalAtPixel;
				#else
					normalWorldAtPixel = i.normalWorld;
				#endif

				#if _LIGHTING_VERT
					finalColor = i.surfaceColor;
				#elif _LIGHTING_FRAG
					half4 albedoColor = half4(1,1,1,1);
					half3 specularColor, diffuseColor,ambientColor;
					specularColor = diffuseColor = ambientColor = half3(0,0,0);

					Light light = GetMainLight();
					float3 lightDir = normalize(light.direction.xyz);
					float3 lightColor = light.color;
					float attenuation = 1;

					diffuseColor = DiffuseLambert(normalWorldAtPixel,lightDir,lightColor,_Diffuse,attenuation);
					albedoColor = tex2D(_MainTex,i.texcoord);

					float3 V = normalize(_WorldSpaceCameraPos.xyz - i.posWorld);
					float3 specularMapColor = tex2D(_SpecularMap,i.texcoord);
					specularColor = RimLight(normalWorldAtPixel,lightDir,V,specularMapColor,_Specular,
					_SpecularPower,attenuation);

					ambientColor = i.ambientColor;
					float alpha = 1 - (specularColor.x + specularColor.y + specularColor.z);
					if(alpha < 0.1)
					{
						alpha = 0;
					}
					specularColor = _Color.xyz;
					return float4(specularColor.xyz,alpha);
					finalColor = float4(diffuseColor * albedoColor.rgb * _Color.rgb + specularColor + ambientColor,
					albedoColor.a * _Color.a);

				#else
					finalColor = float4(normalWorldAtPixel,1); 
				#endif
				
				return finalColor;  
			}

			ENDHLSL
		}
	}

}
