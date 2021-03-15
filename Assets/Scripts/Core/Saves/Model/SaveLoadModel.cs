using System.Collections.Generic;

namespace Core.Saves
{
    public class SaveLoadModel
    {
        public List<GameSaveData> Saves { get; set; }
        
        public GameSaveData SelectedSave { get; set; }
    }
}