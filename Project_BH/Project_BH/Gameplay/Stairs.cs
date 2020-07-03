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
    public class Stairs : GameObject
    {

        public void CreateHitBox()
        {
            this.Position = new Vector2(this.Position.X, this.Position.Y +1);
            this._bodySize = new Vector2(40, 1);
            this.SetBoxCollision(this.World);
            this.CBody.SetCollidesWith(Category.Cat10);
        }
    }
}
