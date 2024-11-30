using UnityEngine;
using UnityEngine.InputSystem;

public class Character_Controller : MonoBehaviour
{
    
    public float movementSpeed;
    public float runningSpeed;
    public float crouchingSpeed;
    public float crawlingSpeed;
    public float wallMovementSpeed;
    public float jumpStrength;

    private Rigidbody2D rb;
    private PlayerController controls;

    private float movement;

    private bool isGrounded;
    private bool running;
    private bool crouching;
    private bool crawling;
    private bool isSloped;

    private float upDown;
    private float upDownTimer;

    private int playerlevel = 0;

    private Animator animator;
    private CapsuleCollider2D capsule;

    private Vector2 crawlSize = new Vector2(0.4840794f, 0.1255283f);
    private Vector2 offsetCrawl = new Vector2(0.0f, 0.04f);
    private Vector2 standSize = new Vector2(0.1255283f, 0.4840794f);
    private Vector2 offsetStand = new Vector2(0.0f, 0.2360024f);
    private Vector2 crouchSize = new Vector2(0.1255283f, 0.4172407f);
    private Vector2 offsetCrouch = new Vector2(0.0f, 0.202583f);

    [SerializeField] private FieldOfView fieldOfView;
    public Transform eyesObject;

    private Mouse mouse;
    private Vector3 mousePos;

    private bool clicking;
    private bool clicked;

    public Texture2D normalCursor;
    public CursorMode cursorMode = CursorMode.Auto;

    private bool hideable, hiding;
    private bool freezeMovement, paused;

    private float groundedCheckValue = 0.02f;
    private float slopeCheckValue = 0.05f;//0.03864431f;
    private Vector2 slopeNormal;

    private LayerMask groundCheckLayer;
    private LayerMask slopeCheckLayer;
    private LayerMask obstructionCheckLayer;
    private Vector2 previousSlopeSpeed;

    public bool lookingAtEnemy;
    private GameObject observedEnemy;

    private void Awake()
    {
        obstructionCheckLayer = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Behind Mask")) | (1 << LayerMask.NameToLayer("Door")) | (1 << LayerMask.NameToLayer("Default"));
        groundCheckLayer = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Behind Mask")) | (1 << LayerMask.NameToLayer("Door"));
        slopeCheckLayer = (1 << LayerMask.NameToLayer("Slope"));
        capsule = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerController();

        controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<float>();
        controls.Player.Movement.canceled += _ => movement = 0;

        //controls.Player.Crouch.started += _ => crouching = true;
        //controls.Player.Crouch.canceled += _ => crouching = false;

        controls.Player.Sprint.started += _ => running = true;
        controls.Player.Sprint.canceled += _ => running = false;

        //controls.Player.Crawl.started += _ => crawling = true;
        //controls.Player.Crawl.canceled += _ => crawling = false;

        controls.Player.UpDown.performed += ctx => upDown = ctx.ReadValue<float>();
        controls.Player.UpDown.canceled += _ => upDown = 0;

        //controls.Player.WallClimb.performed += ctx => wallMovement = ctx.ReadValue<float>();
        //controls.Player.WallClimb.canceled += _ => wallMovement = 0;

        controls.Player.Jump.started += _ => Jump();
        //controls.Player.Leap.started += _ => Leap();
        mouse = Mouse.current;

        controls.Player.Click.started += _ => clicking = true;
        controls.Player.Click.canceled += _ => clicking = false;
    }

    private void OnEnable() => controls.Enable();

    private void OnDisable() => controls.Disable();

    // Start is called before the first frame update
    void Start()
    {
        //CursorPictures();
    }

