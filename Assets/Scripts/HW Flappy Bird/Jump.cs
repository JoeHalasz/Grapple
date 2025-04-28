using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    public AudioClip jumpNoise;


    public float jumpForce;
    Rigidbody2D rb;
    public float maxVelocity;
    Death deathComponent;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        deathComponent = GetComponent<Death>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0) && !deathComponent.isDead)
        {
            rb.velocity = new Vector2(0, jumpForce);

            SoundFxManager.instance.PlaySFXClip(jumpNoise, transform);
        }
        if((rb.velocity.y) >= maxVelocity)
        {
            rb.velocity = new Vector2(0, maxVelocity);
        }
      //  Debug.Log(rb.velocityY);
    }
}
