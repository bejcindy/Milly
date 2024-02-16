using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAnimation : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 startingPos;
    float duration = 2.5f;
    float gap = 5f;
    SpriteRenderer sr;

    private void Awake()
    {
        transform.localPosition = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //startingPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        
        if (MindPalace.showedCursorAnimation)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        if(!sr)
            sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        InvokeRepeating("CursorMove", duration, gap);
    }
    private void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();
    }

    void CursorMove()
    {
        StartCoroutine(MoveCursor());
    }

    IEnumerator MoveCursor()
    {
        float t = 0;
        sr.enabled = true;
        startingPos = transform.parent.position;
        transform.position = startingPos;
        yield return new WaitForSeconds(.3f);
        while (t < duration)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        yield return new WaitForSeconds(.3f);
        sr.enabled = false;
        yield break;
    }
}
