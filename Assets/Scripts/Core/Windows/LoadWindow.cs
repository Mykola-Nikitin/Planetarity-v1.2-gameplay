using Windows;
using Core.Saves;
using Core.UI;
using DI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Windows
{
    public class LoadWindow : BaseWindow
    {
        public Button BackButton;

        [Header("Scroll list")]
        public SaveLoadCell CellPrototype;
        public RectTransform Content;
        public float         Spacing;

        private ISaveLoadService _saveLoadService;

        public override void Show(bool show)
        {
            base.Show(show);

            BackButton.onClick.AddListener(OnBackClicked);
            
            _saveLoadService = DIContainer.Get<ISaveLoadService>();
            string[] saves = _saveLoadService.GetSaves();

            if (saves == null)
            {
                return;
            }

            float cellHeight = CellPrototype.GetHeight();
            float scrollHeight = saves.Length * cellHeight;
        
            Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollHeight);

            for (int i = 0; i < saves.Length; i++)
            {
                GameObject cell = Instantiate(CellPrototype.gameObject, Content);
                cell.SetActive(true);
                cell.TryGetComponent(out SaveLoadCell saveLoadCell);
                saveLoadCell.Clicked = OnCellClicked;

                Vector2 position = saveLoadCell.RectTransform.anchoredPosition;
                position.y = -cellHeight * i - i * Spacing;
            
                saveLoadCell.RectTransform.anchoredPosition = position;
            
                saveLoadCell.Setup(saves[i]);
            }
        }

        private void OnBackClicked()
        {
            WindowsSystem.Instance.GoBack(WindowLayerType.Screen);
        }

        private void OnCellClicked(string name)
        {
            _saveLoadService.Load(name);
        }
    }
}