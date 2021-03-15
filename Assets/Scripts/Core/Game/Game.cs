using UnityEngine;
using Utils.ConditionManager;

namespace Core
{
    public class Game : MonoBehaviour
    {
        public GameObject InGameUI;
        public Transform  Container;
        public Transform  Sun;

        private ConditionManager _conditionManager;

        private GameHandler _handler;

        private void Awake()
        {
            _handler = new GameHandler(this);
            _handler.Initialize();
        }

        private void OnDestroy()
        {
            _handler?.Destroy();
        }
    }
}