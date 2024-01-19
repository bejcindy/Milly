using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class EatObject : PickUpObject
{
    [Foldout("Eat Object")]

    public bool chewing;
    public bool doneEating;


    public GameObject pizzaEatingDialogue;
    public Transform plate;
    public int foodStage = 1;
    public bool foodMoving = false;


    [Foldout("Post Processing Values")]
    public Vector4 shadow;
    public Vector4 midtone;
    public Vector4 highlight;
    public float vignette;

    Animator foodAnim;
    Transform currentMesh;
    protected override void Start()
    {
        base.Start();
        objType = HandObjectType.FOOD;
        currentMesh = transform.GetChild(1);
        foodAnim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        LayerDetection();

        

        //if (inHand)
        //{
        //    if (!playerLeftHand.eating && !foodMoving)
        //    {
        //        if(Input.GetMouseButtonDown(1)) {
        //            transform.SetParent(null);
        //            playerLeftHand.RemoveHandObj();
        //            inHand = false;
        //            StartCoroutine(LerpPosition(0.5f));
        //            StartCoroutine(LerpRotation(0.5f));
        //        }
        //    }
        //}
    }

    private void OnDestroy()
    {
        StartCoroutine(ReferenceTool.postProcessingAdjust.LerpToDefaultColor());
    }

    void LayerDetection()
    {
        if (selected && !thrown)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 9;
            }
        }
        else if (inHand)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 7;
            }
        }
        else if (activated)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 17;
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 0;
            }
        }
    }

    public void Eat()
    {
        if (!foodAnim.isActiveAndEnabled)
        {
            foodAnim.enabled = true;
        }
        else
        {
            foodAnim.Rebind();
            foodAnim.Update(0f);
            foodAnim.Play("Eat");
        }
    }

    public void FoodMeshChange()
    {
        activated = true;
        pizzaEatingDialogue.SetActive(true);
        if (!ran)
        {
            StartCoroutine(ReferenceTool.postProcessingAdjust.LerpToPizzaColor(shadow, midtone, highlight, vignette));
            ran = true;
        }
        if (foodStage < 3)
        {
            foodStage++;
            transform.GetChild(foodStage).GetComponent<Collider>().enabled = true;
            currentMesh.gameObject.SetActive(false);
            currentMesh = transform.GetChild(foodStage);
            rend = currentMesh.GetComponent<Renderer>();
            rigged = false;
        }
        else
        {
            playerHolding.UnoccupyLeft();
            Destroy(gameObject);
        }
    }
    bool ran;
    //public void ChangeFoodMesh()
    //{
    //    activated = true;
    //    pizzaEatingDialogue.SetActive(true);
    //    if (!ran)
    //    {
    //        StartCoroutine(ReferenceTool.postProcessingAdjust.LerpToPizzaColor(shadow, midtone, highlight, vignette));
    //        ran = true;
    //    }
    //    if(foodStage < 3)
    //    {
    //        currentMesh.gameObject.SetActive(false);
    //        foodStage++;
    //        currentMesh = transform.GetChild(foodStage);
    //        currentMesh.gameObject.SetActive(true );
    //        rend = currentMesh.GetComponent<Renderer>();
    //        rigged = false;
    //    }
    //    else
    //    {
    //        doneEating = true;
    //        playerHolding.UnoccupyLeft();
    //        Destroy(gameObject);
    //    }
    //    playerLeftHand.eating = false;
    //}

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
