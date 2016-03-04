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

namespace RussTetris2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameState { Start, InPlay, ClearLines, Lose };
        GameState gameState = GameState.Start;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D background;
        Texture2D blockImage;
        Texture2D loseScreen;

        SpriteFont pericles14;
        SpriteFont periclesBig;

        bool keyReleased = true;

        float moveInterval;
        float moveIntervalStart = 0.5f;
        float frameTime = 0.0f;

        float keyLRInterval = 0.08f;
        float keyLRTime = 0.0f;
        float keyDownInterval = 0.01f;
        float keyDownTime = 0.0f;

        const int gameWindowX = 33;
        const int gameWindowY = 124;
        const int previewWindowX = 415;
        const int previewWindowY = 140;

        const int gameWidth = 10;
        const int gameHeight = 20;
        const int gameHeightBuffer = 5;
        const int blockWidth = 32;
        const int previewSize = 4;
        const float utilityTimer = 5.0f;
        float utilityTimeCount = 0.0f;
        int gameScore;
        int totalLinesComplete;
        int level;

        Random rand = new Random((int)System.DateTime.Now.Ticks);

        Color[,] blockMatrix = new Color[gameWidth, (gameHeight + gameHeightBuffer)];
        TetrisPiece tetrisPiece;
        Vector2[] nextPieceVector = new Vector2[4];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            this.Window.Title = "Russ's Homebrew Tetris";

            //Test code for drawing.
            /*for (int i = 0; i < gameWidth; i++)
                for (int j = gameHeightBuffer; j < (gameHeightBuffer + gameHeight); j++)
                    blockMatrix[i, j] = Color.Red;*/
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blockImage = Content.Load<Texture2D>(@"32x32_block");
            background = Content.Load<Texture2D>(@"background");
            loseScreen = Content.Load<Texture2D>(@"middlefingerkid");
            pericles14 = Content.Load<SpriteFont>(@"Pericles14");
            periclesBig = Content.Load<SpriteFont>(@"PericlesBig");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (gameState == GameState.Start)
            {
                level = 1;
                gameScore = 0;
                totalLinesComplete = 0;
                moveInterval = moveIntervalStart;
                for (int i = 0; i < gameWidth; i++)
                    for (int j = 0; j < (gameHeight + gameHeightBuffer); j++)
                    {
                        blockMatrix[i, j] = Color.Transparent;
                    }

                tetrisPiece = new TetrisPiece(gameWidth, gameHeight + gameHeightBuffer, rand);
                gameState = GameState.InPlay;
            }
            else if(gameState == GameState.InPlay)
            {
                //Keyboard here
                KeyboardState keyState = Keyboard.GetState();
                keyLRTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (keyLRTime > keyLRInterval)
                {
                    keyLRTime = 0.0f;
                    if (keyState.IsKeyDown(Keys.Left))
                    {
                        tetrisPiece.TryMoveLeft(blockMatrix);
                    }

                    else if (keyState.IsKeyDown(Keys.Right))
                    {
                        tetrisPiece.TryMoveRight(blockMatrix);
                    }
                }
                if (keyState.IsKeyDown(Keys.Z) && keyReleased)
                {
                    //CCW
                    tetrisPiece.Rotate(false, blockMatrix);
                    keyReleased = false;
                }
                else if ((keyState.IsKeyDown(Keys.X) || keyState.IsKeyDown(Keys.Up)) && keyReleased)
                {
                    //CC
                    tetrisPiece.Rotate(true, blockMatrix);
                    keyReleased = false;
                }
                else if (!(keyState.IsKeyDown(Keys.X) || keyState.IsKeyDown(Keys.Z) || keyState.IsKeyDown(Keys.Up)))
                    keyReleased = true;

                keyDownTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (keyDownTime > keyDownInterval)
                {
                    keyDownTime = 0.0f;
                    if (keyState.IsKeyDown(Keys.Down))
                    {
                        frameTime = 10 * frameTime;
                    }
                }

                //Check collisions here
                bool successfulMove = true;
                frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (LostGame())
                {
                    gameState = GameState.Lose;
                }
                else
                {

                    if (frameTime > moveInterval)
                    {
                        frameTime = 0.0f; ;
                        successfulMove = tetrisPiece.TryMoveDown(blockMatrix);
                    }

                    if (!successfulMove)
                    {
                        blockMatrix = tetrisPiece.MigrateToMainGame(blockMatrix);
                        int linesComplete = CompletedLines();
                        totalLinesComplete += linesComplete;
                        if (linesComplete == 1) gameScore += 100 * level;
                        else if (linesComplete == 2) gameScore += 400 * level;
                        else if (linesComplete == 3) gameScore += 800 * level;
                        else if (linesComplete == 4) gameScore += 1600*level;
                        if (gameScore > 100 * Math.Pow(2, level))
                        {
                            level++;
                            if(level < 21) moveInterval = moveIntervalStart - (moveIntervalStart*((float)level/20));
                        }
                        tetrisPiece.MakeNewPiece(rand);
                        frameTime = 0.0f; ;


                    }
                }
            }
            else if (gameState == GameState.Lose)
            {
                utilityTimeCount += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (utilityTimeCount > utilityTimer)
                {
                    gameState = GameState.Start;
                    utilityTimeCount = 0.0f;
                } 
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
            DrawPreview();
            DrawMovingPieces();
            DrawStaticPieces();
            DrawScoreEtc();

            if (gameState == GameState.Lose)
            {
                spriteBatch.Draw(loseScreen, new Rectangle(50, 200, 500, 400), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        void DrawScoreEtc()
        {
            spriteBatch.DrawString(pericles14, "Level: " + level.ToString(), new Vector2(380, 300), Color.Black);
            spriteBatch.DrawString(pericles14, "Score: " + gameScore.ToString(), new Vector2(380, 360), Color.Black);
            spriteBatch.DrawString(pericles14, "Lines Completed: " + totalLinesComplete.ToString(), new Vector2(380, 330), Color.Black);
        }

        void DrawStaticPieces()
        {
            for(int i = 0; i< gameWidth;i++)
                for (int j = gameHeightBuffer; j < (gameHeight + gameHeightBuffer); j++)
                {
                    if (blockMatrix[i, j] != Color.Transparent)
                    {
                        Rectangle currentRect = new Rectangle(
                            gameWindowX + (i * blockWidth),
                        gameWindowY + ((j - gameHeightBuffer) * blockWidth),
                        blockWidth,
                        blockWidth);
                        spriteBatch.Draw(blockImage, currentRect, blockMatrix[i,j]);
                    }
                }
        }
        
        void DrawMovingPieces()
        {
            for(int i = 0; i<5; i++)
                for(int j = 0; j<5;j++)
            {
                if (tetrisPiece.GetPiece(i, j) != Color.Transparent)
                {
                    if (tetrisPiece.GetBaseY()+j+1 > gameHeightBuffer)
                    {
                        Rectangle rect = new Rectangle(
                            gameWindowX + ((i+tetrisPiece.GetBaseX()) * blockWidth),
                            gameWindowY + ((j+tetrisPiece.GetBaseY() - gameHeightBuffer) * blockWidth),
                            blockWidth,
                            blockWidth);
                        spriteBatch.Draw(blockImage, rect, tetrisPiece.GetPiece(i,j));
                    }
                }
            }
        }
        
        void DrawPreview()
        {
            nextPieceVector = tetrisPiece.GetPreviewPieces();
            for (int i = 0; i < nextPieceVector.Length; i++)
            {
                Rectangle rect = new Rectangle(
                previewWindowX + (blockWidth * (int)nextPieceVector[i].X),
                previewWindowY + (blockWidth * (int)nextPieceVector[i].Y),
                blockWidth,
                blockWidth);
                spriteBatch.Draw(blockImage, rect, tetrisPiece.GetPreviewColor());
            }
            
        }

        bool LostGame()
        {
            for (int i = 0; i < gameWidth; i++)
                for (int j = 0; j < gameHeightBuffer; j++)
                    if (blockMatrix[i, j] != Color.Transparent) return true;
            return false;
        }

        int CompletedLines()
        {
            

            int completedLines = 0;
                for(int j = gameHeightBuffer; j<gameHeight+gameHeightBuffer;j++)
                {
                    bool lineComplete = true;
                    for (int i = 0; i < gameWidth; i++)
                        if (blockMatrix[i, j] == Color.Transparent) 
                            lineComplete = false;
                    if (lineComplete)
                    {
                        completedLines++;
                        for (int y = j-1; y > gameHeightBuffer; y--)
                        {
                            for (int x = 0; x < gameWidth; x++)
                                blockMatrix[x, y + 1] = blockMatrix[x, y];
                        }
                    }
                }
            return completedLines;
        }

    }
}
