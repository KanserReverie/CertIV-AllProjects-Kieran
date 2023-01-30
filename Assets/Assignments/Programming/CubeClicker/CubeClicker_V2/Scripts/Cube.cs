using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CubeClicker.V2
{
    public class Cube : MonoBehaviour
    {
        // All variables for cube

        private int cubeRow;
        private int cubeColumn;
        private Color cubeUntouchedColor;
        private Color cubeFinalColor;
        private int cubeHealth;
        private int MaxHealthOfCube;
        private Color CubeColorIncrease;
        public TextMeshPro MoneyTextDesplay;
        public GameObject MoneyText;
        private bool cubeHitOnce;
        private ParticleSystem Part;


        // Gets Rigidbody
        private Rigidbody rb;

        private Material cubeMaterial;
        Renderer rend;

        // set up cube with variables
        public void CubeSetup(int _Row, int _Col, Color _FinalColor, int _Health, Color _UntouchColor, Material _CubeMaterial)
        {
            Part = GetComponent<ParticleSystem>();
            Part.Stop();
            cubeHitOnce = false;
            cubeRow = _Row;
            cubeColumn = _Col;
            cubeFinalColor = _FinalColor;
            MaxHealthOfCube = _Health;
            cubeUntouchedColor = _UntouchColor;
            cubeHealth = MaxHealthOfCube;
            CubeColorIncrease = (cubeFinalColor - cubeUntouchedColor) / (cubeHealth - 1);

            // Getting rigidbody
            rb = GetComponent<Rigidbody>();

            // Getting materials
            rend = GetComponent<Renderer>();
            rend.enabled = true;
            rend.sharedMaterial = _CubeMaterial;

            rend.material.color = cubeUntouchedColor;

        }

        // Updates health
        private void Update()
        {
            MoneyTextDesplay.text = cubeHealth + "/" + MaxHealthOfCube;
        }

        // cube click code to decrease health
        public bool Click()
        {
            if (cubeHitOnce == false)
            {   
                Part.Play();
                ParticleSystem.MainModule PartMain = Part.main;
                PartMain.startColor = cubeFinalColor;
                cubeHitOnce = true;
            }

            cubeHealth--;
            if (cubeHealth <= 0)
            {
                return true;
            }
            else
            {
                CubeHit();
                return false;
            }
        }

        // will run if cube drops and makes money
        public void Break(int _MoneyDrop)
        {
            GameObject newMoneyText = Instantiate(MoneyText, transform.position+new Vector3(0,0.3f,0), Quaternion.identity, transform);
            newMoneyText.GetComponent<TextMesh>().text = ("+$$" + _MoneyDrop);

            rend.material.color = cubeUntouchedColor;
            cubeHealth = MaxHealthOfCube;
        }

        // change color if hit 
        private void CubeHit()
        {
            rend.material.color += CubeColorIncrease;
        }

        // if the moster decreases all cubes health it runs this
        // decreases max health and changes all relevent variables
        public void CubeHealthDecrease()
        {
            MaxHealthOfCube--;

            if(CubeColorIncrease.b*CubeColorIncrease.r*CubeColorIncrease.r > cubeUntouchedColor.b*cubeUntouchedColor.r*cubeUntouchedColor.g)
            {
            
            }
            if (cubeHealth > 1)
            {
                cubeHealth--;
                CubeColorIncrease = (cubeFinalColor - cubeUntouchedColor) / (MaxHealthOfCube - 1);
                rend.material.color = cubeUntouchedColor + CubeColorIncrease * (MaxHealthOfCube - cubeHealth);
            }
            else if (cubeHealth<=1)
            {
                CubeColorIncrease = (cubeFinalColor - cubeUntouchedColor) / (MaxHealthOfCube - 1);
                rend.material.color = cubeFinalColor - CubeColorIncrease;
            }
        }
    }
}