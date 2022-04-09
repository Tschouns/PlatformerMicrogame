using Assets.Scripts.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// Represents a register of all the game objects with a variable state within a scene.
    /// </summary>
    public class SceneObjectRegister
    {
        private SceneObjectRegister(
            GameObject player,
            IReadOnlyDictionary<int, GameObject> enemies,
            IReadOnlyDictionary<int, GameObject> tokens)
        {
            Argument.AssertNotNull(player, nameof(player));
            Argument.AssertNotNull(enemies, nameof(enemies));
            Argument.AssertNotNull(tokens, nameof(tokens));

            this.Player = player;
            this.Enemies = enemies;
            this.Tokens = tokens;
        }

        /// <summary>
        /// Gets the player of the scene.
        /// </summary>
        public GameObject Player { get; }

        /// <summary>
        /// Gets the enemies of the scene, identified by a key based on their starting position.
        /// </summary>
        public IReadOnlyDictionary<int, GameObject> Enemies { get; }

        /// <summary>
        /// Gets the tokens of the scene, identified by a key based on their starting position.
        /// </summary>
        public IReadOnlyDictionary<int, GameObject> Tokens { get; }

        /// <summary>
        /// Initializes a new register instance based on the specified scene.
        /// </summary>
        /// <param name="scene">
        /// The scene to analyze
        /// </param>
        /// <returns>
        /// A new register instance
        /// </returns>
        public static SceneObjectRegister FromScene(Scene scene)
        {
            Argument.AssertNotNull(scene, nameof(scene));

            var totalObjects = TraverseSceneObjects(scene);

            var player = totalObjects.Single(o => o.name.Equals("player", StringComparison.OrdinalIgnoreCase));
            var enemies = totalObjects.Where(o => o.name.Equals("enemy", StringComparison.OrdinalIgnoreCase)).ToList();
            var tokens = totalObjects.Where(o => o.name.Equals("token", StringComparison.OrdinalIgnoreCase)).ToList();

            // Determine a key for each object, based on their starting position.
            var enemyDict = enemies.ToDictionary(o => o.transform.position.GetHashCode());
            var tokenDict = tokens.ToDictionary(o => o.transform.position.GetHashCode());

            return new SceneObjectRegister(
                player,
                new ReadOnlyDictionary<int, GameObject>(enemyDict),
                new ReadOnlyDictionary<int, GameObject>(tokenDict));
        }

        /// <summary>
        /// Traverses and gets all the game objects in the scene.
        /// </summary>
        /// <param name="scene">
        /// The scene whose game objects to traverse
        /// </param>
        /// <returns>
        /// All the game objects in the scene
        /// </returns>
        public static IEnumerable<GameObject> TraverseSceneObjects(Scene scene)
        {
            Argument.AssertNotNull(scene, nameof(scene));

            var rootObjects = scene.GetRootGameObjects();
            var totalChildObjects = rootObjects
                .SelectMany(o => GetAllChildObjects(o))
                .ToList();

            return rootObjects.Union(totalChildObjects).ToList();
        }

        /// <summary>
        /// Recursively gets all the child objects of the specified game object.
        /// </summary>
        /// <param name="gameObject">
        /// The game object
        /// </param>
        /// <returns>
        /// All the game object's child objects
        /// </returns>
        private static IEnumerable<GameObject> GetAllChildObjects(GameObject gameObject)
        {
            Argument.AssertNotNull(gameObject, nameof(gameObject));

            if (gameObject.gameObject.transform.childCount == 0)
            {
                return new GameObject[0];
            }

            var children = new List<GameObject>();
            for (var i = 0; i < gameObject.gameObject.transform.childCount; i++)
            {
                children.Add(gameObject.transform.GetChild(i).gameObject);
            }

            return children;
        }
    }
}
