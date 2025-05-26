using Backend.Components;

namespace Backend.Interfaces
{
    public interface IDamager
    {
        /// <summary>
        /// Applies damage to the target.
        /// </summary>
        /// <param name="target">The target to apply damage to.</param>
        /// <param name="damageAmount">The amount of damage to apply.</param>
        void ApplyDamage(BaseComponent target, float damageAmount);
        
        /// <summary>
        /// Gets the damage amount.
        /// </summary>
        /// <returns>The amount of damage.</returns>
        float GetDamageAmount();
    }
}