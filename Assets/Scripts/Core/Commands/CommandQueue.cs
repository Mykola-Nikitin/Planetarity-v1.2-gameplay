using System;
using System.Collections.Generic;

namespace Core.Commands
{
    public class CommandQueue
    {
        private Queue<ICommand> _queue = new Queue<ICommand>();
        
        private readonly Action  _onFinished;

        public CommandQueue(Action onFinished)
        {
            _onFinished = onFinished;
        }

        public void Add(ICommand command)
        {
            command.Finished += OnCommandFinished;
            
            _queue.Enqueue(command);
        }

        public void Run()
        {
           _queue.Dequeue().Execute();
        }

        private void OnCommandFinished(ICommand command)
        {
            command.Finished -= OnCommandFinished;
            
            if (_queue.Count > 0)
            {
                Run();
            }
            else
            {
                _onFinished?.Invoke();
            }
        }
    }
}