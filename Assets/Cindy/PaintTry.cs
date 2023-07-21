using System.Collections;
using System.Collections.Generic;
using Es.InkPainter;
using UnityEngine;

public class PaintTry : MonoBehaviour
{
    [SerializeField]
    private Brush brush;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Debug.Log("called");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(hitInfo.transform.name);
                var paintObject = hitInfo.transform.GetComponent<InkCanvas>();
                if (paintObject != null)
                    paintObject.Paint(brush, hitInfo);
            }
        }
    }
}
