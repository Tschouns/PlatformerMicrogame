using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
    /// <summary>
    /// Provides relevant folder.
    /// </summary>
    /// <remarks>
    /// In a real project this might be configurable.
    /// </remarks>
    public static class FolderInfo
    {
        public static string GetSavegameDirectory()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var saveGameFolder = Path.Combine(appData, "PlatformerMicrogame", "savegames");

            return saveGameFolder;
        }
    }
}
