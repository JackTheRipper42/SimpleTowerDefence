using System;

namespace Assets.Scripts
{
    public class Buff
    {
        private readonly float _healthBoost;

        public Buff(float healthBoost)
        {
            if (healthBoost <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    "healthBoost",
                    healthBoost,
                    "The health boost is less or equal to zero.");
            }

            _healthBoost = healthBoost;
        }

        public float HealthBoost
        {
            get { return _healthBoost; }
        }
    }
}
