using UnityEngine;

public class Editor_Controlador : MonoBehaviour
{
	[Header("Iluminación")]
	public float porcentajeIntensidad;

	public float porcentajeIndirecto;

	[Header("Estático")]
	public bool estatico;

	[ContextMenu("Modificar Iluminación")]
	public void _Iluminacion()
	{
		Light[] array = Object.FindObjectsOfType<Light>();
		foreach (Light obj in array)
		{
			obj.intensity *= porcentajeIntensidad;
			obj.bounceIntensity *= porcentajeIndirecto;
		}
	}

	[ContextMenu("Estático")]
	public void _Estatico()
	{
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].isStatic = estatico;
		}
	}

	[ContextMenu("Layers Por Defecto")]
	public void _LayersPorDefecto()
	{
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].layer = LayerMask.NameToLayer("Default");
		}
	}
}
