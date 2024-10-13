using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Boton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	[Header("Configuración")]
	public IT_Interactivo.Acciones accion;

	protected Image imagen;

	protected int _presionando;

	protected virtual void Awake()
	{
		imagen = GetComponent<Image>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_presionando = 1;
		Click(false);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_presionando = -1;
		Click(true);
	}

	private void Click(bool seSolto)
	{
		ES_Controles.controles.Click(accion, seSolto);
	}
}
