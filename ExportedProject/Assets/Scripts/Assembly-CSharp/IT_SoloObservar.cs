using UnityEngine;

public class IT_SoloObservar : IT_Interactivo
{
	[TextArea(1, 3)]
	public string observacion;

	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger)
		{
			UI_Canvas.canvas.observacion.Observar(observacion);
		}
	}
}
