using UnityEngine;
using UnityEditor;

public class TransformScaleSwitcherWindow : EditorWindow
{
    private SerializedObject serializedObject;
    private Transform[] transformList;
    private Vector2 scrollPosition;
    private Rect dropArea;

    [MenuItem("MH Tool/Transform Scale Switcher")]
    public static void ShowWindow()
    {
        GetWindow<TransformScaleSwitcherWindow>("Scale Switcher");
    }

    private void OnEnable()
    {
        ScriptableObject target = this;
        serializedObject = new SerializedObject(target);
    }

    private void OnGUI()
    {
        GUILayout.Label("Transform Scale Switcher", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        // Create drop area
        dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drop Transforms Here");

        // Handle drag and drop
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    Object[] draggedObjects = DragAndDrop.objectReferences;
                    Transform[] newTransforms = new Transform[draggedObjects.Length];
                    int validCount = 0;

                    foreach (Object obj in draggedObjects)
                    {
                        if (obj is GameObject go)
                        {
                            newTransforms[validCount++] = go.transform;
                        }
                    }

                    if (validCount > 0)
                    {
                        System.Array.Resize(ref newTransforms, validCount);
                        transformList = newTransforms;
                    }
                }
                Event.current.Use();
                break;
        }

        EditorGUILayout.Space(10);

        // Display transforms list with scroll view
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (transformList != null && transformList.Length > 0)
        {
            for (int i = 0; i < transformList.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                transformList[i] = (Transform)EditorGUILayout.ObjectField($"Transform {i}", transformList[i], typeof(Transform), true);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    ArrayUtility.RemoveAt(ref transformList, i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        GUI.enabled = transformList != null && transformList.Length > 0;
        
        if (GUILayout.Button("Switch X-Z Scale"))
        {
            foreach (Transform transform in transformList)
            {
                if (transform != null)
                {
                    Vector3 scale = transform.localScale;
                    float temp = scale.x;
                    scale.x = scale.z;
                    scale.z = temp;
                    transform.localScale = scale;
                }
            }
        }

        if (GUILayout.Button("Clear List"))
        {
            transformList = null;
        }
        
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
    }
}