using UnityEngine;

public class ZC_ZonasDeCrujido : MonoBehaviour
{
	public GameObject zonas;

	private void Start()
	{
		switch (ES_EstadoJuego.estadoJuego.dificultad)
		{
		case ES_EstadoJuego.Dificultad.Extremo:
		case ES_EstadoJuego.Dificultad.Dificil:
		case ES_EstadoJuego.Dificultad.Fantasma:
			zonas.SetActive(true);
			break;
		default:
			zonas.SetActive(false);
			break;
		}
	}
}
