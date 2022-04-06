using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();

        //if (GUILayout.Button("Reset Data"))
        //{
        //    GameManager gameManager = (GameManager)target;

        //    Debug.Log("Data reset in editor mode");
        //}

        GUILayout.EndHorizontal();
    }
}