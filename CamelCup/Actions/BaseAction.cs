using System;

namespace CamelCup.Actions
{
    public abstract class BaseAction
    {
        public abstract int rewardValue { get; }
        public abstract string description { get; }
        protected Player owner;

        public virtual void DoAction(Player owner)
        {
            this.owner = owner;
        }

        public abstract void AskActionParameters();
    }
}
