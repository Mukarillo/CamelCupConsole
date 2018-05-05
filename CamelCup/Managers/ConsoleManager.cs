using System;
using System.Collections.Generic;
using System.Threading;

namespace CamelCup
{
    public struct ConsoleMessage{
        public List<string> message;
        public List<ConsoleColor> colors;
        public int delay;

        public bool singleLine => message.Count == 1;

        public ConsoleMessage(List<string> message, List<ConsoleColor> colors, int delay)
        {
            this.message = message;
            this.colors = colors;
            this.delay = delay;
        }
    }

    public static class ConsoleManager
    {
        private static Queue<ConsoleMessage> mMessageQueue = new Queue<ConsoleMessage>();
        private static bool mIsDequeuing = false;

        public static void Print(string message, int delay = 0)
        {
            PrintColored(new List<string> { message }, new List<ConsoleColor> { ConsoleColor.Black }, delay);
        }

        public static void JumpLine(){
            Print("");
        }

        public static void PrintColored(List<string> message, List<ConsoleColor> colors, int delay = 0)
        {
            mMessageQueue.Enqueue(new ConsoleMessage(message, colors, delay));
            if (!mIsDequeuing)
                DequeuePrint();
        }

        private static void DequeuePrint()
        {
            mIsDequeuing = true;

            var cm = mMessageQueue.Dequeue();
            Thread.Sleep(cm.delay);

            var originalColor = ConsoleColor.Black;

            for (int i = 0; i < cm.message.Count; i++)
            {
                Console.ForegroundColor = cm.colors[i];
                if (cm.singleLine)
                    Console.WriteLine(cm.message[i]);
                else
                    Console.Write(cm.message[i]);
                Console.ForegroundColor = originalColor;
            }

            if (!cm.singleLine)
                Console.WriteLine("");

            if (mMessageQueue.Count > 0)
                DequeuePrint();
            else
                mIsDequeuing = false;
        }
    }
}
