using UnityEngine;

namespace Assets.Scripts
{
    public interface ITower
    {
        bool CanUpgrade();

        void Upgrade();
    }
}
