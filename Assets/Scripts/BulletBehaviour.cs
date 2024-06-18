using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    Rigidbody2D myRidigiBody;
    [SerializeField] float bulletSpeed;
    PlayerMovement player;
    float xSpeed;
    void Start()
    {
        myRidigiBody = GetComponent<Rigidbody2D>();  
        player = FindObjectOfType<PlayerMovement>(); 
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        myRidigiBody.velocity = new Vector2(xSpeed,0f);
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
