Shader "Custom/Grid" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_LineColor ("Line Color", Color) = (.3, .3, .3, 1)
		_LineWidth ("Line Width", Range(0, 1)) = 0.1
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_GeometrySize ("Geometry Size", Vector) = (1, 1, 0, 0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _LineColor;
		float _LineWidth;
		fixed4 _GeometrySize;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c;
			fixed2 geomCoords = (IN.uv_MainTex.x * _GeometrySize.r,
								 IN.uv_MainTex.y * _GeometrySize.g);
			int2 intGeomCoords = (int(geomCoords.x), 
								  int(geomCoords.y));
//			if((intGeomCoords.x % 10 == 0 && (geomCoords.x - intGeomCoords.x) < _LineWidth)
//				|| intGeomCoords.y % 10 == 0 && (geomCoords.y - intGeomCoords.y) < _LineWidth) {
			if(((geomCoords.x - intGeomCoords.x) < _LineWidth)
				|| (geomCoords.y - intGeomCoords.y) < _LineWidth) {
				c = tex2D (_MainTex, IN.uv_MainTex) * _LineColor;
//			if(IN.uv_MainTex.x == 1 || IN.uv_MainTex.x == 0 || IN.uv_MainTex.y == 1 || IN.uv_MainTex.y == 0) {
//			fixed4	c = fixed4 (100, 100, 100, 0);
//				c.r -= .3;
//				c.g -= .3;
//				c.b -= .3;
			} else {
				c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			}
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
