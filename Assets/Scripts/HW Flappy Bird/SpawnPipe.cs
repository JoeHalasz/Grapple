using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPipe : MonoBehaviour
{
    public bool willSpawn;
    public List<GameObject> freezeList = new List<GameObject>();

    public GameObject pipe;
    public GameObject bird;

    public GameObject scoreColliderBox;

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
        willSpawn = true;

        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);

        Renderer renderPipe = pipe.GetComponent<Renderer>();
        pipeHeight = renderPipe.bounds.size.y;


        Renderer renderBird = bird.GetComponent<Renderer>();
        birdHeight = renderBird.bounds.size.y;
        //  Debug.Log(pipeHeight);

        minHeight = -pipeHeight + (birdHeight*3);
        maxHeight = -(birdHeight*pipeSpacing) - (birdHeight*3);
      //  Debug.Log(minHeight);
      //  Debug.Log(maxHeight);
     // Debug.Log(renderPipe.bounds.size.x);
    }

   void SpawnObject()
    {
        if(willSpawn)
        {
            spawnHeight = Random.Range(minHeight, maxHeight);

            //bottom pipe
            freezeList.Add(Instantiate(pipe, new Vector3(spawnPosX, spawnHeight, pipe.transform.position.z), Quaternion.identity));
            //top pipe
            freezeList.Add(Instantiate(pipe, new Vector3(spawnPosX, (spawnHeight + (pipeHeight + (birdHeight * pipeSpacing))), pipe.transform.position.z), Quaternion.identity));

            //collider checker for point score
           freezeList.Add(Instantiate(scoreColliderBox, new Vector3(spawnPosX, 0, scoreColliderBox.transform.position.z), Quaternion.identity));

            // Debug.Log(spawnHeight);
        }

    }
}
