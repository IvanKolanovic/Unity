using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private string[] sentences;
    [SerializeField] private bool zoomOutCamera;
    [SerializeField] [Range(0f, 1f)] private float typingSpeed = 0.96f;
    private int index;
    private float typeSpeed => 1 - typingSpeed;
    private WaitForSeconds letterWaitTime;

    private void Awake() => letterWaitTime = new WaitForSeconds(typeSpeed);

    private void OnTriggerEnter2D(Collider2D collider)
    {
        SoundManager.Instance.PlayCentral();
        StartCoroutine(Type());
        GetComponent<Collider2D>().enabled = false;
        if (zoomOutCamera) Camera.main.gameObject.GetComponent<Animator>()
            .Play("ZoomInOut", 0, 0.0f);
    }

    IEnumerator Type()
    {
        textDisplay.text = "> ";
        foreach (char letter in sentences[index])
        {
            textDisplay.text += letter;
            yield return letterWaitTime;
        }

        yield return new WaitForSeconds(1.0f);
        NextSentence();
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "> ";
            StartCoroutine(Type());
        }
        else
        {
            index = 0;
            textDisplay.text = "";
        }
    }
}