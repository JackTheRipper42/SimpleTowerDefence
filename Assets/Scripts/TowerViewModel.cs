using Assets.Scripts.Binding;

namespace Assets.Scripts
{
    public class TowerViewModel
    {
        public readonly NotifyingObject<bool> VisibleProperty;

        public TowerViewModel()
        {
            VisibleProperty = new NotifyingObject<bool>(false);
        }

        public void OnMouseEnter()
        {
            VisibleProperty.SetValue(true);
        }

        public void OnMouseExit()
        {
            VisibleProperty.SetValue(false);
        }
    }
}
