using System;
using Windows.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        public WindowType   Type;
        public Button       CloseButton;
        public IWindowData  Args   { get; set; }
        public Action<bool> Closed { get; set; } = delegate {  };

        protected virtual void OnEnable()
        {
            CloseButton?.onClick.AddListener(Close);
        }

        protected virtual void OnDisable()
        {
            CloseButton?.onClick.RemoveListener(Close);
        }

        public virtual void SetArgs(IWindowData args) 
        {
            Args = args;
        }

        public virtual void Close()
        {
            Closed(false);
        }

        public virtual void Show(bool show)
        {
            gameObject.SetActive(show);
        }
    }
}