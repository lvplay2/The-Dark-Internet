using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class FP_Joystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
{
	public RectTransform stick;

	public float returnRate = 15f;

	public float dragRadius = 65f;

	public AlphaControll colorAlpha;

	public RectTransform _canvas;

	private bool _returnHandle;

	private bool pressed;

	private bool isEnabled = true;

	private Vector3 globalStickPos;

	private Vector2 stickOffset;

	private CanvasGroup canvasGroup;

	private Vector2 Coordinates
	{
		get
		{
			if (stick.anchoredPosition.magnitude < dragRadius)
			{
				return stick.anchoredPosition / dragRadius;
			}
			return stick.anchoredPosition.normalized;
		}
	}

	public event Action<FP_Joystick, Vector2> OnStartJoystickMovement;

	public event Action<FP_Joystick, Vector2> OnJoystickMovement;

	public event Action<FP_Joystick> OnEndJoystickMovement;

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		pressed = true;
		_returnHandle = false;
		stickOffset = GetJoystickOffset(eventData);
		stick.anchoredPosition = stickOffset;
		if (this.OnStartJoystickMovement != null)
		{
			this.OnStartJoystickMovement(this, Coordinates);
		}
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		stickOffset = GetJoystickOffset(eventData);
		stick.anchoredPosition = stickOffset;
		if (this.OnJoystickMovement != null)
		{
			this.OnJoystickMovement(this, Coordinates);
		}
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		pressed = false;
		_returnHandle = true;
		if (this.OnEndJoystickMovement != null)
		{
			this.OnEndJoystickMovement(this);
		}
	}

	private Vector2 GetJoystickOffset(PointerEventData eventData)
	{
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas, eventData.position, eventData.pressEventCamera, out globalStickPos))
		{
			stick.position = globalStickPos;
		}
		Vector2 vector = stick.anchoredPosition;
		if (vector.magnitude > dragRadius)
		{
			vector = vector.normalized * dragRadius;
			stick.anchoredPosition = vector;
		}
		return vector;
	}

	private void Start()
	{
		canvasGroup = GetComponent("CanvasGroup") as CanvasGroup;
		_returnHandle = true;
		(GetComponent("RectTransform") as RectTransform).pivot = Vector2.one * 0.5f;
		stick.transform.SetParent(base.transform);
		Transform transform2 = base.transform;
	}

	public void ReseteaarJoystick()
	{
		stick.anchoredPosition = Vector2.zero;
	}

	private void FixedUpdate()
	{
		if (_returnHandle)
		{
			if (stick.anchoredPosition.magnitude > Mathf.Epsilon)
			{
				stick.anchoredPosition -= new Vector2(stick.anchoredPosition.x * returnRate, stick.anchoredPosition.y * returnRate) * Time.deltaTime;
			}
			else
			{
				_returnHandle = false;
			}
		}
		switch (isEnabled)
		{
		case true:
		{
			canvasGroup.alpha = (pressed ? colorAlpha.pressedAlpha : colorAlpha.idleAlpha);
			CanvasGroup obj2 = canvasGroup;
			bool interactable = (canvasGroup.blocksRaycasts = true);
			obj2.interactable = interactable;
			break;
		}
		case false:
		{
			canvasGroup.alpha = 0f;
			CanvasGroup obj = canvasGroup;
			bool interactable = (canvasGroup.blocksRaycasts = false);
			obj.interactable = interactable;
			break;
		}
		}
	}

	public Vector3 MoveInput()
	{
		return new Vector3(Coordinates.x, 0f, Coordinates.y);
	}

	public void Rotate(Transform transformToRotate, float speed)
	{
		if (Coordinates != Vector2.zero)
		{
			transformToRotate.rotation = Quaternion.Slerp(transformToRotate.rotation, Quaternion.LookRotation(new Vector3(Coordinates.x, 0f, Coordinates.y)), speed * Time.deltaTime);
		}
	}

	public bool IsPressed()
	{
		return pressed;
	}

	public void Enable(bool enable)
	{
		isEnabled = enable;
	}
}
