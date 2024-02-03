using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class TattooPanel : MonoBehaviour
{
    public bool panelOn;
    public CenterTattoo centerTat;
    public MainTattooMenu mainTattooMenu;
    public GameObject charPanelIcon;

    public Image panelBG;
    Animator fadeAnim;
    protected CanvasGroup canvasGroup;

    public bool showCenter;

    protected RectTransform panelTransform;
    protected float mouseDragSpeed = 70;
    protected float scrollSizeSpeed = .1f;
    public bool lerping;

    public bool noDrag;

    CharacterPanel characterPanel;
    public Vector2 startPos;

    public bool activatedOnce;
    public RectTransform VFXCanvas;

    [Foldout("NewTat")]
    public CinemachineVirtualCamera panelCam;
    protected virtual void Awake()
    {
        panelTransform = GetComponent<RectTransform>();
        fadeAnim = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        characterPanel = mainTattooMenu.characterPanel;
        startPos = panelTransform.anchoredPosition;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (panelOn)
        {

            GetComponentInParent<GraphicRaycaster>().enabled = false;

            charPanelIcon.SetActive(true);
            characterPanel.currentPanel = transform;
            mainTattooMenu.activePanel = this;
            if (!centerTat.colorOn && !centerTat.activated)
            {
                centerTat.forceActivate = true;
            }

            if (!noDrag)
            {
                DragPanel();
            }
        }
    }

    float AlphaBasedOnScale(float maxA)
    {
        float alpha = Mathf.Lerp(maxA, 0, Mathf.InverseLerp(.5f, .2f, transform.localScale.x));
        return alpha;
    }

    public void FocusOnTattoo(RectTransform tat)
    {
        panelTransform.anchoredPosition = -tat.anchoredPosition;
        transform.localScale = new Vector2(1, 1);
    }

    public void MakePanelVisible()
    {
        canvasGroup.alpha = 1;
    }
    void MakePanelInvisible()
    {
        canvasGroup.alpha = 0;
    }

    public void ResetPosition()
    {
        panelTransform.anchoredPosition = startPos;
        VFXCanvas.anchoredPosition = Vector2.zero;
    }

    protected virtual void DragPanel()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 dragAmount = new Vector2(Input.GetAxis("Mouse X") * mouseDragSpeed, Input.GetAxis("Mouse Y") * mouseDragSpeed);
            panelTransform.anchoredPosition += dragAmount;
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            float scrollAmount = Input.mouseScrollDelta.y;
            //1 to 0.5: resize
            //0.5 to 0.2: fade to outer menu
            transform.localScale = new Vector2(Mathf.Clamp(transform.localScale.x + scrollAmount * scrollSizeSpeed, .2f, 2f), Mathf.Clamp(transform.localScale.y + scrollAmount * scrollSizeSpeed, .2f, 2f));

            if (transform.localScale.x < .5f)
            {
                mainTattooMenu.lerping = true;
                mainTattooMenu.characterPanel.gameObject.SetActive(true);
                mainTattooMenu.characterPanel.ResetLocalVar();
                canvasGroup.alpha = AlphaBasedOnScale(1);
            }
            else
                canvasGroup.alpha = 1;
        }
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
            VFXCanvas.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, time / duration));
            yield return null;
        }
        GetComponent<RectTransform>().anchoredPosition = targetPosition;
        lerping = false;
    }
}
