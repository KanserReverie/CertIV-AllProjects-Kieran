using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

// All just using the same NameSpace this project.
namespace doodleJump
{
    // This is only on one gameObject in the finish scene and handles the highscores and previous score.
    public class LoadHighScores : MonoBehaviour
    {
        // This is the current List of Highscores.
        public HighScoreList highScoreList;
        // This is the lastScore.
        public PlayerData lastScore;
        // This is the TextBoxes to display the Highscores and the Score from the previous run.
        public Text displayHighScores;          // <--- !!! TO BE SET IN INSPECTOR!!!
        public Text displayLastScore;           // <--- !!! TO BE SET IN INSPECTOR!!!

        // Start is called before the first frame update
        void Start()
        {
            // Gets ALL previous Scores saved from all runs since the first run or last clear.
            highScoreList = SaveSystem.LoadHighScoreList();
            // Gets the Players Last Runs Score.
            lastScore = SaveSystem.LoadPlayer();
            // Sorts all the scores by fastest time.
            highScoreList.myHighScoreList.Sort();
            // Displays All or top 10 Highscores
            for (int i = 0; i < highScoreList.myHighScoreList.Count && i < 10; i++)
            {
                // Displays Each Score and Player name, then makes a new line in the HighScore text box.
                displayHighScores.text = (displayHighScores.text + highScoreList.myHighScoreList[i].PlayerName +" " + highScoreList.myHighScoreList[i].PlayerScore.ToString("F1") + "\n");
            }
            // Displays the score from the last run in the last run text box.
            displayLastScore.text = ("Your Score = " + lastScore.PlayerName + " " + lastScore.PlayerScore.ToString("F1"));
        }

        // Clears all High Scores.
        public void ClearHighScores()
        {
            // Resets the text of the "HighScore" text box.
            displayHighScores.text = "High Scores Reset";
            // Calls the function that makes a new binary script with NO scores in it in the SaveSystem.
            SaveSystem.ClearHighScoreList();

        }
    }
}