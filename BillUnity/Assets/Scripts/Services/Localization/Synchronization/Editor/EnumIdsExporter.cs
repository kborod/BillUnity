using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Kborod.Services.Localization
{
    public class EnumIdsExporter : EditorWindow
	{
		private string assemblyName = "Code";
		private string enumName = "BuildingType";
		private TranslationForm translationForm = TranslationForm.Standart;

		[MenuItem("Tools/Localization/Enum translation IDs exporter")]
		public static void ShowWindow()
		{
			GetWindow<EnumIdsExporter>("Enum IDs exporter");
		}

		private void OnGUI()
		{
			GUILayout.Label("Copy enum translation ids to clipboard", EditorStyles.boldLabel);

			assemblyName = EditorGUILayout.TextField("Assembly", assemblyName);
			enumName = EditorGUILayout.TextField("EnumName", enumName);
			translationForm = (TranslationForm)EditorGUILayout.EnumPopup("TranslationForm", translationForm);

			if (GUILayout.Button("Copy"))
			{
				var a = GetAssemblyByName(assemblyName);
				if (a == null)
                {
					Debug.Log($"<color=red>Assembly '{assemblyName}' not found</color>");
					return;
				}

				var query = a.GetTypes()
					.Where(t =>
						{
							if (t.IsEnum == false && t.IsPublic == false) return false;
							var typeFullName = t.Name;
							int pos = typeFullName.LastIndexOf(".") + 1;
							var typeName = typeFullName.Substring(pos, typeFullName.Length - pos);
							return typeName == enumName;
						});

				if (query.Count() == 0)
                {
					Debug.Log($"<color=red>Enum '{enumName}' not found in assembly '{assemblyName}'</color>");
					return;
                }
				if (query.Count() > 1)
                {
					Debug.Log($"<color=red>There are more than one Enum by name '{enumName}' in assembly '{assemblyName}'</color>");
					return;
				}

				var postfix = translationForm == TranslationForm.Standart ? string.Empty : $".{translationForm}";

				var s = "";
				foreach (Type t in query)
				{
                    foreach (var item in Enum.GetValues(t))
                    {
						s += $"{t.Name}.{item}{postfix}\n";
					};
				}

				GUIUtility.systemCopyBuffer = s;

				Debug.Log($"<color=green>Enum IDs copied to clipboard</color>");
			}
		}

		private Assembly GetAssemblyByName(string name)
		{
			return AppDomain.CurrentDomain.GetAssemblies().
				   SingleOrDefault(assembly => assembly.GetName().Name == name);
		}
	}
}

/* Copyright: Made by Appfox */