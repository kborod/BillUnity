using UnityEditor;
using UnityEngine;

namespace Kborod.Services.Localization
{
    public class SynchronizerWindow : EditorWindow
	{
		private Synchronizator synchronizator = new Synchronizator();

		[MenuItem("Tools/Localization/Localization synchronizer")]
		public static void ShowWindow()
		{
			GetWindow<SynchronizerWindow>("Localization synchronizer");
			Selection.activeObject = SyncronizationSettings.Instance;
		}

		void OnGUI()
		{
			GUILayout.Label("Synchronize localization from google sheet", EditorStyles.boldLabel);

			if (GUILayout.Button("Settings"))
			{
				Selection.activeObject = SyncronizationSettings.Instance;
			}

			if (GUILayout.Button("Synchronize"))
			{
				synchronizator.Sync(SyncronizationSettings.Instance.TableId, SyncronizationSettings.Instance.Sheets, SyncronizationSettings.Instance.SaveFolder);
			}
		}
	}
}

/* Copyright: Made by Appfox */