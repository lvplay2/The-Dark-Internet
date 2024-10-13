using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ES_EscenaCargando : MonoBehaviour
{
	[Header("Referencias")]
	public Slider barraCargando;

	public GameObject contenedor;

	private bool _enProceso;

	[Header("Cargar Automaticamente")]
	public bool cargarAlIniciar;

	public string escenaCargar;

	public bool cargarConDelay;

	private void Start()
	{
		if (cargarAlIniciar)
		{
			CargarEscenaAsyncronica(escenaCargar);
		}
	}

	public void CargarEscenaAsyncronica(string escena)
	{
		if (!_enProceso)
		{
			if (!contenedor.activeSelf)
			{
				contenedor.SetActive(true);
			}
			StartCoroutine(CargarEscena(escena));
		}
	}

	private IEnumerator CargarEscena(string escena)
	{
		_enProceso = true;
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(escena);
		while (!asyncLoad.isDone)
		{
			float value = Mathf.Clamp01(asyncLoad.progress / 0.9f);
			barraCargando.value = value;
			yield return null;
		}
		_enProceso = false;
	}
}
