using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Vector3 bounds;
    public Vector2Int numRays;

    RaycastHit[,] hits; 
    bool[,] activeSpots;

    // Start is called before the first frame update
    void Start()
    {
        hits = new RaycastHit[numRays.x,numRays.y];
        activeSpots = new bool[numRays.x,numRays.y];
    }

    // Update is called once per frame
    void Update()
    {
        SpawnableAreas();
    }

    void SpawnableAreas(){
        for(int i = 0; i < numRays.x; i++){
            for(int j = 0; j < numRays.y; j++){
                Vector3 position = this.transform.position + new Vector3(bounds.x * ((float)i / (float)numRays.x), 0, bounds.z * ((float)j / (float)numRays.y));

                if(Physics.Raycast(position, Vector3.down, out hits[i,j], Mathf.Abs(bounds.y))){
                    Debug.DrawLine(position, hits[i,j].point, Color.green);
                }
            }
        } 

        float maxDist = 1.5f * Mathf.Max(bounds.x * (1 / (float)numRays.x), bounds.y * (1 / (float)numRays.y));
        print(maxDist);
        for(int x = 1; x < numRays.x - 1; x++){
            for(int y = 1; y < numRays.y - 1; y++){
                for(int i = -1; i <= 1; i++){
                    for(int j = -1; j <= 1; j++){
                        activeSpots[x, y] |= (Vector3.Distance(hits[x, y].point, hits[x + i, y + j].point) > maxDist);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos() {
        for(int i = 0; i < numRays.x; i++){
            for(int j = 0; j < numRays.y; j++){
                Vector3 position = this.transform.position + new Vector3(bounds.x * ((float)i / (float)numRays.x), 0, bounds.z * ((float)j / (float)numRays.y));
                if(hits != null)
                    if(activeSpots[i,j])
                        Gizmos.DrawSphere(hits[i,j].point, .5f);
            }
        }
        Gizmos.DrawWireCube(this.transform.position + bounds/2, bounds);
    }
}
