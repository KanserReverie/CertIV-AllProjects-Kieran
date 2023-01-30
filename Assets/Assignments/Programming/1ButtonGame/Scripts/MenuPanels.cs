using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanels : MonoBehaviour
{
    public GameObject pauseButton;

    // Start is called before the first frame update
    void Awake()
    {
        pauseButton.SetActive(false);
    }
}
