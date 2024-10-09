using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyPipe : MonoBehaviour
{
    public float timeTillDespawn;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timeTillDespawn);
    }

}
