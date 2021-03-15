using System;
using System.Collections.Generic;

namespace Utils.ConditionManager
{
    public class ConditionManager : IDisposable
    {
        public event Action ConditionsDone = delegate { };
        
        private List<ICondition> _conditions;

        public ConditionManager()
        {
            _conditions = new List<ICondition>();
        }

        public void AddCondition(ICondition condition)
        {
            condition.Check += Check;
            _conditions.Add(condition);
        }

        public void Check()
        {
            bool allConditionsDone = true;
            
            for (int i = 0; i < _conditions.Count; i++)
            {
                if (!_conditions[i].IsDone)
                {
                    allConditionsDone = false;
                    break;
                }
            }

            if (allConditionsDone)
            {
                ConditionsDone();
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _conditions.Count; i++)
            {
                _conditions[i].Check -= Check;
                _conditions[i].Dispose();
            }
        }
    }
}