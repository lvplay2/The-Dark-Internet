using UnityEngine;

public class SD_SonidosMenu : MonoBehaviour
{
	public static SD_SonidosMenu sonidosMenu;

	[Header("Audio")]
	public AudioSource fuente1;

	public AudioSource fuente2;

	public AudioClip seleccionar_1;

	public AudioClip seleccionar_2;

	public AudioClip whoosh;

	private void Awake()
	{
		sonidosMenu = this;
	}

	public void ReproducirSonido(AudioClip audioClip, float volumen)
	{
		if (!fuente1.isPlaying)
		{
			fuente1.clip = audioClip;
			fuente1.volume = volumen;
			fuente1.Play();
		}
		else
		{
			fuente2.clip = audioClip;
			fuente2.volume = volumen;
			fuente2.Play();
		}
	}
}
