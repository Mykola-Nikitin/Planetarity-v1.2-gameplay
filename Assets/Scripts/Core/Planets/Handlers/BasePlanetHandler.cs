using Core.Saves;
using Core.Settings;
using Core.Signals;
using Core.Utils;
using Core.Weapons;
using DI;
using Helpers;
using UnityEngine;
using Utils.SignalBus;

namespace Core.Planets
{
    public abstract class BasePlanetHandler : IPlanetHandler
    {
        protected readonly PlanetComponent PlanetComponent;
        protected readonly PlanetsModel    PlanetsModel;
        protected readonly GameModel       GameModel;
        protected readonly IConfigsService ConfigsService;
        protected readonly IRocketsService RocketsService;
        protected readonly IPlanetsService PlanetsService;
        protected readonly IMessageBus     MessageBus;

        protected IMoveLogic     MoveLogic;
        protected IRechargeLogic RechargeLogic;
        protected Camera         MainCamera;

        protected readonly Bindable<float> RechargeField = new Bindable<float>();
        protected readonly Bindable<int>   HealthField   = new Bindable<int>();

        public PlanetData PlanetData { get; private set; }

        public BasePlanetHandler(PlanetComponent planetComponent)
        {
            PlanetComponent = planetComponent;
            PlanetsModel    = DIContainer.Get<PlanetsModel>();
            GameModel       = DIContainer.Get<GameModel>();
            ConfigsService  = DIContainer.Get<IConfigsService>();
            RocketsService  = DIContainer.Get<IRocketsService>();
            PlanetsService  = DIContainer.Get<IPlanetsService>();
            MessageBus      = DIContainer.Get<IMessageBus>();
        }

        public virtual void Initialize(int id)
        {
            PlanetConfig planetConfig = ConfigsService.GetPlanetConfig();

            PlanetComponent.transform.localScale = Vector3.one * planetConfig.PlanetRadius;
            PlanetComponent.transform.SetParent(GameModel.Parent);

            CreateData(id);
            Setup();
        }

        public void Load(PlanetData data)
        {
            PlanetData = data.Clone();

            PlanetConfig planetConfig = ConfigsService.GetPlanetConfig();

            PlanetComponent.transform.localScale = Vector3.one * planetConfig.PlanetRadius;
            PlanetComponent.transform.SetParent(GameModel.Parent);
            PlanetComponent.transform.position = PlanetData.Position;

            Setup();
        }

        public virtual void Destroy(bool isSilent)
        {
            MessageBus.RemoveListener<FinishSignal>(OnGameFinish);

            PlanetsModel.Planets.Remove(this);
            PlanetComponent.Hud.Dispose();

            RechargeField.Unsubscribe(OnRechargeChanged);
            HealthField.Unsubscribe(OnHealthChanged);

            PlanetsService.RemovePlanet(PlanetComponent.gameObject, isSilent);
        }

        public virtual void Update()
        {
            MoveLogic.Move();
            RechargeLogic.Update();

            PlanetData.Position = GetPosition();
            PlanetData.Timer    = MoveLogic.Timer;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out RocketComponent rocketComponent))
            {
                int damage = rocketComponent.GetDamage();
                int owner = rocketComponent.GetOwner();

                if (owner != PlanetData.ID)
                {
                    HealthField.Value -= damage;
                }
            }
        }

        protected virtual void Setup()
        {
            HealthField.Value   = PlanetData.CurrentHealth;
            RechargeField.Value = PlanetData.CurrentRecharge;

            PlanetComponent.Hud.Initialize(HealthField, RechargeField, PlanetData.MaxHealth, PlanetData.RechargeDuration);

            MoveLogic     = new MoveLogic(PlanetComponent.transform, PlanetsModel.SunPosition, PlanetData.OrbitDistance, PlanetData.Speed);
            RechargeLogic = new RechargeLogic(RechargeField, PlanetData.RechargeDuration);
            
            RechargeLogic.Check();

            RechargeField.Subscribe(OnRechargeChanged);
            HealthField.Subscribe(OnHealthChanged);

            MoveLogic.Initialize(PlanetData.Timer);

            MainCamera = Camera.main;
            MessageBus.AddListener<FinishSignal>(OnGameFinish);

            PlanetsModel.Planets.Add(this);
        }

        protected void OnHealthChanged(int oldValue, int newValue)
        {
            PlanetData.CurrentHealth = newValue;

            if (PlanetData.CurrentHealth <= 0)
            {
                Destroy(false);
            }
        }

        protected void OnRechargeChanged(float oldValue, float newValue)
        {
            PlanetData.CurrentRecharge = newValue;
        }

        private void CreateData(int id)
        {
            PlanetConfig planetConfig = ConfigsService.GetPlanetConfig();
            PlanetGenerationConfig generationConfig = ConfigsService.GetPlanetGenerationConfig();
            RocketConfig[] rocketTypes = ConfigsService.GetAllRocketConfigs();
            RocketConfig rocketConfig = rocketTypes.GetRandom();

            float speed = Random.Range(planetConfig.MinPlanetsSpeed, planetConfig.MaxPlanetsSpeed);
            int rocketType = rocketConfig.Type;
            float orbitDistance = generationConfig.DistanceBetweenPlanets * id + generationConfig.DistanceFromSun;
            bool isPlayer = this is PlanetHandler;
            Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

            PlanetData = new PlanetData
            {
                ID               = id,
                RocketType       = rocketType,
                IsPlayer         = isPlayer,
                OrbitDistance    = orbitDistance,
                Speed            = speed,
                RechargeDuration = rocketConfig.RechargeTime,
                CurrentRecharge  = rocketConfig.RechargeTime,
                MaxHealth        = planetConfig.PlanetHealth,
                CurrentHealth    = planetConfig.PlanetHealth,
                Position         = PlanetComponent.transform.position,
                Color            = randomColor
            };
        }
        
        private Vector3 GetPosition()
        {
            return PlanetComponent.transform.position;
        }

        private void OnGameFinish(FinishSignal signalData)
        {
            Destroy(true);
        }
    }
}