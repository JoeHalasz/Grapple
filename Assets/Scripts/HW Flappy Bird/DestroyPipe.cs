using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyPipe : MonoBehaviour
{
    public float timeTillDespawn;
    GameObject pipeManager;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DespawnObject", timeTillDespawn);
    }

    public void DespawnObject()
    {
        pipeManager = GameObject.Find("PipeManager");
        pipeManager.GetComponent<SpawnPipe>().freezeList.Remove(this.gameObject);
        Destroy(this.gameObject, timeTillDespawn);
    }
}
