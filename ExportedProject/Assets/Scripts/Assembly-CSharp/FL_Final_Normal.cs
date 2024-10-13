using UnityEngine;
using UnityEngine.Audio;

public class FL_Final_Normal : MonoBehaviour
{
	public Camera camara;

	public AudioSource vidrioRompiendose;

	public AudioSource corriendoConcreto;

	public AudioSource boom;

	public AudioSource boomInstantaneo;

	public AudioSource vozFinal;

	public AudioSource suspenso;

	public AudioMixerGroup audioMixer;

	public GameObject enemigo;

	public GameObject ventanaSana;

	public GameObject ventanaRota;

	public GameObject canvas;

	public Transform puerta;

	private bool _reducirPasos;

	private void Update()
	{
		if (_reducirPasos)
		{
			corriendoConcreto.volume = Mathf.Lerp(corriendoConcreto.volume, 0f, 0.5f * Time.deltaTime);
		}
	}

	public void Escapar()
	{
		vidrioRompiendose.Play();
		audioMixer.audioMixer.SetFloat("EscenaGanar_Normal_Volumen", -60f);
		camara.cullingMask = 0;
	}

	public void Boom()
	{
		boom.Play();
	}

	public void CambiarToma()
	{
		puerta.localEulerAngles = new Vector3(-90f, 0f, 23f);
		enemigo.SetActive(true);
		ventanaSana.SetActive(false);
		ventanaRota.SetActive(true);
		camara.cullingMask = 1;
	}

	public void ReproducirCorriendoConcreto()
	{
		corriendoConcreto.Play();
		_reducirPasos = true;
	}

	public void ReproducirVozFinal()
	{
		vozFinal.Play();
	}

	public void ReproducirSuspenso()
	{
		suspenso.Play();
	}

	public void ActivarCanvas()
	{
		canvas.SetActive(true);
		boomInstantaneo.Play();
		Invoke("RedireccionarEscenaMenu", 5f);
	}

	private void RedireccionarEscenaMenu()
	{
		audioMixer.audioMixer.SetFloat("EscenaGanar_Normal_Volumen", -7f);
		Object.FindObjectOfType<ES_EscenaCargando>().CargarEscenaAsyncronica("EscenaMenu");
	}
}
