using UnityEditor;

namespace Kborod.Services.UIScreenManager
{
    [CustomEditor(typeof(UIScreensLoader))]
    public class UIScreensLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var searchInAllAssembliesProperty = serializedObject.FindProperty("_searchInAllAssemblies");
            bool searchInAllAssemblies = searchInAllAssembliesProperty.boolValue;

            if (!searchInAllAssemblies)
                DrawDefaultInspector();
            else
                DrawPropertiesExcluding(serializedObject, "_assemblyNames");

            serializedObject.ApplyModifiedProperties();
        }
    }
}