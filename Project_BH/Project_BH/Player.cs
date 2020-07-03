using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;
using UmbrellaToolKit;
using UmbrellaToolKit.Sprite;

namespace Project_BH
{
    public class Player : GameObject
    {

        public override void Start()
        {
            this.Sprite = this.Content.Load<Texture2D>("Sprites/player/prototype_character");
            this.AsepriteDefinitions = this.Content.Load<AsepriteDefinitions>("Sprites/player/prototype_character_json");
            this.AsepriteAnimation = new AsepriteAnimation(this.AsepriteDefinitions);

            this.TextureSize = new Vector2(64, 64);
            this._bodySize = new Vector2(10, 32);

            this.SetBoxCollision(this.World);
            this.CBody.Tag = "Player";
            this.CBody.BodyType = BodyType.Dynamic;
            this.CBody.SetCollidesWith(Category.Cat1);
            this.CBody.SetFriction(1);

            this.SpriteColor = Color.Black;

            // sword hit-box
            this.Sword = new GameObject();
            this.Sword.Sprite = Content.Load<Texture2D>("Sprites/player/swordhitbox");
            this.Sword._bodySize = new Vector2(27, 27);
            this.Sword.TextureSize = new Vector2(27, 27);
            this.Sword.Position = new Vector2(this.Position.X + 15, this.Position.Y + 5);
            this.Sword.SetBoxCollision(this.World);
            this.Sword.CBody.Tag = "PlayerSword";
            this.Sword.CBody.BodyType = BodyType.Static;
            this.Sword.CBody.SetCollidesWith(Category.Cat1);
            this.Sword.DontCollisionWithTag.Add("Player");
            this.Sword.DontCollisionWithTag.Add("Knight");
            this.World.Remove(this.Sword.CBody);
        }

        bool _swordHitBox = false;
        public void CreateSwordHitBox()
        {
            this.World.Add(this.Sword.CBody);
            _swordHitBox = true;
        }

        public void RemoveSwordHitBox()
        {
            this.World.Remove(this.Sword.CBody);
            _swordHitBox = false;
        }
        
        #region Check objects

        bool _isGround;

        bool _isUpingRight;
        bool _isUpingLeft;

        bool _isDowningRight;
        bool _isDowningLeft;

        string _eventString;

        bool _goToStairs;
        bool _isOnStairs;

        bool _isWall_R;
        bool _isWall_R_T;
        bool _isWall_R_C;
        bool _isWall_R_B;

        bool _isWall_L;
        bool _isWall_L_T;
        bool _isWall_L_C;
        bool _isWall_L_B;

        GameObject Sword;
        
        #region check if is grounded
        private void CheckGround()
        {
            _isGround = false;
            _eventString = "";
            if (!_isOnStairs && !_goToStairs)
            {
                _isDowningRight = false;
                _isDowningLeft = false;
                _isUpingRight = false;
                _isUpingLeft = false;
                _isOnStairs = false;
            }

            List<Fixture> fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X, this.CBody.Position.Y + 17));

