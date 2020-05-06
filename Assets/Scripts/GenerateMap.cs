using PGC_Terrain;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public PGCTerrain pgcTerrain;
    public AnimationCurve buildingHeightDistribution;
    public AnimationCurve buildingFootprintSizeDistribution;
    [Range(10,50)]
    public int numberOfBuildings = 20;
    [Range(5,20)]
    public float maxBuildingHeight = 10;
    [Range(1,5)]
    public float minBuildingHeight = 3;
    [Range(8,20)]
    public float maxBuildingFootprint = 15;
    [Range(3, 8)]
    public float minBuildingFootprint = 4;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRandomTerrain();
        BuildWalls();
        PlaceBuildings();
    }

    private void GenerateRandomTerrain()
    {
        pgcTerrain.perlinScale = 93;
        pgcTerrain.perlinFrequency = 5;
        pgcTerrain.perlinAmplitude = 0.01f;

        pgcTerrain.ApplyPerlinNoise(pgcTerrain.gameObject.GetComponent<Terrain>(), false);
    }

    // Build walls around the outside of the map
    private void BuildWalls()
    {

    }

    private void PlaceBuildings()
    {
        int frequencyResolution = 100;
        List<float> heightFrequencyDistribution = new List<float>();
        List<float> footprintFrequencyDistribution = new List<float>();
        for (int curvePositionIndex = 0; curvePositionIndex < frequencyResolution; curvePositionIndex++)
        {
            float step = (float)curvePositionIndex / (float)frequencyResolution;
            int heightFrequency = (int)(Mathf.Clamp01(buildingHeightDistribution.Evaluate(step)) * frequencyResolution);
            float heightRange = maxBuildingHeight - minBuildingHeight;
            for(int i = 0; i < heightFrequency; i++)
            {
                heightFrequencyDistribution.Add(step * heightRange + minBuildingHeight);
            }


            int footprintFrequency = (int)(Mathf.Clamp01(buildingFootprintSizeDistribution.Evaluate(step)) * frequencyResolution);
            float footprintRange = maxBuildingFootprint - minBuildingFootprint;
            for (int i = 0; i < heightFrequency; i++)
            {
                footprintFrequencyDistribution.Add(step * footprintRange + minBuildingFootprint);
            }
        }

        for(int i = 0; i < numberOfBuildings; i++)
        {
            float buildingHeight = heightFrequencyDistribution[Random.Range(0, heightFrequencyDistribution.Count)];
            float buildingFootprint = footprintFrequencyDistribution[Random.Range(0, footprintFrequencyDistribution.Count)];

            Vector3 buildingPosition = Random.insideUnitSphere * pgcTerrain.gameObject.GetComponent<Terrain>().terrainData.size.x / 2 + new Vector3(pgcTerrain.gameObject.GetComponent<Terrain>().terrainData.size.x, 0, pgcTerrain.gameObject.GetComponent<Terrain>().terrainData.size.x) / 2;
            buildingPosition.y = pgcTerrain.gameObject.GetComponent<Terrain>().SampleHeight(buildingPosition);

            GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
            building.transform.position = buildingPosition;
            building.transform.localScale = new Vector3(buildingFootprint, buildingHeight * 2, buildingFootprint);
            building.transform.rotation = Quaternion.Euler(Random.Range(-7f, 7f), Random.Range(-45f, 45f), Random.Range(-7f, 7f));
        }
    }
}
