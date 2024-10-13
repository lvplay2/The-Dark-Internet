using UnityEngine;

public class ZC_ZonaCrujido : MonoBehaviour
{
	private EN_Enemigo enemigo;

	private void Awake()
	{
		enemigo = Object.FindObjectOfType<EN_Enemigo>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.Jugador))
		{
			AudioSource.PlayClipAtPoint(Sonidos.sonidos.piso_cruje.Random(), base.transform.position, 0.175f);
			Invoke("AlertarAlEnemigo", 0.5f);
		}
	}

	private void AlertarAlEnemigo()
	{
		enemigo.Ruido(base.transform.position, EN_Enemigo.EventoEspecial.EscucharCrujido);
	}
}
