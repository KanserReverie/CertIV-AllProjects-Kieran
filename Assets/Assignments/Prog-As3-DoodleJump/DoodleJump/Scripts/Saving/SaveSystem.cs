using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

// All just using the same NameSpace this project.
namespace doodleJump
{
    // This is the save system able to be called from anywhere.
    public static class SaveSystem
    {
        // This will save just the inputed player
        public static void SavePlayer(PlayerHS _player)
        {
            // Binary saves the inputed player as "LastPlayer"

            // Makes a formatter.
            BinaryFormatter formatter = new BinaryFormatter();
            // Makes the path in the game files and then a file called "LastPlayer".
            string path = Application.persistentDataPath + "/LastPlayer.hss";
            // Makes a stream of data to that path.
            FileStream stream = new FileStream(path, FileMode.Create);
            // Saves the player to that path.
            PlayerData data = new PlayerData(_player);
            // Serializes it with the stream and data.
            formatter.Serialize(stream, data);
            // Closes the Save.
            stream.Close();
        }

        // Save the last score to the list.
        public static void SavePlayerToList(PlayerHS _player)
        {
            // Makes the path in the game files and then a file called "playerHSList".
            string path = Application.persistentDataPath + "/playerHSList.hss";

            // If the File doesn't exist make a new one with this name ane with a class "HighScoreList"
            if (!File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Create);
                HighScoreList HSdata = new HighScoreList();
                formatter.Serialize(stream, HSdata);
                stream.Close();
            }
            // Basically will open, temp save file, add to temp file , save new file , close.

            // Getting the list and adding a new score to it.
            BinaryFormatter formatter2 = new BinaryFormatter();
            // Makes a stream of data to that path.
            FileStream stream2 = new FileStream(path, FileMode.Open);
            // We are making a new HighScore List.
            HighScoreList data = new HighScoreList();
            // We are then making a new entery to add to this list.
            PlayerData data3 = new PlayerData(_player);
            // Makes data = a stream of data to that path.
            data = formatter2.Deserialize(stream2) as HighScoreList;
            // We will then close this list so that we can now add to it.
            stream2.Close();
            // We will then add to this list.
            data.myHighScoreList.Add(data3);
            // We are now opening this for the last time to add to the list again.
            BinaryFormatter formatter3 = new BinaryFormatter();
            // Makes a NEW stream of data to that path.
            FileStream stream3 = new FileStream(path, FileMode.Open);
            // Saves and closes it.
            formatter2.Serialize(stream3, data);
            // Closes the stream.
            stream2.Close();
        }

        // This it to load the last score.
        public static PlayerData LoadPlayer()
        {
            // Makes path in game files and then a file called "LastPlayer".
            string path = Application.persistentDataPath + "/LastPlayer.hss";
            // If the file exists.
            if (File.Exists(path))
            {
                // Getting the list and adding a new score to it.
                BinaryFormatter formatter = new BinaryFormatter();
                // Makes a stream of data to that path.
                FileStream stream = new FileStream(path, FileMode.Open);
                // Saves data.
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                // Closes file.
                stream.Close();
                // Returns data.
                return data;
            }
            // If not found produces error.
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }

        // Opens all the highscores in a list.
        public static HighScoreList LoadHighScoreList()
        {
            // Get a path to the HighScore list in game files and then a path called "playerHSList".
            string path = Application.persistentDataPath + "/playerHSList.hss";
            // If the file exists
            if (File.Exists(path))
            {
                // Make a new Binary Formatter.
                BinaryFormatter formatter = new BinaryFormatter();
                // Open the file and make a new stream of data = to the data in the file, REMEMBER THE FILE ALREADY EXISTS.
                FileStream stream = new FileStream(path, FileMode.Open);
                // Deserialise and then Save the data in that file in a "HighScoreList" ("HighScoreList" is the class we saved to this file).
                HighScoreList data = formatter.Deserialize(stream) as HighScoreList;
                // Close this file.
                stream.Close();
                // Return the data which is the High Score list.
                return data;
            }
            // Produces an error if no file exists.
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }

        // Clears all the highscores in a list.
        public static void ClearHighScoreList()
        {
            // Get a path to the HighScore list in game files and then a path called "playerHSList".
            string path = Application.persistentDataPath + "/playerHSList.hss";
            // If the Path Exists.
            if (File.Exists(path))
            {
                // Make a new binary formatter for how we will read this binary file.
                BinaryFormatter formatter = new BinaryFormatter();
                // Make a new file stream = to this file.
                FileStream stream = new FileStream(path, FileMode.Create);
                // Make a new temp HSdata file = a NEW HighScoreList.
                HighScoreList HSdata = new HighScoreList();
                // Serialize this NEW temp "HSdata" to the file.
                formatter.Serialize(stream, HSdata);
                // Closes the file.
                stream.Close();
            }
        }
    }
}