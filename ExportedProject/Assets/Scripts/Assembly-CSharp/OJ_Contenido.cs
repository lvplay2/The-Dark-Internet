using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OJ_Contenido : MonoBehaviour
{
	[Header("Posiciones Objetos")]
	public List<OJ_Posicion> posiciones;

	public List<OJ_Posicion> posiciones_tamaño_pequeño;

	public List<OJ_Posicion> posiciones_tamaño_grande;

	public List<OJ_Posicion> posiciones_tamaño_muy_grande;

	public List<OJ_Posicion> posiciones_bateria;

	public List<OJ_Posicion_ParaTirar> posiciones_para_tirar;

	[ContextMenu("Encontrar")]
	public void Encontrar()
	{
		posiciones = Enumerable.ToList(Object.FindObjectsOfType<OJ_Posicion>());
		posiciones_para_tirar = Enumerable.ToList(Object.FindObjectsOfType<OJ_Posicion_ParaTirar>());
		posiciones_tamaño_pequeño.Clear();
		posiciones_tamaño_grande.Clear();
		posiciones_tamaño_muy_grande.Clear();
		posiciones_bateria.Clear();
		for (int i = 0; i < posiciones.Count; i++)
		{
			switch (posiciones[i].grupo)
			{
			case OJ_Posicion.Grupo.TamañoPequeño:
				posiciones_tamaño_pequeño.Add(posiciones[i]);
				break;
			case OJ_Posicion.Grupo.TamañoGrande:
				posiciones_tamaño_grande.Add(posiciones[i]);
				break;
			case OJ_Posicion.Grupo.TamañoMuyGrande:
				posiciones_tamaño_muy_grande.Add(posiciones[i]);
				break;
			case OJ_Posicion.Grupo.Bateria:
				posiciones_bateria.Add(posiciones[i]);
				break;
			}
		}
	}

	public Transform ObtenerPosicion_ParaTirar()
	{
		int index = Random.Range(0, posiciones_para_tirar.Count);
		Transform result = posiciones_para_tirar[index].transform;
		posiciones_para_tirar.RemoveAt(index);
		return result;
	}

	public OJ_Posicion ObtenerPosicion_Elemento(OJ_Posicion.Grupo grupo)
	{
		List<OJ_Posicion> list;
		switch (grupo)
		{
		case OJ_Posicion.Grupo.TamañoPequeño:
			list = posiciones_tamaño_pequeño;
			break;
		case OJ_Posicion.Grupo.TamañoGrande:
			list = posiciones_tamaño_grande;
			break;
		case OJ_Posicion.Grupo.TamañoMuyGrande:
			list = posiciones_tamaño_muy_grande;
			break;
		case OJ_Posicion.Grupo.Bateria:
			list = posiciones_bateria;
			break;
		default:
			list = null;
			break;
		}
		int index = Random.Range(0, list.Count);
		OJ_Posicion result = list[index];
		list.RemoveAt(index);
		return result;
	}
}
