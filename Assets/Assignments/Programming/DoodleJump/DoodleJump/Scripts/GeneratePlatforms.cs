using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All just using the same NameSpace this project.
namespace doodleJump
{
	// This is put on one Game object in the game scene and will only run once.
	public class GeneratePlatforms : MonoBehaviour
	{
		// Gets the platform Prefab.
		public GameObject platformPrefab;           // <--- !!! TO BE SET IN INSPECTOR!!!
		// Gets the number of platforms to be spawned.
		public int numberOfPlatforms = 200;
		// How wide the game Scene is.
		public float levelWidth = 3f;
		// The 'Randomness' of how often the platforms spawn by ".Y" between them.
		public float minY = .2f;
		public float maxY = 1.5f;

		// Use this for initialization
		void Start()
		{
			// Makes a new Vector 3 as spawnPosition.
			Vector3 spawnPosition = new Vector3();

			// Loops through = to how many platforms there are.
			for (int i = 0; i < numberOfPlatforms; i++)
			{
				// Gets a random "Y" from above and adds to current instattiation..
				spawnPosition.y += Random.Range(minY, maxY);
				// Gets width besed on level width.
				spawnPosition.x = Random.Range(-levelWidth, levelWidth);
				// Makes a platform at that position.
				Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
			}
		}
	}
}