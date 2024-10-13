using UnityEngine;

public class IT_Recogible : IT_Interactivo
{
	public enum TipoDeSonido
	{
		SinSonido = 0,
		Vidrio = 1,
		Metal = 2
	}

	public enum Caida
	{
		EjercerFuerza = 0,
		DejarCaer = 1,
		PosicionarEnSuelo = 2
	}

	public delegate void Lo_Recogio();

	public delegate void Lo_Solto();

	[HideInInspector]
	public Rigidbody rigidbody;

	protected JG_Jugador jugador;

	private EN_Enemigo enemigo;

	private OJ_Contenido posiciones;

	protected Collider collider;

	[Header("Sonidos")]
	public TipoDeSonido tipoDeSonido;

	[Header("Observacion")]
	public bool observar;

	[TextArea(2, 3)]
	public string observacion;

	private bool _arrojado;

	private bool _yaColisiono;

	private bool _cancelarRuidoTemporalmente;

	private Vector3 _escalaAnterior;

	private Transform _padrePosicionElemento;

	protected bool _colisionable = true;

	protected bool _recodigo;

	protected Caida _caidaSoltar;

	protected Caida _caidaIntercambiar = Caida.DejarCaer;

	public Lo_Recogio lo_Recogio;

	public Lo_Solto lo_Solto;

	public Vector3 posicionBrazo;

	public Vector3 rotacionBrazo;

	public float escalaBrazo;

	public JG_Brazo.Lado lado;

	[Header("Objeto Guardado")]
	public OJ_Posicion.Grupo grupo;

	public bool posicionarseAleatoriamente;

	public bool hacerHijo = true;

	private const float fuerzaArrojar = 5.5f;

	protected virtual void Awake()
	{
		ObtenerReferencias();
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
	}

	private void ObtenerReferencias()
	{
		rigidbody = GetComponent<Rigidbody>();
		jugador = Object.FindObjectOfType<JG_Jugador>();
		enemigo = Object.FindObjectOfType<EN_Enemigo>();
		collider = GetComponent<Collider>();
		posiciones = Object.FindObjectOfType<OJ_Contenido>();
	}

