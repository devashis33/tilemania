using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForPickup = 100;
    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !wasCollected){
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(pointsForPickup);
            AudioSource.PlayClipAtPoint(coinPickupSFX,Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
