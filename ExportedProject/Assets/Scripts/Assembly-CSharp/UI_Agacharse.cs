using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Agacharse : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	[Header("Referencias")]
	public FP_Controller controlador;

	public Image imagen;

	[Header("Sprites")]
	public Sprite hombreParado;

	public Sprite hombreAgachado;

	private void OnEnable()
	{
		imagen.AsignarEstado(true);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		controlador.crouch = ((!controlador.IsCrouching() || !controlador.CanStand()) ? true : false);
		imagen.sprite = (controlador.crouch ? hombreAgachado : hombreParado);
	}
}
