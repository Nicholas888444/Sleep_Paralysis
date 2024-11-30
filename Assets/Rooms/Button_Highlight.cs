using UnityEngine;
using UnityEngine.UI;

public class Button_Highlight : MonoBehaviour
{
    public Text buttonText;
    public Color HoverColor, normalColor, clicked;
    
    public void HoverStart() {
        buttonText.color = HoverColor;
    }

    public void NormalColor() {
        buttonText.color = normalColor;
    }

    public void ClickedColor() {
        buttonText.color = clicked; //
    }
}
