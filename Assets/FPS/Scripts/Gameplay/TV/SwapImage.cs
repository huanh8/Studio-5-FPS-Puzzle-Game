using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapImage : MonoBehaviour
{
    public float swapTime;
    public Sprite imageOne;
    public Sprite imageTwo;

    public Image imageComponent;

    public bool change = false;

    void Start()
    {
        imageComponent.sprite = imageOne;
    }

    void Update()
    {
        if(!change)
            StartCoroutine(changeToImageTwo());
        else
            StartCoroutine(changeToImageOne());
    }

    public IEnumerator changeToImageTwo()
    {
        yield return new WaitForSecondsRealtime(swapTime);
        imageComponent.sprite = imageTwo;
        change = true;
    }

    public IEnumerator changeToImageOne()
    {
        yield return new WaitForSecondsRealtime(swapTime);
        imageComponent.sprite = imageOne;
        change = false;
    }
}
