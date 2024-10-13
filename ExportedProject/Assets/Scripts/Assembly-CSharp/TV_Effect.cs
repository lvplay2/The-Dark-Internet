using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/TV Effect")]
public class TV_Effect : MonoBehaviour
{
	public Material _TV_Material;

	[Range(0f, -16f)]
	public float _hardScan;

	[Range(16f, 1f)]
	public float _resolution;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		_TV_Material.shader = Shader.Find("Hidden/TV");
		_TV_Material.SetFloat("hardScan", _hardScan);
		_TV_Material.SetFloat("resScale", _resolution);
		Graphics.Blit(source, destination, _TV_Material);
	}
}
