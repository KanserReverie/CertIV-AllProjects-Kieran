using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeClicker.V2
{
    public class Exit : MonoBehaviour
    {
        // Will exit the game.
        public void ExitApplication()
        {
            Application.Quit();
        }
    }
}