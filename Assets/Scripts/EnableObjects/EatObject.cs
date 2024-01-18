using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class EatObject : PickUpObject
{
    [Foldout("Eat Object")]
    public Transform[] foodMeshes;
    public Transform currentMesh;
    public bool doneEating;

    public GameObject pizzaEatingDialogue;
    public Transform plate;

    bool foodMoving = false;
    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.FOOD;
        currentMesh = foodMeshes[0];
    }

    protected override void Update()
    {
        base.Update();

        if (selected && !thrown)
            currentMesh.gameObject.layer = 9;
        else if (inHand)
            currentMesh.gameObject.layer = 7;
        else if (activated)
            currentMesh.gameObject.layer = 17;
        else
            currentMesh.gameObject.layer = 0;

        if (inHand)
        {
            if (!playerLeftHand.eating && !foodMoving)
            {
                if(Input.GetMouseButtonDown(1)) {
                    transform.SetParent(null);
                    playerLeftHand.RemoveHandObj();
                    inHand = false;
                    StartCoroutine(LerpPosition(0.5f));
                    StartCoroutine(LerpRotation(0.5f));
                }
            }
        }
    }

    public void ChangeFoodMesh()
    {
        pizzaEatingDialogue.SetActive(true);
        if(currentMesh.GetSiblingIndex() < transform.childCount-1)
        {
            currentMesh.gameObject.SetActive(false);
            currentMesh = transform.GetChild(currentMesh.GetSiblingIndex() + 1);
            currentMesh.gameObject.SetActive(true);
            rend = currentMesh.GetComponent<Renderer>();
            rigged = false;
        }
        else
        {
            doneEating = true;
            playerHolding.UnoccupyLeft();
            Destroy(gameObject);
        }

    }

    IEnumerator LerpPosition(float duration)
    {
        foodMoving = true;
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, plate.position + new Vector3(0, 0.1f, 0), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = plate.position + new Vector3(0, 0.1f, 0);
        foodMoving = false;

    }

    IEnumerator LerpRotation(float duration)
    {
        float time = 0;
        Quaternion startValue = transform.rotation;
        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, Quaternion.identity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.identity;
    }
}
