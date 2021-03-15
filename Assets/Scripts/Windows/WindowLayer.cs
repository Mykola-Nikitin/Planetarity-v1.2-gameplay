using System.Collections.Generic;
using System.Linq;
using Windows;
using UnityEngine;

namespace Core.Components.Windows
{
    public class WindowLayer : MonoBehaviour
    {
        public GameObject Blackout;
        private List<WindowProxy> _windowsQueue = new List<WindowProxy>();

        public  WindowType  PrevWindow;
        private WindowProxy _currentWindow;

        public void AddWindow(WindowProxy windowProxy)
        {
            windowProxy.Closed += OnClosed;
            
            if (_windowsQueue.Any())
            {
                PrevWindow = _currentWindow.WindowType;
                _currentWindow?.Close(true);
                _windowsQueue.Remove(_currentWindow);
                _currentWindow = null;
            }
            
            Blackout.SetActive(true);

            _currentWindow?.Hide();
            
            windowProxy.Open();
        
            _windowsQueue.Add(windowProxy);

            _currentWindow = windowProxy;
        }

        private void OnClosed(WindowProxy proxy)
        {
            _windowsQueue.Remove(proxy);

            proxy.Closed -= OnClosed;
            
            Blackout.SetActive(false);

            WindowProxy prev = _windowsQueue.LastOrDefault();

            _currentWindow = prev;
            prev?.Open();
        }

        public void Close()
        {
            _currentWindow?.Close(false);
        }

        public void CloseAll()
        {
            for (var i = 0; i < _windowsQueue.Count; i++)
            {
                _windowsQueue[i].Close(true);
            }
            
            _currentWindow = null;
        }
    }
}