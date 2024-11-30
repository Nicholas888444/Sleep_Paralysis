using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportTo;
    public float distance;
    private PlayerCamera playerCam;
    public Room next;
    private Game_Handler game_Handler;

    void Awake() {
        playerCam = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        game_Handler = GameObject.Find("Game Handler").GetComponent<Game_Handler>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag == "Player") {
            collider.transform.position = new Vector2(teleportTo.position.x, teleportTo.position.y);

            float yMin = playerCam.yRestrictions.y + distance;
            float yMax = playerCam.yRestrictions.x + distance;

            playerCam.yRestrictions = new Vector2(yMax, yMin);
            game_Handler.currentRoom = next;
        }
    }
}
