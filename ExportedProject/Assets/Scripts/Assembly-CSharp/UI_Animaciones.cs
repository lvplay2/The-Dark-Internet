using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Animaciones : MonoBehaviour
{
	public UI_EscenaJuego escenaJuego;

	private Coroutine alfaInterpolacion;

	public void Efecto_Alfa_Interpolacion(Image imagen, float transparencia, float velocidad)
	{
		if (alfaInterpolacion != null)
		{
			StopCoroutine(alfaInterpolacion);
		}
		alfaInterpolacion = StartCoroutine(Alfa_Interpolacion(imagen, transparencia, velocidad));
	}

	private IEnumerator Alfa_Interpolacion(Image imagen, float transparencia, float velocidad)
	{
		while (Mathf.Abs(transparencia - imagen.color.a) > 0.05f)
		{
			imagen.color = new Color(imagen.color.r, imagen.color.g, imagen.color.b, Mathf.Lerp(imagen.color.a, transparencia, velocidad * Time.deltaTime));
			yield return null;
		}
		imagen.color = new Color(imagen.color.r, imagen.color.g, imagen.color.b, transparencia);
		alfaInterpolacion = null;
	}
}
