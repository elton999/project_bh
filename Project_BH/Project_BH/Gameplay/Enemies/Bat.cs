using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using UmbrellaToolKit;

namespace Project_BH.Gameplay.Enemies
{
    public class Bat : Enemy
    {
        public override void Start()
        {
            this.Sprite = this.Content.Load<Texture2D>("Sprites/Enemies/bat");

            this.SpriteColor = Color.Blue;

            this.TextureSize = new Vector2(32, 32);
            this._bodySize = new Vector2(12, 10);

            this.SetBoxCollision(this.World);
            this.CBody.Tag = "Bat";
            this.CBody.BodyType = BodyType.Dynamic;
            this.CBody.SetCollidesWith(Category.Cat1);
            this.CBody.SetFriction(1);

            this.DontCollisionWithTag.Add("Player");
            this.DontCollisionWithTag.Add("Knight");
            this.DontCollisionWithTag.Add("Troll");

            base.Start();
        }

        public Vector2 Target;
        private Vector2 moviment;
        public float moveSpeed = 150f;
        private int buffArea = 10;
        public override void Update(GameTime gameTime)
        {
            Vector2 direction = (Target - this.CBody.Position);
            direction.Normalize();

            moviment = this.CBody.Position + (direction * moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            this.CBody.SetTransformIgnoreContacts(ref moviment, 0f);
            if ((Target.Y + buffArea > moviment.Y && Target.Y - buffArea < moviment.Y) && (Target.X + buffArea > moviment.X && Target.X - buffArea < moviment.X))
                Target = this.Scene.Players[0].CBody.Position;
        }

        public override void OnCollision(string tag)
        {
            if (tag == "PlayerSword")
                this.RemoveFromScene = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.DrawSprite(spriteBatch);
        }
    }
}
