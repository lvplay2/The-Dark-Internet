using System.Collections;
using UnityEngine;

public class EJ_Observar : IT_Interactivo
{
	public enum Eje
	{
		X = 0,
		Y = 1,
		Z = 2
	}

	public Eje eje = Eje.Y;

	public float distancia;

	[TextArea(2, 3)]
	public string observacion;

	private Transform camaraObjetivo;

	public Vector3 posicionObjetivo;

	public Vector3 rotacionObjetivo;

	public bool cambioRapido;

	private float velocidadPos = 18f;

	private float velocidadRot = 100f;

	private bool procesoActivo;

	private bool observandoElemento;

	private JG_Jugador jugador;

	private void Awake()
	{
		jugador = Object.FindObjectOfType<JG_Jugador>();
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Observar && !procesoActivo)
		{
			StartCoroutine(IniciarProceso());
		}
	}

	private void AccionSecundaria(string accion)
	{
		if (accion.Equals("soltar"))
		{
			observandoElemento = false;
		}
	}

	private IEnumerator IniciarProceso()
	{
		camaraObjetivo = ES_EstadoEscena.estadoEscena.camaraPrincipal.transform;
		observandoElemento = true;
		procesoActivo = true;
		Vector3 posInicial = base.transform.localPosition;
		Quaternion rotInicial = base.transform.localRotation;
		Transform padre = base.transform.parent;
		Transform hijo = base.transform.GetChild(0);
		Quaternion rotHijoInicial = hijo.localRotation;
		int layerInicial = hijo.gameObject.layer;
		hijo.gameObject.layer = LayerMask.NameToLayer("Observar");
		if (hijo.transform.childCount > 0)
		{
			hijo.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Observar");
		}
		base.transform.parent = camaraObjetivo;
		bool restaurado = false;
		bool restauracionIniciada = false;
		UI_Canvas.canvas.observacion.Observar(observacion);
		while (!restaurado)
		{
			if (observandoElemento)
			{
				Vector3 b = new Vector3(0f, 0f, distancia) + posicionObjetivo;
				base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, b, velocidadPos * Time.deltaTime);
				base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, Quaternion.Euler(rotacionObjetivo), velocidadRot / 10f * Time.deltaTime);
				float num = (float)(-(int)Input.GetAxisRaw("Horizontal")) * velocidadRot * Time.deltaTime;
				hijo.Rotate(new Vector3((eje == Eje.X) ? num : 0f, (eje == Eje.Y) ? num : 0f, (eje == Eje.Z) ? num : 0f));
				jugador.fp_Controller.canControl = false;
			}
			else
			{
				if (cambioRapido)
				{
					hijo.gameObject.layer = layerInicial;
					if (hijo.transform.childCount > 0)
					{
						hijo.transform.GetChild(0).gameObject.layer = layerInicial;
					}
				}
				if (!restauracionIniciada)
				{
					UI_Canvas.canvas.observacion.DetenerObservacion();
					restauracionIniciada = true;
				}
				base.transform.parent = padre;
				base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, posInicial, velocidadPos * Time.deltaTime);
				base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, rotInicial, velocidadRot / 10f * Time.deltaTime);
				hijo.localRotation = Quaternion.Slerp(hijo.localRotation, rotHijoInicial, velocidadRot / 5f * Time.deltaTime);
				jugador.fp_Controller.canControl = true;
				if (Vector3.Distance(base.transform.localPosition, posInicial) < 0.001f)
				{
					restaurado = true;
					hijo.gameObject.layer = layerInicial;
					if (hijo.transform.childCount > 0)
					{
						hijo.transform.GetChild(0).gameObject.layer = layerInicial;
					}
				}
			}
			yield return null;
		}
		procesoActivo = false;
	}
}
