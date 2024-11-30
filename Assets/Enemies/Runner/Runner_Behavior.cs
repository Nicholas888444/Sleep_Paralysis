using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner_Behavior : MonoBehaviour
{
    public float speed;
    public Queue<Transform> runningPoints;

    private Transform targetPosition;
    private Vector3 targetDirection;

    private AudioSource audioSource;
    private float lastPlayTime;

    private bool pause;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    

    public void UpdatePositions(Transform pointsParent) {
        runningPoints = new Queue<Transform>();
        foreach(Transform point in pointsParent) {
            if(point != pointsParent)
                runningPoints.Enqueue(point);
        }

        targetPosition = runningPoints.Dequeue();
    }

    // Update is called once per frame
    void Update()
    {
        targetDirection = (targetPosition.position - transform.position).normalized;

        transform.position += new Vector3(targetDirection.x, targetDirection.y, 0.0f) * speed * Time.deltaTime;

        if(Vector2.Distance(targetPosition.position, transform.position) < 0.1f) {
            if(runningPoints.Count != 0)
                targetPosition = runningPoints.Dequeue();
            else
                Destroy(gameObject);
        }
    }

    public void Pause(bool tog) {
        pause = tog;
        if(pause) {
            audioSource.Stop();
            lastPlayTime = audioSource.time;
        } else {
            audioSource.time = lastPlayTime;
            audioSource.Play();
        }
    }
}
