using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainController))]
public class ObjectBuilderEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MainController mc = (MainController)target;

        if(GUILayout.Button("Quick Fill GameObject"))
        {
            mc.FillGameObject();
        }
    }
}
