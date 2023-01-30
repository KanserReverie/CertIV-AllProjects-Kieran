using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CubeClicker.V1_1
{
    public class Money : MonoBehaviour
    {
        // Variables for constant money increase current money and pricing text
        private int currentMoney;
        private int constantMoneyIncrease;
        public Text currentMoneyDisplay;
        private string pricingText = "$$";
        // Start is called before the first frame update
        void Start()
        {
            currentMoney = 0;
            currentMoneyDisplay.text = pricingText + currentMoney.ToString("N0");
        }

        public void boxBreak(int currencyGained)
        {
            currentMoney = currentMoney + currencyGained;
            currentMoneyDisplay.text = pricingText + currentMoney.ToString("N0");
        
        }
    }
}