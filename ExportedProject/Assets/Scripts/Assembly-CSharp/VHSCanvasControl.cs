using UnityEngine;

public class VHSCanvasControl : MonoBehaviour
{
	public VHS_Effect _vhs;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void NoiseTexture(float f)
	{
		_vhs._textureIntensity = f;
	}

	public void VerticalOffset(float f)
	{
		_vhs._verticalOffset = f;
	}

	public void OffsetColor(float f)
	{
		_vhs.offsetColor = f;
	}

	public void OffsetDistortion(float f)
	{
		_vhs._OffsetDistortion = f;
	}
}
