using UnityEngine;

public class MS_MusicaPersecucion : MonoBehaviour
{
	public AudioSource audioSource;

	public AudioClip[] versiones;

	private int _version;

	[HideInInspector]
	public float volumen;

	private void Update()
	{
		audioSource.volume = volumen;
	}

	public void Reproducir()
	{
		audioSource.Play();
	}

	public void CambiarDeAudio()
	{
		if (!(audioSource.volume > 0.1f))
		{
			_version = ((_version != versiones.Length - 1) ? (_version + 1) : 0);
			audioSource.clip = versiones[_version];
			Reproducir();
		}
	}

	public void Detener()
	{
		audioSource.Stop();
	}
}
