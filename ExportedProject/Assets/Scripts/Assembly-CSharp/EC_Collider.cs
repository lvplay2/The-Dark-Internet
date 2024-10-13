using UnityEngine;

public class EC_Collider : IT_Interactivo
{
	private JG_Jugador jugador;

	private EN_Enemigo enemigo;

	private JG_Brazo brazo;

	private GameObject camara;

	private string observacion = "No puedo entrar, ¡Me persigue el psicopata!";

	private bool _dentro;

	private void Start()
	{
		base.VisibleParaOtro = true;
	}

	private void Awake()
	{
		ObtenerReferencias();
	}

	private void ObtenerReferencias()
	{
		jugador = Object.FindObjectOfType<JG_Jugador>();
		enemigo = Object.FindObjectOfType<EN_Enemigo>();
		brazo = Object.FindObjectOfType<JG_Brazo>();
		camara = base.transform.GetChild(0).gameObject;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		switch (accion)
		{
		case Acciones.Entrar:
			if (!enemigo.ObjetivoConstante.HasValue)
			{
				if (!_dentro)
				{
					Entrar();
				}
			}
			else
			{
				UI_Canvas.canvas.observacion.Observar(observacion);
			}
			break;
		case Acciones.Salir:
			if (_dentro)
			{
				Salir();
			}
			break;
		}
	}

	private void Entrar()
	{
		camara.gameObject.SetActive(true);
		jugador.Escondido = true;
		jugador.fp_Controller.canControl = false;
		jugador.camaraJugador.enabled = false;
		brazo.Escondido(true);
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.agarrarObjeto_1, base.transform.position, 0.15f, ES_EstadoEscena.estadoEscena.audioGlobal);
		IT_Cartera.cartera.Asignar_ElementoEnUso(this);
		IT_Interactivo.AsignarAcciones(new Acciones[2]
		{
			Acciones.Salir,
			Acciones.MirarAlrededor
		});
		_dentro = true;
	}

	private void Salir()
	{
		camara.gameObject.SetActive(false);
		jugador.Escondido = false;
		jugador.fp_Controller.canControl = true;
		jugador.camaraJugador.enabled = true;
		brazo.Escondido(false);
		IT_Cartera.cartera.Quitar_ElementoEnUso();
		IT_Interactivo.AsignarAcciones(IT_Interactivo.AccionesPredeterminadas);
		_dentro = false;
	}
}
