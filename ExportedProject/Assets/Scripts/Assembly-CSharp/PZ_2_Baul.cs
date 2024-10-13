using System.Collections;
using UnityEngine;

public class PZ_2_Baul : IT_Interactivo
{
	public GameObject cabra;

	private bool _abierto;

	private float _velocidad = 1f;

	public float _grados;

	private string observacion = "Al parecer necesito una llave con forma de cruz";

	private void Start()
	{
		base.VisibleParaMano = !_abierto;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion != 0)
		{
			return;
		}
		if (IT_Cartera.cartera.Contiene<PZ_2_Llave>())
		{
			if (!_abierto)
			{
				StartCoroutine(Abrir());
			}
		}
		else if (!_abierto)
		{
			UI_Canvas.canvas.observacion.Observar(observacion);
		}
	}

	private IEnumerator Abrir()
	{
		_abierto = true;
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.baulAbriendose, base.transform.position, 0.7f, ES_EstadoEscena.estadoEscena.audioGlobal);
		cabra.SetActive(true);
		float tiempo = Time.time;
		while (Time.time - tiempo < 3f)
		{
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, Quaternion.Euler(_grados, 0f, 0f), _velocidad * Time.deltaTime);
			yield return null;
		}
	}
}
