using UnityEngine;

namespace Core.Planets
{
    public class MoveLogic : IMoveLogic
    {
        public float Timer => _timer;

        private float _timer;

        private readonly Transform _transform;
        private readonly Vector3   _sunPosition;
        private readonly float     _orbitDistance;
        private readonly float     _speed;

        public MoveLogic(Transform transform, Vector3 sunPosition, float orbitDistance, float speed)
        {
            _transform     = transform;
            _sunPosition   = sunPosition;
            _orbitDistance = orbitDistance;
            _speed         = speed;
        }

        public void Initialize(float timer)
        {
            _timer = timer;

            _transform.position =
                new Vector3(
                    _sunPosition.x + Mathf.Sin(_timer) * _orbitDistance,
                    _sunPosition.y + Mathf.Cos(_timer) * _orbitDistance,
                    _sunPosition.z);
        }

        public void Move()
        {
            _timer += Time.fixedDeltaTime * _speed;
            
            _transform.position = Vector3.MoveTowards(
                _transform.position,
                new Vector3(
                    _sunPosition.x + Mathf.Sin(_timer) * _orbitDistance,
                    _sunPosition.y + Mathf.Cos(_timer) * _orbitDistance,
                    _sunPosition.z),
                0.35f);
        }
    }
}