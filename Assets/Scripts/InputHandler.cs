using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputHandler
    {
        private readonly IEnumerable<IRaycastHitHandler> _handlers;

        public InputHandler([NotNull] params IRaycastHitHandler[] handlers)
        {
            if (handlers == null)
            {
                throw new ArgumentNullException("handlers");
            }

            _handlers = handlers;
        }

        public void HandleRaycastHit(RaycastHit hit)
        {
            foreach (var handler in _handlers)
            {
                var component = hit.transform.gameObject.GetComponent(handler.ComponentType);
                if (component != null)
                {
                    handler.Handle(hit, component);
                }
            }
        }
    }
}
