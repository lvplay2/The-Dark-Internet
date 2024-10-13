using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class UI_Ver_Comic : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Header("UI")]
	public Canvas canvasComic;

	public Canvas canvasMenu;

	public UI_Diapositivas diapositivas;

	private bool _comicActivado;

	[Header("Audio")]
	public AudioSource musicaComic;

	public AudioMixerGroup audioMixerGroup;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && _comicActivado)
		{
			Desactivar_Comic();
		}
	}

	private void Start()
	{
		UI_Diapositivas uI_Diapositivas = diapositivas;
		uI_Diapositivas.comic_Cerro = (UI_Diapositivas.Comic_Cerro)Delegate.Combine(uI_Diapositivas.comic_Cerro, new UI_Diapositivas.Comic_Cerro(Desactivar_Comic));
		diapositivas.detectarToques = false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Activar_Comic();
	}

	private void Activar_Comic()
	{
		canvasComic.enabled = true;
		canvasMenu.enabled = false;
		musicaComic.Play();
		audioMixerGroup.audioMixer.SetFloat("MenuAmbiente_Volumen", -80f);
		diapositivas.Reiniciar();
		diapositivas.detectarToques = true;
		SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.seleccionar_2, 0.8f);
		_comicActivado = true;
	}

	public void Desactivar_Comic()
	{
		canvasMenu.enabled = true;
		canvasComic.enabled = false;
		musicaComic.Stop();
		audioMixerGroup.audioMixer.SetFloat("MenuAmbiente_Volumen", -10f);
		diapositivas.detectarToques = false;
		_comicActivado = false;
	}
}
