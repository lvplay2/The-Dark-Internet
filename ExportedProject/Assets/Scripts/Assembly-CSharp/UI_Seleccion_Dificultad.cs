using UnityEngine;
using UnityEngine.UI;

public class UI_Seleccion_Dificultad : MonoBehaviour
{
	[Header("Botones")]
	public Image[] botones;

	public Shadow[] sombraBotones;

	public GameObject[] dificultades_Informacion;

	[HideInInspector]
	public int dificultad;

	private Color colorSeleccionado = Color.white;

	private Color colorDeseleccionado = new Color(0.25f, 0.25f, 0.25f, 1f);

	private void Start()
	{
		Dificultad_Seleccionada(3);
	}

	public void Dificultad_Seleccionada(int dificultad)
	{
		this.dificultad = dificultad;
		for (int i = 0; i < botones.Length; i++)
		{
			bool flag = i == dificultad;
			botones[i].color = (flag ? colorSeleccionado : colorDeseleccionado);
			botones[i].transform.localScale = new Vector3(1f, 1f, 1f);
			sombraBotones[i].enabled = flag;
			dificultades_Informacion[i].SetActive(false);
			if (flag)
			{
				botones[i].transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
				botones[i].transform.SetSiblingIndex(botones.Length - 1);
				dificultades_Informacion[i].SetActive(true);
			}
		}
		SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.seleccionar_1, 0.8f);
	}
}
