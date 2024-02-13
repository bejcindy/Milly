using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinylHolder : MonoBehaviour
{
    public Vinyl myVinyl;
    public bool hasVinyl;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount > 0)
        {
            myVinyl = transform.GetChild(0).GetComponent<Vinyl>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myVinyl)
        {
            hasVinyl = true;
        }
        else
        {
            hasVinyl = false;
        }
    }

    public void PlaceVinyl(Vinyl holdingVinyl)
    {
        myVinyl = holdingVinyl;
        myVinyl.holder = this;
        holdingVinyl.transform.SetParent(transform);
        holdingVinyl.holder = this;
        holdingVinyl.gameObject.layer = 17;
        holdingVinyl.onRecordPlayer = false;
        holdingVinyl.inHand = false;
        holdingVinyl.onStand = true;
        holdingVinyl.selected = false;
        holdingVinyl.transform.localPosition = Vector3.zero;
        holdingVinyl.transform.localRotation = Quaternion.identity;
        holdingVinyl.transform.localScale = Vector3.one;
    }

    public void ReleaseVinyl()
    {
        myVinyl.GetComponent<Rigidbody>().isKinematic = true;
        myVinyl.inHand = true;
        myVinyl.onStand = false;
        myVinyl.holder = null;
        myVinyl = null;
        hasVinyl = false;
    }
}
