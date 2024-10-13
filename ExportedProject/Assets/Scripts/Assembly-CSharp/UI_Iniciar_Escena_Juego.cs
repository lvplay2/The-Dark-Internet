using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Iniciar_Escena_Juego : MonoBehaviour
{
	public Image fondoNegro;

	private void Start()
	{
		StartCoroutine(Iniciar_Escena());
	}

	private IEnumerator Iniciar_Escena()
	{
		fondoNegro.gameObject.SetActive(true);
		AudioSource.PlayClipAtPoint(Sonidos.sonidos.suspenso_1, Object.FindObjectOfType<JG_Jugador>().transform.position, 0.5f);
		yield return new WaitForSeconds(1f);
		fondoNegro.gameObject.SetActive(false);
		yield return null;
	}
}
