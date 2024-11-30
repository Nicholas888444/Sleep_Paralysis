using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControls : MonoBehaviour
{
    /*[SerializeField] private FieldOfView fieldOfView;
    public Transform eyesObject;

    private Mouse mouse;
    private Vector3 mousePos;

    private bool clicking;
    private bool clicked;

    public Texture2D normalCursor;
    public CursorMode cursorMode = CursorMode.Auto;

    void Awake() {
        mouse = Mouse.current;

        controls.Player.Click.started += _ => clicking = true;
        controls.Player.Click.canceled += _ => clicking = false;

        character_Controller = GetComponent<Character_Controller>();
        controls = character_Controller.GetPlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        CursorPictures();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
        HidingSpotFinder();
        DoorFinder();

        Vector3 aimDir = (mousePos - eyesObject.position).normalized;
        fieldOfView.SetAimDirection(aimDir);
        fieldOfView.SetOrigin(eyesObject.position);
    }


    void HidingSpotFinder() {
        RaycastHit2D raycastHit = Physics2D.CircleCast(new Vector2(mousePos.x, mousePos.y), 0.1f, new Vector2(0.0f, 0.0f), 0.0f, (1 << 10));

        if(raycastHit.transform != null) {
            if(clicking && !clicked) {
                if(character_Controller.TestHiding()) {
                    character_Controller.HidePlayer(true);
                    fieldOfView.Toggle(true);
                } else {
                    character_Controller.HidePlayer(false);
                    fieldOfView.Toggle(false);
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
    }*/

    
}
