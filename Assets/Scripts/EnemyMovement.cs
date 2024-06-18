using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.velocity = new Vector2(moveSpeed,myRigidBody.velocity.y);
    }

    void OnTriggerExit2D(Collider2D other) {
        moveSpeed = -moveSpeed;
        FlipEnemySpirite();
    }
    void FlipEnemySpirite(){
        transform.localScale = new Vector2((Mathf.Sign(moveSpeed)),1f);
    }
    
}
