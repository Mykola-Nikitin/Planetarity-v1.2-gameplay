using Core.Signals;
using DI;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.SignalBus;

namespace Utils
{
    public class InputHandler : MonoBehaviour, IPointerDownHandler
    {       
        private IMessageBus _messageBus;

        public void Start()
        {
            _messageBus = DIContainer.Get<IMessageBus>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _messageBus.Fire(new PointerDownSignal {PointerPosition = eventData.pressPosition});
        }
    }
}