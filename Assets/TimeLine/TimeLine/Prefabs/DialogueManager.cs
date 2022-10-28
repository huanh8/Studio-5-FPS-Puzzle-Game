using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private Queue<AudioClip> audioClips;
    public AudioSource audioSource;
    public TextMeshProUGUI Text;
    public GameObject DialogueBox;
    public GameObject panel;
    void Start()
    {
        sentences = new Queue<string>();
        audioClips = new Queue<AudioClip>();
    }
    public void StartDialogue(Dialogue dialogue)
    {
        audioSource = dialogue.audioSource;
        Debug.Log(sentences.Count);

        if (sentences.Count == 0)
        {
            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            foreach (AudioClip audioClip in dialogue.audioClips)
            {
                audioClips.Enqueue(audioClip);
            }
        }
        StartCoroutine(PlayerOrWait(dialogue.name));


    }
    IEnumerator PlayerOrWait(string name)
    {
        if (audioSource.isPlaying)
        {
            Debug.Log("Playing");
            yield return null;
        }
        else
        {
            // show dialogue box
            DisplayNextSentence(name);
            // play audio
            PlayNextAudioClip();
        }

    }
    public void DisplayNextSentence(string name)
    {

        if (sentences.Count == 0)
        {
            sentences.Clear();
            return;
        }
        string sentence = sentences.Dequeue();
        Text.text = name + ": " + sentence;
    }
    public void PlayNextAudioClip()
    {
        if (audioClips.Count == 0)
        {
            return;
        }
        AudioClip audioClip = audioClips.Dequeue();
        audioSource.PlayOneShot(audioClip);
    }

    public void ShowPanel()
    {
        DialogueBox.SetActive(true);
    }
    public void HidePanel()
    {
        DialogueBox.SetActive(false);
    }
}
