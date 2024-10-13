using UnityEngine;

public class EN_Sensor : MonoBehaviour
{
	public EN_Enemigo enemigo;

	public DR_Dron dron;

	[Header("Dron")]
	public float distanciaMinima;

	private bool _entroEnZona;

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, dron.transform.position) < distanciaMinima)
		{
			if (dron.Usando && !_entroEnZona)
			{
				enemigo.voces.Actualizar_EventoEspecial(EN_Enemigo.EventoEspecial.VerDron);
				enemigo.voces.ReproducirVoz(true, 100f);
				_entroEnZona = true;
			}
		}
		else
		{
			_entroEnZona = false;
		}
	}
}
