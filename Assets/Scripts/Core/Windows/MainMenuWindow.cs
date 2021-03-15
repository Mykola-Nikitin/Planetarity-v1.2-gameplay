using Windows;
using Core.Signals;
using DI;
using UnityEngine.UI;
using Utils.SignalBus;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Windows
{
    public class MainMenuWindow : BaseWindow
    {
        public Button PlayButton;
        public Button LoadButton;
        public Button QuitButton;
    
        private IMessageBus _messageBus;

        public override void Show(bool show)
        {
            base.Show(show);

            _messageBus = DIContainer.Get<IMessageBus>();
        
            PlayButton.onClick.AddListener(OnPlayClick);
            LoadButton.onClick.AddListener(OnLoadClick);
            QuitButton.onClick.AddListener(OnQuitClick);

        }
    
        private void OnDestroy()
        {
            PlayButton.onClick.RemoveListener(OnPlayClick);
            LoadButton.onClick.RemoveListener(OnLoadClick);
            QuitButton.onClick.RemoveListener(OnQuitClick);
        }
    
        private void OnPlayClick()
        {
            _messageBus.Fire(new PlaySignal());
        } 
    
        private void OnLoadClick()
        {
            WindowsSystem.Instance.CreateWindow(WindowType.SaveLoad, WindowLayerType.Screen, null);
        }  
    
        private void OnQuitClick()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
        
        }

    }
}