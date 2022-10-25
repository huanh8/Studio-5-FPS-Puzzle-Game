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
        Debug.Log("Starting conversation with " + dialogue.name);

        //sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        foreach (AudioClip audioClip in dialogue.audioClips)
        {
            audioClips.Enqueue(audioClip);
        }

        // show dialogue box
        DisplayNextSentence();
        // play audio
        PlayNextAudioClip();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            sentences.Clear();
            return;
        }
        string sentence = sentences.Dequeue();
        Text.text = sentence;
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
