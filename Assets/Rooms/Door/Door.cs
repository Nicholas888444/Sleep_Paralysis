using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider;
    private bool openable;
    private bool opened;
    private AudioSource audioSource;
    public AudioClip open, close;

    void Awake() {
        openable = true;
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayerDoor() {
        if(openable) {
            ToggleDoor();
        }
    }

    public void ToggleDoor() {
        if(opened) {
            animator.SetTrigger("Close");
            opened = false;
            gameObject.layer = 11;
            audioSource.clip = close;
            audioSource.Play();
        } else {
            animator.SetTrigger("Open");
            opened = true;
            gameObject.layer = 12;
            audioSource.clip = open;
            audioSource.Play();
        }
    }

    public void LockDoor(bool tog) {
        openable = !tog;
    }

    public void EnemyDoor() {
        if(openable && !opened) {
            animator.SetTrigger("Open");
            opened = true;
            gameObject.layer = 12;
            audioSource.clip = open;
            audioSource.Play();
        }
    }
}
