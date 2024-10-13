using UnityEngine;

public class CD_Conducto_Fantasma : MonoBehaviour
{
	public EN_Enemigo enemigo;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.Jugador))
		{
			enemigo.vision._perderDeVista = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.Jugador))
		{
			enemigo.vision._perderDeVista = false;
		}
	}
}
