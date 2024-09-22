using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogJump : MonoBehaviour
{
    Rigidbody2D dog;
    float addedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        dog = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        addedTime += Time.deltaTime;
        if (addedTime > 5)
        {
            dog.AddForce(new Vector2(0, 500));
            dog.AddTorque(163);
            addedTime = 0;
        }
        Debug.Log(dog.position);
    }
}
