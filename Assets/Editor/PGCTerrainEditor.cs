using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PGCTerrain))]
[CanEditMultipleObjects]

public class PGCTerrainEditor : Editor{

    PGCTerrain terrianGenerator;
    SerializedProperty generator;

    public void OnEnable(){
        terrianGenerator = (PGCTerrain)target;
        generator = serializedObject.FindProperty("generator");
    }

    public override void OnInspectorGUI(){
        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(generator);

        switch(terrianGenerator.generator){
            case (Generator.Random):
                GUILayout.Label("Using Random Generation", EditorStyles.boldLabel);
                terrianGenerator.weight = EditorGUILayout.Slider(terrianGenerator.weight, 0, 1);
            break;
            case (Generator.SinglePerlin):
                GUILayout.Label("Using Single Perlin Noise Generation", EditorStyles.boldLabel);
                terrianGenerator.weight = EditorGUILayout.Slider(terrianGenerator.weight, 0, 1);
            break;
            case (Generator.MuliplePerlin):
                GUILayout.Label("Using Multiple Perlin Noise Generation", EditorStyles.boldLabel);
                terrianGenerator.weight = EditorGUILayout.Slider(terrianGenerator.weight, 0, 1);
                terrianGenerator.octaves = EditorGUILayout.IntSlider(terrianGenerator.octaves, 1, 20);
            break;
        }    

        GUILayout.Label("Set the strength of the generation", EditorStyles.boldLabel);

        if(GUILayout.Button("Generate Terrain")){
            terrianGenerator.GenerateTerrain(terrianGenerator.weight, terrianGenerator.generator);
        }

        if(GUILayout.Button("Reset Terrain")){
            terrianGenerator.ResetTerrain();
        }

        serializedObject.ApplyModifiedProperties();
        
    }
}
