using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleClick : MonoBehaviour
{

    public bool inputCheck;
    public float timeForInput = 0.25f;
    public float inputTime = 0.0f;
    public int timesPressed = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            inputTime = 0;
            timesPressed++;
            inputCheck = true;
        }

        if (inputCheck)
        {
            inputTime += Time.deltaTime;
            if(inputTime>timeForInput)
            {
                if (timesPressed == 1)          Debug.Log("Single click");
                else if (timesPressed == 2)     Debug.Log("Double click");
                timesPressed = 0;
                inputCheck = false;
            }
        }
    }
}
