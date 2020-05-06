using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Vector3 bounds;
    public Vector2Int numRays;

    public float spawnRange;
    public float spawnDistance;

    public GameObject[] items;

    RaycastHit[,] hits; 
    bool[,] activeSpots;

    public float spawnCooldown;
    Vector3 offset;
    

    // Start is called before the first frame update
    void Start(){
        hits = new RaycastHit[numRays.x,numRays.y];
        activeSpots = new bool[numRays.x,numRays.y];
        offset = new Vector3(bounds.x * (1 / (float)numRays.x)/2, 0, bounds.z * (1 / (float)numRays.y)/2);
        SpawnableAreas();

        StartCoroutine(ItemSpawnerSystem());
    }

    // Update is called once per frame
    void Update(){

    }

    IEnumerator ItemSpawnerSystem(){

        while(true){

            int index = Random.Range(0, items.Length);
            int x = Random.Range(0, numRays.x);
            int y = Random.Range(0, numRays.y);

            GameObject.Instantiate(items[index], hits[x, y].point, Quaternion.identity, this.transform);
            yield return new WaitForSeconds(spawnCooldown);
        }
    }


    void SpawnableAreas(){
        for(int i = 0; i < numRays.x; i++){
            for(int j = 0; j < numRays.y; j++){
                
                Vector3 position = this.transform.position + offset + new Vector3(bounds.x * ((float)i / (float)numRays.x), 0, bounds.z * ((float)j / (float)numRays.y));

                if(Physics.Raycast(position, Vector3.down, out hits[i,j], Mathf.Abs(bounds.y))){}
            }
        } 


        //Check if this spot in too high or next to one that is too high
        float maxDist = 1.5f * Mathf.Max(bounds.x * (1 / (float)numRays.x), bounds.y * (1 / (float)numRays.y));
        print(maxDist);

        for(int x = 1; x < numRays.x - 1; x++){
            for(int y = 1; y < numRays.y - 1; y++){
                Vector3 offset = new Vector3(1f, 0, 1);
                Vector3 position = this.transform.position + offset + new Vector3(bounds.x * ((float)x / (float)numRays.x), 0, bounds.z * ((float)y / (float)numRays.y));
                float dist = Vector3.Distance(hits[x, y].point, position);
                if(!(dist - spawnRange < spawnDistance && dist + spawnRange > spawnDistance)){
                    activeSpots[x, y] = false;
                    continue;
                }

                for(int i = -1; i <= 1; i++){
                    for(int j = -1; j <= 1; j++){
                        activeSpots[x, y] |= (Vector3.Distance(hits[x, y].point, hits[x + i, y + j].point) < maxDist);
                    }
                }
            }
        }
    }

    public void OnDrawGizmos() {
        for(int i = 0; i < numRays.x; i++){
            for(int j = 0; j < numRays.y; j++){
                Vector3 position = this.transform.position + offset + new Vector3(bounds.x * ((float)i / (float)numRays.x), 0, bounds.z * ((float)j / (float)numRays.y));
                Gizmos.DrawRay(position, Vector3.down);

                if(hits != null){
                    if(activeSpots[i,j])
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;
                    
                    Gizmos.DrawSphere(hits[i,j].point, .5f);
                }

            }
        }
        Gizmos.color = Color.white;


        Gizmos.DrawWireCube(this.transform.position + new Vector3(bounds.x/2, -spawnDistance, bounds.z/2), new Vector3(bounds.x, spawnRange, bounds.z));
        Gizmos.DrawWireCube(this.transform.position + bounds/2, bounds);
    }
}
