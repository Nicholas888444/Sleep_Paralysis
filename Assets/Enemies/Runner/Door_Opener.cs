using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Opener : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        print(other.gameObject.name);
        if(other.gameObject.layer == 11) {
            other.GetComponent<Door>().EnemyDoor();
        }
    }
}
