using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Text_Box : MonoBehaviour
{
    public Text text;
    public string[] lines;

    public float textSpeed;
    private int index;

    public RawImage image, picture;

    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        TypeText(0);
    }

    public void TypeText(int x) {
        index = x;
        text.text = "";
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        text.enabled = true;
        image.enabled = true;
        picture.enabled = true;
        foreach(char c in lines[index].ToCharArray()) {
            text.text += c;
            audioSource.Play();
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(3.0f);
        text.enabled = false;
        image.enabled = false;
        picture.enabled = false;
    }
}
