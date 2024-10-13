using UnityEngine;

[ExecuteInEditMode]
public class EN_Ojos : MonoBehaviour
{
	public Transform mirada;

	private void Update()
	{
		base.transform.LookAt(mirada, base.transform.up);
		base.transform.Rotate(0f, -90f, 0f, Space.Self);
	}
}
