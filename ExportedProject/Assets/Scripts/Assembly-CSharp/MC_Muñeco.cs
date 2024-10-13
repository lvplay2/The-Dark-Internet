using System.Collections;
using UnityEngine;

public class MC_Muñeco : MonoBehaviour
{
	public EN_Enemigo enemigo;

	public Animator animator;

	public AudioSource audioSource;

	public AudioClip sonido;

	private const float probabilidad = 25f;

	private Coroutine alertarEnemigo;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.Jugador) && (float)Random.Range(0, 100) <= 25f)
		{
			if (alertarEnemigo != null)
			{
				StopCoroutine(alertarEnemigo);
			}
			alertarEnemigo = StartCoroutine(AlertarEnemigo(other.transform.position));
			switch (0)
			{
			case 0:
				animator.Play("Alertar_1-1");
				break;
			case 1:
				animator.Play("Alertar_2-1");
				break;
			}
			audioSource.clip = sonido;
			audioSource.Play();
		}
	}

	private IEnumerator AlertarEnemigo(Vector3 posicion)
	{
		yield return new WaitForSeconds(0.75f);
		enemigo.Ruido(posicion, EN_Enemigo.EventoEspecial.EscucharMuñeco);
	}
}
