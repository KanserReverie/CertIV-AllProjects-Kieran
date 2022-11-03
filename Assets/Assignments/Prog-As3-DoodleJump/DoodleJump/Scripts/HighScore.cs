using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// All just using the same NameSpace this project.
namespace doodleJump
{
    // This will be attached to an empty game object in the GamePlay scene in inspector. It will hold the current Runs Score and Save it.
    public class HighScore : MonoBehaviour
    {
        // The text box of the current score.
        public Text myHighScoreText;            // <--- !!! TO BE SET IN INSPECTOR!!!
        // The players Current score.
        [HideInInspector] public float myHighScore;

        // Start is called before the first frame update
        void Start()
        {
            // The score starts at 0 in the scene.
            myHighScore = 0;
        }

        // Just increases by game time.
        // Your score is how long it takes for you to hit the end
        void LateUpdate()
        {
            // Score increases by game time.
            myHighScore += Time.deltaTime;
            // Displays the "HighScore" or more accuratly "This Run Score" to 1 decimal place in the Score.
            myHighScoreText.text = myHighScore.ToString("F1");
        }
    }
}