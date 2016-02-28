using UnityEngine;

namespace Assets.Scripts
{
    public class AutoPath : Path
    {
        public override Vector3[] GetPath()
        {
            var path = new Vector3[transform.childCount];
            for (int index = 0; index < transform.childCount; index++)
            {
                var child = transform.GetChild(index);
                path[index] = child.transform.position;
            }
            return path;
        }
    }
}
