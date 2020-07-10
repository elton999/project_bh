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
    public class Troll : Enemy
    {
        private AsepriteDefinitions AsepriteDefinitions;
        private AsepriteAnimation AsepriteAnimation;

        public override void Start()
        {
            this.Sprite = this.Content.Load<Texture2D>("Sprites/Enemies/troll");
            this.AsepriteDefinitions = this.Content.Load<AsepriteDefinitions>("Sprites/Enemies/troll_info");
            this.AsepriteAnimation = new AsepriteAnimation(this.AsepriteDefinitions);

            this.SpriteColor = Color.Red;

            this.TextureSize = new Vector2(64, 64);
            this._bodySize = new Vector2(10, 32);

            this.SetBoxCollision(this.World);
            this.CBody.Tag = "Troll";
            this.CBody.BodyType = BodyType.Dynamic;
            this.CBody.SetCollidesWith(Category.Cat1);
            this.CBody.SetFriction(1);

            this.DontCollisionWithTag.Add("Player");
            this.DontCollisionWithTag.Add("Knight");
            this.DontCollisionWithTag.Add("Troll");

            _right = false;
            this.CurrentAnimation = AnimationType.BORN;
        }

        public override void OnCollision(string tag)
        {
            if (tag == "PlayerSword" && this.CurrentAnimation != AnimationType.DEATH)
                this.Die();
        }

        public void Born()
        {

        }

        public void Die()
        {
            this.CurrentAnimation = AnimationType.DEATH;
        }

        public override void Update(GameTime gameTime)
        {
            this.AnimationUpdate(gameTime);
            if(this.CurrentAnimation != AnimationType.DEATH)
                this.VigiliantMode();
            this.CheckDirection();
        }
        
        public enum AnimationType {WALK, BORN, DEATH}
        private AnimationType CurrentAnimation;

        public void AnimationUpdate(GameTime gameTime)
        {
            switch (CurrentAnimation) {
                case AnimationType.WALK :
                    this.AsepriteAnimation.Play(gameTime, "walk");
                    break;
                case AnimationType.BORN :
                    this.AsepriteAnimation.Play(gameTime, "borning");
                    if (this.AsepriteAnimation.lastFrame)
                    {
                        this.VigiliantModeActive = true;
                        CurrentAnimation = AnimationType.WALK;
                    }
                    break;
                case AnimationType.DEATH :
                    this.AsepriteAnimation.Play(gameTime, "death");
                    if (this.AsepriteAnimation.lastFrame)
                        this.Destroy();
                    break;
            }
            this.Body = this.AsepriteAnimation.Body;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this.DrawSprite(spriteBatch);
        }
    }
}
