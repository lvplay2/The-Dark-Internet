using UnityEngine;

public class EX_CopiarTransform_Eje_Y : MonoBehaviour
{
	public Transform objetivoPosicion;

	public Transform objetivoRotacion;

	[HideInInspector]
	public bool copiando = true;

	private void Update()
	{
		if (copiando)
		{
			base.transform.position = objetivoPosicion.position;
			base.transform.eulerAngles = new Vector3(0f, objetivoRotacion.eulerAngles.y, 0f);
		}
	}

	public void AsignarTransform(Transform t)
	{
		base.transform.position = t.position;
		base.transform.eulerAngles = new Vector3(0f, t.eulerAngles.y, 0f);
	}
}
