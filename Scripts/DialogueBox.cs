using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public GameObject canvas;
    public Text textBox;
    public Image imageBox;
    public Button btnA;
    public Button btnB;
    public Sprite dialogueImage;
    public bool dialogOpen;


    void Start ()
    {
        canvas.SetActive(false);
	}

    public void ShowDialogue(string dialogueText, string btnAText, string btnBText)
    {
        dialogOpen = true;
        canvas.SetActive(true);
        textBox.text = dialogueText;
        btnA.GetComponentInChildren<Text>().text = btnAText;
        btnB.GetComponentInChildren<Text>().text = btnBText;

        btnA.onClick.AddListener(() => { CloseDialog(); DialogAudio(); });
    }

    public void CloseDialog()
    {
        dialogOpen = false;
        canvas.SetActive(false);
        btnA.onClick.RemoveAllListeners();
        btnB.onClick.RemoveAllListeners();
    }

    public void DialogAudio()
    {
        Audio.gameAudio.PlaySFX("btnClick1");
    }
}
