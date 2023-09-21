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
    public GameObject kickHint;
    public GameObject instantiatedHint;
    public Vector3 hintPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (activateTrigger.interactable)
        {
            if(instantiatedHint == null)
            {
                instantiatedHint = Instantiate(kickHint, hintPosition, Quaternion.identity);
                //instantiatedHint.transform.SetParent(GameObject.Find("Canvas").transform);

            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Destroy(instantiatedHint);
                activateTrigger.activated = true;
                transform.GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = false;
                transform.GetChild(1).gameObject.GetComponent<MeshCollider>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = true;
                rb.isKinematic = false;
                rb.AddForce(player.forward * 5);
                Invoke(nameof(DeactivateEnabler), 2f);
            }
        }
        else
        {
            Destroy(instantiatedHint);
        }

    }

    public void DeactivateEnabler()
    {
        activateTrigger.enabled = false;
    }
}
