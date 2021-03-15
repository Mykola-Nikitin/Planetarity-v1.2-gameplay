using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Planets;
using Core.Signals;
using Core.Weapons;
using DI;
using UnityEngine;
using Utils.SignalBus;

namespace Core.Saves
{
    public class SaveLoadService : ISaveLoadService
    {
        private string _path;

        private SaveLoadModel _model;
        private PlanetsModel  _planetsModel;
        private RocketsModel  _rocketsModel;
        private IMessageBus   _messageBus;

        public void Initialize(Action onInitialized = null)
        {
            _path         = Path.Combine(Application.persistentDataPath, "saves.pt");
            _model        = DIContainer.Get<SaveLoadModel>();
            _planetsModel = DIContainer.Get<PlanetsModel>();
            _rocketsModel = DIContainer.Get<RocketsModel>();
            _messageBus   = DIContainer.Get<IMessageBus>();

            if (File.Exists(_path))
            {
                var bf = new BinaryFormatter();
                FileStream file = File.Open(_path, FileMode.Open);

                _model.Saves = (List<GameSaveData>) bf.Deserialize(file);
            }
            else
            {
                _model.Saves = new List<GameSaveData>();
            }

            onInitialized?.Invoke();
        }

        public string[] GetSaves()
        {
            return _model.Saves?.Select(x => x.Name).ToArray();
        }

        public void Save()
        {
            string saveName = DateTime.Now.ToFileTime().ToString();

            var saveData = new GameSaveData
            {
                Name    = saveName,
                Planets = _planetsModel.Planets.Select(x => x.PlanetData).ToArray(),
                Rockets = _rocketsModel.Rockets.Select(x => x.RocketData).ToArray()
            };

            _model.Saves.Add(saveData);

            var bf = new BinaryFormatter();
            FileStream file = File.Create(_path);
            bf.Serialize(file, _model.Saves);

            file.Close();
        }

        public void Load(string saveName)
        {
            GameSaveData savedGame = _model.Saves.FirstOrDefault(x => x.Name == saveName);

            if (savedGame != null)
            {
                _model.SelectedSave = savedGame;
                
                _messageBus.Fire(new LoadSignal {GameSaveData = savedGame});

                _messageBus.Fire(new PauseSignal {Paused = false});
            }
        }
    }
}