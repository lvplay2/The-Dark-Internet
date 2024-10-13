using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PZ_3_Peluches : MonoBehaviour
{
	public PZ_3_Puerta puerta;

	public PZ_3_Peluche[] peluches;

	public Material[] materiales;

	private int _pelucheSeleccionado;

	private MeshRenderer[] _meshRenderers;

	private List<Material> _materialesDesordenados;

	private void Awake()
	{
		_meshRenderers = new MeshRenderer[peluches.Length];
		for (int i = 0; i < peluches.Length; i++)
		{
			_meshRenderers[i] = peluches[i].transform.GetChild(0).GetComponent<MeshRenderer>();
		}
	}

	private void Start()
	{
		_pelucheSeleccionado = Random.Range(0, peluches.Length);
		peluches[_pelucheSeleccionado].Seleccionado = true;
		_materialesDesordenados = Enumerable.ToList(materiales);
		_materialesDesordenados.Mezclar();
		puerta.pelucheSeleccionado = _pelucheSeleccionado;
		puerta.materialesDesordenados = _materialesDesordenados;
		for (int i = 0; i < _meshRenderers.Length; i++)
		{
			_meshRenderers[i].material = _materialesDesordenados[i];
		}
	}
}
