using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public float cameraDistance = 1.0f;
    private Camera cam;

    public Vector2 xRestrictions;
    public Vector2 yRestrictions;
    private Vector3 offset;

    void Awake()
    {
        cam = GetComponent<Camera>();
        offset = new Vector3(0.0f, target.GetComponent<CapsuleCollider2D>().offset.y, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10) + offset;

        if(transform.position.x > xRestrictions.x) {
            transform.position = new Vector3(xRestrictions.x, transform.position.y, -10);
        } else if(transform.position.x < xRestrictions.y) {
            transform.position = new Vector3(xRestrictions.y, transform.position.y, -10);
        }

        if(transform.position.y > yRestrictions.x) {
            transform.position = new Vector3(transform.position.x, yRestrictions.x, -10);
        } else if(transform.position.y < yRestrictions.y) {
            transform.position = new Vector3(transform.position.x, yRestrictions.y, -10);
        }

        cam.orthographicSize = cameraDistance;

    }
}
