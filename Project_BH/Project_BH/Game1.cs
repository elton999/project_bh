using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UmbrellaToolKit;
using Project_BH.Gameplay.Enemies;
using tainicom.Aether.Physics2D.Dynamics;

namespace Project_BH
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Scene Scene;
        ScreemController ScreemController;
        AssetManagement AssetManagement;
        CameraManagement CameraManagement;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Scene = new Scene();
            ScreemController = new ScreemController(graphics, graphics.GraphicsDevice.Adapter, GraphicsDevice, 0);

            AssetManagement = new AssetManagement();
            AssetManagement.Scene = this.Scene;

            // Player
            AssetManagement.Set<Player>("player");

            // Enemies
            AssetManagement.Set<Knight>("knight");
            AssetManagement.Set<Troll>("troll");
            AssetManagement.Set<TrollPoint>("troll_point");
            AssetManagement.Set<AirEnemyPoint>("air_attack_point");

            // Stairs
            AssetManagement.Set<Gameplay.UpStairsRight>("hitbox_upstairs_right");
            AssetManagement.Set<Gameplay.UpStairsLeft>("hitbox_upstairs_left");
            AssetManagement.Set<Gameplay.DownStairsLeft>("hitbox_downstairs_left");
            AssetManagement.Set<Gameplay.DownStairsRight>("hitbox_downstairs_right");

            Scene.ScreemOffset = new Vector2(400, 240);
            Scene.AssetManagement = this.AssetManagement;
            Scene.Content = Content;
            Scene.ScreemGraphicsDevice = GraphicsDevice;
            Scene.Screem = ScreemController;
            Scene.World = new World(new Vector2(0, 10));
            Scene.SetBackgroundColor = Color.CornflowerBlue;
            Scene.SetLevel(1);
            Scene.LevelReady = true;

            CameraManagement = new CameraManagement();
            CameraManagement.ScreemTargetAreaLimits = new Vector2(100, 100);
            CameraManagement.MoveSpeed = 5f;
            ScreemController.CameraManagement = CameraManagement;
            ScreemController.Scene = Scene;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        float _target_y = 0;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for(int i = 0; i < 4; i++) this.Scene.World.Step(gameTime.ElapsedGameTime);
            
            if (_target_y == 0) _target_y = Scene.Players[0].CBody.Position.Y - (8*8);
            CameraManagement.Target = new Vector2(-Scene.Players[0].CBody.Position.X, -_target_y);
            
            ScreemController.Update(gameTime);
            ScreemController.Position = Vector2.Zero;
            Scene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            this.ScreemController.BeginDraw(GraphicsDevice, spriteBatch);
            Scene.Draw(spriteBatch, GraphicsDevice);
            this.ScreemController.EndDraw(GraphicsDevice, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
