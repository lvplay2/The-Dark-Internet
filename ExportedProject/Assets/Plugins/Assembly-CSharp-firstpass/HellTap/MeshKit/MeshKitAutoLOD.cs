using HellTap.MeshDecimator.Unity;
using UnityEngine;
using UnityEngine.Rendering;

namespace HellTap.MeshKit
{
	[DisallowMultipleComponent]
	[AddComponentMenu("MeshKit/Automatic LOD")]
	public sealed class MeshKitAutoLOD : MonoBehaviour
	{
		[HideInInspector]
		public bool advancedMode;

		[HideInInspector]
		public bool preserveBorders;

		[HideInInspector]
		public bool preserveSeams;

		[HideInInspector]
		public bool preserveFoldovers;

		[HideInInspector]
		public LODSettings[] levels;

		[HideInInspector]
		[Range(0f, 99.9f)]
		public float cullingDistance = 1f;

		[HideInInspector]
		public bool generated;

		public LODSettings[] Levels
		{
			get
			{
				if (!advancedMode)
				{
					return new LODSettings[3]
					{
						new LODSettings(0.8f, 50f, SkinQuality.Auto, true, ShadowCastingMode.On),
						new LODSettings(0.65f, 16f, SkinQuality.Bone2, true, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, false),
						new LODSettings(0.4f, 7f, SkinQuality.Bone1, false, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, false)
					};
				}
				return levels;
			}
			set
			{
				levels = value;
			}
		}

		public bool IsGenerated
		{
			get
			{
				return generated;
			}
		}

		public void Reset()
		{
			levels = new LODSettings[3]
			{
				new LODSettings(0.8f, 50f, SkinQuality.Auto, true, ShadowCastingMode.On),
				new LODSettings(0.65f, 16f, SkinQuality.Bone2, true, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, false),
				new LODSettings(0.4f, 7f, SkinQuality.Bone1, false, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, false)
			};
			cullingDistance = 1f;
			ResetLODs();
		}

		public void GenerateLODs(LODStatusReportCallback statusCallback = null)
		{
			if (levels != null)
			{
				LODGenerator.GenerateLODs(base.gameObject, Levels, statusCallback, preserveBorders, preserveSeams, preserveFoldovers);
			}
			generated = true;
		}

		public void ResetLODs()
		{
			LODGenerator.DestroyLODs(base.gameObject);
			generated = false;
			advancedMode = false;
		}
	}
}
