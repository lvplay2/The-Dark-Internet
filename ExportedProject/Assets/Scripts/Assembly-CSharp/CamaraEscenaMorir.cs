using System.Collections;
using UnityEngine;

public class CamaraEscenaMorir : MonoBehaviour
{
	[Header("Referencias")]
	public Animator animatorEnemigo;

	public AudioSource audioFuente;

	public GameObject canvas;

	public AudioSource boom;

	private const float tiempoEscena = 20f;

	public void Activar_Audio()
	{
		audioFuente.Play();
	}

	public void Mover_Enemigo()
	{
		animatorEnemigo.Play("Animacion");
	}

	public void Fin_Del_Juego()
	{
		canvas.SetActive(true);
		boom.Play();
		StartCoroutine(FinDelJuego());
	}

	private IEnumerator FinDelJuego()
	{
		yield return new WaitForSeconds(5f);
		Object.FindObjectOfType<ES_EscenaCargando>().CargarEscenaAsyncronica("EscenaMenu");
	}
}
