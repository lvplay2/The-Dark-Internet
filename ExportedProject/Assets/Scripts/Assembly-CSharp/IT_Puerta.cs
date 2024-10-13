using System;
using UnityEngine;
using UnityEngine.AI;

public class IT_Puerta : IT_Interactivo
{
	public enum Eje
	{
		X = 0,
		Y = 1,
		Z = 2
	}

	public enum TipoDeSonido
	{
		Madera = 0,
		Tejido = 1,
		Metal = 2
	}

	[Header("Configuración")]
	public Eje eje = Eje.Y;

	public float grados;

	public bool positivo = true;

	public bool abiertaPorDefecto;

	public bool abrirSoloUnaVez;

	public bool visibleParaMano = true;

	[Header("Sonido")]
	public TipoDeSonido tipoDeSonido;

	[Header("Llave (puede ser nulo)")]
	public IT_Recogible llave;

	public NavMeshObstacle navMeshObstacle;

	public bool cerradaConLlave;

	public string observacion;

	public bool interaccionaConEnemigo;

	[Header("Bloqueada")]
	public bool bloqueada;

	private float _velocidad = 3.5f;

	private float _tiempoInicio;

	private float _x;

	private float _y;

	private float _z;

	private float _t;

	private float _grados;

	private bool _jugadorAtravesado;

	private EN_Enemigo enemigo;

	private Vector3 _rotacionInicial;

	private Collider collider;

	[HideInInspector]
	public bool _abierta;

	[HideInInspector]
	public bool _cerrojoAbierto;

	public OJ_Posicion posicion;

	private void Awake()
	{
		enemigo = UnityEngine.Object.FindObjectOfType<EN_Enemigo>();
		collider = GetComponent<Collider>();
		navMeshObstacle = GetComponent<NavMeshObstacle>();
	}

	private void OnEnable()
	{
		base.VisibleParaMano = visibleParaMano;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (posicion != null)
		{
			posicion.Interaccionar();
		}
		if (seSolto || accion != 0)
		{
			return;
		}
		if (bloqueada)
		{
			UI_Canvas.canvas.observacion.Observar(observacion);
		}
		else
		{
			if (abrirSoloUnaVez && _abierta)
			{
				return;
			}
			if (cerradaConLlave && !_cerrojoAbierto)
			{
				if (IT_Cartera.cartera.Contiene(llave))
				{
					Abrir_PuertaConCerrojo();
				}
				else
				{
					PuertaCerrada();
				}
			}
			else
			{
				Abrir_PuertaSinCerrojo(true);
			}
		}
	}

