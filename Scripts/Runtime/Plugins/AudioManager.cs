using System.Collections.Generic;
using UnityEngine;

namespace NobodyMakesGames.Audio
{
	public struct SoundClipRow
	{
		public SoundClipSO SoundClip;
		public int SelectedClipIndex;
		public bool IsPlaying;

		public AudioClip CurrentClip => SoundClip.Clips != null && SoundClip.Clips.Length > 0
			? SoundClip.Clips[SelectedClipIndex]
			: null;
	}
	/// <summary>
	/// 
	/// </summary>
	public class AudioManager : MonoBehaviour
	{
		#region Properties And Fields
		public static AudioManager Instance
		{
			get; private set;
		}

		[SerializeField] private int _InitialPoolSize = 10;

		private readonly Queue<SoundEmitter> _Pool = new();
		private readonly Dictionary<SoundClipSO, List<SoundEmitter>> _ActiveEmitters = new();
		#endregion

		#region Lifecycle
		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}

			Instance = this;
			DontDestroyOnLoad(gameObject);

			for (int i = 0; i < _InitialPoolSize; i++)
				_Pool.Enqueue(CreateEmitter());
		}
		#endregion

		#region Public

		public void Play(SoundClipSO sound)
		{
			if (sound == null)
				return;

			var clip = sound.GetClipToPlay();
			if (clip == null)
				return;

			Play(sound, clip);
		}

		public void PlayOneShot(SoundClipSO sound)
		{
			if (sound == null)
				return;

			var clip = sound.GetClipToPlay();
			if (clip == null)
				return;

			PlayOneShot(sound, clip);
		}

		public void Stop(SoundClipSO sound)
		{
			if (sound == null || !_ActiveEmitters.ContainsKey(sound))
				return;

			foreach (var emitter in _ActiveEmitters[sound])
				emitter.Stop();

			_ActiveEmitters[sound].Clear();
		}

		public void Pause(SoundClipSO sound)
		{
			if (sound == null || !_ActiveEmitters.ContainsKey(sound))
				return;

			foreach (var emitter in _ActiveEmitters[sound])
				emitter.Pause();
		}

		#endregion

		#region Private

		public void Play(SoundClipSO sound, AudioClip clip)
		{
			if (_Pool.Count == 0)
				_Pool.Enqueue(CreateEmitter());

			var emitter = _Pool.Dequeue();

			if (!_ActiveEmitters.ContainsKey(sound))
				_ActiveEmitters[sound] = new List<SoundEmitter>();

			_ActiveEmitters[sound].Add(emitter);
			emitter.Play(sound, clip, () => RecycleEmitter(sound, emitter));
		}

		public void PlayOneShot(SoundClipSO sound, AudioClip clip)
		{
			if (_Pool.Count == 0)
				_Pool.Enqueue(CreateEmitter());

			var emitter = _Pool.Dequeue();

			if (!_ActiveEmitters.ContainsKey(sound))
				_ActiveEmitters[sound] = new List<SoundEmitter>();

			_ActiveEmitters[sound].Add(emitter);
			emitter.Play(sound, clip, () => RecycleEmitter(sound, emitter), forceLoop: false);
		}

		private SoundEmitter CreateEmitter()
		{
			var go = new GameObject("SoundEmitter");
			go.transform.SetParent(transform);
			var emitter = go.AddComponent<SoundEmitter>();
			emitter.Initialize();
			return emitter;
		}

		private void RecycleEmitter(SoundClipSO sound, SoundEmitter emitter)
		{
			if (_ActiveEmitters.ContainsKey(sound))
				_ActiveEmitters[sound].Remove(emitter);

			_Pool.Enqueue(emitter);
		}

		#endregion
	}
}
