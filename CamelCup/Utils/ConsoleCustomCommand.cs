using System;
namespace CamelCup.Utils
{
    public class ConsoleCustomCommand
    {
        public string command;
        public string description;
        public Action action;

        public ConsoleCustomCommand(string command, string description, Action action)
        {
            this.command = command;
            this.description = description;
            this.action = action;
        }

		public override string ToString()
		{
            return $"{command}: {description}";
		}
	}
}
