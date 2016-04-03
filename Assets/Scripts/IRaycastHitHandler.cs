using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IRaycastHitHandler
    {
        Type ComponentType { get; }

        void Handle(RaycastHit hit, [NotNull] object component);
    }
}