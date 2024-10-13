using UnityEngine;

public class UI_Canvas : MonoBehaviour
{
	public static UI_Canvas canvas;

	[Header("Botones")]
	public Boton[] botones;

	[Header("Otros")]
	public UI_Observacion observacion;

	private void Awake()
	{
		canvas = this;
	}

	private void Start()
	{
		DesactivarBotones();
		ActivarBotones(IT_Interactivo.AccionesPredeterminadas, true);
	}

	public void DesactivarBotones()
	{
		for (int i = 0; i < botones.Length; i++)
		{
			if (botones[i].desactivarImagen)
			{
				botones[i].boton.AsignarEstado(false);
			}
			botones[i].boton.gameObject.SetActive(false);
		}
	}

	public void ActivarBotones(IT_Interactivo.Acciones[] acciones, bool desactivarBotones)
	{
		if (desactivarBotones)
		{
			DesactivarBotones();
		}
		for (int i = 0; i < acciones.Length; i++)
		{
			for (int j = 0; j < botones.Length; j++)
			{
				if (acciones[i] == botones[j].accion)
				{
					botones[j].boton.gameObject.SetActive(true);
				}
			}
		}
	}
}
