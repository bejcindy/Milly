using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TattooPanel : MonoBehaviour
{
    public bool panelOn;
    public CenterTattoo centerTat;

    public Image panelBG;
    public Animator fade;

    public bool showCenter;

    [SerializeField] Material blurMaterial;
    RectTransform panelTransform;
    public bool lerping;

    // Start is called before the first frame update
    void Start()
    {
        panelTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (panelOn)
        {
            if (!centerTat.colorOn && !centerTat.activated)
            {
                centerTat.forceActivate = true;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {

                panelOn = false;
            }
        }
    }

    public void FocusOnTattoo(RectTransform tat)
    {
        panelTransform.anchoredPosition = -tat.anchoredPosition;
        transform.localScale = new Vector2(1, 1);
    }

    public IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        lerping = true;
        float time = 0;
        Vector3 startPosition = GetComponent<RectTransform>().anchoredPosition;
        while (time < duration)
        {
            time += Time.deltaTime;
            GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, time / duration));
            yield return null;
        }
        GetComponent<RectTransform>().anchoredPosition = targetPosition;
        lerping = false;
    }
}
