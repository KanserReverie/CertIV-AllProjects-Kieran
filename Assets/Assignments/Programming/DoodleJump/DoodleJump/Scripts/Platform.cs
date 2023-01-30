using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All just using the same NameSpace this project.
public class Platform : MonoBehaviour
{
	// This is the jump force applied to the character when it hits the platform.
	public float jumpForce = 12f;

	// On Collision TOUCH fuction with another collider (only applies to 2D colliders). !!NOTE!! For a collision to register one of the objects needs a rigid body.
	void OnCollisionEnter2D(Collision2D _collision)
	{
		// If the other rigid body is going in a negative direction or standing still.
		// In this case the player falling onto the platform.
		if (_collision.relativeVelocity.y <= 0f && _collision.collider.tag == "Player") // I added the (&& _collision.collider.tag == "Player") so this only triggers for the Player 2d collider.
		{
			// Get the rigid body of the collided object.
			Rigidbody2D myrb = _collision.collider.GetComponent<Rigidbody2D>();
			// Since only the player will have a rigid body and be touching the platform it gets its Animator.
			Animator myAnimator = _collision.collider.GetComponent<Animator>();
			// If it has an Animator and Rigidbody
			if (myrb != null && myAnimator!=null)
			{
				// Temp saves the velocity of this rigid body.
				Vector2 velocity = myrb.velocity;
				// Makes the y velocity of the Temp velocity = the "jumpForce".
				velocity.y = jumpForce;
				// Makes the rigidBody Velocity of the player = this temp velocity.
				myrb.velocity = velocity;
				// Plays the "hitPlatform" Animation in the Animator for the Collided Player.
				myAnimator.SetTrigger("hitPlatform");
			}
		}
	}
}
