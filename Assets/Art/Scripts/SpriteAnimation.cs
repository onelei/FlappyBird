using UnityEngine;
using UnityEngine.UI;
using System;

public class SpriteAnimation : MonoBehaviour
{
    public bool playAwake;
    public Sprite[] frames;
    public float speed = 0.05f;
    public int actionFrame = -1;
    public Action frameAction;

    private Image container;
    private int ticked;
    private float time;
    private bool doAnim;

    private void Awake()
    {
        container = GetComponent<Image>();
        Init();
    }

    private void Init()
    {
        ticked = 0;
        time = 0;
        doAnim = playAwake;
        container.sprite = frames[0];
    }

    public void Play()
    {
        ticked = 0;
        time = 0;
        doAnim = true;
        container.sprite = frames[0];
    }

    public void Pause()
    {
        doAnim = false;
    }

    public void Resume()
    {
        doAnim = true;
    }

    public void Stop()
    {
        ticked = 0;
        time = 0;
        doAnim = false;
        container.sprite = frames[0];
    }

    void Update()
    {
        if (doAnim)
        {
            time += Time.deltaTime;
            if (time > speed)
            {
                ticked++;
                if (ticked == frames.Length)
                    ticked = 0;
                else
                    time = 0;

                if (ticked == actionFrame)
                {
                    if (frameAction != null)
                    {
                        frameAction();
                    }
                }

                container.sprite = frames[ticked];
            }
        }
    }
}