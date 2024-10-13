using UnityEngine;
using UnityEngine.Audio;

public class ES_EstadoEscena : MonoBehaviour
{
	public static ES_EstadoEscena estadoEscena;

	[HideInInspector]
	public Camera camaraPrincipal;

	[HideInInspector]
	public Camera camaraJugador;

	[HideInInspector]
	public AudioListener audioListenerJugador;

	public AudioMixerGroup audioGlobal;

	public AudioMixerGroup audioJugador;

	private int vidas;

	private void Awake()
	{
		estadoEscena = this;
	}

	public void SumarVida()
	{
		vidas++;
	}

	public void RestarVida()
	{
		vidas--;
	}

	public bool SinVidas()
	{
		return vidas == 5;
	}

	public int Vidas()
	{
		return vidas;
	}
}
