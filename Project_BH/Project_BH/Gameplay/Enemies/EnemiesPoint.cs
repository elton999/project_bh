using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmbrellaToolKit;
using Microsoft.Xna.Framework;

namespace Project_BH.Gameplay.Enemies
{
    public class EnemiesPoint : GameObject
    {
        public bool EnemyCreated = false;
        public virtual void CreateEnemie() {
            this.EnemyCreated = true;
        }

        public override void OnVisible()
        {
            if (!this.EnemyCreated)
                this.CreateEnemie();
            base.OnVisible();
        }
    }

    public class TrollPoint : EnemiesPoint
    {
        public override void CreateEnemie()
        {
            Troll troll = new Troll();
            troll.Position = new Vector2(this.Position.X + 4, this.Position.Y - 20);
            troll.Scene = this.Scene;
            this.Scene.Enemies.Add(troll);
            troll.Content = Content;
            troll.World = this.World;
            troll.Start();
            base.CreateEnemie();
        }
    }

    public class AirEnemyPoint : EnemiesPoint
    {
        public override void CreateEnemie()
        {
            Bat bat = new Bat();
            bat.Position = new Vector2(this.Position.X, this.Position.Y);
            bat.Target = this.Scene.Players[0].CBody.Position;
            bat.Scene = this.Scene;
            this.Scene.Enemies.Add(bat);
            bat.Content = Content;
            bat.World = this.World;
            bat.Start();
            base.CreateEnemie();
        }
    }
}
