using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.State
{
    /// <summary>
    /// Represents a game object with a variable state.
    /// </summary>
    public interface IHasState<TState>
        where TState : class, IState
    {
        /// <summary>
        /// Gets the game object's current state.
        /// </summary>
        /// <returns>
        /// The game object's current state
        /// </returns>
        TState GetCurrentState();

        /// <summary>
        /// Applies the specified state to the game object.
        /// </summary>
        /// <param name="state">
        /// The game state to apply
        /// </param>
        void ApplyState(TState state);
    }
}
