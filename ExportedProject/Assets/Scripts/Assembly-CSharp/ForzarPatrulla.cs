using UnityEngine;

public class ForzarPatrulla : MonoBehaviour
{
	public EN_Enemigo enemigo;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.Jugador))
		{
			enemigo.ComenzarAPatrullar();
		}
	}
}
