using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;

    // For flipping the collider rotation
    private float buttonHoldTime;

    //For reseting the platform collider rotation
    public float rotationResetTime = 0.5f;
    private bool rotationIsFlipped = false;
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            buttonHoldTime -= Time.deltaTime; //We start the timer (while holding the key) which i made small so it seems instant and not when holding
            if (buttonHoldTime <= 0) // When the time reaches 0 we rotate the collider 180 degres and set it as flipped and we reset the timer
            {
                effector.rotationalOffset = 180f;
                rotationIsFlipped = true;
                buttonHoldTime = 0.01f;
            }
        }

        //Reseting the roation if the player releases the key while the rotation code is running
        if (Input.GetKeyUp(KeyCode.S))
            buttonHoldTime = 0.01f;

        //Reseting Rotation
        if (rotationIsFlipped)
        {
            rotationResetTime -= Time.deltaTime; // We start the timer till the rotation is reset (i used a timer and not made it instant to give the player time to fall down and not get stuck in the collider)
            if (rotationResetTime <= 0) // When the timer reaches 0 we rest the rotation and then the timer and set it as not flipped so we could be able to repeat this code
            {
                effector.rotationalOffset = 0f;
                rotationResetTime = 0.5f;
                rotationIsFlipped = false;
            }
        }

    }
}