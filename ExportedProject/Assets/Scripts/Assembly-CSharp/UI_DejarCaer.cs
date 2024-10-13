public class UI_DejarCaer : UI_Boton
{
	private void Update()
	{
		imagen.AsignarEstado(IT_Cartera.cartera.ContieneAlgo());
	}
}
