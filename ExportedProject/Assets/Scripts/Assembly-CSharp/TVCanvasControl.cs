using UnityEngine;

public class TVCanvasControl : MonoBehaviour
{
	public TV_Effect _tv;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Resolution(float f)
	{
		_tv._resolution = f;
	}
}
