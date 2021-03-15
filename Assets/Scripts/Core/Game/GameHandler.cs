using Windows;
using Core.Conditions;
using Core.Planets;
using Core.Signals;
using Core.Utils;
using Core.Weapons;
using Core.Windows.Data;
using DI;
using UnityEngine;
using Utils.ConditionManager;
using Utils.SignalBus;

namespace Core
{
    public class GameHandler
    {
        private IMessageBus     _messageBus;
        private IPlanetsService _planetsService;
        private IRocketsService _rocketsService;

        private GameModel _gameModel;
        private Game      _view;

        private ConditionManager _conditionManager;
        private Bindable<bool>   _isReadyField = new Bindable<bool>();
        private Bindable<bool>   _onPlanetsLoaded = new Bindable<bool>();
        private Bindable<bool>   _onRocketsLoaded = new Bindable<bool>();

        public GameHandler(Game view)
        {
            _view = view;

            _messageBus     = DIContainer.Get<IMessageBus>();
            _planetsService = DIContainer.Get<IPlanetsService>();
            _rocketsService = DIContainer.Get<IRocketsService>();

            _gameModel = DIContainer.Get<GameModel>();
        }

        public void Initialize()
        {
            _messageBus.AddListener<FinishSignal>(OnFinished);
            _messageBus.AddListener<PlaySignal>(OnPlay);
            _messageBus.AddListener<PauseSignal>(OnPause);
            _messageBus.AddListener<LoadSignal>(OnLoad);
        }

        public void Destroy()
        {
            _messageBus.RemoveListener<FinishSignal>(OnFinished);
            _messageBus.RemoveListener<PlaySignal>(OnPlay);
            _messageBus.RemoveListener<PauseSignal>(OnPause);
            _messageBus.RemoveListener<LoadSignal>(OnLoad);
        }

        private void OnPlay(PlaySignal signalData)
        {
            WindowsSystem.Instance.CreateWindow(WindowType.Loading, WindowLayerType.Screen, null);
            
            _isReadyField.Value = false;

            _planetsService.Clear();
            _rocketsService.Clear();

            _gameModel.Sun    = _view.Sun;
            _gameModel.Parent = _view.Container;

            _conditionManager = new ConditionManager();

            _conditionManager.ConditionsDone += OnConditionsDone;
            _conditionManager.AddCondition(new Condition<bool>(_isReadyField, true));
            _conditionManager.Check();

            _planetsService.GeneratePlanets(OnGenerationFinished);
        }

        private void OnPause(PauseSignal signalData)
        {
            if (signalData.Paused)
            {
                _view.InGameUI.SetActive(false);

                Time.timeScale = 0f;

                WindowsSystem.Instance.CreateWindow(WindowType.Pause, WindowLayerType.Screen, null);
            }
            else
            {
                _view.InGameUI.SetActive(true);

                Time.timeScale = 1f;
            }
        }

        private void Win()
        {
            _view.Container.gameObject.SetActive(false);

            var finishData = new FinishWindowData {Text = "Congratulations!\nYOU WIN!!!"};
            WindowsSystem.Instance.CreateWindow(WindowType.Finish, WindowLayerType.Screen, finishData);
        }

        private void Lose()
        {
            _view.Container.gameObject.SetActive(false);

            var finishData = new FinishWindowData {Text = "So close!\nYOU LOSE :("};
            WindowsSystem.Instance.CreateWindow(WindowType.Finish, WindowLayerType.Screen, finishData);
        }

        private void OnGenerationFinished()
        {
            _isReadyField.Value = true;
        }

        private void OnConditionsDone()
        {
            _conditionManager.ConditionsDone -= OnConditionsDone;
            _conditionManager.Dispose();
            _conditionManager = null;

            WindowsSystem.Instance.CloseWindow(WindowLayerType.Screen);

            _view.Container.gameObject.SetActive(true);
            _view.InGameUI.SetActive(true);
        }

        private void OnFinished(FinishSignal signalData)
        {
            if (signalData.IsWin)
            {
                Win();
            }
            else
            {
                Lose();
            }
        }

        private void OnLoad(LoadSignal signalData)
        {
            WindowsSystem.Instance.CreateWindow(WindowType.Loading, WindowLayerType.Screen, null);
            
            _planetsService.Clear();
            _rocketsService.Clear();
            
            _gameModel.Sun    = _view.Sun;
            _gameModel.Parent = _view.Container;

            _conditionManager = new ConditionManager();
            
            _conditionManager.ConditionsDone += OnLoadingDone;
            _conditionManager.AddCondition(new Condition<bool>(_onPlanetsLoaded, true));
            _conditionManager.AddCondition(new Condition<bool>(_onRocketsLoaded, true));

            _planetsService.Load(signalData.GameSaveData.Planets, OnPlanetsLoaded);
            _rocketsService.Load(signalData.GameSaveData.Rockets, OnRocketsLoaded);
        }

        private void OnLoadingDone()
        {
            _conditionManager.ConditionsDone -= OnLoadingDone;
            
            OnConditionsDone();

            _onPlanetsLoaded.Value = false;
            _onRocketsLoaded.Value = false;
        }

        private void OnPlanetsLoaded()
        {
            _onPlanetsLoaded.Value = true;
        }
        
        private void OnRocketsLoaded()
        {
            _onRocketsLoaded.Value = true;
        }
    }
}