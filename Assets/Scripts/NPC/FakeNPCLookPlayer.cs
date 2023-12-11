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
    float lookWeight;
    Animator anim;

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
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allowLookPlayer)
        {
            LookAtPlayer();
        }
    }

    void LookAtPlayer()
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
            lookWeight = Mathf.Lerp(lookWeight, 1, lookPlayerSpeed * Time.deltaTime);
        else
            lookWeight = Mathf.Lerp(lookWeight, 0, lookPlayerSpeed * Time.deltaTime);
    }

    private void OnAnimatorIK()
    {
        if (allowLookPlayer)
        {
            anim.SetLookAtWeight(lookWeight);
            anim.SetLookAtPosition(Camera.main.transform.position);
        }
        else
        {
            if (lookWeight > 0)
                lookWeight -= .05f;
            else
                lookWeight = 0;
            anim.SetLookAtWeight(lookWeight);
        }
    }
}
