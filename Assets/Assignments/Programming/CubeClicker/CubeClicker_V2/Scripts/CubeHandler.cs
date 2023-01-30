using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    namespace CubeClicker.V2
    {
        public class CubeHandler : MonoBehaviour
        {
            /// <summary>
            /// Just initialising these private variables kinda self explanatory.
            /// </summary>
            /// Just found out initialising means "set to the value or put in the condition appropriate to the start of an operation".
            /// Thus it would be "initialising" -> "creating and initialising".

            // Max ammount of rows in Cube grid.
            // Called in Start() Instantiate.
            private int RowMax = 3;      
            // Max ammount of Cubes in each row.
            // Called in Start() Instantiate.
            private int ColMax = 4;
            // Distace between each Cube.
            // Called in Start() Instantiate.
            private float cubeDistanceBetweenCubes = 2;
            // The base (e.g we are base 4) for breaking new cube.
            private int moneyBaseIncreaseForBreakingCube = 4;
            // Base health of cubes.
            // Everything is pretty based, on 8.
            // Will be changed in pets.
            private int baseHealthOfCubes = 8;
            // This is an array of Cube. Remember these are still GameObjects.
            // To call Cube on each one you need:
            // cubeGrid[x,y].GetComponent<Cube>().ScriptWantToCall;
            // A jagged array [x][y] is an array of arrays and a multidimensional array [x,y] is just a normal 2d array.
            // I don't get jagged arrays yet and how to initalise or call thus this works.
            private GameObject[,] cubeGrid = new GameObject[3, 4]; // [Row] [Col]

            /// <summary>
            /// All the public variables choosen in the client.
            /// </summary>
            /// Need to fix at some point either by putting in functions or by privating and hard coding in.

            // This is just a 1d array of colors choosen in inspector and looped through in Instansiating. FIX + ASK IF TIME
            // When this was a 2d array but wasn't showing in inspector.
            // There is a way to make this a 2d array showing the grid of colors.
            // It looks like it uses strusts and "[System.Serializable]" at the front and at the top of the class
            // https://www.youtube.com/watch?v=uoHc-Lz9Lsc
            // Might manually input all and "private" in start if it starts reseting too often.
            public Color[] CubeGridColors = new Color[12];// -- NEEDED
            // Just connects to the money manager.
            public Money MoneyCount;
            // Links to the Cube GameObject Prefab.
            public GameObject CubePrefab;
            // Vector3 of where to start placing cubes.
            //-6.05 0.55 -14.15
            public Vector3 StartPointCubeGrid;
            // The original cube color.
            // Again might turn private and hard code the colors in start().
            public Color CubeUntouchedColor;    
            //The Material of each cube.
            // Each cube needs a material to change color of it.
            public Material cubeMaterial;

            public Text ButtonClickText;


            private void Start()
            {
                //PanelBuy.interactable = false;
                // sets up game objects and instansiates them
                GameObject newCube = new GameObject();
                Vector3 extraDistance = new Vector3(0, 0, 0);

                // This will make a fill cubeGrid array
                for (int y = 0; y < RowMax; y++)
                {
                    for (int x = 0; x < ColMax; x++)
                    {
                        extraDistance = new Vector3(x * cubeDistanceBetweenCubes, 0, y * cubeDistanceBetweenCubes);
                        newCube = Instantiate(CubePrefab, transform.position + extraDistance, Quaternion.identity, transform) as GameObject;
                        // This will get the script 'Cube' on the gameObject newCube.
                        // Thus we can still keep an array of Cube.
                        newCube.GetComponent<Cube>()
                               .CubeSetup(y, x, CubeGridColors[(y * ColMax + (x + 1))-1], baseHealthOfCubes, CubeUntouchedColor, cubeMaterial);
                        cubeGrid[y, x] = newCube;

                    }
                }
            }

            // works out to click on cube and get money
            public void Click()
            {
                bool CubeDone = false;
                int CurRow = 0;
                int CurCol = 0;
                int moneyMade = 0;

                // I did it here is a do while loop
                do
                {  
                    CubeDone = cubeGrid[CurRow, CurCol].GetComponent<Cube>().Click();

                    if (CubeDone == true)
                    {
                        moneyMade = CubeBreakMoney(CurRow, CurCol);
                        cubeGrid[CurRow, CurCol].GetComponent<Cube>().Break(moneyMade);
                        CurCol++;

                        if (CurCol >= ColMax)
                        {
                            CurCol = 0;
                            CurRow++;
                            if (CurRow == RowMax)
                            {
                                print("Done ^_^");
                                return;
                            }
                        }

                    }
                } while (CubeDone == true);
            }

            // Runs if cube breaks and finds out how much money to give.
            private int CubeBreakMoney(int _CubeRow, int _CubeCol)
            {
                int MoneyGained =                            // It works x(y^2)
                    (Mathf.RoundToInt(                       // It works x(y^2)
                        moneyBaseIncreaseForBreakingCube*        // It works x(y^2)
                        (Mathf.Pow                               // It works x(y^2)
                        ((((_CubeRow) * ColMax) + (_CubeCol+1))  // It works x(y^2)
                            , 2                                      // It works x(y^2)
                        ))));                                    // It works x(y^2)

                MoneyCount.MoneyGained(MoneyGained);
                return MoneyGained;
            }

            // Increase how much money a cube gives.
            public void IncreaseCubeMoney()
            {
                moneyBaseIncreaseForBreakingCube++;
            }

            // Decreases cube health by 1.
            public void DecreaseCubeHealth()
            {   

                baseHealthOfCubes--;
        
                for (int y = 0; y < RowMax; y++)
                {
                    for (int x = 0; x < ColMax; x++)
                    {
                        cubeGrid[y, x].GetComponent<Cube>().CubeHealthDecrease();
                    }
                }
            }
        }
    }