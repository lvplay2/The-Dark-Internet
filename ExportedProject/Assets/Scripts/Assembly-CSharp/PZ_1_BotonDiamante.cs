public class PZ_1_BotonDiamante : IT_Recogible
{
	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
	}
}
