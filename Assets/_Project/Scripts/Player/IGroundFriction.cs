using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    interface IGroundFriction
    {
        // Used for DeAcceleration
        float Friction { get; }
        // Used for Acceleration
        float Grip { get; }
        float MaxSpeedBoost { get; }
    }
}
