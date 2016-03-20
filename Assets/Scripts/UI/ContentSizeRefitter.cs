using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ContentSizeRefitter : ContentSizeFitter
    {
        public void Refit()
        {
            SetDirty();
        }
    }
}
