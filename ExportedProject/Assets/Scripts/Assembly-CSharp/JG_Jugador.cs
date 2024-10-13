using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class JG_Jugador : MonoBehaviour, IN_IPoder
{
	[Header("Referencias")]
	public JG_Vision vision;

	public JG_Brazo brazo;

	public JG_AnimacionMorir jugadorAnimacionMorir;

	public EN_Enemigo enemigo;

	public FP_Controller fp_Controller;

	public FP_CameraLook fp_CameraLook;

	public Camera camaraJugador;

	public AudioListener audioListener;

	public NavMeshObstacle navMeshObstacle;

	public AudioSource audioJugador;

	[Header("Resurección")]
	public Transform puntoResucitacion;

	public Animator camaraAnimator;

	[Header("Power-Ups Referencias")]
	public Camera camaraOutline;

	public IT_Remarcar[] objetosParaDestacar;

	[HideInInspector]
	public bool Reactivado;

	[HideInInspector]
	public bool Muerto;

	[HideInInspector]
	public bool Escondido;

	[HideInInspector]
	public float superVelocidad_velocidadCaminar = 3.75f;

	[HideInInspector]
	public float superVelocidad_velocidadAgachado = 1.8f;

	[HideInInspector]
	public bool SuperVelocidad_Activado;

	[HideInInspector]
	public float tiempoDeJuego;

	private bool _recolocando;

	private void Awake()
	{
		Inicializar();
	}

	private void Start()
	{
		Escondido = false;
		UI_Canvas.canvas.DesactivarBotones();
		UI_Canvas_Dinamico.canvas_dinamico.Aparecer(UI_Canvas_Dinamico.canvas_dinamico.fondoNegro, 0.75f, true, 1.35f);
		Invoke("CerrarPuerta", 1f);
		jugadorAnimacionMorir.camaraAnimacion.enabled = true;
		jugadorAnimacionMorir.camaraJugador.enabled = false;
		jugadorAnimacionMorir.animator.Play("Inicio");
		Invoke("CambiarCamara", 2.6f);
		InicializarPoderes();
	}

	private void CerrarPuerta()
	{
		AudioSource.PlayClipAtPoint(Sonidos.sonidos.puerta_cerrandose_2, base.transform.position + new Vector3(2f, 0f, 0f), 0.4f);
	}

	private void CambiarCamara()
	{
		UI_Canvas.canvas.ActivarBotones(IT_Interactivo.AccionesPredeterminadas, true);
		jugadorAnimacionMorir.camaraJugador.enabled = true;
		jugadorAnimacionMorir.camaraAnimacion.enabled = false;
	}

	private void Update()
	{
		tiempoDeJuego += Time.deltaTime;
	}

	private void InicializarPoderes()
	{
		int poder_Activado = ES_EstadoJuego.estadoJuego.DatosControlador.extrasActivados.poder_Activado;
		StartCoroutine(Activar_Poder(poder_Activado));
	}

	public IEnumerator Activar_Poder(int poder)
	{
		yield return null;
		switch (poder)
		{
		case 7:
		{
			DesactivarPoder(poder);
			for (int j = 0; j < objetosParaDestacar.Length; j++)
			{
				objetosParaDestacar[j].gameObject.SetActive(true);
				objetosParaDestacar[j].Activar();
			}
			camaraOutline.enabled = true;
			yield return new WaitForSeconds(120f);
			for (int i = 0; i < 8; i++)
			{
				camaraOutline.enabled = !camaraOutline.enabled;
				yield return new WaitForSeconds(0.3f);
			}
			for (int k = 0; k < objetosParaDestacar.Length; k++)
			{
				objetosParaDestacar[k].Desactivar();
			}
			camaraOutline.enabled = false;
			break;
		}
		case 8:
			DesactivarPoder(poder);
			SuperVelocidad_Activado = true;
			fp_Controller.walkSpeed = superVelocidad_velocidadCaminar;
			fp_Controller.crouchSpeed = superVelocidad_velocidadAgachado;
			yield return new WaitForSeconds(120f);
			fp_Controller.walkSpeed = fp_Controller.velocidadCaminar_Inicial;
			fp_Controller.crouchSpeed = fp_Controller.velocidadAgachado_Inicial;
			SuperVelocidad_Activado = false;
			break;
		}
	}

	private void DesactivarPoder(int poder)
	{
		ES_EstadoJuego.estadoJuego.DatosControlador.Desactivar_Extra(poder, ES_Datos_Controlador.TipoOtro.Poder);
	}

	private void Inicializar()
	{
		ES_EstadoEscena.estadoEscena.camaraPrincipal = (ES_EstadoEscena.estadoEscena.camaraJugador = camaraJugador);
		ES_EstadoEscena.estadoEscena.audioListenerJugador = audioListener;
	}

	public void Es_Atacado()
	{
		if (!ES_EstadoEscena.estadoEscena.SinVidas())
		{
			ES_EstadoEscena.estadoEscena.SumarVida();
		}
		if (IT_Cartera.cartera.Contiene<DR_Dron>())
		{
			DR_Dron dR_Dron = (DR_Dron)IT_Cartera.cartera.ElementoEnUso;
			if (dR_Dron == null)
			{
				dR_Dron = (DR_Dron)IT_Cartera.cartera.ElementoEnCartera;
			}
			if (dR_Dron != null && dR_Dron.Usando)
			{
				dR_Dron.DejarDeManejar();
			}
		}
		BloquearMovimiento();
		jugadorAnimacionMorir.Reproducir();
		Reactivado = false;
	}

	public void Obstaculo(bool estado)
	{
		navMeshObstacle.enabled = estado;
	}

	public void Recolocar_Y_Despertar()
	{
		base.transform.position = puntoResucitacion.position;
		base.transform.rotation = puntoResucitacion.rotation;
		camaraAnimator.Play("Despertar");
		fp_Controller.Cancel_Crouch();
		Invoke("Reactivar", 4.5f);
	}

	public void Recolocar_Conducto(Transform origen, Transform final)
	{
		if (!_recolocando)
		{
			StartCoroutine(C_Recolocar_Conducto(origen, final));
		}
	}

	private IEnumerator C_Recolocar_Conducto(Transform origen, Transform final)
	{
		_recolocando = true;
		Escondido = true;
		UI_Canvas_Dinamico.canvas_dinamico.Aparecer(UI_Canvas_Dinamico.canvas_dinamico.fondoNegro, 2f, true, 1f);
		BloquearMovimiento();
		base.transform.position = final.position;
		base.transform.localEulerAngles = new Vector3(0f, final.localEulerAngles.y, 0f);
		base.transform.GetChild(0).localEulerAngles = new Vector3(final.eulerAngles.x, 0f, 0f);
		DesbloquearMovimiento();
		yield return new WaitForSeconds(0.4f);
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.cerrar_compuerta, origen.position, 1f, ES_EstadoEscena.estadoEscena.audioGlobal);
		_recolocando = false;
		yield return new WaitForSeconds(2.6f);
		enemigo.vision._perderDeVista = false;
		Escondido = false;
	}

	public void DesbloquearMovimiento()
	{
		fp_Controller.enabled = true;
		fp_CameraLook.enabled = true;
	}

	public void BloquearMovimiento()
	{
		fp_Controller.enabled = false;
		fp_CameraLook.enabled = false;
	}

	public void ActivarCamara()
	{
		camaraJugador.enabled = true;
		audioListener.enabled = true;
	}

	public void DesactivarCamara()
	{
		camaraJugador.enabled = false;
		audioListener.enabled = false;
	}

	private void Reactivar()
	{
		Reactivado = true;
	}
}
