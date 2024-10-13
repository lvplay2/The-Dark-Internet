using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GT_Gato : MonoBehaviour
{
	private enum Animaciones
	{
		Caminar = 0,
		Atacar1 = 1,
		Atacar2 = 2,
		Morir = 3
	}

	public enum Gato
	{
		Gato1 = 0,
		Gato2 = 1
	}

	[Header("Referencias")]
	public JG_Jugador jugador;

	private NavMeshAgent agente;

	[Header("Puntos de Recorrido")]
	public Transform[] puntosDeRecorrido;

	[Header("Animacion")]
	public Animator animator;

	[Header("UI")]
	public CanvasGroup sangre;

	[Header("Sonido")]
	public AudioSource audioSource;

	[Header("Configuracion")]
	public Gato gato;

	private bool _muerto;

	private NavMeshPath _ruta;

	private int _posicionActual;

	private bool _abriendoPuerta;

	private bool _atacando;

	private float _ultimoAtaque;

	private Coroutine _bajarVelocidadJugador;

	private Coroutine _atacar;

	private void Awake()
	{
		agente = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		if (ComprobarActivacion())
		{
			_atacar = StartCoroutine(MovimientoGeneral());
			StartCoroutine(Ronronear());
		}
	}

	private IEnumerator MovimientoGeneral()
	{
		_ruta = new NavMeshPath();
		do
		{
			ActualizarDestino(puntosDeRecorrido[_posicionActual].position);
		}
		while (!RutaPosible(DestinoActual()));
		while (!_muerto)
		{
			if (EnDestino(DestinoActual()))
			{
				Vector3? vector;
				do
				{
					SumarPuntoActual();
					vector = puntosDeRecorrido[_posicionActual].position;
				}
				while (!RutaPosible(vector.Value));
				ActualizarDestino(vector.Value);
			}
			yield return null;
		}
	}

	private void ReproducirAnimacion(Animaciones animacion)
	{
		ResetearTriggers();
		animator.SetTrigger(animacion.ToString());
	}

	private void SumarPuntoActual()
	{
		_posicionActual = ((_posicionActual != puntosDeRecorrido.Length - 1) ? (_posicionActual + 1) : 0);
	}

	public void ActualizarDestino(Vector3 posicion)
	{
		agente.destination = posicion;
	}

	private Vector3 DestinoActual()
	{
		return agente.destination;
	}

	private bool EnDestino(Vector3 posicion)
	{
		return Vector3.Distance(base.transform.position, posicion) <= agente.stoppingDistance;
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

	public void AbrirPuerta(IT_Puerta puerta)
	{
		if (!_abriendoPuerta)
		{
			StartCoroutine(_AbrirPuerta(puerta));
		}
	}

	public void Morir()
	{
		StopCoroutine(_atacar);
		Movimiento(false);
		ReproducirAnimacion(Animaciones.Morir);
		_muerto = true;
		Object.FindObjectOfType<EN_Enemigo>().Ruido(base.transform.position, EN_Enemigo.EventoEspecial.GatoMuere, true);
		agente.enabled = false;
	}

	private bool ComprobarActivacion()
	{
		switch (ES_EstadoJuego.estadoJuego.dificultad)
		{
		case ES_EstadoJuego.Dificultad.Normal:
		case ES_EstadoJuego.Dificultad.Facil:
			base.gameObject.SetActive(false);
			return false;
		case ES_EstadoJuego.Dificultad.Dificil:
			if (gato == Gato.Gato2)
			{
				base.gameObject.SetActive(false);
				return false;
			}
			break;
		}
		return true;
	}

	private IEnumerator _AbrirPuerta(IT_Puerta puerta)
	{
		if (!_muerto && !puerta._abierta && ((puerta.cerradaConLlave && puerta._cerrojoAbierto) || !puerta.cerradaConLlave))
		{
			_abriendoPuerta = true;
			Movimiento(false);
			ReproducirAnimacion(Animaciones.Atacar2);
			yield return new WaitForSeconds(0.7f);
			AudioSource.PlayClipAtPoint(Sonidos.sonidos.razguñada, puerta.transform.position, 0.4f);
			yield return new WaitForSeconds(0.1f);
			puerta.Interaccionar(IT_Interactivo.Acciones.Recoger, false);
			yield return new WaitForSeconds(0.95f);
			Movimiento(true);
			ReproducirAnimacion(Animaciones.Caminar);
			_abriendoPuerta = false;
		}
	}

	private IEnumerator AtacarAlJugador()
	{
		if (_muerto || jugador.Escondido || Time.time - _ultimoAtaque < 2f)
		{
			yield break;
		}
		_ultimoAtaque = Time.time;
		bool yaAviso = false;
		ES_Logros_Activador.logrosActivador.Gato_Ataco();
		_atacando = true;
		Movimiento(false);
		ReproducirAnimacion(Animaciones.Atacar1);
		audioSource.clip = Sonidos.sonidos.gato_atacar;
		audioSource.volume = 0.3f;
		audioSource.Play();
		if (_bajarVelocidadJugador != null)
		{
			StopCoroutine(_bajarVelocidadJugador);
		}
		_bajarVelocidadJugador = StartCoroutine(BajarVelocidadJugador());
		float tiempoRotacion = 0.2f;
		float tiempo = 0f;
		Quaternion rotacionInicial = base.transform.rotation;
		Quaternion rotacionFinal = Quaternion.LookRotation(jugador.transform.position - base.transform.position);
		while (tiempo < 1f)
		{
			base.transform.rotation = Quaternion.Euler(base.transform.eulerAngles.x, Quaternion.Lerp(rotacionInicial, rotacionFinal, tiempo).eulerAngles.y, base.transform.eulerAngles.z);
			tiempo += Time.deltaTime / tiempoRotacion;
			if (tiempo > 0.5f && !yaAviso)
			{
				Object.FindObjectOfType<EN_Enemigo>().Ruido(base.transform.position, EN_Enemigo.EventoEspecial.GatoAtaca);
				yaAviso = true;
			}
			yield return null;
		}
		base.transform.rotation = rotacionFinal;
		yield return new WaitForSeconds(1f);
		Movimiento(true);
		ReproducirAnimacion(Animaciones.Caminar);
		_atacando = false;
	}

	private IEnumerator BajarVelocidadJugador()
	{
		jugador.fp_Controller.walkSpeed = jugador.fp_Controller.velocidadCaminar_Inicial * 0.5f;
		jugador.fp_Controller.crouchSpeed = jugador.fp_Controller.velocidadAgachado_Inicial * 0.5f;
		jugador.fp_CameraLook.headBob.BobFrequency = 0.9f;
		jugador.fp_CameraLook.headBob.BobHeight = 0.2f;
		jugador.fp_CameraLook.headBob.BobSwayAngle = 0.8f;
		float tiempoTransicion2 = 0.1f;
		float tiempo2 = 0f;
		sangre.gameObject.SetActive(true);
		while (tiempo2 < 1f)
		{
			sangre.alpha = Mathf.Lerp(0f, 1f, tiempo2);
			tiempo2 += Time.deltaTime / tiempoTransicion2;
			yield return null;
		}
		yield return new WaitForSeconds(7f);
		tiempoTransicion2 = 0.5f;
		tiempo2 = 0f;
		while (tiempo2 < 1f)
		{
			sangre.alpha = Mathf.Lerp(1f, 0f, tiempo2);
			tiempo2 += Time.deltaTime / tiempoTransicion2;
			yield return null;
		}
		sangre.gameObject.SetActive(false);
		if (jugador.SuperVelocidad_Activado)
		{
			jugador.fp_Controller.walkSpeed = jugador.superVelocidad_velocidadCaminar;
			jugador.fp_Controller.crouchSpeed = jugador.superVelocidad_velocidadAgachado;
		}
		else
		{
			jugador.fp_Controller.walkSpeed = jugador.fp_Controller.velocidadCaminar_Inicial;
			jugador.fp_Controller.crouchSpeed = jugador.fp_Controller.velocidadAgachado_Inicial;
		}
		jugador.fp_CameraLook.headBob.BobFrequency = jugador.fp_Controller.bobFrequency_Inicial;
		jugador.fp_CameraLook.headBob.BobHeight = jugador.fp_Controller.bobHeight_Inicial;
		jugador.fp_CameraLook.headBob.BobSwayAngle = jugador.fp_Controller.bobSwayAngle_Inicial;
		_bajarVelocidadJugador = null;
	}

	private IEnumerator Ronronear()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(25f, 35f));
			while (audioSource.isPlaying || _abriendoPuerta || _atacando)
			{
				yield return null;
			}
			if (_muerto)
			{
				break;
			}
			audioSource.clip = ((Random.Range(0, 2) == 0) ? Sonidos.sonidos.ronroneo : Sonidos.sonidos.maullido);
			audioSource.volume = 0.3f;
			audioSource.Play();
		}
	}

	private void Movimiento(bool agenteEnMovimiento)
	{
		agente.isStopped = !agenteEnMovimiento;
	}

	private void ResetearTriggers()
	{
		animator.ResetTrigger("Caminar");
		animator.ResetTrigger("Atacar1");
		animator.ResetTrigger("Atacar2");
		animator.ResetTrigger("Morir");
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(ES_Tags.Jugador) && !_atacando)
		{
			StartCoroutine(AtacarAlJugador());
		}
	}
}
