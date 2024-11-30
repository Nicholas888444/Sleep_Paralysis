using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Generator : MonoBehaviour
{
    public Room[] startingRooms;
    public Room[] twoDoorRooms;
    public Room[] endingRooms;

    [Range(3, 10)]
    public int numberRooms;
    private Game_Handler game_Handler;


    // Start is called before the first frame update
    void Awake()
    {
        game_Handler = GameObject.Find("Game Handler").GetComponent<Game_Handler>();
        
        Room previousRoom = null;
        for(int i = 0; i < numberRooms; i ++) {
            if(i == 0) {
                int rand = Random.Range(0, startingRooms.Length);
                Room room = Instantiate(startingRooms[rand], new Vector3(0.0f, -6.0f*i, 1.0f), Quaternion.identity);
                previousRoom = room;
                game_Handler.currentRoom = room;
            } else if(i > 0 && i < numberRooms - 1) {
                int rand = Random.Range(0, twoDoorRooms.Length);
                Room room = Instantiate(twoDoorRooms[rand], new Vector3(0.0f, -6.0f*i, 1.0f), Quaternion.identity);
                previousRoom.SetRightRoom(room);
                room.SetLeftRoom(previousRoom);
                previousRoom = room;
            } else {
                int rand = Random.Range(0, endingRooms.Length);
                Room room = Instantiate(endingRooms[rand], new Vector3(0.0f, -6.0f*i, 1.0f), Quaternion.identity);
                previousRoom.SetRightRoom(room);
                room.SetLeftRoom(previousRoom);
            }
        }
    }
}
