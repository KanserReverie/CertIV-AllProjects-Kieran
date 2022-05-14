using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CubeClicker.V2
{
    public class Pet_1 : MonoBehaviour
    {
        // cost and level and current money
        private int Level = 0;
        private int cost;
        private int CurrentMoney;

        // all necasery inputs
        public TMP_Text LevelOutput;
        public TMP_Text CostOutput;
        public Button BuyPet;

        // Money variable gets in start
        public Money money;

        // sets up all relevent variables
        void Start()
        {
            money = FindObjectOfType<Money>();
            //money.MoneyValue

            cost = (Mathf.RoundToInt(10 * (Mathf.Pow(1.4f, Level))));

            BuyPet.interactable = false;

            LevelOutput.text=("Level: " + Level);
            CostOutput.text =("$$" + cost.ToString("N0"));

            StartCoroutine(FarmingMoney());
        }

        // lets them buy if they have the money
        void Update()
        {
            CurrentMoney = money.MoneyValue;
            if (CurrentMoney >= cost)
            {
                BuyPet.interactable = true;
            }
            else
            {
                BuyPet.interactable = false;
            }
        }

        // runs when they but the pet
        // gets money every second
        public void PurchasePet()
        {
            if (CurrentMoney>=cost)
            {
                money.MoneyLost(cost);
                Level++;
                cost = (Mathf.RoundToInt(10*(Mathf.Pow(1.4f, Level))));
                LevelOutput.text = ("Level: " + Level);
                CostOutput.text = ("$$" + cost.ToString("N0"));
            }
        }

        // co routine for them to get money equal to level
        private IEnumerator FarmingMoney()
        {
            while(true)
            {
                yield return new WaitForSeconds(0.6f);
                money.MoneyGained(Level);
            }
        }
    }
}