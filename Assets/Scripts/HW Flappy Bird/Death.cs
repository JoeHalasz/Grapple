using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    bool isColliding;
    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            Debug.Log("You died");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
    }
}
