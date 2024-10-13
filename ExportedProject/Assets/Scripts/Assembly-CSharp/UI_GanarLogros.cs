using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GanarLogros : MonoBehaviour
{
	public static UI_GanarLogros ganarLogros;

	public Animator animator;

	private UI_VisualizadorLogros visualizadorLogros;

	private void Awake()
	{
		if (ganarLogros == null)
		{
			ganarLogros = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (ganarLogros != this)
		{
			Object.Destroy(base.gameObject);
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		visualizadorLogros = Object.FindObjectOfType<UI_VisualizadorLogros>();
	}

	public void GanarLogro(int indexLogro)
	{
		ES_EstadoJuego.estadoJuego.DatosControlador.Registrar_Logro_Desbloqueado(indexLogro);
		animator.gameObject.SetActive(true);
		animator.Rebind();
		animator.Play("Animacion");
		if (visualizadorLogros != null)
		{
			UI_VisualizadorLogros.Visualizador_Logros_Aplicar_Cambios visualizador_Logros_Aplicar_Cambios = visualizadorLogros.visualizador_Logros_Aplicar_Cambios;
			if (visualizador_Logros_Aplicar_Cambios != null)
			{
				visualizador_Logros_Aplicar_Cambios();
			}
		}
	}

	public void Desactivarse()
	{
		animator.gameObject.SetActive(false);
	}
}
