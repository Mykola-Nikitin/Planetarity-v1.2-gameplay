namespace Core.Planets
{
    public interface IMoveLogic
    {
        float Timer { get; }
        void  Move();
        void  Initialize(float timer);
    }
}