using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StopMovement : MonoBehaviour
{
    GameObject player;
    GameObject pipe;

    Rigidbody2D rb;
    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        pipe = GameObject.Find("PipeManager");
    }

    public void FreezePos()
    {
        Debug.Log("We are in FreezePos");
        foreach(GameObject pipesToFreeze in pipe.GetComponent<SpawnPipe>().freezeList)
        {
            Debug.Log(pipesToFreeze);
            pipesToFreeze.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

}

