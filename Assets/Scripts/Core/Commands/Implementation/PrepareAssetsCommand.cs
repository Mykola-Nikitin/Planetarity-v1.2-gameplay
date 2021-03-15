using System;
using Core.Conditions;
using Core.Utils;
using Utils.ConditionManager;

namespace Core.Commands.Implementation
{
    public class PrepareAssetsCommand : ICommand
    {
        public event Action<ICommand> Finished;

        private ConditionManager _conditionManager;

        private readonly Bindable<bool> _poolReadyField = new Bindable<bool>();

        private bool PoolReady
        {
            set => _poolReadyField.Value = value;
        }

        public void Execute()
        {
            _conditionManager = new ConditionManager();

            _conditionManager.ConditionsDone += OnConditionsDone;
            _conditionManager.AddCondition(new Condition<bool>(_poolReadyField, true));
            _conditionManager.Check();

            SimplePool.Instance.Initialize(OnPoolInitCompleted);
        }

        private void OnConditionsDone()
        {
            _conditionManager.ConditionsDone -= OnConditionsDone;
            _conditionManager.Dispose();
            _conditionManager = null;
            
            Finished?.Invoke(this);
        }

        private void OnPoolInitCompleted()
        {
            PoolReady = true;
        }
    }
}