using UnityEngine;

public class WindZoneController : MonoBehaviour
{
	public WindZone[] windZones;

	public float valor;

	[ContextMenu("Aplicar")]
	public void Aplicar()
	{
		WindZone[] array = windZones;
		foreach (WindZone obj in array)
		{
			obj.radius *= valor;
			obj.windMain *= valor;
			obj.windTurbulence *= valor;
			obj.windPulseMagnitude *= valor;
			obj.windPulseFrequency *= valor;
		}
	}
}
