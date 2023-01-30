using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// All just using the same NameSpace this project.
namespace doodleJump
{
    // System Serializable allows it to be written to a binary file.
    [System.Serializable]
    // Player Data to save and load.
    // This Implements "IComparable to allow us to sort through the Player Data.
    public class PlayerData : IComparable<PlayerData>
    {
        // Will save the Player Name and Score.
        public string PlayerName;
        public float PlayerScore;

        // This is a Constructor so it will save a "PlayerHS" to this "player data".
        // Constructors are called when the game is made.
        public PlayerData(PlayerHS _PlayerHS)
        {
            // Saves .this Player Name and Score = to the inputed Player's Name and Score
            PlayerName = _PlayerHS.Name;
            PlayerScore = _PlayerHS.Score;
        }

        // This will give us the smallest one first. !!NOTE!! You can only compare 'ints' in this funtion
        public int CompareTo(PlayerData _Player)
        {
            // If there is nothing in the given "Player data"
            if (_Player == null)
            {
                // Returns "1".
                return 1;
            }
            // You can only compare 'ints' in this funtion

            // Times both scores by 100 then cuts them off at the decimal place to make them an int
            // .this player Score
            float x1 = PlayerScore * 100;
            int x2 = Mathf.RoundToInt(x1);
            // The inputed Player Score.
            float y1 =_Player.PlayerScore * 100;
            int y2 = Mathf.RoundToInt(y1);
            // Returns the negative difference so that they will sort from smallest to largest.
            return x2 - y2;
        }
    }

    // Serializes it so that it can be saved as a binary file.
    [System.Serializable]
    // List of all the HighScores to display.
    public class HighScoreList
    {
        // Just holds a list of scores.
        public List<PlayerData> myHighScoreList = new List<PlayerData>();
    }
}