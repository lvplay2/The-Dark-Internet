using UnityEngine;

public class EN_Menu : MonoBehaviour
{
	public Animator animator;

	public AudioSource tecleando;

	public AudioSource murmullo;

	private string trigger1 = "Otro_1";

	private string trigger2 = "Otro_2";

	private int _trigger;

	private void Start()
	{
		_trigger = 1;
		Invoke("ReproducirTrigger", Random.Range(5f, 8f));
	}

	private void ReproducirTrigger()
	{
		ResetearTriggers();
		if (_trigger == 1)
		{
			animator.SetTrigger(trigger1);
			_trigger = 2;
			Invoke("ReproducirTrigger", Random.Range(15f, 20f));
			Invoke("DesactivarAudioSource", 0.4f);
			Invoke("ReactivarAudioSource", 4.7f);
		}
		else if (_trigger == 2)
		{
			tecleando.Stop();
			animator.SetTrigger(trigger2);
			_trigger = 1;
			Invoke("ReproducirTrigger", Random.Range(15f, 20f));
			Invoke("DesactivarAudioSource", 0.4f);
			Invoke("ReactivarAudioSource", 4.7f);
		}
	}

	private void ResetearTriggers()
	{
		animator.ResetTrigger(trigger1);
		animator.ResetTrigger(trigger2);
	}

	private void DesactivarAudioSource()
	{
		tecleando.Stop();
		murmullo.Play();
	}

	private void ReactivarAudioSource()
	{
		tecleando.Play();
	}
}
