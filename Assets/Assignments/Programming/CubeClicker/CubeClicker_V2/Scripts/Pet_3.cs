using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CubeClicker.V2
{
    public class Pet_3 : MonoBehaviour
    {
        // cost and level and current money
        private int Level = 0;
        private int cost;
        private int CurrentMoney;
        private int MaxLevel;

        // all necasery inputs
        public TMP_Text LevelOutput;
        public TMP_Text CostOutput;
        public Button BuyPet;

        // Money variable gets in start
        public Money money;
        public CubeHandler cubeHandler;
        public GameObject Locked1;


        // sets up all the variables
        void Start()
        {
            MaxLevel = 5;
            money = FindObjectOfType<Money>();
            cubeHandler = FindObjectOfType<CubeHandler>();

            cost = (Mathf.RoundToInt(3000 * (Mathf.Pow(1.3f, Level))));

            BuyPet.interactable = false;

            LevelOutput.text = ("Level: " + Level);
            CostOutput.text = ("$$" + cost.ToString("N0"));
        }

        // lets you buy if money is good and below max level
        void Update()
        {
            CurrentMoney = money.MoneyValue;
            if (CurrentMoney > 3000)
            {
                Locked1.SetActive(false);
            }

            CurrentMoney = money.MoneyValue;
            if (CurrentMoney >= cost && Level < MaxLevel)
            {
                BuyPet.interactable = true;
            }
            else if (CurrentMoney < cost && Level < MaxLevel)
            {
                BuyPet.interactable = false;
            }
            else if (Level >= MaxLevel)
            {
                BuyPet.interactable = false;
                CostOutput.text = ("MAX!!");
            }
        }

        // Runs if they buy the pet.
        // Will increase money of all cubes
        public void PurchasePet()
        {
            if (CurrentMoney >= cost)
            {

                money.MoneyLost(cost);
                Level++;
                cost = (Mathf.RoundToInt(3000 * (Mathf.Pow(1.3f, Level))));
                LevelOutput.text = ("Level: " + Level);
                CostOutput.text = ("$$" + cost.ToString("N0"));
                cubeHandler.IncreaseCubeMoney();
            }
        }
    }
}