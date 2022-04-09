using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.State
{
    public class TokenState : IState
    {
        public bool Collected { get; set; }
    }
}
