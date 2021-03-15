using Core.Signals;
using DI;
using UnityEngine;
using UnityEngine.UI;
using Utils.SignalBus;

namespace Core.UI
{
    public class InGameUI : MonoBehaviour
    {
        public  Button     PauseButton;
        private IMessageBus _messageBus;

        private void Awake()
        {
            _messageBus = DIContainer.Get<IMessageBus>();
            
            PauseButton.onClick.AddListener(OnPauseClick);
        }

        private void OnDestroy()
        {
            PauseButton.onClick.RemoveListener(OnPauseClick);
        }

        private void OnPauseClick()
        {
            _messageBus.Fire(new PauseSignal {Paused = true});
        }
    }
}