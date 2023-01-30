using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeClicker.V1_2
{
    public class ArrayOfColumns : MonoBehaviour
    {
        private int RowNumber;
        private bool isComplete;
        public CubeCode[] AllOfTheCubesInRow = new CubeCode[4];
        public Color[] colorsOfCubes = new Color[4];
        private ArrayOfColumns thisColumn;
        private ArrayOfRows theArrayOfAllRows;
        private ArrayOfColumns AoC;



        public void GetRowVariables(int x)
        {
            RowNumber = x;
            isComplete = false;

            AoC = this;

            // When activated it will go through each cube in the row and add in each variable
            for (int i = 0; i < AllOfTheCubesInRow.Length; i++)
            {

                if ((i + 1) >= AllOfTheCubesInRow.Length)
                {
                    AllOfTheCubesInRow[i].GetCubeVariables(RowNumber, i + 1, colorsOfCubes[i], AllOfTheCubesInRow[i], true, AoC);
                }
                else
                {
                    AllOfTheCubesInRow[i].GetCubeVariables(RowNumber, i + 1, colorsOfCubes[i], AllOfTheCubesInRow[i + 1], false, AoC);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void rowCompleted()
        {
            for (int i = 0; i < AllOfTheCubesInRow.Length; i++)
            {
                AllOfTheCubesInRow[i].cubeCompletedAndDone();
            }

        }
    }
}