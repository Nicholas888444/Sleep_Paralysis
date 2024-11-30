using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    private Game_Handler game_Handler;

    void Start() {
        game_Handler = FindObjectOfType<Game_Handler>();
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            game_Handler.CollectedCup();
            FindObjectOfType<Text_Box>().TypeText(1);
            Destroy(gameObject);
        }
    }
}
