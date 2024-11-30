using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public Game_Handler game_Handler;

    void Start() {
        game_Handler = FindObjectOfType<Game_Handler>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            FindObjectOfType<Game_Handler>().EndGame();
        }
    }
}
