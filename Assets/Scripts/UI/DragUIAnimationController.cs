using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragUIAnimationController : MonoBehaviour
{
    Animator dragAnim;
    PlayerHolding playerHolding;
    public string test;
    // Start is called before the first frame update
    void Start()
    {
        dragAnim = GetComponent<Animator>();
        playerHolding = GameObject.Find("Player").GetComponent<PlayerHolding>();
        if (transform.parent.GetComponent<Image>())
            GetComponent<Image>().sprite = transform.parent.GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerHolding.dragAnimDirection)
        {
            case "Left":
                dragAnim.SetBool("Left", true);
                dragAnim.SetBool("Right", false);
                break;
            case "Right":
                dragAnim.SetBool("Left", false);
                dragAnim.SetBool("Right", true);
                break;
            case "LeftRight":
                dragAnim.SetBool("Left", true);
                dragAnim.SetBool("Right", true);
                break;
            case "Up":
                dragAnim.SetBool("Up", true);
                dragAnim.SetBool("Down", false);
                break;
            case "Down":
                dragAnim.SetBool("Up", false);
                dragAnim.SetBool("Down", true);
                break;
            case "UpDown":
                dragAnim.SetBool("Up", true);
                dragAnim.SetBool("Down", true);
                break;
        }
    }
}
