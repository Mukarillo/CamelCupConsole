using System.Linq;
using System.Collections.Generic;
using CamelCup.Boards.Pieces;
using System;
using CamelCup.Utils;

namespace CamelCup.Boards
{
    public static class Board
    {
        public const int MAX_SPACES = 17;

        private static List<List<BoardPiece>> mBoardPositions= new List<List<BoardPiece>>();

        public static void ResetBoard()
        {
            mBoardPositions = new List<List<BoardPiece>>();
            for (int i = 0; i < MAX_SPACES; i++)
            {
                mBoardPositions.Add(new List<BoardPiece>());
            }
        }

        public static int PlacePiece(BoardPiece piece, int position)
        {            
            mBoardPositions[position].Add(piece);
            return position;
        }

        public static void RemovePiece(BoardPiece piece)
        {
            mBoardPositions[piece.position].Remove(piece);
        }

        public static int MovePiece(Camel camel, int spaces)
        {
            int finalPos = camel.position + spaces;

            bool gameOver = false;
            if(finalPos > MAX_SPACES - 1)
            {
                gameOver = true;
                finalPos = MAX_SPACES - 1;
            }

            bool trapBackward = false;

            var trap = mBoardPositions[finalPos].OfType<Trap>().ToList();
            if(trap.Count > 0 && trap[0] != null)
            {
                if (trap[0].positionModifier < 0)
                    trapBackward = true;
                
                finalPos += trap[0].positionModifier;
                trap[0].owner.ChangeCoins(trap[0].actionCoinValue);

                List<string> message = new List<string>
                {
                    "Camel ",
                    camel.name,
                    $" got caught in {trap[0].owner.name} trap. His position now is {finalPos}."
                };
                List<ConsoleColor> colors = new List<ConsoleColor>
                {
                    ConsoleColor.Black,
                    ColorUtils.GetConsoleColor(camel.color),
                    ConsoleColor.Black
                };
                ConsoleManager.PrintColored(message, colors);
            }

            var stack = GetCamelStack(camel);

            mBoardPositions[camel.position].Remove(camel);
            mBoardPositions[finalPos].Insert(trapBackward ? 0 : mBoardPositions[finalPos].Count, camel);

            for (int i = 0; i < stack.Count; i++)
            {
                mBoardPositions[camel.position].Remove(stack[i]);
                stack[i].position = finalPos;
                mBoardPositions[finalPos].Insert(mBoardPositions[finalPos].Count, stack[i]);
            }

            if (gameOver)
                TurnManager.EndGame(camel);

            return finalPos;
        }

        public static List<int> GetAvailableTrapPositions()
        {
            List<int> positionsAvailable = new List<int>();

            for (int i = 2; i < mBoardPositions.Count; i++)
            {
                if (mBoardPositions[i].Count > 0)
                    continue;

                var left = mBoardPositions[i - 1].OfType<Trap>().Any();
                var right = i == mBoardPositions.Count - 1;
                if (!right)
                    right = mBoardPositions[i + 1].OfType<Trap>().Any();

                if(!left && !right){
                    positionsAvailable.Add(i);
                }
            }

            return positionsAvailable;
        }

        public static int GetCamelPosition(Camel camel) 
        {
            int position = GameManager.GetAllCamels().Count - 1;

            for (int i = 0; i < mBoardPositions.Count; i++)
            {
                for (int j = 0; j < mBoardPositions[i].Count; j++)
                {
                    if (mBoardPositions[i][j] is Camel && ((Camel)mBoardPositions[i][j]).name.Equals(camel.name))
                        return position;
                    else
                        position--;
                }
            }

            return position;
        }

        public static void LogFirstCamel()
        {
            var camel = GetCamelAtPosition(0);
            ConsoleManager.Print($"1st camel {camel.fullName}");
        }

        public static void LogLastCamel()
        {
            var camel = GetCamelAtPosition(GameManager.GetAllCamels().Count - 1);
            ConsoleManager.Print($"{TextUtils.Colocation(GameManager.GetAllCamels().Count)} camel {camel.fullName}");
        }

        public static Camel GetCamelAtPosition(int position)
        {
            var camels = GameManager.GetAllCamels();

            if (position > camels.Count)
                throw new Exception("Position out of range.");

            for (int i = 0; i < camels.Count; i++)
            {
                if (GetCamelPosition(camels[i]) == position)
                    return camels[i];
            }

            return null;
        }

        private static List<Camel> GetCamelStack(Camel camel)
        {
            List<Camel> toReturn = new List<Camel>();

            if (camel.position == 0)
                return toReturn;
            
            int index = mBoardPositions[camel.position].IndexOf(camel) + 1;

            for (int i = index; i < mBoardPositions[camel.position].Count; i++)
            {
                toReturn.Add(mBoardPositions[camel.position][i] as Camel);
            }

            return toReturn;
        }

        public static void Dump()
        {
            List<string> toPrint = new List<string>();
            List<ConsoleColor> colors = new List<ConsoleColor>();

            for (int i = 0; i < mBoardPositions.Count; i++)
            {
                toPrint.Add($"[{i}: ");
                colors.Add(ConsoleColor.Black);
                if (mBoardPositions[i].Count > 0)
                {
                    for (int j = 0; j < mBoardPositions[i].Count; j++)
                    {
                        var obj = mBoardPositions[i][j];
                        toPrint.Add(obj.name + " ");

                        if(obj is Camel)
                            colors.Add(ColorUtils.GetConsoleColor((obj as Camel).color));
                        else
                            colors.Add(ConsoleColor.Black);
                    }
                    toPrint[toPrint.Count - 1] = toPrint[toPrint.Count - 1].Remove(toPrint[toPrint.Count - 1].Length - 1);
                }

                toPrint.Add($"]");
                colors.Add(ConsoleColor.Black);
            }

            ConsoleManager.PrintColored(toPrint, colors);
        }
    }
}
