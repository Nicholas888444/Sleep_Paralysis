using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hurtbox : MonoBehaviour
{
    public Game_Handler game_Handler;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            game_Handler.Die();
        }
    }
}
