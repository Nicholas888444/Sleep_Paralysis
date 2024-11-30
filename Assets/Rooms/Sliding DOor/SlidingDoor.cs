using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip open, close;

    void Awake() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag == "Player") {
            animator.SetTrigger("Open");
            collider.GetComponent<Character_Controller>().SetHideable(true);
            audioSource.clip = open;
            audioSource.Play();
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.tag == "Player") {
            animator.SetTrigger("Close");
            collider.GetComponent<Character_Controller>().SetHideable(false);
            audioSource.clip = close;
            audioSource.Play();
        }
    }
}
