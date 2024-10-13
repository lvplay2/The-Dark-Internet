public class UI_Disparar : UI_Boton
{
	protected override void Awake()
	{
		base.Awake();
	}

	private void Update()
	{
		imagen.AsignarEstado(IT_Cartera.cartera.Contiene<PZ_5_Arma>() && ((PZ_5_Arma)IT_Cartera.cartera.ElementoEnCartera).Cargada);
	}
}
