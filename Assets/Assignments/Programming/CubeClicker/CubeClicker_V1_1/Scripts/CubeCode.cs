using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeClicker.V1_1
{
    public class CubeCode : MonoBehaviour
    {
        // Cube Row and Number in row in example."Cube 2,3" is row 2 column 3
        public int cubeRow;
        public int cubeColumn;

        // Call on the next Cube
        // public GameObject nextCube;
        [SerializeField] private CubeCode nextCube;


        // public GameObject moneyCounter;
        [SerializeField] private Money moneyCount;

        // Is Row and Cube active
        private bool rowActive = false;
        private bool cubeActive = false;

        // Cube current and max 'life' starting at 0 going to 8 * 1.2^(Row-1)
        // trust me it works for the maths
        // Change this if you need to vary healths
        private float cubeCount = 0;
        private float cubeMaxCount;


        // How much does the box go up -- Default numbers
        // How much does the box will rotate  -- Default numbers
        // Cube Force and Rotation will only ever be based on column because once a Row is done its onto the next one
        private float cubeHitForceMin;
        private float cubeHitForceMax;
        private float cubeHitTorqueMin;
        private float cubeHitTorqueMax;

        // This is the pure color
        public Color cubePureColour;

        // This is the color all cubes will come to 
        private Color cubeUntouchedColour = new Color(0.655f, 0.624f, 0.549f, 0.000f);
        private Color cubeColourGap;
        private Color firstColor;

        // Sets material
        [SerializeField] private Material[] cubeMaterial = new Material[2];
        Renderer rend;

        // Gets Rigidbody
        private Rigidbody rb;

        // Sets max and min money from breaking cube, and the double coin range incase you are in the bottom 5% you will actually get 5 times money
        private int coinMax;
        private int coinMin;
        private int coinAverage;
        private int coinMutiplierChance;
        private int coinMutiplierAmmount;


        // Is the cube Completed
        private bool cubeCompleted = false;

        void Start()
        {
            // Getting materials
            rend = GetComponent<Renderer>();
            rend.enabled = true;
            rend.sharedMaterial = cubeMaterial[0];

            // Getting rigidbody
            rb = GetComponent<Rigidbody>();

            // How much money a cube will give and 90% - 130% of it
            float coinBaseMidPoint  = (10 + (8 * (cubeColumn - 1)) * (8 * cubeRow-1));
            coinMin = Mathf.RoundToInt(coinBaseMidPoint * 0.9f);
            coinMax = Mathf.RoundToInt(coinBaseMidPoint * 1.3f);

            // When should you % get Multiplied money
            coinMutiplierChance = 5;
            coinMutiplierAmmount = 3;

            // Coin Average
            coinAverage = Mathf.RoundToInt(Mathf.Floor(((float)coinMax - coinMin) / 2f) + coinMin);

            // Cube Force and Rotation on click and thus compleation
            cubeHitForceMin = 2 * 8 * cubeColumn;
            cubeHitForceMax = cubeHitForceMin + 8 * cubeColumn;
            cubeHitTorqueMin = cubeColumn * cubeRow;
            cubeHitTorqueMax = cubeHitTorqueMin + cubeColumn;

            // Gets max count getting 8 by default and expedentially harder per row
            cubeMaxCount = 8 * (float)System.Math.Pow(1.2f , (cubeRow - 1));

            // Gets the difference between the default gray and the final colour
            cubeColourGap = (cubePureColour - cubeUntouchedColour) / (cubeMaxCount - 1);
        }

        private void Update()
        {
            // Decrease cubeCount by 1 every 5 seconds
            if (cubeCount > 0)
            {
                cubeCount = cubeCount - (1f / 5f * Time.deltaTime);
                rend.material.color = rend.material.color - cubeColourGap * (1f / 5f * Time.deltaTime);
                if (cubeCount < 0)
                {
                    cubeCount = 0;
                    rend.material.color = cubeUntouchedColour;
                }
            }
        }


        public void cubeClicked()
        {

            // If row isn't active make it active
            if (!rowActive)
            {
                rowActive = true;
                // set other cubes in row to active //                                                            <<< CODE IT!!
            }

            if (!cubeActive)
            {
                cubeActive = true;
                rend.sharedMaterial = cubeMaterial[1];
                rend.material.color = cubeUntouchedColour; 
                return;
            }

            // Plus one because you will only ever do 1 dmg per click
            if (cubeCount + 1 >= cubeMaxCount)
            {
                Debug.Log("CUBE BREAK");
                cubeCount = 0;
                cubeBreak();
                return;
            }

            // ------------------- !!CUBE CLICKED!! -------------------
            // SEEMS LIKE THIS NEEDS TO BE DIFFERENT AS IT IS THE MAIN CODE RUN 90% OF THE TIME
            // Will run if Cube isn't at Max AND everything else has been checked
            cubeCount++;
            cubeJump();
            rend.material.color += cubeColourGap;
            // ________________________________________________________

        }

        private void cubeJump()
        {
            rb.AddForce(0, Random.Range(cubeHitForceMin, cubeHitForceMax), 0);
            rb.AddTorque(transform.up * Random.Range(cubeHitTorqueMin, cubeHitTorqueMax) * (Random.Range(0, 2) * 2 - 1));
            rb.AddTorque(transform.right * Random.Range(cubeHitTorqueMin, cubeHitTorqueMax) * (Random.Range(0, 2) * 2 - 1));
            rb.AddTorque(transform.forward * Random.Range(cubeHitTorqueMin, cubeHitTorqueMax) * (Random.Range(0, 2) * 2 - 1));
        }
        private void cubeBreak()
        {
            // Get how much money will spawn from cube and thus its jump will be relative * if in the bottom 5% will actually give double max money and jump 3x as high
            int moneyGiven = Random.Range(coinMin, coinMax);
            float jumpHeight;
            jumpHeight = ((float)moneyGiven / coinAverage) + 1;

            bool doubleCoinCheck = false;
            int x = Random.Range(0, 100);
            if ((x < coinMutiplierChance))
            {
                moneyGiven = coinMax * coinMutiplierAmmount;
                jumpHeight = 3;
                doubleCoinCheck = true;
            }

            // Jumps up relative to coin given
            rb.AddForce(0, cubeHitForceMax * jumpHeight, 0);

            // Resets Colour
            rend.material.color = cubeUntouchedColour;

            // Clicks the next Cube
            nextCube.cubeClicked();

            // ADD MONEY TO OUTSIDE
            print("money given = " + coinMin + " to " + coinMax);
            print("money given !box! = " + moneyGiven);
            moneyCount.boxBreak(moneyGiven);

            // RUN MONEY SPRITE
            // RUN GLOW SPRITE
        }

    }
}