            foreach (Fixture fixture in fixtures)
            {
                if (fixture != null)
                {
                    if ((string)fixture.Body.Tag == "ground")
                        _isGround = true;
                    else if ((string)fixture.Body.Tag == "hitbox_upstairs_right")
                    {
                        _isUpingRight = true;
                        _goToPosition = fixture.Body.Position;
                        _eventString = "hitbox_upstairs_right";
                    }
                    else if ((string)fixture.Body.Tag == "hitbox_upstairs_left")
                    {
                        _isUpingLeft = true;
                        _goToPosition = fixture.Body.Position;
                        _eventString = "hitbox_upstairs_left";
                    }
                    else if ((string)fixture.Body.Tag == "hitbox_downstairs_right")
                    {
                        _isDowningRight = true;
                        _goToPosition = fixture.Body.Position;
                        _eventString = "hitbox_downstairs_right";
                    }
                    else if ((string)fixture.Body.Tag == "hitbox_downstairs_left")
                    {
                        _isDowningLeft = true;
                        _goToPosition = fixture.Body.Position;
                        _eventString = "hitbox_downstairs_left";
                    }
                }
            }
        }
        #endregion

        #region CheckWall
        private void CheckWall()
        {
            _isWall_R = false;
            _isWall_R_T = false;
            _isWall_R_C = false;
            _isWall_R_B = false;

            _isWall_L = false;
            _isWall_L_T = false;
            _isWall_L_C = false;
            _isWall_L_B = false;
            
            // check Right
            List<Fixture> fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X + 6, this.CBody.Position.Y));
            foreach (Fixture fixture in fixtures)
            {
                if (fixture != null && (string)fixture.Body.Tag == "ground")
                {
                    _isWall_R = true;
                    _isWall_R_C = true;
                }
            }

            fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X + 6, this.CBody.Position.Y + 16));
            foreach (Fixture fixture in fixtures)
            {
                if (fixture != null && (string)fixture.Body.Tag == "ground")
                {
                    _isWall_R = true;
                    _isWall_R_B = true;
                }
            }

            fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X + 6, this.CBody.Position.Y - 16));
            foreach (Fixture fixture in fixtures)
            {
                if (fixture != null && (string)fixture.Body.Tag == "ground")
                {
                    _isWall_R = true;
                    _isWall_R_T = true;
                }
            }

            // check Left
            fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X - 6, this.CBody.Position.Y));
            foreach (Fixture fixture in fixtures)
            {
                if (fixture != null && (string)fixture.Body.Tag == "ground")
                {
                    _isWall_L = true;
                    _isWall_L_C = true;
                }
            }

            fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X - 6, this.CBody.Position.Y + 16));
            foreach (Fixture fixture in fixtures)
            {
                if (fixture != null && (string)fixture.Body.Tag == "ground") {
                    _isWall_L = true;
                    _isWall_L_B = true;
                }
            }

            fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X - 6, this.CBody.Position.Y - 16));
            foreach (Fixture fixture in fixtures)
            {
                if (fixture != null && (string)fixture.Body.Tag == "ground")
                {
                    _isWall_L = true;
                    _isWall_L_T = true;
                }
            }
        }
        #endregion


        private void SetHitBoxSword()
        {
            if (_right)
            {
                Vector2 _position = new Vector2(this.CBody.Position.X + 18, this.CBody.Position.Y + 3);
                if (_swordHitBox) this.Sword.CBody.SetTransformIgnoreContacts(ref _position, 0f);
            }
            else
            {
                Vector2 _position = new Vector2(this.CBody.Position.X - 18, this.CBody.Position.Y + 3);
                if(_swordHitBox) this.Sword.CBody.SetTransformIgnoreContacts(ref _position, 0f);
            }
        }
        #endregion

        #region Stairs
        public void DisableCollision()
        {
            if (!this.CBody.World.IsLocked)
            {
                this.CBody.IgnoreGravity = true;
                this.CBody.SetCollidesWith(Category.Cat10);
            }
        }

        public void EnableCollision()
        {
            if (!this.CBody.World.IsLocked)
            {
                _isDowningRight = false;
                _isDowningLeft  = false;
                _isUpingRight   = false;
                _isUpingLeft    = false;
                _isOnStairs     = false;
                this.CBody.SetCollidesWith(Category.Cat1);
                this.CBody.IgnoreGravity = false;
            }
        }

        Vector2 _goToPosition;
        Vector2 _initialPosition;
        public void GoToStairs()
        {
            if (_goToStairs)
            {
                float _xPosition = 0;
                _xPosition = _goToPosition.X;
                
                if ((int)this.CBody.Position.X > _xPosition)
                    this.CBody.SetTransform(new Vector2(this.CBody.Position.X - 1, this.CBody.Position.Y), this.Rotation);
                else if ((int)this.CBody.Position.X < _xPosition)
                    this.CBody.SetTransform(new Vector2(this.CBody.Position.X + 1, this.CBody.Position.Y), this.Rotation);
                else
                {
                    _isOnStairs = true;
                    _goToStairs = false;
                    this.DisableCollision();
                    // set initial position value
                    if (_isUpingRight)
                    {
                        this.CBody.SetTransform(new Vector2(this.CBody.Position.X + _velocityOnStairs, this.CBody.Position.Y - _velocityOnStairs), this.Rotation);
                        _initialPosition = new Vector2(this.CBody.Position.X + _velocityOnStairs, this.CBody.Position.Y - _velocityOnStairs);
                    }
                    else if (_isUpingLeft)
                    {
                        this.CBody.SetTransform(new Vector2(this.CBody.Position.X - _velocityOnStairs, this.CBody.Position.Y - _velocityOnStairs), this.Rotation);
                        _initialPosition = new Vector2(this.CBody.Position.X - _velocityOnStairs, this.CBody.Position.Y - _velocityOnStairs);
                    }
                    else if (_isDowningRight)
                    {
                        this.CBody.SetTransform(new Vector2(this.CBody.Position.X + _velocityOnStairs, this.CBody.Position.Y + _velocityOnStairs), this.Rotation);
                        _initialPosition = new Vector2(this.CBody.Position.X + _velocityOnStairs, this.CBody.Position.Y + _velocityOnStairs);
                    }
                    else if (_isDowningLeft)
                    {
                        this.CBody.SetTransform(new Vector2(this.CBody.Position.X - _velocityOnStairs, this.CBody.Position.Y + _velocityOnStairs), this.Rotation);
                        _initialPosition = new Vector2(this.CBody.Position.X - _velocityOnStairs, this.CBody.Position.Y + _velocityOnStairs);
                    }
                }
                    
            }
        }
        #endregion

        #region Update
        float _velocity = 2f;
        float _velocityOnStairs = 2f;
        bool _isFall = true;
        bool _isPressJump = false;
        float _lastFrame;
        public override void Update(GameTime gameTime)
        {
            this.CheckGround();
            this.GoToStairs();


            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !_isAtacking)
            {
                this.CheckWall();
                if (_isUpingRight && !_isOnStairs && Keyboard.GetState().IsKeyDown(Keys.Up))
                    this._goToStairs = true;
                else if (_isDowningRight && !_isOnStairs && Keyboard.GetState().IsKeyDown(Keys.Down))
                    this._goToStairs = true;

                if (!_isWall_R && !_isOnStairs && !_goToStairs)
                {
                    this.CBody.SetTransform(new Vector2(this.CBody.Position.X + _velocity, this.CBody.Position.Y), this.Rotation);
                    _right = true;
                    if (_isGround)
                        this.CurrentAnimation = AnimationTypes.WALK;
                    //else
                    //    this.CurrentAnimation = AnimationTypes.JUMP;
                }

                if (_isOnStairs && (_isUpingRight || _isDowningLeft))
                {
                    this.CBody.SetTransform(new Vector2(this.CBody.Position.X + _velocityOnStairs, this.CBody.Position.Y - _velocityOnStairs), this.Rotation);

                    if (_initialPosition == this.CBody.Position && _isDowningLeft)
                        this.EnableCollision();
                    if (_eventString == "hitbox_downstairs_left" && _isUpingRight)
                        this.EnableCollision();
                }
                else if (_isOnStairs && (_isUpingLeft || _isDowningRight))
                {
                    this.CBody.SetTransform(new Vector2(this.CBody.Position.X + _velocityOnStairs, this.CBody.Position.Y + _velocityOnStairs), this.Rotation);

                    if (_initialPosition == this.CBody.Position && _isUpingLeft)
                        this.EnableCollision();
                    if (_eventString == "hitbox_upstairs_left" && _isDowningRight)
                        this.EnableCollision();
                }

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !_isAtacking)
            {
                this.CheckWall();
                if (_isUpingLeft && !_isOnStairs && Keyboard.GetState().IsKeyDown(Keys.Up))
                    this._goToStairs = true;
                else if (_isDowningLeft && !_isOnStairs && Keyboard.GetState().IsKeyDown(Keys.Down))
                    this._goToStairs = true;

                if (!_isWall_L && !_isOnStairs && !_goToStairs)
                {
                    this.CBody.SetTransform(new Vector2(this.CBody.Position.X - _velocity, this.CBody.Position.Y), this.Rotation);
                    _right = false;
                    if (_isGround)
                        this.CurrentAnimation = AnimationTypes.WALK;
                    //else
                    //    this.CurrentAnimation = AnimationTypes.JUMP;
                }

                if (_isOnStairs && (_isUpingRight || _isDowningLeft))
                {
                    this.CBody.SetTransform(new Vector2(this.CBody.Position.X - _velocityOnStairs, this.CBody.Position.Y + _velocityOnStairs), this.Rotation);

                    if (_initialPosition == this.CBody.Position && _isUpingRight)
                        this.EnableCollision();
                    if (_eventString == "hitbox_upstairs_right" && _isDowningLeft)
                        this.EnableCollision();
                }
                else if (_isOnStairs && (_isUpingLeft || _isDowningRight))
                {
                    this.CBody.SetTransform(new Vector2(this.CBody.Position.X - _velocityOnStairs, this.CBody.Position.Y - _velocityOnStairs), this.Rotation);

                    if (_initialPosition == this.CBody.Position && _isDowningRight)
                        this.EnableCollision();
                    if (_eventString == "hitbox_downstairs_right" && _isUpingLeft)
                        this.EnableCollision();
                }
            }
            else if(_isGround && !_isAtacking) this.CurrentAnimation = AnimationTypes.IDLE;

            // jump
            if (Keyboard.GetState().IsKeyDown(Keys.Z) && _isGround && !_isOnStairs && !_isPressJump && !_isAtacking)
            {
                this.CurrentAnimation = AnimationTypes.JUMP;
                this.CBody.ApplyLinearImpulse(new Vector2(0, -(8 * 7)));
                _isFall = false;
                _isPressJump = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Z))
                _isPressJump = false;

            // atack
            if (Keyboard.GetState().IsKeyDown(Keys.X) && !_isAtacking)
            {
                _isAtacking = true;
                this.CreateSwordHitBox();

                if (_isGround)
                    this.CurrentAnimation = AnimationTypes.ATACK;
                else
                    this.CurrentAnimation = AnimationTypes.JUMP_ATACK;
            }

            if(!_isOnStairs && !_goToStairs)
                this.CBody.ApplyLinearImpulse(new Vector2(0, 1));


            if (_lastFrame > -this.CBody.Position.Y)
                _isFall = true;
            _lastFrame = -this.CBody.Position.Y;

            this.SetHitBoxSword();
            this.JumpCornerCorrection();
            this.Animation(gameTime);
        }
        #endregion

        #region Jump Corner Correction
        bool _isFloor;
        bool _isFloorR;
        bool _isFloorL;
        bool _isFloorC;
        bool _isJumpCornerCorrection;

        bool _isJumpRight
        {
            get => (_right && (!_isWall_R_B || !_isWall_R_C || !_isWall_R_T));
        }
        bool _isJumpLeft
        {
            get => (!_right && (!_isWall_L_B || !_isWall_L_C || !_isWall_L_T));
        }

        private void JumpCornerCorrection()
        {
            _isFloor = false;
            _isFloorL = false;
            _isFloorR = false;
            _isFloor = false;

            if (_isGround)
            {
                _isJumpCornerCorrection = false;
                this.CBody.SetFriction(1);
            }

            if (!_isGround && !_isOnStairs && !_goToStairs)
            {
                List<Fixture> fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X + 4, this.CBody.Position.Y - 18));
                foreach (Fixture fixture in fixtures)
                    if (fixture != null && (string)fixture.Body.Tag == "ground")
                    {
                        _isFloor = true;
                        _isFloorR = true;
                    }

                fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X - 4, this.CBody.Position.Y - 18));
                foreach (Fixture fixture in fixtures)
                    if (fixture != null && (string)fixture.Body.Tag == "ground")
                    {
                        _isFloor = true;
                        _isFloorL = true;
                    }

                fixtures = this.World.TestPointAll(new Vector2(this.CBody.Position.X, this.CBody.Position.Y - 18));
                foreach (Fixture fixture in fixtures)
                    if (fixture != null && (string)fixture.Body.Tag == "ground")
                    {
                        _isFloor = true;
                        _isFloorC = true;
                    }

                if (_isFloor && !_isJumpCornerCorrection && !_isFall && (_isJumpRight || _isJumpLeft))
                {
                    float j = 1f;
                    this.CBody.SetFriction(0);
                    Vector2 _velocity;

                    if (_isFloorR && _isFloorL) j = 4f;
                    else if ((_isFloorR && _isFloorC && !_isFloorL && _right) || (!_isFloorR && _isFloorC && _isFloorL && !_right)) j = 3f;

                    if (_right) _velocity = new Vector2(5 * j, 0);
                    else _velocity = new Vector2(-5 * j, 0);
                    this.CBody.Position = this.CBody.Position + _velocity;
                    _isJumpCornerCorrection = true;
                }
            }
        }
        #endregion

        #region Sprite Animation
        private AsepriteDefinitions AsepriteDefinitions;
        private AsepriteAnimation AsepriteAnimation;
        private enum AnimationTypes { IDLE, WALK, JUMP, ATACK, JUMP_ATACK, UPSTAIRS, DOWNSTAIRS }
        private AnimationTypes CurrentAnimation = AnimationTypes.IDLE;
        private bool _right = true;
        private bool _isAtacking;
        public void Animation(GameTime gameTime)
        {
            if (this.CurrentAnimation == AnimationTypes.IDLE)
                this.AsepriteAnimation.Play(gameTime, "idle");
            else if (this.CurrentAnimation == AnimationTypes.JUMP)
            {
                this.AsepriteAnimation.Play(gameTime, "jump");
            }
            else if (this.CurrentAnimation == AnimationTypes.ATACK)
            {
                this.AsepriteAnimation.Play(gameTime, "atack");

                if (AsepriteAnimation.lastFrame)
                {
                    if (_isGround)
                        this.CurrentAnimation = AnimationTypes.IDLE;
                    else
                        this.CurrentAnimation = AnimationTypes.JUMP;
                    _isAtacking = false;
                    this.RemoveSwordHitBox();
                }
            } else if (this.CurrentAnimation == AnimationTypes.JUMP_ATACK)
            {
                this.AsepriteAnimation.Play(gameTime, "jump_atack");

                if (AsepriteAnimation.lastFrame)
                {
                    if (_isGround)
                        this.CurrentAnimation = AnimationTypes.IDLE;
                    else
                        this.CurrentAnimation = AnimationTypes.JUMP;
                    _isAtacking = false;
                    this.RemoveSwordHitBox();
                }
            }
            else if (this.CurrentAnimation == AnimationTypes.WALK)
                this.AsepriteAnimation.Play(gameTime, "walk");

            this.Body = this.AsepriteAnimation.Body;

            if (_right)
                this.spriteEffect = SpriteEffects.None;
            else
                this.spriteEffect = SpriteEffects.FlipHorizontally;
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            // if(_swordHitBox)
                // this.Sword.DrawSprite(spriteBatch);
            this.DrawSprite(spriteBatch);
        }

    }
}
