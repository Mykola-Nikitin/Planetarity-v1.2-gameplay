using System;

namespace Core.Commands
{
    public interface ICommand
    {
        event Action<ICommand> Finished;
        
        void Execute();
    }
}