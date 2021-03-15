using Windows;
using Core.Signals;
using Core.Windows.Data;
using DI;
using TMPro;
using UnityEngine.UI;
using Utils.SignalBus;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Windows
{
    public class FinishWindow : BaseWindow
    {
        public  Button     NewGameButton;
        public  Button     LoadGameButton;
        public  Button     QuitButton;
        private IMessageBus _messageBus;

        public TMP_Text Message;

        public new FinishWindowData Args => base.Args as FinishWindowData;

        public override void Show(bool show)
        {
            base.Show(show);

            _messageBus = DIContainer.Get<IMessageBus>();

            NewGameButton.onClick.AddListener(OnNewGameClick);
            LoadGameButton.onClick.AddListener(OnLoadGameClick);
            QuitButton.onClick.AddListener(OnQuitClick);
            
            if (Args is FinishWindowData finishData)
            {
                Message.text = finishData.Text;
            }
        }

        private void OnDestroy()
        {
            NewGameButton.onClick.RemoveListener(OnNewGameClick);
            LoadGameButton.onClick.RemoveListener(OnLoadGameClick);
            QuitButton.onClick.RemoveListener(OnQuitClick);
        }

        private void OnNewGameClick()
        {
            _messageBus.Fire(new PlaySignal());
        }

        private void OnLoadGameClick()
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