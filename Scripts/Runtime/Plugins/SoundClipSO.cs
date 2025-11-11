using UnityEngine;
using UnityEngine.Audio;

namespace NobodyMakesGames.Audio
{
	[CreateAssetMenu(menuName = "Audio/Sound Clip")]
	public class SoundClipSO : ScriptableObject
	{
		#region Properties And Fields

		[Header("General Settings")]
		[Range(0f, 1f)] public float Volume = 1f;
		[Range(-3f, 3f)] public float Pitch = 1f;
		public bool PlayOnAwake = false;
		public bool Loop = false;
		[Range(0f, 1.1f)] public float ReverbZoneMix = 1f;
		public bool Mute = false;

		[Header("Pitch Variation")]
		public Vector2 PitchVariance = new Vector2(1f, 1f);

		[Header("Audio Clips")]
		public AudioClip[] Clips;               // Array of clips
		public AudioMixerGroup Output;

		[Header("Options")]
		public bool PlayRandom = false;         // Play a random clip when played

		#endregion

		#region Public Playback Methods

		public void Play()
		{
			if (AudioManager.Instance == null)
			{
				Debug.LogWarning("AudioManager is null! Cannot play sound: " + name);
				return;
			}
			AudioManager.Instance.Play(this);
		}

		public void Stop()
		{
			if (AudioManager.Instance == null)
				return;
			AudioManager.Instance.Stop(this);
		}

		public void Pause()
		{
			if (AudioManager.Instance == null)
				return;
			AudioManager.Instance.Pause(this);
		}

		public void PlayOneShot()
		{
			if (AudioManager.Instance == null)
				return;
			AudioManager.Instance.PlayOneShot(this);
		}

		public float GetRandomPitch() => Random.Range(PitchVariance.x, PitchVariance.y);

		/// <summary>Returns a clip to play, either random or first</summary>
		public AudioClip GetClipToPlay()
		{
			if (Clips == null || Clips.Length == 0)
				return null;

			return PlayRandom ? Clips[Random.Range(0, Clips.Length)] : Clips[0];
		}

		#endregion
	}
}
