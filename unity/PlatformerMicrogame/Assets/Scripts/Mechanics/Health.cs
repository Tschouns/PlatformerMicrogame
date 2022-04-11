using System;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        public int maxHP = 1;

        /// <summary>
        /// Indicates if the entity should be considered 'alive'.
        /// </summary>
        public bool IsAlive => this.CurrentHP > 0;

        public int CurrentHP { get; set; } 

        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>
        public void Increment()
        {
            this.CurrentHP = Mathf.Clamp(this.CurrentHP + 1, 0, maxHP);
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement()
        {
            this.CurrentHP = Mathf.Clamp(this.CurrentHP - 1, 0, maxHP);
            if (this.CurrentHP == 0)
            {
                var ev = Schedule<HealthIsZero>();
                ev.health = this;
            }
        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die()
        {
            while (this.CurrentHP > 0) Decrement();
        }

        void Awake()
        {
            this.CurrentHP = maxHP;
        }
    }
}
