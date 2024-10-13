public class PZ_1_Codigo : IT_Recogible
{
	protected override void Start()
	{
		base.Start();
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
	}
}
