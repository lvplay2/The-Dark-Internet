public class UI_Casete : UI_Boton
{
	private void Update()
	{
		imagen.AsignarEstado(IT_Cartera.cartera.Contiene<PZ_Casete>());
	}
}
