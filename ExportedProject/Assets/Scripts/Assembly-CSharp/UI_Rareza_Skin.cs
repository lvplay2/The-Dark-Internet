using UnityEngine;
using UnityEngine.UI;

public class UI_Rareza_Skin : MonoBehaviour
{
	private const int estrellasTotales = 5;

	public Image[] estrellas;

	private void OnEnable()
	{
	}

	public void AsignarRareza(int rareza)
	{
		for (int i = 0; i < estrellas.Length; i++)
		{
			estrellas[i].enabled = rareza > 0;
			rareza--;
		}
	}
}
