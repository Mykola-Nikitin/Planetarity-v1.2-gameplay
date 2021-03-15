using Core.Planets;
using Core.Saves;
using Core.Settings;
using DI;
using UnityEngine;
using Utils;

namespace Core.Weapons
{
    public class RocketHandler : IRocketHandler
    {
        public RocketData RocketData { get; private set; }

        public int Damage  => _config?.Damage ?? 0;
        public int OwnerId => RocketData?.OwnerId ?? -1;

        private readonly RocketComponent _component;

        private IConfigsService _configsService;
        private IRocketsService _rocketsService;
        private RocketsModel    _model;
        private GameModel       _gameModel;
        private RocketConfig    _config;

        private IMoveLogic _moveLogic;

        public RocketHandler(RocketComponent component)
        {
            _component      = component;
            _rocketsService = DIContainer.Get<IRocketsService>();
            _configsService = DIContainer.Get<IConfigsService>();
            _model          = DIContainer.Get<RocketsModel>();
            _gameModel      = DIContainer.Get<GameModel>();
        }


        public void Initialize(int type, int ownerId, Vector3 ownerPosition, Vector3 lookPosition)
        {
            _config = _configsService.GetRocketConfig(type);
            _component.transform.SetParent(_gameModel.Parent);

            SetPositionAndRotation(ownerPosition, lookPosition);
            CreateData(type, ownerId, ownerPosition, lookPosition);

            SetupPhysics();
            SetupColor();

            _moveLogic = new RocketMoveLogic(_component, _config.Weight, _config.Acceleration);

            _model.Rockets.Add(this);

            Runner.Instance.Run(Destroy, _config.Lifetime);
        }

        public void Load(RocketData rocketData)
        {
            RocketData = rocketData.Clone();

            _config = _configsService.GetRocketConfig(RocketData.Type);

            _component.transform.SetParent(_gameModel.Parent);

            LoadPositionAndRotation();
            SetupPhysics();
            SetupColor();

            _moveLogic = new RocketMoveLogic(_component, _config.Weight, _config.Acceleration);

            _model.Rockets.Add(this);

            Runner.Instance.Run(Destroy, _config.Lifetime);
        }

        #region Interfaces implementation

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _component.SUN_LAYER)
            {
                Destroy();
            }
            else if (other.TryGetComponent(out PlanetComponent planet))
            {
                if (planet.GetHandler().PlanetData.ID != OwnerId)
                {
                    Destroy();
                }
            }
        }

        public void Update()
        {
            _moveLogic?.Move();
        }

        public void Destroy()
        {
            Runner.Instance.Remove(Destroy);

            _component.Collider.enabled   = false;
            _component.Rigidbody.velocity = new Vector3(0f, 0f, 0f);

            _model.Rockets.Remove(this);

            _rocketsService.RemoveRocket(_component.gameObject);
        }

        #endregion

        private void SetPositionAndRotation(Vector3 ownerPosition, Vector3 lookPosition)
        {
            Vector3 localLookPosition = _gameModel.Parent.InverseTransformPoint(lookPosition);
            Vector3 localOwnerPosition = _gameModel.Parent.InverseTransformPoint(ownerPosition);

            localLookPosition.z = localOwnerPosition.z;

            _component.transform.localPosition = localOwnerPosition + (localLookPosition - localOwnerPosition).normalized *
                (_configsService.GetPlanetConfig().PlanetRadius + 0.2f);

            var direction = new Vector3(localLookPosition.x, localLookPosition.y, 0f) - _component.transform.localPosition;
            var rotation = Quaternion.LookRotation(Vector3.forward, direction);

            _component.transform.rotation = rotation;
        }

        private void LoadPositionAndRotation()
        {
            _component.transform.position = RocketData.Position;
            _component.transform.rotation = Quaternion.Euler(RocketData.EulerRotation);
        }

        private void SetupPhysics()
        {
            _component.Rigidbody.centerOfMass = new Vector3(0f, 0.5f, 0f);
            _component.Rigidbody.velocity     = RocketData.Velocity;
            _component.Collider.enabled       = true;
        }

        private void SetupColor()
        {
            for (var i = 0; i < _component.Parts.Length; i++)
            {
                MeshRenderer part = _component.Parts[i];
                var propBlock = new MaterialPropertyBlock();
                part.GetPropertyBlock(propBlock);
                propBlock.SetColor("_BaseColor",  _config.Color);
                part.SetPropertyBlock(propBlock);
            }
        }

        private void CreateData(int type, int ownerId, Vector3 ownerPosition, Vector3 lookPosition)
        {
            Vector3 position = _component.transform.position;
            Vector3 rotation = _component.transform.eulerAngles;
            Vector3 velocity = _component.Rigidbody.velocity;

            RocketData = new RocketData
            {
                Target        = lookPosition,
                Type          = type,
                OwnerId       = ownerId,
                OwnerPosition = ownerPosition,
                Position      = position,
                EulerRotation = rotation,
                Velocity      = velocity,
            };
        }
    }
}