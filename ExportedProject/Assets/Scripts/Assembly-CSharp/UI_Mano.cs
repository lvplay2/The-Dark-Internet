using UnityEngine;

public class UI_Mano : UI_Boton
{
	[Header("Referencias")]
	public JG_Vision vision;

	private void Update()
	{
		imagen.AsignarEstado(vision.ElementoEnVista != null && vision.ElementoEnVista.VisibleParaMano);
	}
}
