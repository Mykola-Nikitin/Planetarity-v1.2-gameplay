namespace Core.Saves
{
    public interface ISaveLoadService : IService
    {
        void Save();
        void Load(string name);
        string[] GetSaves();
    }
}