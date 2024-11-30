using UnityEngine;

public class Bird_Behavior : MonoBehaviour
{
    public float movementSpeed;
    public float jumpStrength;

    private Rigidbody2D rb;

    private float movement;

    private bool isGrounded;
    private bool isSloped;

    private Animator animator;


    private float groundedCheckValue = 0.02f;

    private LayerMask groundCheckLayer;

    private float waitTimer = 5.0f;
    private float lookTimer = 0.5f;
    public Transform groundCheckEnd;
    private int birdState;
    private Character_Controller player;

    private AudioSource audioSource;
    public AudioClip anger, normal;

    private bool pause;
    private float lastPlayTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        groundCheckLayer = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Behind Mask")) | (1 << LayerMask.NameToLayer("Door"));
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Controller>();

        audioSource = GetComponent<AudioSource>();
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGrounded();

        ArtificalIntelligence();
        AnimationChecks();

        if (movement < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (movement > 0)
            transform.localScale = new Vector3(1, 1, 1);

        if (isGrounded) {
            rb.gravityScale = 3.0f;
            rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
        } else {
            rb.gravityScale = 3.0f;
        }
    }
    

    private void CheckGrounded() {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, groundedCheckValue, groundCheckLayer);
        if(raycastHit.transform != null) {
            isGrounded = true;
        }
        else
            isGrounded = false;
        
    }

    private void Jump()
    {
        if (isGrounded || isSloped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        }
    }

    private void ArtificalIntelligence() {
        //AFK
        if(birdState == 0) {
            movement = 0;
            waitTimer -= Time.deltaTime;

            if(waitTimer <= 0.0f){
                birdState = 1;
                movement = 1;
                waitTimer = Random.Range(3.0f, 10.0f);
            }

            if(LookingAtPlayer()) {
                waitTimer = Random.Range(3.0f, 8.0f);
                audioSource.Stop();
                birdState = 2;
            }
        }
        //Roaming
        else if(birdState == 1) {
            waitTimer -= Time.deltaTime;

            if(!GroundInFront() || WallInFront()) {
                movement *= -1;
            }

            if(LookingAtPlayer()) {
                waitTimer = Random.Range(3.0f, 8.0f);
                audioSource.Stop();
                birdState = 2;
            }

            if(waitTimer <= 0.0f){
                birdState = 0;
                waitTimer = Random.Range(2.0f, 10.0f);
            }
        }
        //Staring 
        else if(birdState == 2) {
            movement = 0;
            waitTimer -= Time.deltaTime;

            if(player.GetMovement() != 0.0f) {
                lookTimer -= Time.deltaTime;

                if(lookTimer <= 0.0f) {
                    birdState = 3;
                    lookTimer = 0.5f;
                    
                    audioSource.clip = anger;
                    audioSource.Play();
                }
            }

            if(waitTimer <= 0.0f) {
                birdState = 0;
                waitTimer = 8.0f;
                audioSource.Play();
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            }
        }
        //Chasing
        else if(birdState == 3) {
            float direction = player.transform.position.x - transform.position.x;
            if(direction < 0) {
                movement = -1;
            } else if(direction > 0) {
                movement = 1;
            }

            waitTimer -= Time.deltaTime;

            if(waitTimer <= 0.0f) {
                birdState = 1;
                audioSource.clip = normal;
                audioSource.Play();
                waitTimer = Random.Range(3.0f, 10.0f);
            }
        }
    }

    private void AnimationChecks() {
        if(movement != 0) {
            animator.SetBool("Walking", true);
        } else {
            animator.SetBool("Walking", false);
        }

        if(LookingAtPlayer() && (movement == 0)) {
            animator.SetBool("Tilt", true);
        } else {
            animator.SetBool("Tilt", false);
        }


    }

    private bool GroundInFront() {

        RaycastHit2D raycastHit = Physics2D.Raycast(groundCheckEnd.position, Vector2.down, 0.1f, groundCheckLayer);
        
        if(raycastHit.transform != null)
            return true;
        else
            return false;

    }

    private bool WallInFront() {

        RaycastHit2D raycastHit = Physics2D.Raycast(groundCheckEnd.position, transform.localScale.x * Vector2.right, 0.7f, groundCheckLayer);
        
        if(raycastHit.transform != null)
            return true;
        else
            return false;

    }

    private bool LookingAtPlayer() {
        RaycastHit2D raycastHit = Physics2D.Raycast(groundCheckEnd.position, transform.localScale.x * Vector2.right, 1.5f, (1 << 6));
        
        if(raycastHit.transform != null)
            return true;
        else
            return false;
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
