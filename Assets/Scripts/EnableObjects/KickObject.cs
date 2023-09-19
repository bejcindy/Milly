using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickObject : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    Transform player;
    public LivableObject activateTrigger;
    public RiggedVisibleDetector detector;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (detector.isVisible)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                activateTrigger.activated = true;
                transform.GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = false;
                transform.GetChild(1).gameObject.GetComponent<MeshCollider>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = true;
                rb.isKinematic = false;
                rb.AddForce(player.forward);
                Invoke(nameof(DeactivateEnabler), 2f);
            }
        }

    }

    public void DeactivateEnabler()
    {
        activateTrigger.enabled = false;
    }
}
