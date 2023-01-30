using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// All just using the same NameSpace this project.
namespace doodleJump
{
    // This Class Will be added to the Player.
    public class Player : MonoBehaviour
    {
        // This is the movement speed the player moves left and right.
        public float movementSpeed = 8f;
        // This is the Animator of the Player.
        [NonSerialized] public Animator myAnimator; // [NonSerialized] Keeps it public but not seen in inspector.
        // This will be the DYNAMIC xMovement of the player, in this case it will only take the left and right with "W/Left" or "R/Right".
        private float xMovement = 0f;
        // The rigid body of the player.
        private Rigidbody2D myrb;
        // Holds the Game over screen.
        public GameObject GameOver;         // <--- !!! TO BE SET IN INSPECTOR!!!
        public GameObject WinMenu;          // <--- !!! TO BE SET IN INSPECTOR!!!
        public GameObject Menu;             // <--- !!! TO BE SET IN INSPECTOR!!!

        // Start is called before the first frame update.
        void Start()
        {
            // Gets the Animator attached to .this game object this script is attached to. !! Note can only have one Animator per a game object.
            myAnimator = GetComponent<Animator>();
            // Gets the 2D Rigidbody attached to .this game object this script is attached to. !! Note can only have one Rigidbody2D per a game object.
            myrb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame.
        void Update()
        {
            // Takes the "Horizontal" Control and adds that direction * movementSpeed and saves it.
            xMovement = Input.GetAxis("Horizontal") * movementSpeed;
        }

        // Called every fixed frame.
        void FixedUpdate()
        {
            // Makes a new Temp Vector 2 as 'velocity' = the rigid body velocity.
            Vector2 velocity = myrb.velocity;
            // Makes the x of the 'velocity' = to the current pressed key in the most recent "Update".
            velocity.x = xMovement;
            // Make the velocity of the of the attached rigid body the stored 'velocity'.
            myrb.velocity = velocity;
        }

        // When enters collision with a 2D Collider. In this case it wont handle the platforms
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // If it is the "Lose" collider, found by the gameObject tag being "Lose".
            if (collision.gameObject.tag == "Lose")
            {
                // Makes the "GameOver" Pannel Active or "Turns On".
                GameOver.gameObject.SetActive(true);
                // Pauses Time.
                Time.timeScale = 0;
            }
            // If it is the "Finish" collider, found by the gameObject tag being "Finish".
            else if (collision.gameObject.tag == "Finish")
            {
                // Makes the "Menu" Pannel NOT Active or "Turns Off".
                Menu.SetActive(false);
                // Makes the "WinMenu" Pannel Active or "Turns On".
                WinMenu.gameObject.SetActive(true);
                // Pauses Time.
                Time.timeScale = 0;
            }
        }
    }
}