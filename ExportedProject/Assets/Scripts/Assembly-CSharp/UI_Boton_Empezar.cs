using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Boton_Empezar : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public CanvasGroup canvasGroupInicio;

	public CanvasGroup canvasGroupEmpezar;

	private Coroutine abrir_Panel_Empezar;

	private Coroutine cerrar_Panel_Empezar;

	public GameObject flechaVolver;

	private bool yaPreisonado;

	public void OnPointerClick(PointerEventData eventData)
	{
		AbrirPanelEmpezar(true);
	}

	public void FlechaCerrar()
	{
		CerrarPanelEmpezar();
	}

	public void AbrirPanelEmpezar(bool conFlechaVolver)
	{
		flechaVolver.SetActive(conFlechaVolver);
		if (abrir_Panel_Empezar != null)
		{
			StopCoroutine(abrir_Panel_Empezar);
		}
		if (cerrar_Panel_Empezar != null)
		{
			StopCoroutine(cerrar_Panel_Empezar);
		}
		abrir_Panel_Empezar = StartCoroutine(Abrir_Panel_Empezar());
	}

	public void CerrarPanelEmpezar()
	{
		if (abrir_Panel_Empezar != null)
		{
			StopCoroutine(abrir_Panel_Empezar);
		}
		if (cerrar_Panel_Empezar != null)
		{
			StopCoroutine(cerrar_Panel_Empezar);
		}
		cerrar_Panel_Empezar = StartCoroutine(Cerrar_Panel_Empezar());
	}

	private IEnumerator Abrir_Panel_Empezar()
	{
		canvasGroupInicio.blocksRaycasts = false;
		canvasGroupEmpezar.blocksRaycasts = true;
		float inicioAlfaInicial = canvasGroupInicio.alpha;
		float empezarAlfaInicial = canvasGroupEmpezar.alpha;
		float tiempo = 0f;
		while (tiempo < 1f)
		{
			canvasGroupInicio.alpha = Mathf.Lerp(inicioAlfaInicial, 0f, tiempo);
			canvasGroupEmpezar.alpha = Mathf.Lerp(empezarAlfaInicial, 1f, tiempo);
			tiempo += Time.deltaTime / 0.8f;
			yield return null;
		}
		canvasGroupInicio.alpha = 0f;
		canvasGroupEmpezar.alpha = 1f;
	}

	private IEnumerator Cerrar_Panel_Empezar()
	{
		canvasGroupInicio.blocksRaycasts = true;
		canvasGroupEmpezar.blocksRaycasts = false;
		float inicioAlfaInicial = canvasGroupInicio.alpha;
		float empezarAlfaInicial = canvasGroupEmpezar.alpha;
		float tiempo = 0f;
		while (tiempo < 1f)
		{
			canvasGroupInicio.alpha = Mathf.Lerp(inicioAlfaInicial, 1f, tiempo);
			canvasGroupEmpezar.alpha = Mathf.Lerp(empezarAlfaInicial, 0f, tiempo);
			tiempo += Time.deltaTime / 0.5f;
			yield return null;
		}
		canvasGroupInicio.alpha = 1f;
		canvasGroupEmpezar.alpha = 0f;
	}
}
