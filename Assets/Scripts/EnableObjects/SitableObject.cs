using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SitableObject : LivableObject
{
    [SerializeField] public KeyCode interactKey;
    public PlayerCam camController;
    public bool interacted;
    Vector3 fixedPosition = new Vector3 (0f, 0f, 1f);
    Vector3 fixedRotation = Vector3.zero;

    protected override void Update()
    {
        base.Update();
        if (!interacted)
        {
            if (Input.GetKeyDown(interactKey))
            {
                interacted = true;
                activated = true;
                camController.enabled = false;
            }
        }
        else
        {
            PositionPlayer();
        }
    }

    public void PositionPlayer()
    {
        player.SetParent(transform, true);
        player.localPosition = Vector3.Lerp(player.localPosition, fixedPosition, 2f);
        Quaternion target = Quaternion.Euler(0,0,0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 2f);
    }

}
