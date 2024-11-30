using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Setter : MonoBehaviour
{
    private PlayerCamera cameraScript;

    public Vector2 xRestrictions;
    public Vector2 yRestrictions;

    public Transform originalSpawn;



    void Awake() {
        cameraScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        
        float xDifference = originalSpawn.position.x - transform.position.x;
        float yDifference = originalSpawn.position.y - transform.position.y;

        Vector2 originalX = originalSpawn.GetComponent<Camera_Setter>().xRestrictions;
        Vector2 originalY = originalSpawn.GetComponent<Camera_Setter>().yRestrictions;

        

    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            cameraScript.xRestrictions = xRestrictions;
            cameraScript.yRestrictions = yRestrictions;
        }
    }
}
