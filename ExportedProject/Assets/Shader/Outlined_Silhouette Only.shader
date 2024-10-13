Shader "Outlined/Silhouette Only" {
	Properties {
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range(0, 0.5)) = 0.005
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" }
		Pass {
			Name "BASE"
			Tags { "QUEUE" = "Transparent" }
			Blend Zero One, Zero One
			Fog {
				Mode 0
			}
			GpuProgramID 73412
			// No subprograms found
		}
		Pass {
			Name "OUTLINE"
			Tags { "LIGHTMODE" = "ALWAYS" "QUEUE" = "Transparent" }
			Blend One OneMinusDstColor, One OneMinusDstColor
			Cull Front
			GpuProgramID 39757
			// No subprograms found
		}
	}
	Fallback "Diffuse"
}