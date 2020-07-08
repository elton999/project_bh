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
using UmbrellaToolKit.Sprite;

namespace Project_BH.Gameplay.Enemies
{
    public class Knight : Enemy
    {
        private AsepriteDefinitions AsepriteDefinitions;
        private AsepriteAnimation AsepriteAnimation;

        public override void Start()
        {
            this.Sprite = this.Content.Load<Texture2D>("Sprites/player/prototype_character");
            this.AsepriteDefinitions = this.Content.Load<AsepriteDefinitions>("Sprites/player/prototype_character_json");
            this.AsepriteAnimation = new AsepriteAnimation(this.AsepriteDefinitions);

            this.SpriteColor = Color.Red;

            this.TextureSize = new Vector2(64, 64);
            this._bodySize = new Vector2(10, 32);

            this.SetBoxCollision(this.World);
            this.CBody.Tag = "Knight";
            this.CBody.BodyType = BodyType.Dynamic;
            this.CBody.SetCollidesWith(Category.Cat1);
            this.CBody.SetFriction(1);

            this.DontCollisionWithTag.Add("Player");
            this.DontCollisionWithTag.Add("Knight");
            this.DontCollisionWithTag.Add("Troll");

            this.VigiliantModeActive = true;
        }

        public override void OnCollision(string tag)
        {
            if (tag == "PlayerSword")
                this.Destroy();
        }

        
        public override void Update(GameTime gameTime)
        {
            this.AsepriteAnimation.Play(gameTime, "idle");
            this.Body = this.AsepriteAnimation.Body;
            this.VigiliantMode();
            this.CheckDirection();
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            this.DrawSprite(spriteBatch);
        }
    }
}
