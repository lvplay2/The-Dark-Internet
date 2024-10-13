using UnityEngine;
using UnityEngine.UI;

public class ST_Capturador : MonoBehaviour
{
	public Camera camara;

	public RawImage imagen;

	private int ancho = Screen.width;

	private int alto = Screen.height;

	private void Update()
	{
		RenderTexture renderTexture = new RenderTexture(ancho, alto, 0);
		Texture2D texture2D = new Texture2D(ancho, alto, TextureFormat.RGB24, false);
		camara.targetTexture = renderTexture;
		camara.Render();
		RenderTexture.active = renderTexture;
		imagen.texture = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, ancho, alto), 0, 0);
		camara.targetTexture = null;
		RenderTexture.active = null;
		Object.Destroy(renderTexture);
	}
}
