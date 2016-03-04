using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RussBreaker
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D spriteSheet;
        public Rectangle screenBounds;
        Paddle PaddleManager;
        BallManager ballManager;
        CollisionManager collisionManager;
        BlockManager blockManager;
        public static int spriteWidth = 31;
        public static int spritePadding = 0;
        public static Rectangle gameBounds;

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
            spriteSheet = Content.Load<Texture2D>(@"Spritesheets\arinoid_master_transparentback");
            background = Content.Load<Texture2D>(@"Spritesheets\background");
            screenBounds = new Rectangle(0, 0, background.Width, background.Height);

            graphics.PreferredBackBufferWidth = background.Width;
            graphics.PreferredBackBufferHeight = background.Height;
            graphics.ApplyChanges();

            //CHANGE THIS TO WHATEVER IT ACTUALLY IS LATER
            gameBounds = new Rectangle(0, 0, background.Width, background.Height);

            PaddleManager = new Paddle(
                spriteSheet,
                new Vector2((float)(screenBounds.Width / 2), (float)(screenBounds.Height * 0.95f)),
                Paddle.PaddleType.Big,
                3);

            ballManager = new BallManager(spriteSheet);
            blockManager = new BlockManager(spriteSheet);
            blockManager.AddBlocks();
            collisionManager = new CollisionManager(PaddleManager, ballManager,blockManager);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (!(ballManager.ballsRemaining)) ballManager.SpawnNormalBall(new Vector2(500, 500), new Vector2(1, 1));

            PaddleManager.Update(gameTime, Mouse.GetState(), screenBounds);
            collisionManager.checkCollisions();
            ballManager.Update(gameTime);
            blockManager.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(background, graphics.GraphicsDevice.Viewport.Bounds, Color.White);
            PaddleManager.Draw(spriteBatch);
            blockManager.Draw(spriteBatch);
            ballManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
