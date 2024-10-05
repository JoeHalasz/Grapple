using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class callforthviolence : MonoBehaviour

{
    public GameObject GameObject1;
    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var bullet = Instantiate(bulletPrefab, GameObject1.transform.position, transform.rotation) as GameObject;

            bullet.transform.Translate(Vector2.right * 10f * Time.deltaTime);


        }
    }
}
