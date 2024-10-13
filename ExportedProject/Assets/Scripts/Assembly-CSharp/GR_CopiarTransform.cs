using UnityEngine;

public class GR_CopiarTransform : MonoBehaviour
{
	public Transform objetivo;

	private void LateUpdate()
	{
		base.transform.position = objetivo.transform.position;
	}
}
