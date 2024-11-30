using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Combination_Lock : MonoBehaviour
{
    public Lock_Combination[] combinations;
    public int[] answers;
    public TextMeshProUGUI[] revealed;

    public Door lockedDoor;

    // Start is called before the first frame update
    void Start()
    {
        RandomCombination();
        SetCombinationLocks();
        lockedDoor.LockDoor(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckLock()) {
            lockedDoor.LockDoor(false);
        }
    }

    private void RandomCombination() {
        for(int i = 0; i < answers.Length; i ++) {
            answers[i] = Random.Range(0, 10);
        }
    }

    private void SetCombinationLocks() {
        for(int i = 0; i < combinations.Length; i ++) {
            revealed[i].text = "" + answers[i];
        }
    }

    private bool CheckLock() {
        for(int i = 0; i < combinations.Length; i ++) {
            if(combinations[i].GetNumber() != answers[i]) {
                return false;
            }
        }

        return true;
    }
}
