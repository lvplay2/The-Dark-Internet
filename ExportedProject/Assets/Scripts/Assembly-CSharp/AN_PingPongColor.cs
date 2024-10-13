using UnityEngine;

public class AN_PingPongColor : MonoBehaviour
{
	public Material material;

	private void Update()
	{
		float a = Mathf.PingPong(Time.time, 1f);
		material.color = new Color(1f, 0f, 0f, a);
	}
}
