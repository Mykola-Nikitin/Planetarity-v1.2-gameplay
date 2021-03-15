using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class SaveLoadCell : MonoBehaviour
    {
        public RectTransform RectTransform;
        
        [SerializeField]
        private TMP_Text Name;
        [SerializeField]
        private Button   Button;    
        
        public Action<string> Clicked;

        private string _name;

        public void Setup(string saveName)
        {
            _name     = saveName;
            Name.text = GetReadableName(saveName);

            Button.onClick.AddListener(OnClicked);
        }

        public float GetHeight()
        {
           return RectTransform.sizeDelta.y;
        }
        
        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            Clicked?.Invoke(_name);
        }
        
        private string GetReadableName(string saveName)
        {
            string displayName = saveName;
            if (long.TryParse(saveName, out long ticks))
            {
                displayName = DateTime.FromFileTime(ticks).ToString(CultureInfo.CurrentCulture);
            }
            return displayName;
        }
    }
}