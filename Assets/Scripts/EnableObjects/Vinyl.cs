using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vinyl : MonoBehaviour
{
    public RecordPlayer recordPlayer;
    public bool onRecordPlayer;
    public GameObject mySong;
    // Start is called before the first frame update
    void Start()
    {
        recordPlayer = ReferenceTool.recordPlayer;
    }

    // Update is called once per frame
    void Update()
    {

        if (onRecordPlayer)
        {
            if (recordPlayer.isPlaying)
            {
                if (!mySong.activeSelf)
                    mySong.SetActive(true);
                transform.Rotate(Vector3.up * 50 * Time.deltaTime, Space.Self);
            }
            else
            {
                if (mySong.activeSelf)
                {
                    mySong.SetActive(false);
                }
            }
        }
        else
        {
            if (mySong.activeSelf)
            {
                mySong.SetActive(false);
            }
        }

    }
}
