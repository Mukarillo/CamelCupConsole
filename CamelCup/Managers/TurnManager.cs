using System;
using System.Linq;
using System.Collections.Generic;
using CamelCup.Actions;
using CamelCup.Boards;
using CamelCup.Boards.Pieces;
using CamelCup.Utils;

namespace CamelCup
{
    public static class TurnManager
    {
        private static int mCurrentPlayerIndex = 0;
        private static Player mCurrentPlayer;
        public static bool isGameOver;

        public static void StartGame()
        {
            GameManager.ResetDices();
            Board.ResetBoard();
            GameManager.ResetCamels();
            GameManager.ResetLegBetCards();
            GameManager.GetAllPlayers().ForEach(x => x.ResetOverallBetCards());

            PlayerTurn(mCurrentPlayerIndex); 
        }

        public static void Back()
        {
            PlayerTurn(mCurrentPlayerIndex);
        }

        public static void EndGame(Camel camel){
            isGameOver = true;

            Board.Dump();

            ConsoleManager.JumpLine();

            ConsoleManager.Print($"The game is over. Camel {camel.fullName} crossed the line.", 200);
            var camels = GameManager.GetAllCamels();
            string pretty = "";
            for (int i = 0; i < camels.Count; i++)
            {
                pretty = camels.Count - 1 == 0 ? "  ╚═" : "  ╠═";
                ConsoleManager.Print(pretty + $"{TextUtils.Colocation(i+1)} camel: {Board.GetCamelAtPosition(i).fullName}", 200);
            }

            int betIndex = 0;
            pretty = "";
            Camel winner = Board.GetCamelAtPosition(0);

            ConsoleManager.JumpLine();

            ConsoleManager.Print("Checking overall winner bets cards", 200);

            var winQueue = GameManager.GetOverallWinnerQueue();
            if (winQueue.Count == 0)
            {
                ConsoleManager.Print($"  ╚═No one placed a bet on winner, so no one get coins.", 200);
            }
            else
            {
                do
                {
                    var bet = winQueue.Dequeue();
                    pretty = winQueue.Count == 0 ? "  ╚═" : "  ╠═";
                    var value = 0;

                    if (bet.camel.name.Equals(winner.name))
                    {
                        value = GameManager.overallBetPaytable[betIndex];
                        betIndex++;
                    }
                    else
                    {
                        value = GameManager.overallMissBet;
                    }

                    ConsoleManager.Print(pretty + $"{bet.owner.name} made a bet on {bet.camel.fullName} to win and {TextUtils.EarnedOrLost(value)} {TextUtils.NoSignal(value)} {TextUtils.CoinOrCoins(value)}.", 200);
                    bet.owner.ChangeCoins(value);
                } while (winQueue.Count > 0);
            }

            Camel looser = Board.GetCamelAtPosition(GameManager.GetAllCamels().Count - 1);
            betIndex = 0;

            ConsoleManager.JumpLine();

            ConsoleManager.Print("Checking overall looser bets cards", 200);

            var looseQueue = GameManager.GetOverallLooserQueue();
            if (looseQueue.Count == 0)
            {
                ConsoleManager.Print($"  ╚═ No one placed a bet on looser, so no one get coins.");
            }
            else
            {
                do
                {
                    var bet = looseQueue.Dequeue();
                    pretty = looseQueue.Count == 0 ? "  ╚═" : "  ╠═";
                    var value = 0;

                    if (bet.camel.name.Equals(looser.name))
                    {
                        value = GameManager.overallBetPaytable[betIndex];
                        betIndex++;
                    }
                    else
                    {
                        value = GameManager.overallMissBet;
                    }

                    ConsoleManager.Print(pretty + $"{bet.owner.name} made a bet on {bet.camel.fullName} to loose and {TextUtils.EarnedOrLost(value)} {TextUtils.NoSignal(value)} {TextUtils.CoinOrCoins(value)}.", 200);
                    bet.owner.ChangeCoins(value);
                } while (looseQueue.Count > 0);
            }

            ConsoleManager.JumpLine();

            GameManager.LogPlayers();

            ConsoleManager.JumpLine();

            var playerWinner = GameManager.GetAllPlayers().OrderByDescending(x => x.coins).First();
            ConsoleManager.Print($"Congratulations {playerWinner.name} you won the game with {playerWinner.coins} {TextUtils.CoinOrCoins(playerWinner.coins)}.!", 200);
        }

        private static void PlayerTurn(int index)
        {
            if (isGameOver) return;

            mCurrentPlayer = GameManager.GetPlayer(index);

            ConsoleManager.Print($"═ New turn ═ Player({index}): {mCurrentPlayer.name}", 200);

            var action = GetPlayerAction();

            ConsoleManager.JumpLine();

            action.DoAction(mCurrentPlayer);
            action.AskActionParameters();

            ConsoleManager.JumpLine();

            mCurrentPlayerIndex++;
            if(mCurrentPlayerIndex >= GameManager.PlayerCount())
                mCurrentPlayerIndex = 0;

            if (GameManager.GetDiceCount() == 0)
                EndRound();

            PlayerTurn(mCurrentPlayerIndex);
        }

        private static void EndRound()
        {
            if (isGameOver) return;

            ConsoleManager.Print("═ End of leg ═ ", 200);
            ConsoleManager.Print("╦══ Leg result ", 200);

            var players = GameManager.GetAllPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                string pretty = (player.legBetCards.Count == 0 && i == players.Count -1) ? "╚═ " : "╠═ ";
                if (player.diceRolled > 0)
                {
                    ConsoleManager.Print(pretty + $"{player.name} rolled {player.diceRolled} so he/she earn {player.diceCoinValue} {TextUtils.CoinOrCoins(player.diceCoinValue)} coins.", 200);
                    player.ChangeCoins(player.diceCoinValue);
                }

                for (int j = 0; j < player.legBetCards.Count; j++)
                {
                    var card = player.legBetCards[j];
                    var betValue = card.rewards[Board.GetCamelPosition(card.camel)];
                    player.ChangeCoins(betValue);

                    pretty = (i == players.Count - 1 && j == player.legBetCards.Count - 1) ? "╚═ " : "╠═ ";
                    string earnedOrLost = TextUtils.EarnedOrLost(betValue);
                    ConsoleManager.Print(pretty + $"{player.name} {earnedOrLost} {TextUtils.NoSignal(betValue)} {TextUtils.CoinOrCoins(betValue)} betting on {card.camel.fullName}.", 200);
                }

                player.RoundReset();
            }

            ConsoleManager.Print(TextUtils.SEPARATOR, 2000);
            GameManager.ResetDices();
        }

        private static BaseAction GetPlayerAction()
        {
            ConsoleManager.Print($"╦═ {mCurrentPlayer.name}, you have these actions to choose:", 200);
            var actions = GetActions();
            List<string> options = new List<string>();
            for (int i = 0; i < actions.Count; i++){
                string pretty = i == actions.Count - 1 ? "╚═" : "╠═";
                ConsoleManager.Print(pretty + $"[{i}] {actions[i].description}", 100);
                options.Add(i.ToString());
            }
            var playerOption = int.Parse(CommandManager.GetMultipleChoiseAnswer(options));
            return actions[playerOption];
        }

        private static List<BaseAction> GetActions()
        {
            var toReturn = new List<BaseAction> { new RollDieAction(), new PlaceTrapAction() };
            if(mCurrentPlayer.legBetCards.Count > 0)
                toReturn.Add(new BetLegAction());
            if (mCurrentPlayer.overallBetCards.Count > 0)
                toReturn.Add(new BetOverallAction());

            return toReturn;
        }
    }
}