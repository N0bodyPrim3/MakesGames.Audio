#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NobodyMakesGames.Audio
{
	/// <summary>
	/// 
	/// </summary>
	public static class EditorAudioPlayer
	{
		#region Properties And Fields
		private static MethodInfo _playClip;
		private static MethodInfo _stopAllClips;
		private static MethodInfo _isClipPlaying;
		#endregion

		#region Public
		static EditorAudioPlayer()
		{
			var audioUtil = typeof(AudioImporter).Assembly.GetType("UnityEditor.AudioUtil");

			_playClip = audioUtil.GetMethod(
				"PlayPreviewClip",
				BindingFlags.Static | BindingFlags.Public,
				null,
				new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
				null
			);

			_stopAllClips = audioUtil.GetMethod(
				"StopAllPreviewClips",
				BindingFlags.Static | BindingFlags.Public
			);

			// NOTE: This has no params in most Unity versions
			_isClipPlaying = audioUtil.GetMethod(
				"IsPreviewClipPlaying",
				BindingFlags.Static | BindingFlags.Public
			);
		}

		public static void Play(AudioClip clip)
		{
			if (clip == null)
				return;
			_playClip?.Invoke(null, new object[] { clip, 0, false });
		}

		public static void StopAll()
		{
			_stopAllClips?.Invoke(null, null);
		}

		public static bool IsAnyPlaying()
		{
			if (_isClipPlaying == null)
				return false;
			return (bool)_isClipPlaying.Invoke(null, null);
		}
		#endregion
	}
#endif
}