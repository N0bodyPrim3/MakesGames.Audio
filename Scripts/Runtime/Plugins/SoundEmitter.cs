using System;
using System.Collections;
using UnityEngine;

namespace NobodyMakesGames.Audio
{
	/// <summary>
	/// 
	/// </summary>
	public class SoundEmitter : MonoBehaviour
	{
		#region Properties And Fields
		private AudioSource _source;
		private Action _onFinished;
		#endregion

		#region Public 
		public void Initialize()
		{
			_source = gameObject.AddComponent<AudioSource>();
			_source.playOnAwake = false;
		}

		// Updated to accept any clip
		public void Play(SoundClipSO sound, AudioClip clip, Action finishedCallback, bool? forceLoop = null)
		{
			if (clip == null)
			{
				Debug.LogWarning("SoundEmitter.Play called with null clip!");
				return;
			}

			_onFinished = finishedCallback;

			_source.clip = clip;
			_source.volume = sound.Volume;
			_source.pitch = UnityEngine.Random.Range(sound.PitchVariance.x, sound.PitchVariance.y);
			_source.loop = forceLoop ?? sound.Loop;
			_source.mute = sound.Mute;
			_source.reverbZoneMix = sound.ReverbZoneMix;
			_source.outputAudioMixerGroup = sound.Output;

			_source.Play();

			if (!(_source.loop))
				StartCoroutine(WaitForEnd());
		}

		public void Stop()
		{
			StopAllCoroutines();
			_source.Stop();
			_onFinished?.Invoke();
		}

		public void Pause()
		{
			_source.Pause();
		}
		#endregion

		#region Private
		private IEnumerator WaitForEnd()
		{
			yield return new WaitWhile(() => _source.isPlaying);
			_onFinished?.Invoke();
		}
		#endregion
	}
}
