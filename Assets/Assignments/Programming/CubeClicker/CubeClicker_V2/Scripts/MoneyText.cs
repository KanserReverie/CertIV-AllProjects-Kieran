using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeClicker.V2
{
    public class MoneyText : MonoBehaviour
    {
        // sext to be made when a cube is broken
        // Will move up and then disapear after 2 seconds.
        public Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, 4, 0);
            Destroy(gameObject, 1);
        }
    }
}