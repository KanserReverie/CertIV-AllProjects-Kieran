using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CubeClicker.V2
{
    public class Money : MonoBehaviour
    {
        // Sets up all relevent variables
        public int MoneyValue { get { return money; } }

        private int money;
        public Text moneyDisplay;
        private string pricingText = "$$";

        // sets money to 0
        private void Start()
        {
            money = 0;
        }

        // displays money every second
        private void Update()
        {
            moneyDisplay.text = pricingText + money.ToString("N0");
            if (money < 0)
                money = 0;
        }

        // gains money equal to money inputted
        public void MoneyGained(int _q1)
        {
            money += _q1;
        }

        // takes away money equal to input
        public void MoneyLost(int _q2)
        {
            money -= _q2;
        }

        // displays money avaliable
        public int GetMoney()
        {
            return money;
        }
    }
}