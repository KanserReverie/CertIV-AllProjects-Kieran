using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CubeClicker.V2
{
    public class Pet_4 : MonoBehaviour
    {
        // cost and level and current money
        private int Level = 0;
        private int cost;
        private int CurrentMoney;
        private int MaxLevel = 6;

        // all necasery inputs
        public TMP_Text LevelOutput;
        public TMP_Text CostOutput;
        public Button BuyPet;

        // Money variable gets in start
        public Money money;
        public CubeHandler cubeHandler;
        public GameObject Locked2;

        // sets up variables including cost and such
        void Start()
        {
            money = FindObjectOfType<Money>();
            cubeHandler = FindObjectOfType<CubeHandler>();

            cost = (Mathf.RoundToInt(1100 * (Mathf.Pow(1.8f, Level))));

            BuyPet.interactable = false;

            LevelOutput.text = ("Level: " + Level);
            CostOutput.text = ("$$" + cost.ToString("N0"));
        }

        // lets you buy if money is good and below max level
        void Update()
        {
            CurrentMoney = money.MoneyValue;
            if (CurrentMoney > 1100)
            {
                Locked2.SetActive(false);
            }

            CurrentMoney = money.MoneyValue;
            if (CurrentMoney >= cost && Level < MaxLevel)
            {
                BuyPet.interactable = true;
            }
            else if(CurrentMoney < cost && Level < MaxLevel)
            {
                BuyPet.interactable = false;
            }
            else if (Level >= MaxLevel)
            {
                BuyPet.interactable = false;
                CostOutput.text = ("MAX!!");
            }
        }

        // runs if they buy the pet
        // will lower health of all cubes
        public void PurchasePet()
        {
            if (CurrentMoney >= cost)
            {
                money.MoneyLost(cost);
                Level++;
                cost = (Mathf.RoundToInt(1100 * (Mathf.Pow(1.8f, Level))));
                LevelOutput.text = ("Level: " + Level);
                CostOutput.text = ("$$" + cost.ToString("N0"));
                cubeHandler.DecreaseCubeHealth();
            }
        }
    }
}