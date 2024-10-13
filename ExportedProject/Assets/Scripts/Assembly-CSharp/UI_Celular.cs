public class UI_Celular : UI_Boton
{
	public PZ_1_Celular celular;

	private void Update()
	{
		imagen.AsignarEstado(IT_Cartera.cartera.Contiene<PZ_1_Celular>() && !celular.Llamo);
	}
}
