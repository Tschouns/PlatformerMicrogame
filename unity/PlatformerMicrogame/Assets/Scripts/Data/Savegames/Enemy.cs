using Assets.Scripts.Data.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.Savegames
{
    public class Enemy
    {
        public int Key { get; set; }
        public EnemyState State { get; set; }
    }
}
