using System;
using Core.Signals;
using DI;
using Utils.SignalBus;

namespace Core.Commands.Implementation
{
    public class PrepareSignalsCommand : ICommand
    {
        public event Action<ICommand> Finished;

        private IMessageBus _messageBus;

        public void Execute()
        {
            _messageBus = DIContainer.Get<IMessageBus>();
        
            _messageBus.DefineSignal<PauseSignal>();
            _messageBus.DefineSignal<PlaySignal>();
            _messageBus.DefineSignal<LoadSignal>();
            _messageBus.DefineSignal<FinishSignal>();

            _messageBus.DefineSignal<PointerDownSignal>();

            Finished?.Invoke(this);
        }
    }
}