using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    bool isColliding;
    public bool isDead;



    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            Debug.Log("You died");
            isDead = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
    }
}
