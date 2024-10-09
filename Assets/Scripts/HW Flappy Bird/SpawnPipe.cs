using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPipe : MonoBehaviour
{
    public GameObject pipe;
    public GameObject bird;

    public float pipeSpacing;
    public float spawnTime;
    public float spawnDelay;
    public float spawnPosX;
    float minHeight;
    float maxHeight;

    Vector3 spawnPos;
    float spawnPosY;
    float pipeHeight;
    float spawnHeight;

    float birdHeight;


    // Start is called before the first frame update
    void Start()
    {
        
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);

        Renderer renderPipe = pipe.GetComponent<Renderer>();
        pipeHeight = renderPipe.bounds.size.y;

        Renderer renderBird = bird.GetComponent<Renderer>();
        birdHeight = renderBird.bounds.size.y;
        //  Debug.Log(pipeHeight);

        minHeight = -pipeHeight + birdHeight;
        maxHeight = -(birdHeight*pipeSpacing);
      //  Debug.Log(minHeight);
      //  Debug.Log(maxHeight);
    }

   void SpawnObject()
    {
        spawnHeight = Random.Range(minHeight, maxHeight);
        //bottom pipe
        Instantiate(pipe, new Vector3(spawnPosX, spawnHeight, pipe.transform.position.z), Quaternion.identity);
        //top pipe
        Instantiate(pipe, new Vector3(spawnPosX, (spawnHeight + (pipeHeight + (birdHeight* pipeSpacing))), pipe.transform.position.z), Quaternion.identity);

       // Debug.Log(spawnHeight);
    }
}