	private void Abrir_PuertaSinCerrojo(bool reproducirSonido)
	{
		_abierta = !_abierta;
		_tiempoInicio = Time.time;
		if (reproducirSonido)
		{
			switch (tipoDeSonido)
			{
			case TipoDeSonido.Madera:
				ST_Audio.audio.ReproducirAudioEnPosicion(_abierta ? Sonidos.sonidos.puerta_abriendose : Sonidos.sonidos.puerta_cerrandose, base.transform.position, 0.475f, ES_EstadoEscena.estadoEscena.audioGlobal);
				break;
			case TipoDeSonido.Tejido:
				ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.puerta_metal_abrir, base.transform.position, 0.2f, ES_EstadoEscena.estadoEscena.audioGlobal);
				break;
			}
			_cerrojoAbierto = true;
		}
	}

	public void Abrir_PuertaConCerrojo()
	{
		_abierta = !_abierta;
		_tiempoInicio = Time.time;
		switch (tipoDeSonido)
		{
		case TipoDeSonido.Madera:
			ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.puerta_cerrada, base.transform.position, 0.3f, ES_EstadoEscena.estadoEscena.audioGlobal);
			ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.puerta_abriendose, base.transform.position, 0.475f, ES_EstadoEscena.estadoEscena.audioGlobal);
			break;
		case TipoDeSonido.Tejido:
			ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.puerta_metal_abrir, base.transform.position, 0.2f, ES_EstadoEscena.estadoEscena.audioGlobal);
			break;
		}
		navMeshObstacle.enabled = false;
		base.VisibleParaMano = false;
		_cerrojoAbierto = true;
	}

	private void PuertaCerrada()
	{
		UI_Canvas.canvas.observacion.Observar(observacion);
		if (tipoDeSonido != 0)
		{
			int num = 1;
		}
		else
		{
			ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.puerta_cerrada, base.transform.position, 0.3f, ES_EstadoEscena.estadoEscena.audioGlobal);
		}
	}

	private void Start()
	{
		_rotacionInicial = N(base.transform.localEulerAngles);
		if (abiertaPorDefecto)
		{
			base.transform.localEulerAngles = ObtenerRotacion();
			_abierta = true;
		}
		Nombre = "Puerta";
	}

	private void FixedUpdate()
	{
		_x = 0f;
		_y = 0f;
		_z = 0f;
		_t = (Time.time - _tiempoInicio) / _velocidad;
		Vector3 vector = N(base.transform.localEulerAngles);
		if (_abierta)
		{
			_x = Mathf.SmoothStep(vector.x, ObtenerRotacion().x, _t);
			_y = Mathf.SmoothStep(vector.y, ObtenerRotacion().y, _t);
			_z = Mathf.SmoothStep(vector.z, ObtenerRotacion().z, _t);
			collider.isTrigger = true;
		}
		else
		{
			_x = Mathf.SmoothStep(vector.x, _rotacionInicial.x, _t);
			_y = Mathf.SmoothStep(vector.y, _rotacionInicial.y, _t);
			_z = Mathf.SmoothStep(vector.z, _rotacionInicial.z, _t);
		}
		base.transform.localEulerAngles = new Vector3(_x, _y, _z);
	}

	private void Update()
	{
		if (_abierta && !collider.isTrigger)
		{
			collider.isTrigger = true;
		}
		else if (Math.Abs(base.transform.localEulerAngles.y.Angulo()) < 1f && !_jugadorAtravesado && collider.isTrigger)
		{
			collider.isTrigger = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(ES_Tags.Jugador))
		{
			_jugadorAtravesado = true;
		}
		if (other.CompareTag(ES_Tags.Enemigo) && interaccionaConEnemigo)
		{
			enemigo.Informar_AbrirPuerta(this);
		}
		if (other.CompareTag(ES_Tags.Gato))
		{
			GT_Gato component = other.GetComponent<GT_Gato>();
			if (component != null)
			{
				component.AbrirPuerta(this);
			}
		}
	}

	private void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject.CompareTag(ES_Tags.Enemigo) && interaccionaConEnemigo)
		{
			enemigo.Informar_AbrirPuerta(this);
		}
		if (coll.gameObject.CompareTag(ES_Tags.Gato))
		{
			GT_Gato component = coll.gameObject.GetComponent<GT_Gato>();
			if (component != null)
			{
				component.AbrirPuerta(this);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.Jugador))
		{
			_jugadorAtravesado = false;
		}
	}

	private Vector3 ObtenerRotacion()
	{
		_grados = (positivo ? grados : (0f - grados));
		switch (eje)
		{
		case Eje.X:
			return new Vector3(_grados, _rotacionInicial.y, _rotacionInicial.z);
		case Eje.Y:
			return new Vector3(_rotacionInicial.x, _grados, _rotacionInicial.z);
		case Eje.Z:
			return new Vector3(_rotacionInicial.x, _rotacionInicial.y, _grados);
		default:
			return Vector3.zero;
		}
	}

	private Vector3 N(Vector3 v)
	{
		return new Vector3(F(v.x), F(v.y), F(v.z));
	}

	private float F(float f)
	{
		if (!(f > 180f))
		{
			return f;
		}
		return f - 360f;
	}
}
