using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using UmbrellaToolKit;

namespace Project_BH.Gameplay.Enemies
{
    public class Enemy : GameObject
    {
        public float Life
        {
            get => this._life;
        }
        private float _life;

        public void TakeDamage(float damageValue)
        {
            _life -= damageValue;
        }


        public void CheckDirection()
        {
            if (_right)
                this.spriteEffect = SpriteEffects.None;
            else
                this.spriteEffect = SpriteEffects.FlipHorizontally;
        }

        bool _right = true;
        public void VigiliantMode()
        {

            bool _turn = false;
            // check the way
            List<Fixture>  fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X - 7, this.CBody.Position.Y + 20));
            if (!_right)
            {
                _turn = true;
                foreach (Fixture fixture in fixtures)
                {
                    if (fixture != null && (string)fixture.Body.Tag == "ground")
                        _turn = false;
                }
            }

            fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X + 7, this.CBody.Position.Y + 20));
            if (_right)
            {
                _turn = true;
                foreach (Fixture fixture in fixtures)
                {
                    if (fixture != null && (string)fixture.Body.Tag == "ground")
                        _turn = false;
                }
            }

            fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X - 6, this.CBody.Position.Y + 16));
            foreach (Fixture fixture in fixtures)
            {
                if (fixture != null && (string)fixture.Body.Tag == "ground" && !_right)
                    _turn = true;
            }

            fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X + 6, this.CBody.Position.Y + 16));
            foreach (Fixture fixture in fixtures)
            {
                if (fixture != null && (string)fixture.Body.Tag == "ground" && _right)
                    _turn = true;
            }
            
            
            if (_turn)
                _right = !_right;

            if (_right)
                this.CBody.SetTransform(new Vector2(this.CBody.Position.X + 1, this.CBody.Position.Y), 0f);
            else
                this.CBody.SetTransform(new Vector2(this.CBody.Position.X - 1, this.CBody.Position.Y), 0f);
        }
    }
}
