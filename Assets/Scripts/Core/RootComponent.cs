using Windows;
using Core.Commands;
using Core.Commands.Implementation;
using UnityEngine;

public class RootComponent : MonoBehaviour
{
    private void Awake()
    {
        WindowsSystem.Instance.CreateWindow(WindowType.Loading, WindowLayerType.Screen, null);
        
        CommandQueue commandQueue = new CommandQueue(OnCommandsFinished);
        commandQueue.Add(new PrepareDependenciesCommand());
        commandQueue.Add(new PrepareSignalsCommand());
        commandQueue.Add(new PrepareServicesCommand());
        commandQueue.Add(new PrepareAssetsCommand());
        commandQueue.Run();
    }

    private void OnCommandsFinished()
    {
        WindowsSystem.Instance.CreateWindow(WindowType.MainMenu, WindowLayerType.Screen, null);
    }
}