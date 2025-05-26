namespace Backend.Interfaces
{
    public interface IDamagable
    {
        /// <summary>
        /// Applies damage to the entity.
        /// </summary>
        /// <param name="damageAmount">The amount of damage to apply.</param>
        void ApplyDamage(float damageAmount);

        /// <summary>
        /// Gets the current health of the entity.
        /// </summary>
        /// <returns>The current health.</returns>
        float GetCurrentHealth();

        /// <summary>
        /// Checks if the entity is alive.
        /// </summary>
        /// <returns>True if the entity is alive, false otherwise.</returns>
        bool IsAlive();
    }
}