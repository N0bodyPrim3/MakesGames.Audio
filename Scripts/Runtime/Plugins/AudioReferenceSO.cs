using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NobodyMakesGames.Audio
{
	/// <summary>
	/// 
	/// </summary>
	[CreateAssetMenu(menuName = "Audio/Audio Reference")]
	public class AudioReferenceSO : ScriptableObject
	{
		#region Properties And Fields
		public AudioMixer Mixer;
		private List<SoundClipSO> _allSounds = new();
		public IReadOnlyList<SoundClipSO> AllSounds => _allSounds;
		#endregion
#if UNITY_EDITOR
		#region Public
		/// <summary>
		/// Refreshes the list by finding all SoundClipSO assets in the project.
		/// Call this from the inspector context menu.
		/// </summary>
		public void RefreshAllSounds()
		{
			_allSounds.Clear();

			string[] guids = AssetDatabase.FindAssets("t:SoundClipSO");
			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				var sound = AssetDatabase.LoadAssetAtPath<SoundClipSO>(path);
				if (sound != null)
					_allSounds.Add(sound);
			}

			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			Debug.Log($"AudioReferenceSO: Found {_allSounds.Count} SoundClipSO assets.");
		}
		#endregion
#endif
	}
}