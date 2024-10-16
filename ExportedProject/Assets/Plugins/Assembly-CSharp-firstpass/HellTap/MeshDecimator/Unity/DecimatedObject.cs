using UnityEngine;
using UnityEngine.Rendering;

namespace HellTap.MeshDecimator.Unity
{
	public sealed class DecimatedObject : MonoBehaviour
	{
		[SerializeField]
		private LODSettings[] levels;

		[SerializeField]
		private bool generated;

		public LODSettings[] Levels
		{
			get
			{
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

		private void Reset()
		{
			levels = new LODSettings[3]
			{
				new LODSettings(0.8f, 0.5f, SkinQuality.Auto, true, ShadowCastingMode.On),
				new LODSettings(0.65f, 0.16f, SkinQuality.Bone2, true, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, false),
				new LODSettings(0.4f, 0.07f, SkinQuality.Bone1, false, ShadowCastingMode.Off, MotionVectorGenerationMode.Object, false)
			};
			ResetLODs();
		}

		public void GenerateLODs(LODStatusReportCallback statusCallback = null)
		{
			if (levels != null)
			{
				LODGenerator.GenerateLODs(base.gameObject, levels, statusCallback);
			}
			generated = true;
		}

		public void ResetLODs()
		{
			LODGenerator.DestroyLODs(base.gameObject);
			generated = false;
		}
	}
}
