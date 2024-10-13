using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PZ_Puerta_Final : IT_Interactivo
{
	public static PZ_Puerta_Final puerta_Final;

	public JG_Jugador jugador;

	public AudioMixerGroup audioTotal;

	public BoxCollider colliderPuerta;

	public GameObject fondoNegro;

	public GameObject enemigo;

	[HideInInspector]
	public int puzles_Desbloqueados;

	private bool _desbloqueando;

	private int puzlesTotales;

	public GameObject puzle1;

	public GameObject puzle2;

	public GameObject puzle3;

	public GameObject puzle4;

	public GameObject puzle5;

	private void Awake()
	{
		puerta_Final = this;
	}

	private void Start()
	{
		audioTotal.audioMixer.SetFloat("AudioGeneral_Volumen", -0f);
		puzles_Desbloqueados = 0;
		puzlesTotales = ObtenerPuzlesTotales();
		Desactivar_Todos();
		switch (puzlesTotales)
		{
		case 2:
			puzle1.SetActive(true);
			puzle3.SetActive(true);
			break;
		case 3:
			puzle1.SetActive(true);
			puzle3.SetActive(true);
			puzle4.SetActive(true);
			break;
		case 4:
			puzle1.SetActive(true);
			puzle3.SetActive(true);
			puzle4.SetActive(true);
			puzle2.SetActive(true);
			break;
		case 5:
			puzle1.SetActive(true);
			puzle3.SetActive(true);
			puzle4.SetActive(true);
			puzle2.SetActive(true);
			puzle5.SetActive(true);
			break;
		}
	}

	private int ObtenerPuzlesTotales()
	{
		switch (ES_EstadoJuego.estadoJuego.dificultad)
		{
		case ES_EstadoJuego.Dificultad.Facil:
			return 2;
		case ES_EstadoJuego.Dificultad.Normal:
			return 3;
		case ES_EstadoJuego.Dificultad.Dificil:
			return 4;
		case ES_EstadoJuego.Dificultad.Extremo:
		case ES_EstadoJuego.Dificultad.Fantasma:
			return 5;
		default:
			return 2;
		}
	}

	private void Desactivar_Todos()
	{
		puzle1.SetActive(false);
		puzle2.SetActive(false);
		puzle3.SetActive(false);
		puzle4.SetActive(false);
		puzle5.SetActive(false);
	}

	private void Update()
	{
		base.VisibleParaMano = Puerta_Desbloqueada();
		if (Puerta_Desbloqueada())
		{
			base.gameObject.layer = LayerMask.NameToLayer(ES_Tags.Interactivo);
			if (!colliderPuerta.enabled)
			{
				colliderPuerta.enabled = true;
			}
		}
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (!_desbloqueando)
		{
			StartCoroutine(Abrir_Puerta());
			_desbloqueando = true;
		}
	}

	public void Nuevo_Puzle_Desbloqueado()
	{
		puzles_Desbloqueados++;
	}

	public bool Puerta_Desbloqueada()
	{
		Debug.Log(puzles_Desbloqueados);
		return puzles_Desbloqueados >= puzlesTotales;
	}

	private IEnumerator Abrir_Puerta()
	{
		ES_Logros_Activador.logrosActivador.Comprobar_Logro_11_Tiempo_Limite(jugador.tiempoDeJuego);
		ES_Logros_Activador.logrosActivador.Comprobar_Logro_12_Experto();
		ES_Logros_Activador.logrosActivador.Comprobar_Logro_14_Gatos_Fantasmas();
		enemigo.SetActive(false);
		fondoNegro.SetActive(true);
		audioTotal.audioMixer.SetFloat("AudioGeneral_Volumen", -80f);
		AudioSource.PlayClipAtPoint(Sonidos.sonidos.puerta_abriendose_y_rechinando, base.transform.position, 0.3f);
		yield return new WaitForSeconds(1.3f);
		AudioSource.PlayClipAtPoint(Sonidos.sonidos.estas_muerto, base.transform.position, 0.4f);
		yield return new WaitForSeconds(2.2f);
		Object.FindObjectOfType<ES_EscenaCargando>().CargarEscenaAsyncronica("EscenaGanar");
	}
}
