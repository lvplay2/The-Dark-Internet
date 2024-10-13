using UnityEngine;

public class UI_Cruz : UI_Boton
{
	private JG_Vision vision;

	protected override void Awake()
	{
		base.Awake();
		vision = Object.FindObjectOfType<JG_Vision>();
	}

	private void Update()
	{
		PZ_2_Espejo pZ_2_Espejo = ((vision.ElementoEnVista is PZ_2_Espejo) ? ((PZ_2_Espejo)vision.ElementoEnVista) : null);
		if (pZ_2_Espejo != null)
		{
			imagen.AsignarEstado(pZ_2_Espejo.cruz == IT_Cartera.cartera.ElementoEnCartera && !pZ_2_Espejo._lanzado);
		}
		else
		{
			imagen.AsignarEstado(false);
		}
	}
}
