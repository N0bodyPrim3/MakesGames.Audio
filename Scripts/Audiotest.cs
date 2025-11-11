using NobodyMakesGames.Audio;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Audiotest : MonoBehaviour
{
	[SerializeField] private SoundClipSO _SoundToPlay;
	[SerializeField] private float _Interval = 2f;
	[SerializeField] private UnityEvent Event;
	private Coroutine loopCoroutine;

	private void Start()
	{
		loopCoroutine = StartCoroutine(PlayLoop());
	}

	private void OnDestroy()
	{
		if (loopCoroutine != null)
			StopCoroutine(loopCoroutine);
	}

	private IEnumerator PlayLoop()
	{
		while (true)
		{
			if (_SoundToPlay != null)
				_SoundToPlay.Play();

			yield return new WaitForSeconds(_Interval);
		}
	}
}
