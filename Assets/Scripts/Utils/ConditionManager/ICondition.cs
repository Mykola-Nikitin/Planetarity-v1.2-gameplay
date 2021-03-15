using System;

namespace Utils.ConditionManager
{
    public interface ICondition
    {
        event Action Check;
        
        bool IsDone { get; }

        void Dispose();
    }
}