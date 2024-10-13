using UnityEngine;

public class UI_Volante : UI_Boton
{
	[Header("Referencias")]
	public JG_Vision vision;

	private void Update()
	{
		imagen.AsignarEstado(vision.ElementoEnVista is DR_Dron);
	}
}
