using TMPro;
using UnityEngine;

public class Lock_Combination : MonoBehaviour
{
    private int currentNumber = 1;
    private TextMeshProUGUI text;

    void Start() {
        text = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    
    public void ChangeNumber(int i) {
        currentNumber += i;
        if(currentNumber > 9) {
            currentNumber = 0;
        } else if(currentNumber < 0) {
            currentNumber = 9;
        }

        text.text = currentNumber + "";
    }

    public int GetNumber() {
        return currentNumber;
    }
}
