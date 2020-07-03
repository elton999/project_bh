using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using UmbrellaToolKit;

namespace Project_BH.Gameplay
{
    public class DownStairsLeft : Stairs
    {
        public override void Start()
        {
            this.CreateHitBox();
            this.CBody.Tag = "hitbox_downstairs_left";
        }
    }
}
