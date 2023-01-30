using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blading_Player;


namespace bladingEnvironment
{
    public class Rails : MonoBehaviour
    {
        public PlayerControls myPlayerControls;
        public EdgeCollider2D[] myColliders;
        public bool areRailsActive;
        public bool CanGrind;

        void Start()
        {
            myPlayerControls = FindObjectOfType<PlayerControls>();
            areRailsActive = false;
            myColliders = GetComponentsInChildren<EdgeCollider2D>();
            foreach (EdgeCollider2D collider in myColliders)
            {
                collider.isTrigger = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Gets the current state of the player and if they "canGrind" turns on the colliders,
            // and visa versa.
            CanGrind = myPlayerControls.canGrind;
            if (myPlayerControls.canGrind && !areRailsActive)
            {
                foreach (EdgeCollider2D collider in myColliders)
                {
                    collider.isTrigger = false;
                }
                areRailsActive = true;
            }
            if(!myPlayerControls.canGrind && areRailsActive)
            {
                foreach (EdgeCollider2D collider in myColliders)
                {
                    collider.isTrigger = true;
                }
                areRailsActive = false;
            }
        }
    }
}