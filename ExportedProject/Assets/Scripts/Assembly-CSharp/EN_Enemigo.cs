using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EN_Enemigo : MonoBehaviour
{
	public enum Estados
	{
		Patrulla = 0,
		Alerta = 1,
		Persecucion = 2
	}

	public enum Accion
	{
		Patrullar = 0,
		Observar = 1,
		Perseguir = 2,
		Atacar = 3
	}

	public enum EventoEspecial
	{
		Nulo = 0,
		VerDron = 1,
		EscucharMuñeco = 2,
		EscucharTelefono = 3,
		RomperTelefono = 4,
		EscucharCrujido = 5
	}

	private const float c_facil_velocidad_Patrulla = 0.9f;

	private const float c_facil_velocidad_Persecucion = 1.3f;

	private const float c_facil_tiempo_Alerta = 7f;

	private const float c_facil_tiempo_Neutralizacion = 60f;

	private const float c_facil_tiempo_Abrir_Puertas = 1.1f;

	private const float c_normal_velocidad_Patrulla = 1.15f;

	private const float c_normal_velocidad_Persecucion = 1.4f;

	private const float c_normal_tiempo_Alerta = 6.25f;

	private const float c_normal_tiempo_Neutralizacion = 45f;

	private const float c_normal_tiempo_Abrir_Puertas = 0.9f;

	private const float c_dificil_velocidad_Patrulla = 1.3f;

	private const float c_dificil_velocidad_Persecucion = 1.5f;

	private const float c_dificil_tiempo_Alerta = 5.5f;

	private const float c_dificil_tiempo_Neutralizacion = 30f;

	private const float c_dificil_tiempo_Abrir_Puertas = 0.8f;

	private const float c_extremo_velocidad_Patrulla = 1.5f;

	private const float c_extremo_velocidad_Persecucion = 1.7f;

	private const float c_extremo_tiempo_Alerta = 5f;

	private const float c_extremo_tiempo_Neutralizacion = 20f;

	private const float c_extremo_tiempo_Abrir_Puertas = 0.65f;

	[HideInInspector]
	public bool _atacando;

	[HideInInspector]
	public bool _neutralizado;

	private bool _persecucionObligatoria;

	private bool _jugadorEnZonaEsferica;

	private bool _comenzoAMoverse;

	private Vector3 _posicionInicial;

	public Vector3 _posicionLuegoDeMatarAlJugador;

	private NavMeshPath _ruta;

	[Header("Referencias")]
	public JG_Jugador jugador;

	public NavMeshAgent agente;

	public EN_Vision vision;

	public EN_Recorrido recorrido;

	public EN_Voces voces;

	public EN_Mandibula mandibula;

	public ST_AudioHD audioHD;

	public Animator animator;

	[Header("Configuración")]
	public float distanciaMinimaAtacar;

	private Coroutine estadoPatrulla;

	private Coroutine estadoAlerta;

	private Coroutine estadoPersecucion;

	private Coroutine accion_AbrirPuerta;

	public Estados estadoActual { get; private set; }

	[HideInInspector]
	public float velocidad_Patrulla { get; private set; }

	[HideInInspector]
	public float velocidad_Persecucion { get; private set; }

	[HideInInspector]
	public float tiempo_Alerta { get; private set; }

	[HideInInspector]
	public float tiempo_Neutralizacion { get; private set; }

	[HideInInspector]
	public float tiempo_Abrir_Puertas { get; private set; }

	public Vector3? ObjetivoConstante { get; private set; }

	public Vector3? ObjetivoNoConstante { get; private set; }

	private void InicializarCaracteristicas()
	{
		ES_EstadoJuego.Dificultad value = ES_EstadoJuego.estadoJuego.dificultad.Value;
		switch (value)
		{
		case ES_EstadoJuego.Dificultad.Facil:
			velocidad_Patrulla = 0.9f;
			velocidad_Persecucion = 1.3f;
			tiempo_Alerta = 7f;
			tiempo_Neutralizacion = 60f;
			tiempo_Abrir_Puertas = 1.1f;
			break;
		case ES_EstadoJuego.Dificultad.Normal:
			velocidad_Patrulla = 1.15f;
			velocidad_Persecucion = 1.4f;
			tiempo_Alerta = 6.25f;
			tiempo_Neutralizacion = 45f;
			tiempo_Abrir_Puertas = 0.9f;
			break;
		case ES_EstadoJuego.Dificultad.Dificil:
			velocidad_Patrulla = 1.3f;
			velocidad_Persecucion = 1.5f;
			tiempo_Alerta = 5.5f;
			tiempo_Neutralizacion = 30f;
			tiempo_Abrir_Puertas = 0.8f;
			break;
		case ES_EstadoJuego.Dificultad.Extremo:
		case ES_EstadoJuego.Dificultad.Fantasma:
			velocidad_Patrulla = 1.5f;
			velocidad_Persecucion = 1.7f;
			tiempo_Alerta = 5f;
			tiempo_Neutralizacion = 20f;
			tiempo_Abrir_Puertas = 0.65f;
			break;
		}
		if (value == ES_EstadoJuego.Dificultad.Fantasma)
		{
			vision._ignorarJugador = true;
			jugador.Escondido = true;
			jugador.Obstaculo(true);
		}
	}

	private void Start()
	{
		InicializarEnemigo();
		_posicionInicial = base.transform.position;
	}

	private void InicializarEnemigo()
	{
		InicializarCaracteristicas();
		_ruta = new NavMeshPath();
		Invoke("ComenzarAPatrullar", 20f);
	}

	private void ComenzarAPatrullar()
	{
		if (!_comenzoAMoverse)
		{
			ActivarEstado(Estados.Patrulla, EventoEspecial.Nulo);
		}
	}

	private bool EnDestino()
	{
		if (agente.enabled && agente.remainingDistance <= agente.stoppingDistance)
		{
			return !agente.pathPending;
		}
		return false;
	}

	private bool EnDestino(Vector3 posicion)
	{
		return Vector3.Distance(base.transform.position, posicion) <= agente.stoppingDistance;
	}

	private void ActivarEstado(Estados estado, EventoEspecial eventoEspecial, bool sinVoz = false)
	{
		_comenzoAMoverse = true;
		if (_atacando || !base.gameObject.activeSelf)
		{
			return;
		}
		if (estadoPatrulla != null)
		{
			StopCoroutine(estadoPatrulla);
			estadoPatrulla = null;
		}
		if (estadoAlerta != null)
		{
			StopCoroutine(estadoAlerta);
			estadoAlerta = null;
		}
		if (estadoPersecucion != null)
		{
			StopCoroutine(estadoPersecucion);
			estadoPersecucion = null;
		}
		voces.Actualizar_EventoEspecial(eventoEspecial);
		switch (estado)
		{
		case Estados.Patrulla:
			estadoPatrulla = StartCoroutine(EstadoPatrulla());
			estadoActual = Estados.Patrulla;
			voces.Actualizar_Accion(Accion.Patrullar);
			if (!sinVoz)
			{
				voces.ReproducirVoz(false, 50f);
			}
			break;
		case Estados.Alerta:
			estadoAlerta = StartCoroutine(EstadoAlerta());
			estadoActual = Estados.Alerta;
			voces.Actualizar_Accion(Accion.Observar);
			if (!sinVoz)
			{
				voces.ReproducirVoz(false, 30f);
			}
			break;
		case Estados.Persecucion:
			estadoPersecucion = StartCoroutine(EstadoPersecucion());
			estadoActual = Estados.Persecucion;
			voces.Actualizar_Accion(Accion.Perseguir);
			if (!sinVoz)
			{
				voces.ReproducirVoz(true, 100f);
			}
			break;
		}
	}

	public void Ruido(Vector3 posicion, EventoEspecial evento)
	{
		Vector3? vector = posicion.PosicionDePies(true);
		Vector3 vector2 = (vector.HasValue ? vector.Value : base.transform.position);
		if (!EnDestino(vector2) && RutaPosible(vector2))
		{
			if (estadoActual != Estados.Persecucion)
			{
				ActualizarDestino_NoConstante(vector2);
				ActivarEstado(Estados.Persecucion, evento);
			}
			else if (!PersiguiendoAlJugador())
			{
				ActualizarDestino_NoConstante(vector2);
				voces.Actualizar_EventoEspecial(evento);
				voces.ReproducirVoz(false, 100f);
			}
		}
	}

	public void ContinuarPatrullando()
	{
		if (estadoActual != 0)
		{
			ActivarEstado(Estados.Patrulla, EventoEspecial.Nulo);
		}
	}

	public void ContinuarPatrullando_SinVoz()
	{
		if (estadoActual != 0)
		{
			ActivarEstado(Estados.Patrulla, EventoEspecial.Nulo, true);
		}
	}

	private void PausarMovimiento()
	{
		agente.isStopped = true;
	}

	private void ReanudarMovimiento()
	{
		if (!_atacando)
		{
			agente.isStopped = false;
		}
	}

	private void Update()
	{
		ComprobarVision_Jugador();
		ActualizarDestino_Agente();
		ComprobarDistancia_Jugador();
	}

	private void ActualizarVelocidad_Agente(float velocidad)
	{
		agente.speed = velocidad;
	}

	private void ComprobarVision_Jugador()
	{
		if (vision.JugadorEnVista || (vision.JugadorPerdido && _jugadorEnZonaEsferica))
		{
			Vector3? vector = vision.jugador.transform.position.PosicionDePies(false);
			Vector3 v = (vector.HasValue ? vector.Value : base.transform.position);
			ActualizarDestino_Constante(v);
			if (estadoActual != Estados.Persecucion)
			{
				ActivarEstado(Estados.Persecucion, EventoEspecial.Nulo);
			}
			voces.MusicaPersecucion(true);
			return;
		}
		if (vision.JugadorPerdido)
		{
			Vector3? vector2 = vision.ObjetivoPerdido.Value.PosicionDePies(false);
			Vector3 v2 = (vector2.HasValue ? vector2.Value : base.transform.position);
			ActualizarDestino_NoConstante(v2);
			if (estadoActual != Estados.Persecucion)
			{
				ActivarEstado(Estados.Persecucion, EventoEspecial.Nulo);
			}
		}
		voces.MusicaPersecucion(false);
	}

	private void ActualizarDestino_Agente()
	{
		if (_atacando)
		{
			return;
		}
		if (ObjetivoConstante.HasValue)
		{
			if (agente.enabled)
			{
				agente.destination = ObjetivoConstante.Value;
			}
		}
		else if (ObjetivoNoConstante.HasValue && agente.enabled)
		{
			agente.destination = ObjetivoNoConstante.Value;
		}
	}

	private void ComprobarDistancia_Jugador()
	{
		if (Vector3.Distance(base.transform.position, jugador.transform.position) < distanciaMinimaAtacar && vision.JugadorDisponible && !jugador.Escondido && !_atacando && !_neutralizado && !jugador.Escondido)
		{
			Atacar();
		}
	}

	private void Atacar()
	{
		StartCoroutine(Atacar_Corrutina());
	}

	private IEnumerator Atacar_Corrutina()
	{
		_atacando = true;
		PausarMovimiento();
		jugador.Es_Atacado();
		ES_Controles.controles.Click(IT_Interactivo.Acciones.DejarEnSuelo, false);
		float tiempoRotacion = 0.1f;
		float tiempo = 0f;
		Quaternion rotacionInicial = base.transform.rotation;
		Quaternion rotacionFinal = Quaternion.LookRotation(jugador.transform.position - base.transform.position);
		while (tiempo < 1f)
		{
			base.transform.rotation = Quaternion.Euler(base.transform.eulerAngles.x, Quaternion.Lerp(rotacionInicial, rotacionFinal, tiempo).eulerAngles.y, base.transform.eulerAngles.z);
			tiempo += Time.deltaTime / tiempoRotacion;
			yield return null;
		}
		base.transform.rotation = rotacionFinal;
		yield return new WaitForSeconds(0.215f);
		ReproducirAnimacion(Accion.Atacar);
		mandibula.Abrir();
		voces.Actualizar_Accion(Accion.Atacar);
		voces.ReproducirVoz(true, 100f);
		yield return new WaitForSeconds(3.2f);
		_atacando = false;
		agente.enabled = false;
		base.transform.position = _posicionLuegoDeMatarAlJugador;
		agente.enabled = true;
		PausarMovimiento();
		yield return new WaitForSeconds(1f);
		vision.ObjetivoAlcanzado();
		Invoke("ContinuarPatrullando_SinVoz", 8f);
	}

	private IEnumerator Atacar_Sanamente_Corrutina()
	{
		PausarMovimiento();
		ReproducirAnimacion(Accion.Atacar);
		yield return null;
	}

	private void ActualizarDestino_Constante(Vector3 v)
	{
		if (!_atacando && !_neutralizado)
		{
			ObjetivoConstante = v;
			ObjetivoNoConstante = null;
		}
	}

	private void ActualizarDestino_NoConstante(Vector3 v)
	{
		if (!_atacando && !_neutralizado)
		{
			ObjetivoConstante = null;
			ObjetivoNoConstante = v;
		}
	}

	private bool RutaPosible(Vector3 posicion)
	{
		agente.CalculatePath(posicion, _ruta);
		if (_ruta.status == NavMeshPathStatus.PathPartial)
		{
			return false;
		}
		return true;
	}

	private void ReproducirAnimacion(Accion accion)
	{
		switch (accion)
		{
		case Accion.Patrullar:
			animator.speed = velocidad_Patrulla * 0.65f;
			animator.SetInteger("Estado", 0);
			break;
		case Accion.Perseguir:
			animator.speed = velocidad_Persecucion * 0.65f;
			animator.SetInteger("Estado", 0);
			break;
		case Accion.Observar:
			animator.speed = 1f;
			animator.SetInteger("Estado", 1);
			break;
		case Accion.Atacar:
			animator.speed = 1f;
			animator.SetInteger("Estado", 2);
			break;
		}
	}

	public void AtacarSanamente()
	{
		StartCoroutine(Atacar_Sanamente_Corrutina());
	}

	public bool PersiguiendoAlJugador()
	{
		if (estadoActual == Estados.Persecucion)
		{
			return ObjetivoConstante.HasValue;
		}
		return false;
	}

	public void ActivarAgente()
	{
		agente.updatePosition = true;
		agente.updateRotation = true;
		_neutralizado = false;
	}

	public void DesactivarAgente()
	{
		agente.updatePosition = false;
		agente.updateRotation = false;
		_neutralizado = true;
	}

	private IEnumerator EstadoPatrulla()
	{
		ReanudarMovimiento();
		ActualizarVelocidad_Agente(velocidad_Patrulla);
		ReproducirAnimacion(Accion.Patrullar);
		do
		{
			ActualizarDestino_NoConstante(recorrido.ObtenerPunto());
			yield return null;
		}
		while (EnDestino() || !RutaPosible(ObjetivoNoConstante.Value));
		ReanudarMovimiento();
		while (true)
		{
			if (EnDestino())
			{
				ActivarEstado(Estados.Alerta, EventoEspecial.Nulo);
			}
			yield return null;
		}
	}

	private IEnumerator EstadoAlerta()
	{
		PausarMovimiento();
		ReproducirAnimacion(Accion.Observar);
		yield return new WaitForSeconds(tiempo_Alerta);
		ActivarEstado(Estados.Patrulla, EventoEspecial.Nulo);
	}

	private IEnumerator EstadoPersecucion()
	{
		ReanudarMovimiento();
		ReproducirAnimacion(Accion.Perseguir);
		ActualizarVelocidad_Agente(velocidad_Persecucion);
		yield return null;
		while (!EnDestino())
		{
			yield return null;
		}
		vision.ObjetivoAlcanzado();
		ActivarEstado(Estados.Alerta, EventoEspecial.Nulo);
	}

	private IEnumerator Accion_AbrirPuerta(IT_Puerta puerta)
	{
		if (!puerta._abierta && ((puerta.cerradaConLlave && puerta._cerrojoAbierto) || !puerta.cerradaConLlave))
		{
			PausarMovimiento();
			ReproducirAnimacion(Accion.Observar);
			yield return new WaitForSeconds(tiempo_Abrir_Puertas);
			puerta.Interaccionar(IT_Interactivo.Acciones.Recoger, false);
			yield return new WaitForSeconds(0.5f);
			ReanudarMovimiento();
			ReproducirAnimacion(Accion.Patrullar);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(ES_Tags.ObjetoEnemigo))
		{
			EN_ObjetoEnemigo component = other.GetComponent<EN_ObjetoEnemigo>();
			if (component != null && component.Disponible && !component.Utilizado)
			{
				AtacarSanamente();
				component.Interaccionar();
			}
		}
	}

	public void Informar_AbrirPuerta(IT_Puerta puerta)
	{
		if (accion_AbrirPuerta != null)
		{
			StopCoroutine(accion_AbrirPuerta);
		}
		accion_AbrirPuerta = StartCoroutine(Accion_AbrirPuerta(puerta));
	}
}
