using UnityEngine;

public class EN_Recorrido : MonoBehaviour
{
	[Header("Puntos De Recorrido")]
	public Transform[] puntosRecorrido;

	[Header("Zonas")]
	public Transform[] zonaCocina;

	public Transform[] zonaSalaPrincipal;

	public Transform[] zonaSotano;

	public Transform[] zonaCine;

	public Transform[] zonaPuertaFinal;

	private Transform[] puntosSeleccionados;

	private bool inicializado;

	private void Start()
	{
		Inicializar();
	}

	private void Inicializar()
	{
		ES_EstadoJuego.PreferenciasEnemigo? preferenciasEnemigo = ES_EstadoJuego.estadoJuego.preferenciasEnemigo;
		if (preferenciasEnemigo.HasValue)
		{
			switch (preferenciasEnemigo.GetValueOrDefault())
			{
			case ES_EstadoJuego.PreferenciasEnemigo.Cocina:
				puntosSeleccionados = zonaCocina;
				break;
			case ES_EstadoJuego.PreferenciasEnemigo.SalaPrincipal:
				puntosSeleccionados = zonaSalaPrincipal;
				break;
			case ES_EstadoJuego.PreferenciasEnemigo.Sotano:
				puntosSeleccionados = zonaSotano;
				break;
			case ES_EstadoJuego.PreferenciasEnemigo.Cine:
				puntosSeleccionados = zonaCine;
				break;
			case ES_EstadoJuego.PreferenciasEnemigo.PuertaFinal:
				puntosSeleccionados = zonaPuertaFinal;
				break;
			case ES_EstadoJuego.PreferenciasEnemigo.General:
				puntosSeleccionados = puntosRecorrido;
				break;
			}
		}
	}

	public Vector3 ObtenerPunto()
	{
		if (!inicializado)
		{
			Inicializar();
			inicializado = true;
		}
		return puntosSeleccionados[Random.Range(0, puntosSeleccionados.Length)].position;
	}

	public Vector3 ObtenerPuntoMasLejano(Vector3 origen)
	{
		Vector3 result = default(Vector3);
		float num = 0f;
		for (int i = 0; i < puntosSeleccionados.Length; i++)
		{
			float num2 = Vector3.Distance(origen, puntosSeleccionados[i].position);
			if (num2 > num)
			{
				result = puntosSeleccionados[i].position;
				num = num2;
			}
		}
		return result;
	}
}
