using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.State
{
    public class EnemyState : IState
    {
        public VectorData Position { get; set; }
        public VectorData Velocity { get; set; }
        public VectorData Move { get; set; }
        public bool IsAlive { get; set; }
        public bool ControlEnabled { get; set; }
        public bool ColliderEnabled { get; set; }
    }
}
