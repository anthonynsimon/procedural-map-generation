using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelBuilder))]
public class MapGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelBuilder levelBuilder = (LevelBuilder)target;
        
        if (GUILayout.Button("Generate Map"))
        {
            levelBuilder.GenerateMap();
            EditorUtility.SetDirty(levelBuilder);
        }
        if (GUILayout.Button("Meshify Map"))
        {
            levelBuilder.Meshify();
        }
    }
}
