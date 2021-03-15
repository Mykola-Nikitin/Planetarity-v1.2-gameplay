using System;
using Windows.Data;
using Core.Components.Windows;
using Core.Windows;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Windows
{
    public class WindowProxy
    {
        public IWindowData Args       => _args;
        public WindowType  WindowType => _baseWindow?.Type ?? WindowType.None;

        public Action<WindowProxy> Closed     { get; set; } = delegate { };

        private readonly IWindowData _args;
        private readonly WindowLayer _layer;
        private          BaseWindow  _baseWindow;
        private          BaseWindow  _windowPrototype;

        public WindowProxy(BaseWindow window, IWindowData args, WindowLayer screenLayer)
        {
            _layer           = screenLayer;
            _windowPrototype = window;
            _args            = args;
        }

        public void Open()
        {
            GameObject windowObject = Object.Instantiate(_windowPrototype.gameObject, _layer.transform);

            if (windowObject.TryGetComponent(out _baseWindow))
            {
                _baseWindow.transform.localPosition =  Vector3.zero;
                _baseWindow.transform.localScale    =  Vector3.one;
                _baseWindow.Closed                  += Close;

                _baseWindow.SetArgs(_args);
                _baseWindow.Show(true);
            }
        }

        public void Close(bool silent)
        {
            if (_baseWindow)
            {
                _baseWindow.Closed -= Close;

                Object.Destroy(_baseWindow.gameObject);
            }

            if (!silent)
            {
                Closed(this);
            }
        }

        public void Hide()
        {
            if (_baseWindow)
            {
                _baseWindow.Show(false);
            }
        }

        public void Show()
        {
            if (_baseWindow)
            {
                _baseWindow.Show(true);
            }
        }
    }
}