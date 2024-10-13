using System.Collections;
using UnityEngine;

public class HauntedVentriloquistSound : MonoBehaviour
{
	private enum iterationsName
	{
		creepiness = 0,
		creepiness2 = 1,
		standup = 2,
		totalHaunted = 3
	}

	[SerializeField]
	private AudioClip[] sounds;

	private AudioSource _audioSource;

	private int iterations;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		StartCoroutine(_mecanimSound());
	}

	private IEnumerator _mecanimSound()
	{
		Animator thisAnim = GetComponent<Animator>();
		int creepiNess = Animator.StringToHash("Creepiness#1 PART 1");
		int creepiness2 = Animator.StringToHash("Creepiness2");
		int sittingToStanding = Animator.StringToHash("STAND UP");
		int totalHaunted = Animator.StringToHash("Creepiness#3");
		while (true)
		{
			switch (iterations)
			{
			case 0:
				if (thisAnim.GetCurrentAnimatorStateInfo(0).shortNameHash == creepiNess && !GetComponent<AudioSource>().isPlaying)
				{
					yield return new WaitForSeconds(0.4f);
					_audioSource.clip = sounds[0];
					GetComponent<AudioSource>().Play();
					yield return StartCoroutine(__nextIteration());
				}
				break;
			case 1:
				yield return new WaitForSeconds(3.6f);
				if (thisAnim.GetCurrentAnimatorStateInfo(0).shortNameHash == creepiness2 && !GetComponent<AudioSource>().isPlaying)
				{
					_audioSource.clip = sounds[1];
					GetComponent<AudioSource>().Play();
					yield return StartCoroutine(__nextIteration());
				}
				break;
			case 2:
				if (thisAnim.GetNextAnimatorStateInfo(0).shortNameHash == sittingToStanding && !GetComponent<AudioSource>().isPlaying)
				{
					_audioSource.clip = sounds[2];
					GetComponent<AudioSource>().Play();
					yield return StartCoroutine(__nextIteration());
				}
				break;
			case 3:
				if (thisAnim.GetNextAnimatorStateInfo(0).shortNameHash == totalHaunted && !GetComponent<AudioSource>().isPlaying)
				{
					_audioSource.clip = sounds[3];
					yield return new WaitForSeconds(0.5f);
					GetComponent<AudioSource>().Play();
					yield return StartCoroutine(__nextIteration());
				}
				break;
			}
			if (iterations == 4)
			{
				iterations = 0;
			}
			yield return null;
		}
	}

	private IEnumerator __nextIteration()
	{
		iterations++;
		yield return null;
	}
}
