using UnityEngine;
using UnityEngine.Audio;

public class ST_Audio : MonoBehaviour
{
	public static ST_Audio audio;

	private AudioSource[] grupoAudioSource;

	private const int totalAudioSource = 10;

	private GameObject contenedor;

	private void Awake()
	{
		audio = this;
		contenedor = new GameObject("Contenedor");
		grupoAudioSource = new AudioSource[10];
		for (int i = 0; i < 10; i++)
		{
			grupoAudioSource[i] = new GameObject("AudioSource").AddComponent<AudioSource>();
			grupoAudioSource[i].transform.parent = contenedor.transform;
			Configurar(grupoAudioSource[i]);
		}
	}

	public void ReproducirAudioEnPosicion(AudioClip audioClip, Vector3 posicion, float volumen, AudioMixerGroup audioMixerGroup)
	{
		for (int i = 0; i < 10; i++)
		{
			if (!grupoAudioSource[i].isPlaying)
			{
				grupoAudioSource[i].clip = audioClip;
				grupoAudioSource[i].transform.position = posicion;
				grupoAudioSource[i].volume = volumen;
				grupoAudioSource[i].loop = false;
				grupoAudioSource[i].outputAudioMixerGroup = audioMixerGroup;
				grupoAudioSource[i].Play();
				break;
			}
		}
	}

	private void Configurar(AudioSource audioSource)
	{
		audioSource.spatialBlend = 1f;
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.minDistance = 0f;
		audioSource.maxDistance = 7f;
	}
}
