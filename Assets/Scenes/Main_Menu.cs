using UnityEngine;
using UnityEngine.UI;

public class Main_Menu : MonoBehaviour
{
    public Text title;
    public Text start;

    float timer = 0.0f;

    public Color colorOne, colorTwo;

    public Color HoverColor, normalColor, clicked;
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if((timer > 1.0f) && (timer < 2.0f)) {
            title.color = colorOne;
        } else if(timer > 2.0f) {
            title.color = colorTwo;
            timer = 0.0f;
        }
    }

    public void HoverStart() {
        start.color = HoverColor;
    }

    public void NormalColor() {
        start.color = normalColor;
    }

    public void ClickedColor() {
        start.color = clicked; //
    }
}
