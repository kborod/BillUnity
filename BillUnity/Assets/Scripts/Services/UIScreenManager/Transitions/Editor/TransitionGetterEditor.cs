using UnityEditor;

namespace Kborod.Services.UIScreenManager.Transitions
{
    [CustomEditor(typeof(TransitionGetter))]
    public class TransitionGetterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var transitionSourceProperty = serializedObject.FindProperty("_transitionSource");
            bool manualSelect = transitionSourceProperty.enumValueIndex == (int)TransitionSource.ManualSelect;

            if (manualSelect)
                DrawDefaultInspector();
            else
                DrawPropertiesExcluding(serializedObject, "_transitionType");

            serializedObject.ApplyModifiedProperties();
        }
    }
}