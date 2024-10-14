using System.Collections;
using UnityEngine;

public class SC_Cofre : MonoBehaviour
{
	[Header("Animacion")]
	public Animator animatorCofre;

	public Animator animatorCamaraCofre;

	[Header("Referencias")]
	public UI_VisualizadorSkins visualizador_Skins;

	public SC_SistemaCofre sistema_Cofre;

	public CanvasGroup canvas_Group_Inicio;

	public CanvasGroup canvas_Group_Tienda;

	public GameObject camaraMenu;

	public GameObject camaraAnimacion;

	public UI_Usar botonUsar;

	private const float _tiempoMaximoEspera = 2f;

	private int _toquesRealizados;

	private bool _enProceso;

	private int contador;

	private void Start()
	{
	}

	public void VideoRecompensadoCerrado(bool completado)
	{
		if (!_enProceso)
		{
			animatorCofre.Rebind();
			camaraAnimacion.SetActive(true);
			camaraMenu.SetActive(false);
			canvas_Group_Inicio.alpha = 0f;
			canvas_Group_Tienda.alpha = 0f;
			canvas_Group_Inicio.blocksRaycasts = false;
			canvas_Group_Tienda.blocksRaycasts = false;
			StartCoroutine(ContarToques());
		}
	}

	public void Activar()
	{
	}

	public void Desactivar()
	{
		if (_enProceso)
		{
			StopAllCoroutines();
			_toquesRealizados = 0;
			camaraMenu.SetActive(true);
			camaraAnimacion.SetActive(false);
			canvas_Group_Inicio.alpha = 1f;
			canvas_Group_Tienda.alpha = 1f;
			canvas_Group_Inicio.blocksRaycasts = true;
			canvas_Group_Tienda.blocksRaycasts = true;
			_enProceso = false;
		}
	}

	private IEnumerator ContarToques()
	{
		_enProceso = true;
		float tiempoEspera = 0f;
		bool presionado = true;
		_toquesRealizados = 0;
		while (!Input.GetMouseButtonDown(0))
		{
			yield return null;
		}
		do
		{
			if (presionado)
			{
				_toquesRealizados++;
				tiempoEspera = 0f;
				ResetearTriggers();
				animatorCofre.SetTrigger("Tocado");
			}
			presionado = Input.GetMouseButtonDown(0);
			tiempoEspera += Time.deltaTime;
			yield return null;
		}
		while (_toquesRealizados < 10 && tiempoEspera < 2f);
		ResetearTriggers();
		animatorCofre.SetTrigger("Abrir");
		animatorCamaraCofre.SetTrigger("Subir");
		yield return new WaitForSeconds(1f);
		if (ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Todas_Las_Skins_Desbloqueadas())
		{
			Cofre_Ganar_Poder();
			yield break;
		}
		switch (sistema_Cofre.GenerarPildora())
		{
		case SC_SistemaCofre.Pildora.Skin:
			Cofre_Ganar_Skin();
			break;
		case SC_SistemaCofre.Pildora.Poder:
			Cofre_Ganar_Poder();
			break;
		}
	}

	private void Cofre_Ganar_Skin()
	{
		Ganar_Skin();
	}

	private void Cofre_Ganar_Poder()
	{
		int num = sistema_Cofre.poderes[Random.Range(0, sistema_Cofre.poderes.Length)];
		sistema_Cofre.logros[num].Boton_VisualizarPoder();
		ES_Logros_Activador.logrosActivador.GanarLogro(num);
	}

	private void Cofre_Ganar_HuevoDeOro()
	{
	}

	private void ResetearTriggers()
	{
		animatorCofre.ResetTrigger("Tocado");
		animatorCofre.ResetTrigger("Abrir");
		animatorCamaraCofre.ResetTrigger("Subir");
	}

	private void Ganar_Skin()
	{
		Skin_Informaicion skin_Informaicion = new Skin_Informaicion();
		ES_Skin_Contenedor eS_Skin_Contenedor = sistema_Cofre.GenerarSkin(skin_Informaicion);
		botonUsar.Asigar_Estado(UI_Usar.EstadoBoton.Desactivar);
		contador++;
		if (eS_Skin_Contenedor != null)
		{
			visualizador_Skins.Configurar(UI_VisualizadorSkins.Configuracion.Sin_Interaccion);
			visualizador_Skins.Visualizar_Skin(skin_Informaicion.index, skin_Informaicion.tipoSkin);
		}
		else if (contador < 5)
		{
			Ganar_Skin();
		}
	}
}
