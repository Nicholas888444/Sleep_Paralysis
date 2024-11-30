using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Handler : MonoBehaviour
{
    public Room currentRoom;
    private Room previousRoom;
    public Enemy_Spawner enemy_Spawner;

    public float waitTime;


    private Character_Controller character_Controller;

    public GameObject red, shadow;
    public GameObject deadCanvas;
    public AudioSource audioSource;
    private GameObject currentEnemy;
    private bool spawnable;
    private bool collectedCup;
    private Scene_Manager scene_Manager;

    //private float redMax = 0.48f;
    //private float shadowMax = 0.58f;

    // Start is called before the first frame update
    void Start()
    {
        scene_Manager = FindObjectOfType<Scene_Manager>();
        character_Controller = GameObject.Find("Prey").GetComponent<Character_Controller>();
        //enemy_Spawner.SpawnEnemy(1, currentRoom);
    }

    // Update is called once per frame
    void Update()
    {
        /*waitTime -= Time.deltaTime;

        if(waitTime <= 0.0f) {
            enemy_Spawner.SpawnEnemy(1, currentRoom);
            enemy_Spawner.SpawnEnemy(3, currentRoom);
            waitTime = 10.0f;
        }*/

        if(currentRoom != previousRoom) {
            Destroy(currentEnemy);
            if(currentRoom.RoomID != 66) {
                //
                previousRoom = currentRoom;
                int randomMonster = Random.Range(0, 4);
                if(randomMonster == 2) {
                    waitTime = Random.Range(5.0f, 12.0f);   
                    spawnable = true;
                } else {
                    currentEnemy = enemy_Spawner.SpawnEnemy(randomMonster, currentRoom);
                }
            }
            
        } 
        
        if(spawnable) {
            waitTime -= Time.deltaTime;

            if(waitTime <= 0.0f) {
                currentEnemy = enemy_Spawner.SpawnEnemy(2, currentRoom);
                spawnable = false;
            }
        }
    }

    public void Die() {
        shadow.SetActive(false);
        red.SetActive(true);

        deadCanvas.SetActive(true);
        audioSource.Play();

        Pause(true);
    }

    public void Pause(bool tog) {
        Time.timeScale = 0.0f;
        character_Controller.Pause(tog);

        Eye_Behavior[] eyes = FindObjectsOfType<Eye_Behavior>();
        foreach(Eye_Behavior eye in eyes) {
            eye.gameObject.layer = 0;
            eye.Pause(tog);
        }

        Shadow_Behavior[] shadows = FindObjectsOfType<Shadow_Behavior>();
        foreach(Shadow_Behavior shadow in shadows) {
            shadow.gameObject.layer = 0;
            shadow.Pause(tog);
        }

        Runner_Behavior[] runners = FindObjectsOfType<Runner_Behavior>();
        foreach(Runner_Behavior runner in runners) {
            runner.gameObject.layer = 0;
            runner.Pause(tog);
        }

        Bird_Behavior[] birds = FindObjectsOfType<Bird_Behavior>();
        foreach(Bird_Behavior bird in birds) {
            bird.transform.GetChild(0).gameObject.layer = 0;
            bird.Pause(tog);
        }

    }

    public void CollectedCup() {
        collectedCup = true;
    }

    public void EndGame() {
        if(collectedCup) {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            scene_Manager.LoadScene("End");
        }
    }
}
