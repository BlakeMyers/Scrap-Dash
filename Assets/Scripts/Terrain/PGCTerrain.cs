using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum Generator{
    Random,
    SinglePerlin,
    MuliplePerlin
}

[ExecuteInEditMode]
public class PGCTerrain : MonoBehaviour
{
    public Generator generator;

    TerrainData terrainData;
    public float weight;

    public int octaves = 1;
    public float multiplePerlinWeight = 1;

    void OnEnable(){
        this.terrainData = Terrain.activeTerrain.terrainData;

    }

    public void GenerateTerrain(float weight, Generator generator){
        switch(generator){
            case Generator.Random:
                RandomGeneration(weight);
            break;
            
            case Generator.SinglePerlin:
                SPerlinGeneration(weight);
            break;
            
            case Generator.MuliplePerlin:
                MPerlinGeneration(weight);
            break;
        }
    }

    public void ResetTerrain(){
        int size = terrainData.heightmapResolution;
        float[,] heights = new float[size,size];
        
        terrainData.SetHeights(0,0, heights);
    }

    public void RandomGeneration(float weight){
        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        int w = terrainData.heightmapResolution;
        int h = w;
        
        for(int i = 0; i < w; i++){
            for(int j = 0; j < h; j++){
                heights[i, j] += Random.Range(0f, 1f) * weight;
            }
        }

        terrainData.SetHeights(0,0, heights);
    }

    public void SPerlinGeneration(float weight){
        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        int w = terrainData.heightmapResolution;
        int h = w;
        float xpos = Random.Range(0f, terrainData.heightmapResolution);
        float ypos = Random.Range(0f, terrainData.heightmapResolution);
        
        for(int i = 0; i < w; i++){
            for(int j = 0; j < h; j++){
                float height = Mathf.PerlinNoise(
                    xpos + (float)i/(float)w,
                    ypos + (float)j/(float)h);

                heights[i, j] += height * weight;
            }
        }

        terrainData.SetHeights(0,0, heights);
    }

    public void MPerlinGeneration(float weight){
        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        int w = terrainData.heightmapResolution;
        int h = w;
        float[] randPos = new float[octaves * 2];
        for(int i = 0; i < randPos.Length; i++){
            randPos[i] = Random.Range(0f, terrainData.heightmapResolution);
        }

        for(int i = 0; i < w; i++){
            for(int j = 0; j < h; j++){
                float height = 0;
                for(int k = 0; k < octaves; k++){
                    height += Mathf.PerlinNoise(
                                randPos[k]   + ((float)i/(float)w * (float)Mathf.Pow(2, k)),
                                randPos[k+1] + ((float)j/(float)h* (float)Mathf.Pow(2, k))) * (1/Mathf.Pow(2, k));
                }
                heights[i, j] += height * weight;
            }
        }
        terrainData.SetHeights(0,0, heights);
    }
}
