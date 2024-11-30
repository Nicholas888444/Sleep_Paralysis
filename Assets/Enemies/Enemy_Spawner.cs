using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    private Game_Handler game_Handler;
    public GameObject[] enemies;
    //0 = Shadow
    //1 = Eyeball
    //2 = Runner
    //3 = Bird
    public float minimumHeightEyeBall;
    public Transform spawnValidator;
    private LayerMask spawnCheckLayer;
    private LayerMask obstructionCheckLayer;
    private LayerMask doorCheckLayer;

    //public Room currentRoom;
    //0 = Bedroom
    //1 = Stairwell
    //2 = Doors
    //3 = Brickhall
    //4 = Down stairs
    //5 = Chasm



    // Start is called before the first frame update
    void Awake()
    {
        game_Handler = GameObject.Find("Game Handler").GetComponent<Game_Handler>();
        obstructionCheckLayer = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Behind Mask")) | (1 << LayerMask.NameToLayer("Door")) | (1 << LayerMask.NameToLayer("Player"));
        doorCheckLayer = 1 << LayerMask.NameToLayer("Door");
        spawnCheckLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnEnemy(int x, Room room) {
        int roomId = room.RoomID;
        
        //Runner
        if(x == 2) {
            GameObject runner = Instantiate(enemies[2]);
            runner.transform.position = room.runnerLinks.GetChild(0).position;
            runner.GetComponent<Runner_Behavior>().UpdatePositions(room.runnerLinks);
            return runner;
        }
        //Eye 
        else if(x == 1) {
            Vector3 eyeSpot = EyeSpot(room);
            GameObject eye = Instantiate(enemies[1], eyeSpot, Quaternion.identity);
            return eye;
        }
        //Shadow 
        else if(x == 0) {
            Vector3 shadowSpot = ShadowSpot(room);
            GameObject shadow = Instantiate(enemies[0], shadowSpot, Quaternion.identity);
            return shadow;
        } 
        //Bird
        else if(x == 3) {
            Vector3 birdSpot = BirdSpot(room);
            GameObject bird = Instantiate(enemies[3], birdSpot, Quaternion.identity);
            return bird;
        }

        return null;
        
    }

    private Vector3 EyeSpot(Room room) {
        int randomSpot = Random.Range(0, room.eyeSpawns.childCount);
        Vector3 position = room.eyeSpawns.GetChild(randomSpot).position;
        return position;
    }

    private Vector3 ShadowSpot(Room room) {
        int randomSpot = Random.Range(0, room.shadowSpawns.childCount);
        Vector3 position = room.shadowSpawns.GetChild(randomSpot).position;
        return position;
        //return new Vector3(Random.Range(-3.7f, 3.7f), Random.Range(1.0f, 2.5f) + room.transform.position.y, -1.0f);
    }

    private Vector3 BirdSpot(Room room) {
        int randomSpot = Random.Range(0, room.birdSpawns.childCount);
        Vector3 position = room.birdSpawns.GetChild(randomSpot).position;
        return position;
    }
}
