using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class hlos : MonoBehaviour
{
	public List<Shader> shaderss = new List<Shader>();

	[ContextMenu("Encontrar")]
	public void lala()
	{
		shaderss = Enumerable.ToList(Object.FindObjectsOfType<Shader>());
	}
}
