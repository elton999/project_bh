using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
