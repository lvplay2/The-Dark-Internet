using UnityEngine;

public class FP_Input : MonoBehaviour
{
	public bool UseMobileInput = true;

	public Inputs mobileInputs;

	public Vector3 MoveInput()
	{
		return mobileInputs.moveJoystick.MoveInput();
	}

	public Vector2 LookInput()
	{
		if (!(mobileInputs.lookPad != null))
		{
			return Vector2.zero;
		}
		return mobileInputs.lookPad.LookInput();
	}

	public Vector2 ShotInput()
	{
		if (!(mobileInputs.shotButton != null))
		{
			return Vector2.zero;
		}
		return mobileInputs.shotButton.MoveInput();
	}

	public bool Shoot()
	{
		if (!(mobileInputs.shotButton != null))
		{
			return false;
		}
		return mobileInputs.shotButton.IsPressed();
	}

	public bool Reload()
	{
		if (!(mobileInputs.reloadButton != null))
		{
			return false;
		}
		return mobileInputs.reloadButton.OnRelease();
	}

	public bool Run()
	{
		if (!(mobileInputs.runButton != null))
		{
			return false;
		}
		return mobileInputs.runButton.IsPressed();
	}

	public bool Jump()
	{
		if (!(mobileInputs.jumpButton != null))
		{
			return false;
		}
		return mobileInputs.jumpButton.IsPressed();
	}

	public bool Crouch()
	{
		if (!(mobileInputs.crouchButton != null))
		{
			return false;
		}
		return mobileInputs.crouchButton.Toggle();
	}
}
