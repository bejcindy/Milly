using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBox : MonoBehaviour
{
    public List<Vector3> pizzaPos;
    public List<int> pizzaRot;
    public List<Transform> pizzas;

    public TrashLid boxLid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPizza(Transform newPizza)
    {
        if (pizzas.Contains(newPizza))
        {
            return;
        }
        for(int i = 0; i < pizzas.Count; i++)
        {
            if (pizzas[i] == null)
            {
                pizzas[i] = newPizza;
                newPizza.SetParent(transform);
                StartCoroutine(LerpPosition(newPizza, pizzaPos[i], 2f));
                Vector3 newRot = new Vector3(0, pizzaRot[i], 0);
                StartCoroutine(LerpRotation(newPizza, newRot, 2f));
            }
        }
    }

    public void RemovePizza(Transform pizza)
    {
        if (pizzas.Contains(pizza))
        {
            pizzas.Remove(pizza);
            pizza.SetParent(null);
        }
    }

    IEnumerator LerpPosition(Transform pizza, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = pizza.localPosition;
        while (time < duration)
        {
            pizza.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        pizza.localPosition = targetPosition;
    }

    IEnumerator LerpRotation(Transform pizza, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = pizza.localEulerAngles;
        while (time < duration)
        {
            pizza.localEulerAngles = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        pizza.localEulerAngles = targetPosition;
    }
}
