Shader "Custom/Mandelbrot"{
	Properties{
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		[NoScaleOffset] _Color("Color Texture", 2D) = "white" {}
		_maxIter("Max Iterations", Float) = 512
		_aspectRatio("Aspect Ratio", Float) = 1
		_ST("ScaleOffset", Vector) = (2.5, 2.5, -0.5, 0.0)
	}
	SubShader{
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _Color;
			float4 _ST;
			float _maxIter;
			float _aspectRatio;

			v2f vert(appdata v){
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = (v.uv - 0.5) * _ST.xy + _ST.zw;
				o.uv.x *= _aspectRatio;
				return o;
			}

			float4 mandelbrot(float cR, float cI){
				float zR = cR, zI = cI; int iteration;
				for (iteration = 0; iteration < _maxIter; iteration++){
					float tempZR = zR * zR - zI * zI;
					float tempZI = 2.0f * zR * zI;
					zR = tempZR + cR;
					zI = tempZI + cI;
					if (zR * zR + zI * zI > 4) break;
				}
				float4 color = 0;
				if (iteration < _maxIter){
					color = tex2D(_Color, float2((iteration / (float)_maxIter) * (_maxIter*0.01) + _Time.x, 0));
				}
				return color;
			}

			float4 frag(v2f i) : SV_Target{
				return mandelbrot(i.uv.x, i.uv.y);
			}
			ENDCG
		}
	}
}
