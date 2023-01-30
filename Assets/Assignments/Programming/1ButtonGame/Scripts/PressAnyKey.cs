using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // When they press Any Key Start the game
        if (Input.anyKey)
        {
            SceneManager.LoadScene(1);
        }
    }
}
