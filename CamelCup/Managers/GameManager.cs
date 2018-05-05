using System;
using System.Linq;
using System.Collections.Generic;
using CamelCup.Utils;
using CamelCup.Boards.Pieces;

namespace CamelCup
{
    public static class GameManager
    {
        public static List<int> overallBetPaytable = new List<int> { 8, 5, 3, 2, 1 };
        public static int overallMissBet = -1;

        private static List<Player> mPlayers = new List<Player>();
        private static List<Dice> mDices = new List<Dice>();
        private static List<Camel> mCamels = new List<Camel>();
        private static List<LegBet> mLegBet = new List<LegBet>();
        private static Queue<Bet> mWinnerOverwallBet = new Queue<Bet>();
        private static Queue<Bet> mLooserOverwallBet = new Queue<Bet>();

        private static Random mRandom = new Random();

        #region Players
        public static void ResetPlayers()
        {
            mPlayers.Clear();
        }

        public static void RegisterPlayers()
        {
            ResetPlayers();

            bool addPlayers = true;
            int playerCount = 0;
            while (addPlayers)
            {
                ConsoleManager.Print($"Adding players. Type player({playerCount}) name:");
                AddPlayer(new Player(Console.ReadLine()));
                ConsoleManager.Print($"{GetPlayer(playerCount).name} added as player number: {playerCount}");
                ConsoleManager.Print("Add more players? (Y / N)", 200);
                addPlayers = CommandManager.GetYesNoAnswer();
                playerCount++;
            }
        }

        public static void AddPlayer(Player p)
        {
            mPlayers.Add(p);
        }

        public static Player GetPlayer(int index)
        {
            return mPlayers[index];
        }

        public static List<Player> GetAllPlayers(){
            return mPlayers;
        }

        public static int PlayerCount()
        {
            return mPlayers.Count;
        }

        public static void LogPlayers()
        {
            for (int i = 0; i < mPlayers.Count; i++)
            {
                mPlayers[i].Dump();
            }
        }
        #endregion

        #region Dices
        public static void ResetDices()
        {
            mDices.Clear();

            var camelColors = Enum.GetValues(typeof(CamelColors));

            foreach (CamelColors c in camelColors)
            {
                mDices.Add(new Dice(c));
            }
        }

        public static int RollRandomDice(out CamelColors color)
        {
            if (mDices.Count <= 0)
                throw new Exception("No dices to roll.");

            var dice = mDices[RandomUtils.InRange(0, mDices.Count)];
            color = dice.color;

            mDices.Remove(dice);

            return dice.Roll();
        }

        public static int GetDiceCount()
        {
            return mDices.Count;
        }
        #endregion

        #region Camels

        public static void LogAvailableCamels(){
            ConsoleManager.Print("Camels that didn't moved this leg");
            string pretty;
                          
            for (int i = 0; i < mDices.Count; i++)
            {
                pretty = i == mDices.Count - 1 ? "  ╚═ " : "  ╠═ ";
                ConsoleManager.Print(pretty + $"{ColorUtils.colorsName[mDices[i].color]}");
            }
        }

        private static void CreateCamels()
        {
            var camelColors = Enum.GetValues(typeof(CamelColors));
            foreach (CamelColors c in camelColors)
            {
                Camel camel = new Camel(c);
                mCamels.Add(camel);
            }
        }

        public static void ResetCamels()
        {
            if (mCamels.Count == 0)
                CreateCamels();

            for (int i = 0; i < mCamels.Count; i++)
            {
                mCamels[i].ForcePiecePosition(0);
            }
        }

        public static Camel GetCamel(CamelColors color)
        {
            for (int i = 0; i < mCamels.Count; i++)
            {
                if (mCamels[i].color.Equals(color))
                    return mCamels[i];
            }
            return null;
        }

        public static List<Camel> GetAllCamels()
        {
            return mCamels;
        }
        #endregion

        #region Bets
        public static void ResetLegBetCards()
        {
            mLegBet = new List<LegBet>();
            for (int i = 0; i < mCamels.Count; i++)
            {
                mLegBet.Add(new LegBet(mCamels[i], new List<int> { 5, 1, -1, -1, -1 }));
                mLegBet.Add(new LegBet(mCamels[i], new List<int> { 3, 1, -1, -1, -1 }));
                mLegBet.Add(new LegBet(mCamels[i], new List<int> { 2, 1, -1, -1, -1 }));
            }
        }

        public static void RemoveLegBetCard(LegBet card)
        {
            mLegBet.Remove(card);
        }

        public static List<LegBet> GetBestLegCardFromEachCamel()
        {
            List<LegBet> toReturn = new List<LegBet>();

            for (int i = 0; i < mCamels.Count; i++)
            {
                var bestCard = GetLegBetCard(mCamels[i]);
                if (bestCard == null) continue;
                toReturn.Add(bestCard);
            }

            return toReturn;
        }

        public static LegBet GetLegBetCard(Camel camel)
        {
            var cList = mLegBet.Where(x => x.camel.name.Equals(camel.name)).ToList();
            if (cList.Count == 0) return null;

            LegBet bestCard = cList[0];
            for (int i = 1; i < cList.Count; i++)
            {
                if (cList[i].rewards[0] > bestCard.rewards[0])
                    bestCard = cList[i];
            }
            return bestCard;
        }

        public static void LogLegBets()
        {
            string pretty = "";

            ConsoleManager.Print("Leg bet cards with players", 100);
            var players = GetAllPlayers().Where(x => x.legBetCards.Count > 0).ToList();
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                for (int j = 0; j < player.legBetCards.Count; j++)
                {
                    var card = player.legBetCards[j];
                    pretty = (j == player.legBetCards.Count - 1 && i == players.Count - 1) ? "  ╚═ " : "  ╠═ ";
                    ConsoleManager.Print(pretty + $"{player.name} has {card.ToString()}", 100);
                }
            }
            if (players.Count == 0)
                ConsoleManager.Print("  ╚═ None");

            ConsoleManager.Print("Leg Bet cards availables", 100);
            var bestCards = GetBestLegCardFromEachCamel();
            for (int i = 0; i < bestCards.Count; i++)
            {
                pretty = i == bestCards.Count - 1 ? "  ╚═ " : "  ╠═ ";
                ConsoleManager.Print(pretty + bestCards[i].ToString(), 100);
            }
            if (bestCards.Count == 0)
                ConsoleManager.Print("  ╚═ None");
        }

        public static void QueueOverallBet(Bet bet, bool bettingOnWinner)
        {
            if (bettingOnWinner)
                mWinnerOverwallBet.Enqueue(bet);
            else
                mLooserOverwallBet.Enqueue(bet);
        }

        public static Queue<Bet> GetOverallWinnerQueue()
        {
            return mWinnerOverwallBet;
        }

        public static Queue<Bet> GetOverallLooserQueue()
        {
            return mLooserOverwallBet;
        }

        public static void LogOverallBets()
        {
            ConsoleManager.Print("Overall bets", 100);
            ConsoleManager.Print($"  ╠═ Bets in winner: {mWinnerOverwallBet.Count}", 100);
            ConsoleManager.Print($"  ╚═ Bets in looser: {mLooserOverwallBet.Count}", 100);
        }
        #endregion
    }
}
