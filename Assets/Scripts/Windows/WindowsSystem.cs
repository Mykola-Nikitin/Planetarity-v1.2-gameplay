using System;
using System.Collections.Generic;
using Windows.Data;
using Core.Components.Windows;
using Core.Windows;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Windows
{
    public class WindowsSystem : MonoBehaviour
    {
        public static WindowsSystem Instance { get; private set; }

        public List<BaseWindow> WindowsAssets;
    
        public WindowLayer ScreenLayer;
        public WindowLayer PopupLayer;

        private readonly List<WindowProxy>       _windowsQueue = new List<WindowProxy>();
        private readonly List<IResourceLocation> _locations;

        private readonly Dictionary<WindowType, BaseWindow> _windows = new Dictionary<WindowType, BaseWindow>();

        private Action _initializeCompleted = delegate { };
    
        private void Awake()
        {
            Instance = this;

            for (int i = 0; i < WindowsAssets.Count; i++)
            {
                BaseWindow window = WindowsAssets[i];
                _windows.Add(window.Type, window);
            }
        }

        public void CreateWindow(WindowType windowType, WindowLayerType layer, IWindowData data)
        {
            WindowProxy proxy;

            switch (layer)
            {
                case WindowLayerType.Screen:
                    proxy = new WindowProxy(_windows[windowType], data, ScreenLayer);
                    ScreenLayer.AddWindow(proxy);
                    break;
            
                case WindowLayerType.Popup:
                    proxy = new WindowProxy(_windows[windowType], data, PopupLayer);
                    PopupLayer.AddWindow(proxy);
                    break;
            }
        }

        public void GoBack(WindowLayerType layer)
        {
            WindowType type = WindowType.None;
            
            switch (layer)
            {
                case WindowLayerType.Screen:
                    type = ScreenLayer.PrevWindow;
                    break;
            
                case WindowLayerType.Popup:
                    type = PopupLayer.PrevWindow;
                    break;
            }

            CreateWindow(type, layer, null);
        }

        public void CloseWindow(WindowLayerType layer)
        {
            switch (layer)
            {
                case WindowLayerType.Screen:
                    ScreenLayer.Close();
                    break;
            
                case WindowLayerType.Popup:
                    PopupLayer.Close();
                    break;
            }
        }
    }
}