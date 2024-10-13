using System.Collections;
using UnityEngine;

public class PZ_1_Telefono : EN_ObjetoEnemigo
{
	public GameObject telefonoSano;

	public GameObject telefonoRoto;

	public GameObject particulasExplosion;

	public EN_Enemigo enemigo;

	public PZ_1_Celular celular;

	public AudioSource audioLlamada;

	public AudioSource audioDestruirTelefono;

	public Transform puntoNavMesh;

	public GameObject scriptObservar;

	private bool _llamando;

	public override void Interaccionar()
	{
		base.Interaccionar();
		Romper();
	}

	public void Llamar()
	{
		if (!base.Utilizado && !_llamando)
		{
			StartCoroutine(C_Llamar());
		}
	}

	private void Romper()
	{
		if (!base.Utilizado && _llamando)
		{
			StartCoroutine(C_Romper());
		}
	}

	public void CancelarLlamada()
	{
		_llamando = false;
	}

	private IEnumerator C_Romper()
	{
		scriptObservar.gameObject.SetActive(false);
		float seconds = 0.4f;
		enemigo.voces.Actualizar_EventoEspecial(EN_Enemigo.EventoEspecial.RomperTelefono);
		enemigo.voces.ReproducirVoz(true, 100f);
		yield return new WaitForSeconds(seconds);
		base.Utilizado = true;
		celular.Llamo = true;
		CancelarLlamada();
		telefonoSano.SetActive(false);
		telefonoRoto.SetActive(true);
		particulasExplosion.SetActive(true);
		audioDestruirTelefono.Play();
		enemigo.ContinuarPatrullando();
		yield return new WaitForSeconds(3f);
		particulasExplosion.SetActive(false);
	}

	private IEnumerator C_Llamar()
	{
		_llamando = true;
		base.Disponible = true;
		if (!audioLlamada.isPlaying)
		{
			audioLlamada.Play();
		}
		yield return new WaitForSeconds(1f);
		if (!enemigo.PersiguiendoAlJugador())
		{
			enemigo.Ruido(puntoNavMesh.position, EN_Enemigo.EventoEspecial.EscucharTelefono);
		}
		while (_llamando)
		{
			yield return null;
		}
		audioLlamada.Stop();
		base.Disponible = false;
	}
}
