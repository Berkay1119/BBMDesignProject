using Backend.Interfaces;

namespace Backend.Components
{
    public class DefaultAmmoComponent:ProjectileComponent,IDamager
    {
        public void ApplyDamage(BaseComponent target, float damageAmount)
        {
            
        }

        public float GetDamageAmount()
        {
            throw new System.NotImplementedException();
        }
    }
}