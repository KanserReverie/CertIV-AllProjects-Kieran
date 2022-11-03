using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace doodleJump
{
    // This is on the setting prefab.
    public class SettingsManager : MonoBehaviour
    {
        public GameObject SettingsPannel;

        // Gets the settings menu panel and closes it.
        public void CloseMenu()
        {
            // Sets time to 1 again.
            Time.timeScale = 1;
            SettingsPannel.SetActive(false);
        }

    }
}