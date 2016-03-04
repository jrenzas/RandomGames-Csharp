using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RussBoggle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D squareTex;
        SpriteFont pericles14;
        SpriteFont pericles36;
        const int cScreenWidth = 600;
        const int cScreenHeight = 350;
        const int cStatusOffset = 350;
        const string cWindowTitleText = "Leave Me Alone!";
        Rectangle buttonOutside;
        Rectangle buttonInside;
        Rectangle resetButtonOutside;
        Rectangle resetButtonInside;
        Texture2D dummyOutTex;
        Texture2D dummyInTex;
        Board gameBoard;
        enum GameState { Start, Playing, End };
        GameState gameState;
        bool lClickDown;
        bool rClickDown;
        string[] wordList;

        List<string> possibleWords;
        const int cBoardSize = 4;
        int gamerScore;
        List<string> gamerWords;
        string gamerCurrentWord;

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
            graphics.PreferredBackBufferWidth = cScreenWidth;
            graphics.PreferredBackBufferHeight = cScreenHeight;
            graphics.ApplyChanges();
            this.Window.Title = cWindowTitleText;

            gamerScore = 0;
            gamerWords = new List<string>();

            buttonOutside = new Rectangle(cStatusOffset+10, 315, 145, 25);
            dummyOutTex = new Texture2D(graphics.GraphicsDevice, 1,1);
            dummyOutTex.SetData(new Color[] { Color.Black });

            buttonInside = new Rectangle(buttonOutside.X+2, buttonOutside.Y+2, buttonOutside.Width-4, buttonOutside.Height-4);
            dummyInTex = new Texture2D(graphics.GraphicsDevice, 1, 1);
            dummyInTex.SetData(new Color[] { Color.WhiteSmoke });

            resetButtonOutside = new Rectangle(520, 315, 70, 25);
            resetButtonInside = new Rectangle(resetButtonOutside.X + 2, resetButtonOutside.Y + 2, resetButtonOutside.Width - 4, resetButtonOutside.Height - 4);

            gameState = GameState.Start;
            lClickDown = false;
            rClickDown = false;
            possibleWords = new List<string>();

            this.IsMouseVisible = true;

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
            squareTex = Content.Load<Texture2D>(@"bogglesquares");
            pericles14 = Content.Load<SpriteFont>(@"pericles14");
            pericles36 = Content.Load<SpriteFont>(@"pericles36");
            string line;
            List<string> tempList = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader("wordlist.txt");
            while ((line = file.ReadLine()) != null)
            {
                tempList.Add(line);
            }
            file.Close();
            wordList = new string[tempList.Count];
            int i = 0;
            foreach (string str in tempList)
            {
                wordList[i++] = str;
            }
            Array.Sort(wordList);

            // TODO: use this.Content to load your game content here
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
            if (gameState == GameState.Start)
            {
                gameBoard = new Board(squareTex, pericles36, cBoardSize);
                gameState = GameState.Playing;
                gamerCurrentWord = "";
                gamerWords.Clear();
                possibleWords.Clear();
                FillPossibleWords();
            }
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState ms = Mouse.GetState();
            if (CheckMouseReturnIfWordSubmit(ms))
            {
                bool wordSuccess = CheckSubmittedWord();
                if (!wordSuccess)
                {
                    gameBoard.ResetSelections();
                    gamerCurrentWord = "";
                }
                else
                {
                    if (gamerCurrentWord.Length > 7) gamerScore += 11;
                    else if (gamerCurrentWord.Length == 7) gamerScore += 5;
                    else if (gamerCurrentWord.Length == 6) gamerScore += 3;
                    else if (gamerCurrentWord.Length == 5) gamerScore += 2;
                    else gamerScore += 1;
                    gameBoard.ResetSelections();
                    gamerWords.Add(gamerCurrentWord);
                    gamerCurrentWord = "";
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            spriteBatch.Begin();
            DrawScoreAndWords(spriteBatch);
            gameBoard.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawScoreAndWords(SpriteBatch spriteBatch)
        {
            Vector2 wordLocation = new Vector2(cStatusOffset,55);
            spriteBatch.DrawString(pericles14, "Possible Words: " + possibleWords.Count.ToString(), new Vector2(cStatusOffset, 10), Color.Black);
            spriteBatch.DrawString(pericles14, "Score: " + gamerScore.ToString(), new Vector2(cStatusOffset, 32), Color.Black);
            spriteBatch.DrawString(pericles14, "Player Words:", wordLocation,Color.Black);
            int i = 1;
            foreach (string word in gamerWords)
            {
                //edit this to deal with longer lists of player words later
                if (i > 9)
                {
                    wordLocation.Y = 55;
                    wordLocation.X += 60;
                    i = 1;
                }
                wordLocation.Y += 25;
                spriteBatch.DrawString(pericles14, word, wordLocation, Color.Black);
                i++;
            }
            spriteBatch.Draw(dummyOutTex, buttonOutside, Color.Black);
            spriteBatch.Draw(dummyInTex, buttonInside, Color.WhiteSmoke);
            spriteBatch.DrawString(pericles14, "Submit Word", new Vector2(buttonOutside.X+5, buttonOutside.Y), Color.Black);

            spriteBatch.Draw(dummyOutTex, resetButtonOutside, Color.Black);
            spriteBatch.Draw(dummyInTex, resetButtonInside, Color.WhiteSmoke);
            spriteBatch.DrawString(pericles14, "Reset", new Vector2(resetButtonOutside.X + 5, resetButtonOutside.Y), Color.Black);

        }

        private bool CheckMouseReturnIfWordSubmit(MouseState ms)
        {
            Vector2 v = new Vector2((float)ms.X,(float)ms.Y);
            if (ms.LeftButton == ButtonState.Pressed)
            {
                lClickDown = true;
                gameBoard.CheckFakeCollision(v);
            }
            else if (lClickDown && ms.LeftButton == ButtonState.Released) //already assumes it's released, just there for code clarity
            {
                lClickDown = false;
                gamerCurrentWord += gameBoard.CheckRealCollision(v);
                if (IsSubmitPushed(v)) return true;
                if (IsResetPushed(v)) gameState = GameState.Start;
            }

            if (ms.RightButton == ButtonState.Pressed) rClickDown = true;
            else if ((rClickDown == true) && (ms.RightButton == ButtonState.Released))
            {
                rClickDown = false;
                return true;
            }
            return false;
        }

        private bool IsSubmitPushed(Vector2 v)
        {
            return buttonOutside.Intersects(new Rectangle((int)v.X, (int)v.Y, 1, 1));
        }

        private bool IsResetPushed(Vector2 v)
        {
            return resetButtonOutside.Intersects(new Rectangle((int)v.X, (int)v.Y, 1, 1));
        }

        private bool CheckSubmittedWord()
        {
            if (gamerCurrentWord.Length < 3) return false;
            if(gamerWords.Contains(gamerCurrentWord)) return false;
            if (wordList.Contains(gamerCurrentWord)) return true;
            return false;
        }

        #region Check Possibilities, not used for standard play
        private void FillPossibleWords()
        {
            //some info is in board object, some is here, easier to write it here, not proper though
            foreach (string str in wordList)
            {
                bool wordFound = false;
                int strLen = str.Length;
                if (strLen > 2)
                {
                    for (int i = 0; i < cBoardSize; i++)
                    {
                        if (wordFound) break;
                        for (int j = 0; j < cBoardSize; j++)
                        {
                            if (wordFound)
                            {
                                possibleWords.Add(str);
                                break;
                            }
                            wordFound = CheckAdjacent(i, j, str, strLen-1);
                        }
                    }
                }
            }
        }

        private bool CheckAdjacent(int i, int j, string word, int index)
        {
            if (i < 0 || i >= cBoardSize || j < 0 || j >= cBoardSize || index < 0) return false;
            if (word.Substring(index, 1) != gameBoard.CheckAutomated(i, j)) return false;
            else if (index == 0) return true;
            else
            {

                if (CheckAdjacent(i - 1, j - 1, word, index - 1)) return true;
                if (CheckAdjacent(i, j - 1, word, index - 1)) return true;
                if (CheckAdjacent(i + 1, j - 1, word, index  - 1)) return true;
                if (CheckAdjacent(i - 1, j, word, index - 1)) return true;
                if (CheckAdjacent(i + 1, j, word, index - 1)) return true;
                if (CheckAdjacent(i - 1, j + 1, word, index - 1)) return true;
                if (CheckAdjacent(i, j + 1, word, index - 1)) return true;
                if (CheckAdjacent(i + 1, j + 1, word, index - 1)) return true;
            }
            return false;
        }



        #endregion
    }
}
