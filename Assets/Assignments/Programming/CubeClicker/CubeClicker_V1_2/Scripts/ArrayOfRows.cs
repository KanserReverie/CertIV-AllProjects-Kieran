using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeClicker.V1_2
{
    public class ArrayOfRows : MonoBehaviour
    {

        private CubeCode ActiveClickCube;
        private int ActiveRow;
        [SerializeField] private ArrayOfColumns[] AllOfTheRows = new ArrayOfColumns[3];




        // Start is called before the first frame update
        void Start()
        {
            ActiveRow = 0;

            for (int i = 0; i < AllOfTheRows.Length; i++)
            {

                AllOfTheRows[i].GetRowVariables(i + 1);
            
            }
        }
    }
}