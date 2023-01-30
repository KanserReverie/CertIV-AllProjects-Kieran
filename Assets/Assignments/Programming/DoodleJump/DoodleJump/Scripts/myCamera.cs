using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All just using the same NameSpace this project.
namespace doodleJump
{
	// Attached to the Camera in the main scene.
	public class myCamera : MonoBehaviour
	{
		// Gets the transform, needs to be set to the player.
		public Transform myTarget;          // <--- !!! TO BE SET IN INSPECTOR!!!

		// Called every fixed frame.
		void LateUpdate()
		{
			// If the player goes higher then before for this run.
			if (myTarget.position.y > transform.position.y)
			{
				// Makes .this position transform (that is a Vector 3) = to ( x = .this current x, the players current y, and .this current z).
				transform.position = new Vector3(transform.position.x, myTarget.position.y, transform.position.z);
			}
		}
	}
}