using Assets.Scripts.Data.State;
using System.Collections.Generic;

namespace Assets.Scripts.Data.Savegames
{
    public class SceneSavegame
    {
        public PlayerState PlayerState { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<Token> Tokens { get; set; }
    }
}
