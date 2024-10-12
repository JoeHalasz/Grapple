using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipespawner : MonoBehaviour
    
{
    // Start is called before the first frame update
    public GameObject tube;

    private float TimeBtwSpawn;
    public float StartTimeBtwSpawn;
    private Vector3 SpawnPos;
    float yPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( TimeBtwSpawn <= 1 )
        {
            yPos = Random.Range(-11.8f, -8.4f);
            SpawnPos = new Vector3(transform.position.x, yPos, transform.position.z);
            Instantiate( tube, SpawnPos , transform.rotation);
            TimeBtwSpawn = StartTimeBtwSpawn;
        }
        else
        {
            TimeBtwSpawn -= Time.deltaTime;
        }
    }
}
