using Windows;
using Core.Saves;
using Core.Signals;
using DI;
using UnityEngine.UI;
using Utils.SignalBus;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Windows
{
    public class PauseWindow : BaseWindow
    {
        public Button ResumeButton;
        public Button SaveButton;
        public Button LoadButton;
        public Button ExitButton;

        private IMessageBus      _messageBus;
        private ISaveLoadService _saveLoadService;

        public override void Show(bool show)
        {
            base.Show(show);
        
            ResumeButton.onClick.AddListener(OnResumeClick);
            SaveButton.onClick.AddListener(OnSaveClick);
            LoadButton.onClick.AddListener(OnLoadClick);
            ExitButton.onClick.AddListener(OnExitClick);

            _messageBus      = DIContainer.Get<IMessageBus>();
            _saveLoadService = DIContainer.Get<ISaveLoadService>();
        }

        private void OnResumeClick()
        {
            _messageBus.Fire(new PauseSignal {Paused = false});
        
            Close();
        }

        private void OnSaveClick()
        {
            _saveLoadService.Save();
            _messageBus.Fire(new PauseSignal {Paused = false});
            
            Close();
        }

        private void OnLoadClick()
        {
            WindowsSystem.Instance.CreateWindow(WindowType.SaveLoad, WindowLayerType.Screen, null);
        }

        private void OnExitClick()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}