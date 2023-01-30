using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamMove : MonoBehaviour
{
    [SerializeField ]
    private float movingSpeed = 10f;

    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * movingSpeed;
    }
}