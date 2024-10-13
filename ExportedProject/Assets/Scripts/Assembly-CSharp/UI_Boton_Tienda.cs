using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class UI_Boton_Tienda : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Header("Referencias")]
	public CanvasGroup tienda;

	public Camera camaraDesactivar;

	public AudioMixerGroup audioMixerGroup;

	private bool _activada;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!_activada)
		{
			StartCoroutine(ActivarTienda());
		}
	}

	public void CerrarTienda()
	{
		Debug.Log("Cerrada");
		StopAllCoroutines();
		tienda.alpha = 0f;
		tienda.blocksRaycasts = false;
		_activada = false;
		camaraDesactivar.cullingMask = 1;
		SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.seleccionar_2, 0.6f);
		audioMixerGroup.audioMixer.SetFloat("MenuAmbiente_Volumen", -10f);
	}

	private IEnumerator ActivarTienda()
	{
		_activada = true;
		float tiempo = 0f;
		audioMixerGroup.audioMixer.SetFloat("MenuAmbiente_Volumen", -20f);
		while (tiempo < 1f)
		{
			tienda.alpha = Mathf.Lerp(0f, 1f, tiempo);
			tiempo += Time.deltaTime / 0.45f;
			yield return null;
		}
		camaraDesactivar.cullingMask = 0;
		tienda.blocksRaycasts = true;
	}
}
