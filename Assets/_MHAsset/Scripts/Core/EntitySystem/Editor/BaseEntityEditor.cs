using UnityEngine;
using UnityEditor;

namespace MH.EntitySystem.Editor
{
    [CustomEditor(typeof(BaseEntity), true)]
    public class BaseEntityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BaseEntity baseEntity = (BaseEntity)target;

            EditorGUILayout.Space(10);

            // Store original GUI color
            Color originalColor = GUI.color;

            // Set green color for Find Components button
            GUI.color = Color.green;
            if (GUILayout.Button("Find Components"))
            {
                EntityComponent[] components = baseEntity.GetComponents<EntityComponent>();
                SerializedProperty componentsProp = serializedObject.FindProperty("_components");
                
                componentsProp.arraySize = components.Length;
                for (int i = 0; i < components.Length; i++)
                {
                    componentsProp.GetArrayElementAtIndex(i).objectReferenceValue = components[i];
                }
                
                serializedObject.ApplyModifiedProperties();
            }

            // Set orange color for Clear Components button
            GUI.color = new Color(1f, 0.5f, 0f); // Orange color
            if (GUILayout.Button("Clear Components"))
            {
                SerializedProperty componentsProp = serializedObject.FindProperty("_components");
                componentsProp.arraySize = 0;
                serializedObject.ApplyModifiedProperties();
            }

            // Restore original GUI color
            GUI.color = originalColor;
        }
    }
}