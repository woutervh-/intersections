using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SphereGenerator))]
public class TextureCreatorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck() && Application.isPlaying)
        {
            (target as SphereGenerator).GenerateVertices();
        }
    }
}
