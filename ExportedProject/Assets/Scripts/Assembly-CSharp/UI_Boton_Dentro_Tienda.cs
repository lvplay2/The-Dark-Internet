using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Boton_Dentro_Tienda : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Header("Secciones")]
	public GameObject estaSeccion;

	public GameObject otraSeccion;

	public bool seleccionadoPorDefecto;

	[Header("Colores")]
	public MaskableGraphic[] coloresBotonEstaSeccion;

	public MaskableGraphic[] coloresBotonOtraSeccion;

	public Color colorDeseleccionado;

	[Header("Sonidos")]
	public AudioSource seleccionar_1;

	private void Start()
	{
		if (seleccionadoPorDefecto)
		{
			Seleccionar();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Seleccionar();
	}

	private void Seleccionar()
	{
		seleccionar_1.Play();
		estaSeccion.SetActive(true);
		otraSeccion.SetActive(false);
		for (int i = 0; i < coloresBotonEstaSeccion.Length; i++)
		{
			coloresBotonEstaSeccion[i].color = Color.white;
		}
		for (int j = 0; j < coloresBotonOtraSeccion.Length; j++)
		{
			coloresBotonOtraSeccion[j].color = colorDeseleccionado;
		}
	}
}
