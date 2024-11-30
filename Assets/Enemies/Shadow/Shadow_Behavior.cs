using UnityEngine;

public class Shadow_Behavior : MonoBehaviour
{
    private Transform player;
    private Character_Controller playerController;
    private float shiftAmount;

    private SpriteRenderer sprite;
    private Color colorChange;

    private Vector3 targetPosition;
    public float moveSpeed;
    public float orbitRadius;
    public float orbitSpeed;
    public float orbitMinusRate;
    private float angle;


    public int shadowState;
    public Volume_Settings volume_Settings;


    private float waitTimer = 60.0f;
    private float lookTimer = 2.0f;
    private float alpha;

    bool pause;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        colorChange = sprite.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<Character_Controller>();

        volume_Settings = FindObjectOfType<Volume_Settings>();
    }

    void Update() {
        VolumeCalculator();
    }

    void FixedUpdate() {
        ArtificialIntelligence();
        colorChange.a = Remap(lookTimer, 2.0f, 0.0f, 1.0f, 0.0f);
        sprite.color = colorChange;
        
        shiftAmount = player.GetComponent<CapsuleCollider2D>().offset.y;
        targetPosition = new Vector3(player.position.x, player.position.y + shiftAmount, transform.position.z);


        //Get near player
        if(shadowState == 0) {
            transform.position = new Vector2(0.0f, 10.0f);

        }
        else if(shadowState == 1) {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        //Circle Player 
        else if(shadowState == 2) {
            // Calculate the angle based on the orbit speed
            angle += orbitSpeed * Time.deltaTime;

            // Convert the angle to radians for trigonometric functions
            float angleInRadians = angle * Mathf.Deg2Rad;

            // Calculate the new position
            float x = targetPosition.x + Mathf.Cos(angleInRadians) * orbitRadius;
            float y = targetPosition.y + Mathf.Sin(angleInRadians) * orbitRadius;
            orbitRadius -= orbitMinusRate*0.01f * Time.deltaTime;

            // Apply the new position
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    private void ArtificialIntelligence() {
        if(shadowState == 0) {
            waitTimer -= Time.deltaTime;

            if(waitTimer <= 0.0f){
                shadowState = 1;
                waitTimer = 60.0f;
            }
        } else if(shadowState == 1) {
            waitTimer -= Time.deltaTime;

            if(waitTimer <= 0.0f){
                shadowState = 0;
                waitTimer = 60.0f;
            }

            if(playerController.lookingAtEnemy) {
                lookTimer -= Time.deltaTime;

                if(lookTimer <= 0.0f) {
                    /*shadowState = 0;
                    waitTimer = 60.0f;
                    lookTimer = 0.0f;*/
                    Destroy(gameObject);
                }
            } else {
                lookTimer += Time.deltaTime;
                if(lookTimer > 2.0f) lookTimer = 2.0f;
            }
        } else if(shadowState == 2) {
            //NOthing
        }
    }

    private void VolumeCalculator() {
        float distance = Vector2.Distance(targetPosition, transform.position);
        volume_Settings.SetSettings(distance);
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void Pause(bool tog) {
        pause = tog;
    }
}