	protected virtual void Start()
	{
		if (posicionarseAleatoriamente)
		{
			if (hacerHijo)
			{
				_padrePosicionElemento = posiciones.ObtenerPosicion_Elemento(grupo).ObtenerPosicion(this);
				base.transform.parent = _padrePosicionElemento;
				base.transform.localPosition = Vector3.zero;
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Random.Range(-90f, 90f), base.transform.eulerAngles.z);
				rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
				base.gameObject.SetActive(false);
			}
			else
			{
				base.transform.position = posiciones.ObtenerPosicion_Elemento(grupo).transform.position;
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Random.Range(-90f, 90f), base.transform.eulerAngles.z);
			}
		}
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		switch (accion)
		{
		case Acciones.Recoger:
			if (!seSolto && !_recodigo)
			{
				if (IT_Cartera.cartera.ContieneAlgo())
				{
					IT_Recogible iT_Recogible = IT_Cartera.cartera.Obtener_ElementoEnCartera<IT_Recogible>();
					iT_Recogible.Soltar(iT_Recogible._caidaIntercambiar);
				}
				Recoger();
			}
			break;
		case Acciones.Soltar:
			if (_recodigo)
			{
				Soltar(_caidaSoltar);
			}
			break;
		case Acciones.DejarEnSuelo:
			if (_recodigo)
			{
				Soltar(Caida.PosicionarEnSuelo);
			}
			break;
		}
	}

	public void Suprimir()
	{
		IT_Cartera.cartera.Vaciar();
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		base.gameObject.SetActive(false);
		jugador.brazo.Desactivar();
	}

	public virtual void Recoger(string ObjetoBrazo = "")
	{
		if (observar)
		{
			UI_Canvas.canvas.observacion.Observar(observacion);
		}
		base.gameObject.layer = 0;
		if (collider != null)
		{
			collider.enabled = false;
		}
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
		base.transform.parent = jugador.brazo.brazoAnimado.transform;
		base.transform.localPosition = posicionBrazo;
		base.transform.localRotation = Quaternion.Euler(rotacionBrazo);
		_escalaAnterior = base.transform.lossyScale;
		base.transform.localScale = new Vector3(escalaBrazo, escalaBrazo, escalaBrazo);
		IT_Cartera.cartera.Asignar_ElementoEnCartera(this);
		if (ObjetoBrazo != "")
		{
			jugador.brazo.Activar(null, lado, ObjetoBrazo);
			rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
			base.gameObject.SetActive(false);
		}
		else
		{
			jugador.brazo.Activar(this, lado);
		}
		_recodigo = true;
		_arrojado = false;
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.agarrarObjeto_1, base.transform.position, 0.15f, ES_EstadoEscena.estadoEscena.audioGlobal);
		Lo_Recogio obj = lo_Recogio;
		if (obj != null)
		{
			obj();
		}
	}

	public virtual void Soltar(Caida caida)
	{
		_cancelarRuidoTemporalmente = false;
		if (!base.gameObject.activeSelf)
		{
			rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
			base.gameObject.SetActive(true);
		}
		base.gameObject.layer = LayerMask.NameToLayer(ES_Tags.Interactivo);
		if (collider != null)
		{
			collider.enabled = true;
		}
		IT_Cartera.cartera.Vaciar();
		jugador.brazo.Desactivar();
		base.transform.parent = null;
		base.transform.AsignarEscalaGlobal(_escalaAnterior);
		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
		switch (caida)
		{
		case Caida.EjercerFuerza:
			rigidbody.velocity = ES_EstadoEscena.estadoEscena.camaraPrincipal.transform.forward * 5.5f;
			rigidbody.angularVelocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
			break;
		case Caida.PosicionarEnSuelo:
		{
			_cancelarRuidoTemporalmente = true;
			RaycastHit hitInfo;
			Physics.Raycast(base.transform.position, -Vector3.up, out hitInfo, 10f, LayerMask.GetMask(ES_Tags.Estatico));
			base.transform.position = hitInfo.point + new Vector3(0f, 0.12f, 0f);
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
			break;
		}
		}
		_recodigo = false;
		_arrojado = true;
		_yaColisiono = false;
		Lo_Solto obj = lo_Solto;
		if (obj != null)
		{
			obj();
		}
	}

	private void Colisionar(Collision coll)
	{
		if ((!coll.gameObject.CompareTag(ES_Tags.Suelo) && !coll.gameObject.CompareTag(ES_Tags.Escalera)) || !_arrojado || _yaColisiono || !_colisionable)
		{
			return;
		}
		if (tipoDeSonido != 0 && !_cancelarRuidoTemporalmente)
		{
			enemigo.Ruido(base.transform.position, EN_Enemigo.EventoEspecial.Nulo);
		}
		_yaColisiono = true;
		if (!_cancelarRuidoTemporalmente)
		{
			switch (tipoDeSonido)
			{
			case TipoDeSonido.Vidrio:
				ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.caida_botella.Random(), base.transform.position, 0.25f, ES_EstadoEscena.estadoEscena.audioGlobal);
				break;
			case TipoDeSonido.Metal:
				ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.caida_metal.Random(), base.transform.position, 0.5f, ES_EstadoEscena.estadoEscena.audioGlobal);
				break;
			}
		}
	}

	private void OnCollisionEnter(Collision coll)
	{
		Colisionar(coll);
	}

	[ContextMenu("Obtener Informacion Transform")]
	public void ObtenerInformacionTransform()
	{
		posicionBrazo = base.transform.localPosition;
		rotacionBrazo = base.transform.localEulerAngles;
		escalaBrazo = base.transform.localScale.x;
	}
}
