using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour {

    private static GameObject s;
    private static Vector3 startPos;
    private static Vector3 startDiff;
    private static Vector3 diff;
    [SerializeField] private GameObject player;

    // Use this for initialization
    void Awake () {
        s = this.gameObject;
        diff = this.transform.position - player.transform.position;
        startDiff = this.transform.position - player.transform.position; ;
        startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        print("Start: " + startPos + "    Cur: " + this.transform.position + "     Dif: " + diff);

        Vector3 curPos = this.transform.position;

        //if (curPos.x >= 0)
        //{
        //    if (curPos.x < 6)
        //    {
        //        if (diff.x < -3)
        //        {
        //            curPos.x = player.transform.position.x + diff.x;
        //            this.transform.position = curPos;
        //        } else
        //        {
        //            diff = this.transform.position - player.transform.position;
        //        }
        //    }
        //}

        if (diff.x <= -1.5)
        {
            this.transform.position = new Vector3(Mathf.Clamp(Time.time, 0, 6), 1, -10);
                }
        else if (diff.x >= -1.5)
        {
            diff = this.transform.position - player.transform.position;
        }

            //if (curPos.x >= 0 && curPos.x <= 6)
            //{
            //    if (diff.x <= -1.5)
            //    {
            //        curPos.x = player.transform.position.x + diff.x;
            //        this.transform.position = curPos;
            //    }
            //    else if (diff.x >= -1.5)
            //    {
            //        diff = this.transform.position - player.transform.position;
            //    }
            //} else if (curPos.x >= 6)
            //{

            //}
        }

    public static void resetCamPos()
    {
        s.transform.position = new Vector3(0f, 1f, -10f);
        setDiff(startDiff);
    }


    public static void setDiff(Vector3 val)
    {
        diff = val;
    }
}
