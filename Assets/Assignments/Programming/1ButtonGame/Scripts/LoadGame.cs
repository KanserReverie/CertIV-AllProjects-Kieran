using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // Load a new scene if anyKey is pressed.
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Level1");
        }
    }
}
