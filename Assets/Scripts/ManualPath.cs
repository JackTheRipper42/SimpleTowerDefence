using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class ManualPath : Path
    {
        public Transform[] Nodes;

        protected override Vector3[] GetPath()
        {
            return Nodes.Select(node => node.transform.position).ToArray();
        }
    }
}
