using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace NobodyMakesGames.Audio
{
	/// <summary>
	/// 
	/// </summary>
	public class AudioReferenceWindow : EditorWindow
	{
		#region Properties And Fields
		private AudioReferenceSO _reference;
		private Vector2 _scrollPos;

		// Playback selection
		private AudioClip _selectedClip;
		private bool _isPlaying = false;

		// Per SoundClipSO array foldout states
		private Dictionary<SoundClipSO, bool> _clipsExpanded = new Dictionary<SoundClipSO, bool>();
		#endregion

		#region Public
		[MenuItem("Window/Audio Reference")]
		public static void OpenWindow()
		{
			GetWindow<AudioReferenceWindow>("Audio Reference");
		}

		public static void Open(AudioReferenceSO reference)
		{
			var window = GetWindow<AudioReferenceWindow>("Audio Reference");
			window._reference = reference;
			window.Show();
		}
		#endregion

		#region Private
		private void OnGUI()
		{
			if (_reference == null)
			{
				EditorGUILayout.HelpBox("No AudioReferenceSO assigned.", MessageType.Info);
				return;
			}

			// Column headers
			EditorGUILayout.BeginHorizontal();
			GUIStyle headerStyle = new GUIStyle(EditorStyles.label)
			{
				fontStyle = FontStyle.Bold,
				fontSize = 14
			};

			EditorGUILayout.LabelField("SoundClipSO", headerStyle, GUILayout.MinWidth(150), GUILayout.ExpandWidth(true));
			EditorGUILayout.LabelField("Output", headerStyle, GUILayout.MinWidth(120), GUILayout.ExpandWidth(true));
			EditorGUILayout.EndHorizontal();
			DrawUILine(Color.gray);

			// Scroll view
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

			foreach (var sound in _reference.AllSounds)
			{
				if (sound == null)
					continue;

				if (!_clipsExpanded.ContainsKey(sound))
					_clipsExpanded[sound] = false; // default expanded

				// Outer box
				// Outer vertical box
				EditorGUILayout.BeginVertical("box");

				// Top row: SoundClipSO + Output
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.ObjectField(sound, typeof(SoundClipSO), false, GUILayout.MinWidth(150), GUILayout.ExpandWidth(true));
				sound.Output = (AudioMixerGroup)EditorGUILayout.ObjectField(sound.Output, typeof(AudioMixerGroup), false, GUILayout.MinWidth(120), GUILayout.ExpandWidth(true));
				EditorGUILayout.EndHorizontal();

				// Only keep the array’s built-in foldout label
				SerializedObject serializedSound = new SerializedObject(sound);
				SerializedProperty clipsProp = serializedSound.FindProperty("Clips");

				// Draw array with its own foldout, no extra title above
				clipsProp.isExpanded = _clipsExpanded[sound];
				EditorGUILayout.PropertyField(clipsProp, true);
				_clipsExpanded[sound] = clipsProp.isExpanded;

				serializedSound.ApplyModifiedProperties();

				EditorGUILayout.EndVertical();


				EditorGUILayout.Space();
			}

			EditorGUILayout.EndScrollView();

			DrawUILine(Color.gray, 2, 5);

			// ===== BOTTOM SECTION: Clip Player =====
			EditorGUILayout.BeginHorizontal();

			// Label takes minimal width
			EditorGUILayout.LabelField("Play Any Clip: ", headerStyle, GUILayout.Width(100));

			// Object field next to it
			_selectedClip = (AudioClip)EditorGUILayout.ObjectField(_selectedClip, typeof(AudioClip), false);

			// Play / Stop buttons
			if (!_isPlaying)
			{
				if (GUILayout.Button("Play", GUILayout.Width(150)))
				{
					if (_selectedClip != null)
					{
						EditorAudioPlayer.StopAll();
						EditorAudioPlayer.Play(_selectedClip);
						_isPlaying = true;
					}
				}
			}
			else
			{
				if (GUILayout.Button("Stop", GUILayout.Width(150)))
				{
					EditorAudioPlayer.StopAll();
					_isPlaying = false;
				}
			}

			EditorGUILayout.EndHorizontal();


			// Repaint to update button state if clip ends
			if (_isPlaying)
				Repaint();
		}

		private void DrawUILine(Color color, int thickness = 2, int padding = 5)
		{
			Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
			r.height = thickness;
			r.y += padding / 2;
			r.x -= 2;
			r.width += 6;
			EditorGUI.DrawRect(r, color);
		}
		#endregion
	}
}
