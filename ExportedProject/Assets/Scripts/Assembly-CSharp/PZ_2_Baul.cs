using System.Collections;
using UnityEngine;

public class PZ_2_Baul : IT_Interactivo
{
	public GameObject cabra;

	private bool _abierto;

	private float _velocidad = 1f;

	public float _grados;

	private void Start()
	{
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger && !_abierto && IT_Cartera.cartera.Contiene<PZ_2_Llave>())
		{
			StartCoroutine(Abrir());
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
