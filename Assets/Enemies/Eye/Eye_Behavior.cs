using UnityEngine;

public class Eye_Behavior : MonoBehaviour
{
    private Transform player;
    private Character_Controller playerController;
    private float shiftAmount;

    private Vector3 targetPosition;
    public float moveSpeed;


    public int eyeState;


    public float waitTimer = 60.0f;
    public float lookTimer = 1.0f;

    private Animator animator;
    private AudioSource audioSource;

    private bool pause;
    private float lastPlayTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<Character_Controller>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        ArtificialIntelligence();
        
        
        shiftAmount = player.GetComponent<CapsuleCollider2D>().offset.y;
        targetPosition = new Vector3(player.position.x, player.position.y + shiftAmount, transform.position.z);


        //Get near player
        if(eyeState == 0) {
            

        }
        //Chase player
        else if(eyeState == 2) {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void ArtificialIntelligence() {
        if(eyeState == 0) {

            if(playerController.lookingAtEnemy && (playerController.GetObserved() == gameObject)) {
                lookTimer -= Time.deltaTime;

                if(lookTimer <= 0.0f) {
                    eyeState = 1;
                    animator.SetTrigger("Mutate");
                }
            } else {
                lookTimer += Time.deltaTime;
                if(lookTimer > 1.0f) lookTimer = 1.0f;
            }
        } else if(eyeState == 1) {
            waitTimer -= Time.deltaTime;

            if(waitTimer <= 0.0f){
                eyeState = 2;
                waitTimer = 2.0f;
                animator.SetTrigger("Open");
                audioSource.Play();
            }
        } else if(eyeState == 2) {
            //print(Vector2.Distance(transform.position, player.transform.position));
            if(Vector2.Distance(transform.position, player.transform.position) < 0.25f) {
                print("HELp");
                if(player.GetComponent<Character_Controller>().isHiding()) {
                    waitTimer -= Time.deltaTime;

                    if(waitTimer <= 0.0f) {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void Pause(bool tog) {
        pause = tog;
        if(pause) {
            audioSource.Stop();
            lastPlayTime = audioSource.time;
        } else {
            if(eyeState == 2) {
                audioSource.time = lastPlayTime;
                audioSource.Play();
            }
        }
    }
}
