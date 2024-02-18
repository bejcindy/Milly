using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using FMODUnity;

public class EatObject : PickUpObject
{
    [Foldout("Eat Object")]

    public bool chewing;
    public bool doneEating;
    public bool ate;

    public GameObject pizzaEatingDialogue;
    public Transform plate;
    public PizzaScreen pizzaLid;
    public int foodStage = 1;
    public bool foodMoving = false;
    public EventReference eatSound;
    public CharacterTattoo pizzaTat;

    public Texture2D lutTexture;

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
        ate = true;
        if (!foodAnim.isActiveAndEnabled)
        {
            foodAnim.enabled = true;
        }
        else
        {
            foodAnim.Rebind();
            foodAnim.Update(0f);
            foodAnim.Play("Eat");
            Invoke("DelayedSoundEffect", 1.5f);
        }


    }

    void DelayedSoundEffect()
    {
        RuntimeManager.PlayOneShot(eatSound, transform.position);
    }

    public void FoodMeshChange()
    {
        if(pizzaEatingDialogue)
            pizzaEatingDialogue.SetActive(true);
        pizzaLid.enabled = true;
        pizzaLid.showing = true;
        if (!ran)
        {
            StartCoroutine(ReferenceTool.postProcessingAdjust.LerpToPizzaColor(lutTexture));
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
            doneEating = true;
            playerHolding.UnoccupyLeft();
            rend.enabled = false;
            if (pizzaTat)
            {
                if (!pizzaTat.triggeredOnce && !playerHolding.inDialogue)
                {
                    pizzaTat.triggered = true;
                }
            }

            Invoke(nameof(DestroyPizza), 1f);

        }
        if(gameObject.name == "teriyaki_pizza_milly")
        {
            Ron.akiPizzaAte = true;
        }

        if(gameObject.name == "maegherita_pizza_milly")
        {
            Ron.charlesPizzaAte = true;
        }
    }

    void DestroyPizza()
    {
        Destroy(gameObject);
    }


    bool ran;


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
