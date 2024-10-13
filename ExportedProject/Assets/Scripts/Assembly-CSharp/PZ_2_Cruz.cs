public class PZ_2_Cruz : IT_Recogible
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
