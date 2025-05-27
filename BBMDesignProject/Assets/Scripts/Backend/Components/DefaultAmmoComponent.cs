using Backend.Attributes;
using Backend.Interfaces;

namespace Backend.Components
{
    [Component]
    public class DefaultAmmoComponent:ProjectileComponent,IDamager
    {
        public DefaultAmmoComponent()
        {
            SetName("Default Ammo");
            SetDescription("This component represents the default ammo type used in the game.");
        }
        
        public void ApplyDamage(BaseComponent target, float damageAmount)
        {
            
        }

        public float GetDamageAmount()
        {
            throw new System.NotImplementedException();
        }
    }
}