using UnityEditor;
using UnityEngine;

namespace NobodyMakesGames.Audio
{
	/// <summary>
	/// 
	/// </summary>
	[CustomEditor(typeof(AudioReferenceSO))]
	public class AudioReferenceSOEditor : Editor
	{
		#region Public
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI(); // Draw default fields

			EditorGUILayout.Space(10); // Space above buttons

			// Box for footer
			EditorGUILayout.BeginVertical("box");

			// Full-width buttons
			if (GUILayout.Button("Open Audio Reference Window", GUILayout.Height(30)))
			{
				AudioReferenceWindow.Open((AudioReferenceSO)target);
			}

			if (GUILayout.Button("Refresh Sounds", GUILayout.Height(30)))
			{
				((AudioReferenceSO)target).RefreshAllSounds();
			}

			EditorGUILayout.EndVertical();
		}


		#endregion
	}
}