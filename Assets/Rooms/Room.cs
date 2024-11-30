using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform spawnPointL, spawnPointR;
    public Teleporter teleporterL, teleporterR;
    public Transform runnerLinks;
    public Transform eyeSpawns;
    public Transform birdSpawns;
    public Transform shadowSpawns;

    public int RoomID;

    public void SetLeftRoom(Room room) {
        teleporterL.teleportTo = room.spawnPointR;
        teleporterL.distance = 6;
        teleporterL.next = room;
    }

    public void SetRightRoom(Room room) {
        teleporterR.teleportTo = room.spawnPointL;
        teleporterR.distance = -6;
        teleporterR.next = room;
    }
}
