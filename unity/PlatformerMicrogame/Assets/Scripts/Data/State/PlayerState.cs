using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Platformer.Mechanics.PlayerController;

namespace Assets.Scripts.Data.State
{
    public class PlayerState : IState
    {
        public VectorData Position { get; set; }
        public VectorData Velocity { get; set; }
        public JumpState JumpState { get; set; }
        public bool StopJump { get; set; }
        public bool Jump { get; set; }
        public int CurrentHp { get; set; }
        public bool ControlEnabled { get; set; }
        public VectorData Move { get; set; }
    }
}
