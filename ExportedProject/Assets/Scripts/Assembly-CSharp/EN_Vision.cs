using UnityEngine;

public class EN_Vision : MonoBehaviour
{
	public EN_Enemigo enemigo;

	public JG_Jugador jugador;

	public Transform ojos;

	public Transform pechoJugador;

	private float gradosDeVision = 120f;

	private float distanciaMaxima = 3.5f;

	private bool _perdidoDeVista = true;

	private bool _jamasLoHaVisto = true;

	public bool _ignorarJugador = true;

	[HideInInspector]
	public bool _perderDeVista;

	public LayerMask layer;

	private Vector3 posI;

	private Vector3 posF;

	public bool JugadorEnVista { get; private set; }

	public bool JugadorPerdido { get; private set; }

	public bool JugadorDisponible { get; private set; }

	public Transform ObjetivoEnVista { get; private set; }

	public Vector3? ObjetivoPerdido { get; private set; }

	private void Update()
	{
		CalcularVision();
	}

	private bool EnRangoDeVision()
	{
		Quaternion quaternion = Quaternion.LookRotation(jugador.transform.position - base.transform.position, Vector3.up);
		return !(Mathf.Abs(Mathf.DeltaAngle(base.transform.eulerAngles.y, quaternion.eulerAngles.y)) > gradosDeVision);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(posI, posF);
	}

	private void CalcularVision()
	{
		RaycastHit hitInfo;
		Physics.Linecast(ojos.position, pechoJugador.position, out hitInfo, layer);
		if (EnRangoDeVision())
		{
			if (hitInfo.collider != null)
			{
				if (hitInfo.transform.tag == ES_Tags.Jugador && !_perderDeVista && !_ignorarJugador && !jugador.Escondido)
				{
					posI = ojos.position;
					posF = pechoJugador.position;
					JugadorEnVista = true;
					JugadorPerdido = false;
					JugadorDisponible = true;
					ObjetivoEnVista = jugador.transform;
					ObjetivoPerdido = null;
					_perdidoDeVista = false;
					_jamasLoHaVisto = false;
				}
				else
				{
					JugadorEnVista = false;
					JugadorDisponible = false;
					if (!_perdidoDeVista && !_jamasLoHaVisto && !enemigo._atacando)
					{
						JugadorPerdido = true;
						ObjetivoPerdido = jugador.transform.position;
						_perdidoDeVista = true;
					}
				}
			}
			else
			{
				JugadorDisponible = false;
			}
		}
		else if (hitInfo.collider != null)
		{
			if (hitInfo.transform.tag == ES_Tags.Jugador && !_perderDeVista && !_ignorarJugador)
			{
				JugadorDisponible = true;
			}
			else
			{
				JugadorDisponible = false;
			}
		}
		else
		{
			JugadorDisponible = false;
		}
	}

	public void ObjetivoAlcanzado()
	{
		ObjetivoPerdido = null;
		JugadorPerdido = false;
		_perdidoDeVista = true;
	}
}
