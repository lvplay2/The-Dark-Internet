using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Diapositivas : MonoBehaviour
{
	public delegate void Comic_Cerro();

	[Serializable]
	public class Diapositiva
	{
		public MaskableGraphic diapositiva;

		public bool disolverAnterior = true;
	}

	[Header("UI")]
	public Diapositiva[] diapositivas;

	public GameObject canvasCargando;

	[Header("Sonidos")]
	public AudioMixerGroup audioMusicaComic;

	public AudioSource golpear_Y_Abrir_Puerta;

	[Header("Configuración")]
	public bool cargarEscena;

	public bool detectarToques;

	public Comic_Cerro comic_Cerro;

	private void Start()
	{
		StartCoroutine(Diapositivas());
	}

	public void Reiniciar()
	{
		StopAllCoroutines();
		for (int i = 0; i < diapositivas.Length; i++)
		{
			diapositivas[i].diapositiva.color = new Color(1f, 1f, 1f, 0f);
		}
		StartCoroutine(Diapositivas());
	}

	private IEnumerator Diapositivas()
	{
		int diapositivaActual = 0;
		bool iniciar = true;
		float tiempoEntreDiapositivas = 0f;
		while (true)
		{
			if (((Input.GetMouseButtonDown(0) || iniciar) && detectarToques) || tiempoEntreDiapositivas > 5f)
			{
				if (diapositivaActual == diapositivas.Length)
				{
					break;
				}
				for (int i = 0; i < diapositivas.Length; i++)
				{
					if (i == diapositivaActual)
					{
						StartCoroutine(Disolver(diapositivas[diapositivaActual].diapositiva, 1f));
						diapositivaActual++;
						tiempoEntreDiapositivas = 0f;
						break;
					}
					if (diapositivas[diapositivaActual].disolverAnterior)
					{
						StartCoroutine(Disolver(diapositivas[diapositivaActual - 1].diapositiva, 0f));
					}
				}
				iniciar = false;
			}
			tiempoEntreDiapositivas += Time.deltaTime;
			yield return null;
		}
		if (cargarEscena)
		{
			StartCoroutine(CargarEscenaPrincipal());
			yield break;
		}
		Comic_Cerro obj = comic_Cerro;
		if (obj != null)
		{
			obj();
		}
	}

	private IEnumerator Disolver(MaskableGraphic ui, float alfa, float tiempo = 2f)
	{
		Color colorInicial = ui.color;
		float t = 0f;
		while (t < 1f)
		{
			ui.color = Color.Lerp(colorInicial, new Color(1f, 1f, 1f, alfa), t);
			t += Time.deltaTime / tiempo;
			yield return null;
		}
	}

	private IEnumerator CargarEscenaPrincipal()
	{
		for (int i = 0; i < diapositivas.Length - 1; i++)
		{
			StartCoroutine(Disolver(diapositivas[i].diapositiva, 0f, 1.2f));
		}
		audioMusicaComic.audioMixer.SetFloat("MusicaComic_Volumen", -7f);
		yield return new WaitForSeconds(0.5f);
		golpear_Y_Abrir_Puerta.Play();
		yield return new WaitForSeconds(1f);
		StartCoroutine(Disolver(diapositivas[diapositivas.Length - 1].diapositiva, 0f, 1f));
		canvasCargando.SetActive(true);
		yield return new WaitForSeconds(2.2f);
		UnityEngine.Object.FindObjectOfType<ES_EscenaCargando>().CargarEscenaAsyncronica("EscenaJuego");
	}
}
