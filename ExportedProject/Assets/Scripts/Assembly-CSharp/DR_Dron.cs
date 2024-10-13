using UnityEngine;

public class DR_Dron : IT_Recogible
{
	[Header("Referencias")]
	public new JG_Jugador jugador;

	public GameObject camara;

	public AudioSource audioSource;

	public DR_Iman iman;

	public Canvas canvasInterferencia;

	[Header("Configuración")]
	public float velocidad;

	public float torque;

	public float s_Velocidad;

	public float s_Rotacion;

	private bool _bloquearRotacion;

	private float _Velocidad;

	private float _Rotacion;

	private float _volumenFinal = 0.6f;

	private int inputX;

	private int inputY;

	private RigidbodyConstraints constraints;

	public bool Usando { get; private set; }

	protected override void Start()
	{
		base.Start();
		_caidaSoltar = Caida.PosicionarEnSuelo;
		_caidaIntercambiar = Caida.PosicionarEnSuelo;
		_colisionable = false;
		constraints = rigidbody.constraints;
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		if (!iman.llaveEnIman)
		{
			base.Interaccionar(accion, seSolto);
		}
		switch (accion)
		{
		case Acciones.Recoger:
		{
			if (!iman.llaveEnIman)
			{
				rigidbody.constraints = RigidbodyConstraints.None;
				break;
			}
			DR_Llave component = iman.llave.GetComponent<DR_Llave>();
			if (component != null)
			{
				component.Recoger();
				iman.SoltarLlave();
			}
			break;
		}
		case Acciones.Soltar:
			rigidbody.constraints = constraints;
			break;
		case Acciones.Manejar:
			if (!Usando && !IT_Cartera.cartera.ContieneAlgo())
			{
				Manejar();
			}
			break;
		case Acciones.Salir:
			if (Usando)
			{
				DejarDeManejar();
			}
			break;
		case Acciones.Acelerar:
			inputY = ((!seSolto) ? 1 : 0);
			break;
		case Acciones.Frenar:
			inputY = ((!seSolto) ? (-1) : 0);
			break;
		case Acciones.MoverIzquierda:
			inputX = ((!seSolto) ? (-1) : 0);
			break;
		case Acciones.MoverDerecha:
			inputX = ((!seSolto) ? 1 : 0);
			break;
		case Acciones.DejarDeUsar:
		case Acciones.Entrar:
		case Acciones.Usar_ObjetoRecodigo:
		case Acciones.Observar:
		case Acciones.MoverArriba:
		case Acciones.MoverAbajo:
			break;
		}
	}

	private void Manejar()
	{
		IT_Interactivo.AsignarAcciones(new Acciones[5]
		{
			Acciones.Acelerar,
			Acciones.Frenar,
			Acciones.MoverIzquierda,
			Acciones.MoverDerecha,
			Acciones.Salir
		});
		base.gameObject.layer = LayerMask.NameToLayer(ES_Tags.Default);
		jugador.DesactivarCamara();
		jugador.BloquearMovimiento();
		camara.SetActive(true);
		ES_EstadoEscena.estadoEscena.audioListenerJugador.enabled = false;
		IT_Cartera.cartera.Asignar_ElementoEnUso(this);
		canvasInterferencia.gameObject.SetActive(true);
		Usando = true;
	}

	public void DejarDeManejar()
	{
		IT_Interactivo.AsignarAcciones(IT_Interactivo.AccionesPredeterminadas);
		base.gameObject.layer = LayerMask.NameToLayer(ES_Tags.Interactivo);
		jugador.ActivarCamara();
		jugador.DesbloquearMovimiento();
		camara.SetActive(false);
		ES_EstadoEscena.estadoEscena.audioListenerJugador.enabled = true;
		IT_Cartera.cartera.Vaciar();
		canvasInterferencia.gameObject.SetActive(false);
		inputY = 0;
		Usando = false;
	}

	private void Update()
	{
		if (Usando || !(audioSource.volume <= 0.01f))
		{
			audioSource.volume = Mathf.Lerp(audioSource.volume, (inputY != 0) ? _volumenFinal : 0f, 2f * Time.deltaTime);
			if (_bloquearRotacion)
			{
				rigidbody.constraints = (RigidbodyConstraints)80;
			}
			else
			{
				rigidbody.constraints = constraints;
			}
		}
	}

	private void FixedUpdate()
	{
		if ((_recodigo || Usando) && !_recodigo)
		{
			_Velocidad = Mathf.Lerp(_Velocidad, velocidad * (float)inputY, Mathf.SmoothStep(0f, 1f, s_Velocidad / 100f));
			_Rotacion = Mathf.Lerp(_Rotacion, torque * (float)inputX, Mathf.SmoothStep(0f, 1f, s_Rotacion / 100f));
			Vector3 direction = base.transform.InverseTransformDirection(rigidbody.velocity);
			direction.x = 0f;
			direction.z = _Velocidad;
			rigidbody.velocity = base.transform.TransformDirection(direction);
			rigidbody.rotation = Quaternion.Euler(rigidbody.rotation.eulerAngles.x, rigidbody.rotation.eulerAngles.y, 0f);
			rigidbody.angularVelocity = new Vector3(0f, _Rotacion, 0f) * (_Velocidad / velocidad);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.BloquearDron))
		{
			_bloquearRotacion = true;
		}
		else
		{
			_bloquearRotacion = false;
		}
	}
}
