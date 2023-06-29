using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeAlive : MonoBehaviour
{
    public Material mat;
    public bool activated;
    public float matColorVal;


    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_WhiteDegree");
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            EnableObject();
        }
    }


    public void EnableObject()
    {
        mat.SetFloat("_WhiteDegree", matColorVal);
    }

    IEnumerator FadeInObj()
    {
        mat.SetFloat("_WhiteDegree", 0.1f);
        yield return new WaitForSeconds(0.05f);
        mat.SetFloat("_WhiteDegree", 0.2f);
        yield return new WaitForSeconds(0.05f);
        mat.SetFloat("_WhiteDegree", 0.3f);
        yield return new WaitForSeconds(0.05f);
        mat.SetFloat("_WhiteDegree", 0.4f);
        yield return new WaitForSeconds(0.05f);
        mat.SetFloat("_WhiteDegree", 0.5f);
        yield return new WaitForSeconds(0.05f);
        mat.SetFloat("_WhiteDegree", 0.6f);
        yield return new WaitForSeconds(0.05f);
        mat.SetFloat("_WhiteDegree", 0.7f);
        yield return new WaitForSeconds(0.05f);
        mat.SetFloat("_WhiteDegree", 0.8f);
        yield return new WaitForSeconds(0.05f);
        mat.SetFloat("_WhiteDegree", 0.9f);
        yield return new WaitForSeconds(0.05f);
        mat.SetFloat("_WhiteDegree", 1.0f);
    }
}
