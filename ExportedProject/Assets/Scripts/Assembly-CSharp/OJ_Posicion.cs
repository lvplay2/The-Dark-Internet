using UnityEngine;

public class OJ_Posicion : MonoBehaviour
{
	public enum Grupo
	{
		TamañoPequeño = 0,
		TamañoGrande = 1,
		TamañoMuyGrande = 2,
		Bateria = 3
	}

	[Header("Configuración")]
	public Grupo grupo;

	public IT_Interactivo interactivoActivar;

	public Transform ObtenerPosicion(IT_Interactivo objeto)
	{
		interactivoActivar = objeto;
		return base.transform;
	}

	public void Interaccionar()
	{
		if (interactivoActivar != null)
		{
			interactivoActivar.gameObject.SetActive(true);
		}
	}
}
