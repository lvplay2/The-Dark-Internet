using System.Collections;
using UnityEngine;

public class PZ_1_CajaFuerte : IT_Interactivo
{
	public Transform caja;

	public GameObject botonDeDiamante;

	public float _grados;

	private float _velocidad = 2f;

	private bool _rotado;

	private string observacion = "Necesitare un codigo para poder abrirla";

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger)
		{
			if (!IT_Cartera.cartera.Contiene<PZ_1_Codigo>())
			{
				UI_Canvas.canvas.observacion.Observar(observacion);
			}
			else if (!_rotado)
			{
				StartCoroutine(Rotar());
			}
		}
	}

	private void Update()
	{
		base.VisibleParaMano = !_rotado;
	}

	private IEnumerator Rotar()
	{
		_rotado = true;
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.abrirCajaFuerte, base.transform.position, 0.3f, ES_EstadoEscena.estadoEscena.audioGlobal);
		botonDeDiamante.SetActive(true);
		float tiempo = Time.time;
		float ejeY = 0f;
		while (Time.time - tiempo < 2f)
		{
			ejeY = Mathf.Lerp(ejeY, _grados, _velocidad * Time.deltaTime);
			caja.localEulerAngles = new Vector3(caja.localEulerAngles.x, ejeY, caja.localEulerAngles.z);
			yield return null;
		}
	}
}
