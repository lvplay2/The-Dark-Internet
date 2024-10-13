using UnityEngine;

public class PZ_4_TrozoOuija : IT_Recogible
{
	[HideInInspector]
	public int numeroTrozo;

	protected override void Start()
	{
		base.Start();
		base.VisibleParaMano = true;
	}
}
