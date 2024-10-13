using System.Collections;
using UnityEngine;

public class PZ_5_CajaArma : IT_Interactivo
{
	public GameObject escopeta;

	private bool _abierta;

	private string observacion = "Parece ser la caja de un arma";

	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion != 0)
		{
			return;
		}
		if (IT_Cartera.cartera.Contiene<DR_Llave>())
		{
			if (!_abierta)
			{
				StartCoroutine(Rotar());
			}
		}
		else
		{
			UI_Canvas.canvas.observacion.Observar(observacion);
		}
	}

	private IEnumerator Rotar()
	{
		_abierta = true;
		escopeta.SetActive(true);
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.abrir_puerta_pequeña_1, base.transform.position, 0.4f, ES_EstadoEscena.estadoEscena.audioGlobal);
		float tiempoDeRotacion = 5f;
		float tiempo = 0f;
		while (tiempo < 1f)
		{
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, Quaternion.Euler(new Vector3(-105f, 0f, 0f)), tiempo);
			tiempo += Time.deltaTime / tiempoDeRotacion;
			yield return null;
		}
	}
}
