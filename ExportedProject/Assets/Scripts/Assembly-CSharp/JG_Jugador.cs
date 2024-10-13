using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class JG_Jugador : MonoBehaviour
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

	[HideInInspector]
	public bool Reactivado;

	[HideInInspector]
	public bool Muerto;

	private bool _escondido;

	[HideInInspector]
	public bool Escondido;

	private bool _recolocando;

	private void Awake()
	{
		Inicializar();
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
