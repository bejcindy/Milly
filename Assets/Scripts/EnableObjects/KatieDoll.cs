using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatieDoll : LivableObject
{
    public bool playing;
    public int playCount;
    public AnimationClip[] clips;
    Animator dollAnim;
    bool katieDiaDone;

    protected override void Start()
    {
        base.Start();
        dollAnim = GetComponent<Animator>();
        clips = dollAnim.runtimeAnimatorController.animationClips;
    }
    protected override void Update()
    {
        base.Update();
        if (interactable)
        {
            ChangeDollLayer(9);

            if (!playing)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PlayDoll();
                }
            }

            if(playCount >= 3)
            {
                if (!katieDiaDone)
                {
                    katieDiaDone = true;
                    DialogueManager.StartConversation("NPC/Felix/Katie");
                }

            }

        }
        else
        {
            if (activated)
            {
                ChangeDollLayer(17);
            }
            else
            {
                ChangeDollLayer(0);
            }
        }

        if(dollAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            playing = false;
        }
    }


    void ChangeDollLayer(int layer)
    {
        foreach(Transform t in transform)
        {
            t.gameObject.layer = layer;
        }
    }

    void PlayDoll()
    {
        playCount++;
        playing = true;
        activated = true;
        if (!dollAnim.enabled)
        {
            dollAnim.enabled = true;
        }
        else
        {
            var randInd = Random.Range(0, clips.Length);

            var randClip = clips[randInd];

            dollAnim.Play(randClip.name);
        }

    }
}
