using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Observacion : MonoBehaviour
{
	private Image imagen;

	private Text texto;

	private Coroutine iniciarObservacion;

	private void Awake()
	{
		imagen = GetComponent<Image>();
		texto = GetComponentInChildren<Text>();
	}

	public void Observar(string t)
	{
		if (iniciarObservacion != null)
		{
			StopCoroutine(iniciarObservacion);
		}
		iniciarObservacion = StartCoroutine(IniciarObservacion(t));
	}

	public void DetenerObservacion()
	{
		if (iniciarObservacion != null)
		{
			StopCoroutine(iniciarObservacion);
			RestaurarTexto();
		}
	}

	private IEnumerator IniciarObservacion(string t)
	{
		yield return new WaitForSeconds(0.25f);
		imagen.enabled = true;
		texto.enabled = true;
		texto.text = t;
		yield return new WaitForSeconds(3.5f);
		RestaurarTexto();
	}

	private void RestaurarTexto()
	{
		imagen.enabled = false;
		texto.enabled = false;
		texto.text = "";
	}
}
