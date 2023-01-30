using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// All just using the same NameSpace this project.
namespace doodleJump
{
    // Called in Game to Submit and Save the current Score.
    public class HighScoreSystem : MonoBehaviour
    {
        // Gets a current score manager.
        public HighScore myScore;               // <--- !!! TO BE SET IN INSPECTOR!!!
        // The final score.
        public float myFinalScore;
        // Player name.
        public string myName;
        // The final score Text Box.
        public Text myFinalScoreText;           // <--- !!! TO BE SET IN INSPECTOR!!!

        // Update is called once per frame
        void Update()
        {
            // This is the final score the player gets.
            myFinalScore = myScore.myHighScore;
            // Displays it as a 1 decimal place in the text box.
            myFinalScoreText.text = myFinalScore.ToString("F1");
        }

        // Called in the input field and called with any change.
        public void ReadStringInput(string s)
        {
            // Make this name = the changed name.
            myName = s;
        }

        // Called buy the "Submit Score" button.
        public void SubmitScore()
        {
            // Makes a new temp PlayerHS.
            PlayerHS _PlayerHS = new PlayerHS();
            // Makes the name of the "Player" = to the Player name in this class.
            _PlayerHS.Name = myName;
            // Makes the score of the "Player" = to the Player score in this class.
            _PlayerHS.Score = myFinalScore;
            // Saves this Temp PlayerHS.
            SaveSystem.SavePlayer(_PlayerHS);
            // Opens the current HSList (or makes it), adds this Temp PlayerHS to the list, then saves this new PlayerHS list.
            SaveSystem.SavePlayerToList(_PlayerHS);
            // Loads the next "Score" Scene.
            SceneManager.LoadScene(2);
        }
    }

    // This is a public PlayerHS class.
    public class PlayerHS
    {
        // This just holds the player Name and player Score.
        public string Name;
        public float Score;
    }
}