using UnityEngine;

public class OJ_ParaTirar : IT_Recogible
{
	private OJ_Contenido posiciones;

	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	protected override void Awake()
	{
		base.Awake();
		posiciones = Object.FindObjectOfType<OJ_Contenido>();
	}

	protected override void Start()
	{
		base.Start();
		base.transform.position = posiciones.ObtenerPosicion_ParaTirar().position;
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Random.Range(-90f, 90f), base.transform.eulerAngles.z);
	}
}
