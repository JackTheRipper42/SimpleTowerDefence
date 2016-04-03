using Assets.Scripts.Binding;
using UnityEngine;

namespace Assets.Scripts
{
    public class TowerViewModel
    {
        public readonly NotifyingObject<bool> VisibleProperty;
        public readonly NotifyingObject<Sprite> TowerSpriteProperty; 
        
        public TowerViewModel()
        {
            VisibleProperty = new NotifyingObject<bool>();
            TowerSpriteProperty = new NotifyingObject<Sprite>();
        }

        public void OnLevelChange(TowerLevel level)
        {
            TowerSpriteProperty.SetValue(level.TowerSprite);
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
