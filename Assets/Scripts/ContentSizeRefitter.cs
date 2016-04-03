using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ContentSizeRefitter : ContentSizeFitter
    {
        public void Refit()
        {
            SetDirty();
        }
    }
}
