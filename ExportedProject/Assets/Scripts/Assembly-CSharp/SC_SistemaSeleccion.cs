using UnityEngine;

public class SC_SistemaSeleccion
{
	public static int GenerarIndex(float[] probabilidades)
	{
		float num = Random.Range(0f, probabilidades.Suma());
		float num2 = 0f;
		for (int i = 0; i < probabilidades.Length; i++)
		{
			num2 += probabilidades[i];
			if (num <= num2)
			{
				return i;
			}
		}
		return -1;
	}
}
