using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeNPCLookPlayer : MonoBehaviour
{
    bool inDistance, inAngle;
    Transform head;
    Transform player;
    Vector3 originalForward;
    Vector3 lastForward;
    float t;

    public float lookPlayerDist;
    public float lookPlayerAngle;
    public float lookPlayerSpeed;
    public bool allowLookPlayer;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.name.Contains("Head") && !child.name.Contains("HeadTop"))
                head = child;
        }
        originalForward = head.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowLookPlayer)
        {
            if (Vector3.Distance(player.position, transform.position) < lookPlayerDist)
            {
                inDistance = true;
                if (Mathf.Abs(Vector3.SignedAngle(transform.forward, player.position - head.position, Vector3.up)) < lookPlayerAngle)
                    inAngle = true;
                else
                    inAngle = false;
            }
            else
                inDistance = false;

            if (inDistance && inAngle)
            {
                head.forward = Vector3.Lerp(head.forward, Camera.main.transform.position - head.position, lookPlayerSpeed * Time.deltaTime);
                //head.forward = Camera.main.transform.position - head.forward;
                lastForward = head.forward;
                t = 0;
            }
            else
            {
                if (t < .5f)
                {
                    t += Time.deltaTime;
                    head.forward = Vector3.Lerp(lastForward, originalForward, t/.5f);
                }
                else
                {
                    head.forward = originalForward;
                }
            }
        }
        else
        {
            if (head.forward != originalForward)
            {
                if (t < .5f)
                {
                    t += Time.deltaTime;
                    head.forward = Vector3.Lerp(lastForward, originalForward, t / .5f);
                }
                else
                {
                    head.forward = originalForward;
                }
            }
        }
        
    }
}