    void Update()
    {
        if(!paused) {
            mousePos = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
            Vector3 aimDir = (mousePos - eyesObject.position).normalized;
            fieldOfView.SetAimDirection(aimDir);
            fieldOfView.SetOrigin(eyesObject.position);

            HidingSpotFinder();
            DoorFinder();
            SeeGhost(aimDir);

            if (movement < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (movement > 0)
                transform.localScale = new Vector3(1, 1, 1);
            PlayerState();
            AnimationChecks();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!paused) {
            CheckGrounded();
            CheckSloped();
            if(!freezeMovement) {
                CheckGrounded();
                CheckSloped();
                if (isGrounded) {
                    rb.gravityScale = 3.0f;
                    //Check if running or not
                    if(running && !crouching && !crawling) {
                        rb.velocity = new Vector2(movement * runningSpeed, rb.velocity.y);
                        StandCollider();
                    } else if(crouching) {
                        rb.velocity = new Vector2(movement * crouchingSpeed, rb.velocity.y);
                        CrouchCollider();
                    } else if(crawling) {
                        rb.velocity = new Vector2(movement * crawlingSpeed, rb.velocity.y);
                        CrawlCollider();
                    } else {
                        rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
                        StandCollider();
                    }
                } else if(isSloped) {
                    rb.gravityScale = 0.0f;
                    rb.velocity -= previousSlopeSpeed;
                    if(running && !crouching && !crawling) {
                        previousSlopeSpeed = slopeNormal * movement * runningSpeed;
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y) + previousSlopeSpeed;
                        StandCollider();
                    } else if(crouching) {
                        previousSlopeSpeed = slopeNormal * movement * crouchingSpeed;
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y) + previousSlopeSpeed;
                        CrouchCollider();
                    } else if(crawling) {
                        previousSlopeSpeed = slopeNormal * movement * crawlingSpeed;
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y) + previousSlopeSpeed;
                        CrawlCollider();
                    } else {
                        previousSlopeSpeed = slopeNormal * movement * movementSpeed;
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y) + previousSlopeSpeed;
                        StandCollider();
                    }

                    if(movement == 0) {
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                    }
                    
                } else {
                    if(running) {
                        rb.velocity = new Vector2(movement * runningSpeed, rb.velocity.y);
                    } else {
                        rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
                    }
                    rb.gravityScale = 3.0f;
                }
            } else {
                rb.velocity = Vector2.zero;
                if(!hiding)
                    rb.gravityScale = 3.0f;
                else
                    rb.gravityScale = 0.0f;
            }

            if(!isGrounded && !isSloped)  {
                playerlevel = 0;
                crawling = false;
                crouching = true;
                StandCollider();
            }
            
        }
        
        
    }

    private void Jump()
    {
        if (isGrounded || isSloped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            previousSlopeSpeed = Vector2.zero;
        }
    }

    private void CheckGrounded() {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, groundedCheckValue, groundCheckLayer);
        if(raycastHit.transform != null) {
            isGrounded = true;
            previousSlopeSpeed = Vector2.zero;
        }
        else
            isGrounded = false;
        
    }

    private void CheckSloped() {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, slopeCheckValue, slopeCheckLayer);
        
        if(raycastHit.transform != null) {
            Vector2 normal = raycastHit.normal;
            slopeNormal = new Vector2(normal.y, -normal.x);
            //previousSlopeSpeed = Vector2.zero;
            if(!isSloped)  {
                previousSlopeSpeed = Vector2.zero;
                rb.velocity = Vector2.zero;
            }
            
            isSloped = true;
        }
        else
            isSloped = false;
    }

    private void AnimationChecks() {
        animator.SetBool("Moving", (movement != 0));
        animator.SetBool("InAir", !isGrounded && !isSloped);
        animator.SetBool("Walking", (movement != 0) && (isGrounded || isSloped));
        animator.SetBool("Running", (movement != 0) && (isGrounded || isSloped) && running && !crouching && !crawling);
        animator.SetBool("Crouching", (isGrounded || isSloped) && crouching && !crawling);
        animator.SetBool("Crawling", (isGrounded || isSloped) && !crouching && crawling);
    }

    private void CrawlCollider() {
        capsule.direction = CapsuleDirection2D.Horizontal;
        capsule.size = crawlSize;
        capsule.offset = offsetCrawl;

        groundedCheckValue = 0.04f;
        eyesObject.localPosition = new Vector2(0.214f, 0.077f);
    }

    private void StandCollider() {
        capsule.direction = CapsuleDirection2D.Vertical;
        capsule.size = standSize;
        capsule.offset = offsetStand;

        groundedCheckValue = 0.02f;
        eyesObject.localPosition = new Vector2(0.07f, 0.439f);
    }

    private void CrouchCollider() {
        capsule.direction = CapsuleDirection2D.Vertical;
        capsule.size = crouchSize;
        capsule.offset = offsetCrouch;

        groundedCheckValue = 0.02f;
        eyesObject.localPosition = new Vector2(0.07f, 0.362f);
    }

    private void PlayerState() {

        if(upDown != 0) {
            upDownTimer += Time.deltaTime;
            if(upDownTimer > 0.5f) {
                upDownTimer = 0.0f;
                if(CheckStand(upDown == -1)) {
                    playerlevel += (int)upDown;
                    if(playerlevel < -2) playerlevel = -2;
                    else if(playerlevel > 0) playerlevel = 0;
                } else {
                    print("Too cramped");
                }
                
            }
        } else {
            upDownTimer = 0;
        }

        if(playerlevel == 0) {
            crouching = false;
            crawling = false;
        } else if(playerlevel == -1) {
            crouching = true;
            crawling = false;
        } else if(playerlevel == -2) {
            crouching = false;
            crawling = true;
        }
    }

    private bool CheckStand(bool down) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 10.0f, obstructionCheckLayer);
        
        if(playerlevel == -2) {
            if(hit && hit.distance <= 0.42f)
                return false;
        } else if((playerlevel == -1) && !down) {
            if(hit && hit.distance <= 0.49f)
                return false;
        } else if((playerlevel == -1) && down) {
            if(isSloped) return false;
            RaycastHit2D hitSide = Physics2D.Raycast(transform.position, Vector2.right, 10.0f, obstructionCheckLayer);
            if(hitSide && hitSide.distance <= 0.5f)
                return false;

            RaycastHit2D hitSideLeft = Physics2D.Raycast(transform.position, Vector2.left, 10.0f, obstructionCheckLayer);
            if(hitSideLeft && hitSideLeft.distance <= 0.5f)
                return false;
        }

        return true;
    }

    void HidingSpotFinder() {
        RaycastHit2D raycastHit = Physics2D.CircleCast(new Vector2(mousePos.x, mousePos.y), 0.1f, new Vector2(0.0f, 0.0f), 0.0f, (1 << 10));

        if(raycastHit.transform != null) {
            if(clicking && !clicked) {
                if(hideable && !hiding) {
                    HidePlayer(true);
                } else {
                    HidePlayer(false);
                }
                clicked = true;
            } else if(!clicking && clicked) {
                clicked = false;
            }
            
        }
    }

    void DoorFinder() {
        RaycastHit2D raycastHit = Physics2D.CircleCast(new Vector2(mousePos.x, mousePos.y), 0.1f, new Vector2(0.0f, 0.0f), 0.0f, (1 << 11) | (1 << 12));

        if(raycastHit.transform != null) {
            if(clicking && !clicked) {
                if(Vector2.Distance(transform.position, raycastHit.transform.position) < 0.5f)
                    raycastHit.transform.GetComponent<Door>().PlayerDoor();
                clicked = true;
            } else if(!clicking && clicked) {
                clicked = false;
            }
            
        }
    }

    public void SetHideable(bool tog) {
        hideable = tog;
    }

    public void HidePlayer(bool tog) {
        hiding = tog;
        freezeMovement = tog;
        GetComponent<SpriteRenderer>().enabled = !tog;
        GetComponent<CapsuleCollider2D>().enabled = !tog;
        //fieldOfView.Toggle(!tog);
    }

    

    public bool TestHiding() {
        if(hideable && !hiding) return true;
        else return false;
    }

    public bool isHiding() {
        return hiding;
    }

    public PlayerController GetPlayerControls() {
        return controls;
    }

    private void SeeGhost(Vector3 aimDirection) {
        RaycastHit2D raycast = Physics2D.Raycast(eyesObject.position, aimDirection, fieldOfView.viewDistance, (1 << 8));
        if(raycast.transform != null) {
            if(raycast.transform.tag == "Enemy") {
                observedEnemy = raycast.transform.gameObject;
                lookingAtEnemy = true;
            }
            else  {
                observedEnemy = null;
                lookingAtEnemy = false;
            }
        } else  {
            observedEnemy = null;
            lookingAtEnemy = false;
        }
    }

    private void CursorPictures() {
        //SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode);
        Cursor.SetCursor(normalCursor, Vector2.zero, cursorMode);
    }

    public float GetMovement() {
        return movement;
    }

    public void Pause(bool tog) {
        paused = tog;
    }

    public GameObject GetObserved() {
        return observedEnemy;
    }
}
