using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blading_Player;
public class LevelManager : MonoBehaviour
{
    // All the public values put in for each level.
    public string       Name;                       // Mero.
    public int          levelNo;                    // 0,1,2,3.
    public int          Timer;                      // 80,120,60.
    public int          NextScene;                  // Scene number.
    public Vector3      Location_Start;             // (Start location.x, Start location.y, 0) will get 3 intervals for game after it will start running.
    public Vector3      Location_Finish;            // (Finish location, 0, 0).

    // Private values based on public ones and load in this.
    private int         current_Highscore;          // 30,000.
    private float       current_Besttime;           // 12.32 (to 2db).

    private float       Location_StartIntervals;    // Distance between 3, 2, 1, Go. Will always be the same.

    public PlayerControls inGame_Player;              // The player in the scene.
    private float       inGame_PlayerTimer;         // 74.34.
    private int         inGame_PlayerPoints;        // 12,430.
    private Vector3     inGame_PlayerLocation;      // (x, y, 0).
    private bool        inGame_PlayerHasbestTime;   // Yes / No.
    private bool        inGame_PlayerHasbestTimer;  // Yes / No.

    private GameObject  StartTextPrefab_Number;     // 3... ...2... ...1...
    private GameObject  StartTextPrefab_Go;         // Go!

    // Start is called before the first frame update
    void Awake()
    {
        // Find the player in scene.
        inGame_Player   = FindObjectOfType<PlayerControls>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
