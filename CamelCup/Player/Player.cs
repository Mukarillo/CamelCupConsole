using System;
using System.Collections.Generic;
using CamelCup.Actions;
using CamelCup.Boards.Pieces;
using CamelCup.Utils;

namespace CamelCup
{
    public class Player
    {
        public string name;
        public int coins;
        public Trap trap;
        public List<LegBet> legBetCards = new List<LegBet>();
        public List<Bet> overallBetCards = new List<Bet>();
        public int diceRolled = 0;
        public int diceCoinValue = 0;

        public Player(string name, int coins = 0)
        {
            this.name = name;
            this.coins = coins;
        }

        public void ResetOverallBetCards()
        {
            overallBetCards.Clear();
            var camels = GameManager.GetAllCamels();
            for (int i = 0; i < camels.Count; i++)
            {
                overallBetCards.Add(new Bet(camels[i], this));
            }
        }

        public void ChangeCoins(int value)
        {
            coins += Math.Max(value, 0);
        }

        public void RoundReset()
        {
            legBetCards.Clear();
            diceRolled = 0;
            diceCoinValue = 0;
        }

        public void Dump()
        {
            string betCardsDump = String.Empty;
            string pretty = "";
            for (int i = 0; i < legBetCards.Count; i++)
            {
                pretty = i == legBetCards.Count -1 ? "      ╚═ " : "      ╠═ ";
                betCardsDump += Environment.NewLine + pretty + legBetCards[i].ToString();
            }

            if (betCardsDump.Equals(string.Empty))
                betCardsDump = "None";
            
            ConsoleManager.Print($"{name} "+ Environment.NewLine + $"  ╠═ Coins: {coins}" + Environment.NewLine + $"  ╚═ Bet cards: {betCardsDump}", 200);
        }
    }
}
