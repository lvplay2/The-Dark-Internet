using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ST_AudioHD : MonoBehaviour
{
	public AudioSource[] audioSources;

	public Transform jugador;

	public LayerMask layer;

	private float[] _volumenesIniciales;

	private static float _apagador = 0.5f;

	private static float _distanciaModificador = 3f;

	private void OnEnable()
	{
		Inicializar();
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		for (int i = 0; i < audioSources.Length; i++)
		{
			audioSources[i].volume = _volumenesIniciales[i];
		}
	}

	private void Inicializar()
	{
		_volumenesIniciales = new float[audioSources.Length];
		for (int i = 0; i < _volumenesIniciales.Length; i++)
		{
			_volumenesIniciales[i] = audioSources[i].volume;
		}
		StartCoroutine(RegularSonido());
	}

	private IEnumerator RegularSonido()
	{
		while (true)
		{
			for (int i = 0; i < audioSources.Length; i++)
			{
				float b = (CaminoLibre() ? (_volumenesIniciales[i] * _apagador) : _volumenesIniciales[i]);
				audioSources[i].volume = Mathf.Lerp(audioSources[i].volume, b, 5f * Time.deltaTime);
			}
			yield return null;
		}
	}

	private bool CaminoLibre()
	{
		return Physics.Linecast(base.transform.position, jugador.position, layer);
	}
}
