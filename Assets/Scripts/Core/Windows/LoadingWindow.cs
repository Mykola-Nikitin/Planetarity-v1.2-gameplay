using System.Text;
using TMPro;
using UnityEngine;

namespace Core.Windows
{
    public class LoadingWindow : BaseWindow
    {
        public TMP_Text Loading;
        public int      MaxDots = 3;

        private string _text = "Loading";

        private float _changeInterval = 0.3f;
        private float _timer;
        private int   _currentDot;
    
        private void Update()
        {
            _timer += Time.unscaledDeltaTime;

            if (_timer >= _changeInterval)
            {
                _currentDot++;
                _currentDot %= MaxDots + 1;

                var sb = new StringBuilder(_text);
            
                for (int i = 0; i < _currentDot; i++)
                {
                    sb.Append('.');
                }

                _timer       = 0f;
                Loading.text = $"{sb}";
            }
        }
    }
}