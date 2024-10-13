using UnityEngine;

public class MusicaAleatoria : MonoBehaviour
{
	public AudioSource audioSource;

	public AudioClip[] canciones;

	private void Start()
	{
		audioSource.clip = canciones[Random.Range(0, canciones.Length)];
		audioSource.Play();
	}
}
