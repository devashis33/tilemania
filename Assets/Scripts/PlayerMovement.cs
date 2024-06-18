using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(20f,20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] float bulletReleaseDelay ;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    float gravityValueAtStart;
    bool isAlive = true;


    private CinemachineImpulseSource _myImpulseSource;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityValueAtStart = myRigidbody.gravityScale;
        _myImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if(!isAlive){return;}
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }
    void OnFire(InputValue value){
        if(!isAlive){return;}
        if(value.isPressed){
            myAnimator.SetTrigger("Shooting");
            Invoke("BulletFire",bulletReleaseDelay);
        }
        
    }
    void BulletFire(){
        if(myBodyCollider.transform.localScale.x> Mathf.Epsilon){
            Instantiate(bullet,gun.position,transform.rotation * Quaternion.Euler(0f,0f,90f));
        }
        else if(myBodyCollider.transform.localScale.x< Mathf.Epsilon){
            Instantiate(bullet,gun.position,transform.rotation * Quaternion.Euler(0f,0f,270f));
        }
    }
    // getting the key pressed by th einput system
    void OnMove(InputValue value){
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    //making the gameObject jump
    void OnJump(InputValue value){
        if(!isAlive){return;}
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) ){
            return;
        }
        if(value.isPressed){
            myRigidbody.velocity += new Vector2(0f,jumpSpeed);  
        }
    }
    //used for making the gameObject rum
    void Run(){
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        // to set the running sprite
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning",playerHasHorizontalSpeed);

    }
    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed){
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }    
    void ClimbLadder(){
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            myRigidbody.gravityScale = gravityValueAtStart;
            myAnimator.SetBool("isClimbing",false);
            return;
        }
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x,moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        if(playerHasVerticalSpeed){
            myAnimator.SetBool("isClimbing",playerHasVerticalSpeed);
        }
    }
    void Die(){
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enimies","Hazards"))){
            isAlive = false;
            _myImpulseSource.GenerateImpulse(1);
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick; 
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        
    }
}
