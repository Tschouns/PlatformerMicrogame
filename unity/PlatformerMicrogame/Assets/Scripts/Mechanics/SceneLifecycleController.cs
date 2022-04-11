using Assets.Scripts.Core;
using Assets.Scripts.Data;
using Assets.Scripts.Data.Savegames;
using Assets.Scripts.Data.State;
using Assets.Scripts.Model;
using Newtonsoft.Json;
using Platformer.Core;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Mechanics
{
    /// <summary>
    /// Controls the scene's lifecycle. Restarts, saves and loads the scene's state.
    /// </summary>
    public class SceneLifecycleController : MonoBehaviour
    {
        /// <summary>
        /// A little ugly: when set (not null), indicates that a savegame has to be loaded and applied to the scene.
        /// </summary>
        private static LoadTask loadTask;

        private Scene scene;
        private SceneObjectRegister sceneObjects;
        private string savegameDir;

        void Awake()
        {
            this.scene = SceneManager.GetActiveScene();
            this.sceneObjects = SceneObjectRegister.FromScene(scene);
            this.savegameDir = Path.Combine(FolderInfo.GetSavegameDirectory(), this.scene.name);
            
            // Create the savegame directory.
            if (!Directory.Exists(this.savegameDir))
            {
                Directory.CreateDirectory(this.savegameDir);
            }
        }

        public void Start()
        {
            // Load a savegame, if one is designated.
            if (loadTask != null && loadTask.SceneName == this.scene.name)
            {
                var file = loadTask.File;

                // Delete the load task.
                loadTask = null;

                this.LoadAndApplySavegame(file);
            }
        }

        /// <summary>
        /// Reloads the scene.
        /// </summary>
        public void ReloadScene()
        {
            print("Reload scene...");

            // Work-around:
            // Ideally the simulation system would be non-static and a child to the scene. Since it
            // is static, after a scene has been reloaded, events scheduled by the old scene instance will influence
            // the newly loaded scene. E.g. the player in the old scene can die from falling after the new scene has
            // already been loaded, and "kill" the player in the new scene.
            // To prevent those side-effects, we simply clear the event queue before reloading the scene.
            Simulation.Clear();
            SceneManager.LoadScene(this.scene.name);
        }

        public void QuickSave()
        {
            print("Quick save...");

            // Collect data.
            var savegame = new SceneSavegame
            {
                PlayerState = this.sceneObjects.Player.GetComponent<IHasState<PlayerState>>().GetCurrentState(),
                Enemies = this.sceneObjects.Enemies
                    .Select(e => new Enemy
                    {
                        Key = e.Key,
                        State = e.Value.GetComponent<IHasState<EnemyState>>().GetCurrentState(),
                    })
                    .ToList(),
                Tokens = this.sceneObjects.Tokens
                    .Select(t => new Token
                    {
                        Key = t.Key,
                        State = t.Value.GetComponent<IHasState<TokenState>>().GetCurrentState(),
                    })
                    .ToList(),
            };

            // Serialize as JSON.
            var savegameJson = JsonConvert.SerializeObject(savegame);

            // Save to file.
            var savegameFile = Path.Combine(this.savegameDir, CreateSavegameFileName());
            File.WriteAllText(savegameFile, savegameJson);

            print($"Game saved: {savegameFile}");
        }

        public void QuickLoad()
        {
            print("Quick load...");

            // Select latest savegame file.
            var files = Directory.GetFiles(this.savegameDir);
            if (!files.Any())
            {
                print("No savegames available.");
                return;
            }

            var latestSavegameFile = files.OrderByDescending(f => f).First();

            // Create a "load task", and reload the scene.
            loadTask = new LoadTask(this.scene.name, latestSavegameFile);

            this.ReloadScene();
        }

        private string CreateSavegameFileName()
        {
            var timestamp = DateTime.Now;
            var filename = $"{timestamp.ToString("yyyy-MM-dd-HH-mm-ss-fff")}.save";

            return filename;
        }

        private void LoadAndApplySavegame(string savegameFile)
        {
            // Read and deserialize.
            var savegameJson = File.ReadAllText(savegameFile);
            var savegame = JsonConvert.DeserializeObject<SceneSavegame>(savegameJson);

            // Apply savegame state to scene.
            this.sceneObjects.Player.GetComponent<IHasState<PlayerState>>().ApplyState(savegame.PlayerState);

            foreach (var enemy in this.sceneObjects.Enemies)
            {
                // Retrieve the state of the current enemy (by matching keys).
                var enemyState = savegame.Enemies.SingleOrDefault(e => e.Key == enemy.Key)?.State;
                if (enemyState == null)
                {
                    continue;
                }

                enemy.Value.GetComponent<IHasState<EnemyState>>().ApplyState(enemyState);
            }

            foreach (var token in this.sceneObjects.Tokens)
            {
                // Retrieve the state of the current token (by mathich keys).
                var tokenState = savegame.Tokens.SingleOrDefault(t => t.Key == token.Key)?.State;
                if (tokenState == null)
                {
                    continue;
                }

                token.Value.GetComponent<IHasState<TokenState>>().ApplyState(tokenState);
            }
        }

        private class LoadTask
        {
            public LoadTask(string sceneName, string file)
            {
                Argument.AssertNotNull(sceneName, nameof(sceneName));
                Argument.AssertNotNull(file, nameof(file));

                this.SceneName = sceneName;
                this.File = file;
            }

            public string SceneName { get; }
            public string File { get; }
        }
    }
}
