using UnityEngine;

public class PZ_Casete : IT_Recogible
{
	public int numeroCasete;

	public GameObject objetoActivar;

	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (!seSolto && accion == Acciones.Casete)
		{
			ReproducirCasete();
		}
	}

	public override void Soltar(Caida caida)
	{
		base.Soltar(caida);
		DetenerCasete();
	}

	public override void Recoger(string ObjetoBrazo)
	{
		base.Recoger(objetoActivar.name);
	}

	private void ReproducirCasete()
	{
		ES_Logros_Activador.logrosActivador.Cinta_Escuchada(numeroCasete);
		ES_Logros_Activador.logrosActivador.Comprobar_Logro_15_Todas_Las_Cintas();
		CancelInvoke("DetenerCasete");
		ES_EstadoEscena.estadoEscena.audioGlobal.audioMixer.SetFloat("AudioGeneral_Volumen", -20f);
		jugador.audioJugador.clip = Sonidos.sonidos.audios_casete[numeroCasete];
		jugador.audioJugador.loop = false;
		jugador.audioJugador.volume = 0.4f;
		jugador.audioJugador.Play();
		Invoke("DetenerCasete", Sonidos.sonidos.audios_casete[numeroCasete].length);
	}

	private void DetenerCasete()
	{
		ES_EstadoEscena.estadoEscena.audioGlobal.audioMixer.SetFloat("AudioGeneral_Volumen", 0f);
		if (jugador.audioJugador.isPlaying)
		{
			jugador.audioJugador.Stop();
		}
	}
}
