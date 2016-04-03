using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class RaycastHitHandler<T> : IRaycastHitHandler
    {
        public Type ComponentType
        {
            get { return typeof(T); }
        }

        protected abstract void Handle(RaycastHit hit, [NotNull] T component);

        public void Handle(RaycastHit hit, object component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            Handle(hit, (T) component);
        }
    }
}