using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RussBoggle
{
    class Board
    {
        Texture2D tex;
        int size;
        Block[,] blockArray;
        Random rand;
        const int CUBESIZE = 6;
        const int BLOCKSIZE = 64;
        Rectangle selectedSourceRect;
        Rectangle unselectedSourceRect;
        Vector2 boardStartVec;
        SpriteFont font;
        bool freshBoard;
        int lastI;
        int lastJ;
        //char[,] charArray;

        string[] x4_cubes = new string[16] {
                "AAEEGN","ABBJOO","ACHOPS","AFFKPS",
                "AOOTTW","CIMOTU","DEILRX","DELRVY",
                "DISTTY","EEGHNW","EEINSU","EHRTVW",
                "EIOSST","ELRTTY","HIMNQU","HLNNRZ"
            };

        public Board(Texture2D tex, SpriteFont font, int size)
        {
            this.font = font;
            this.tex = tex;
            this.size = size;
            rand = new Random();
            blockArray = new Block[size, size];
            freshBoard = true;

            unselectedSourceRect = new Rectangle(1, 1, BLOCKSIZE, BLOCKSIZE);
            selectedSourceRect = new Rectangle(67,1,BLOCKSIZE,BLOCKSIZE);
            boardStartVec = new Vector2(50, 50);
            InitCubes();
            //MakeCharArray();
        }

        private void InitCubes()
        {
            int[] indiceArray = new int[size * size];
            for (int i = 0; i < (size * size); i++) indiceArray[i] = i;

            for (int i = 0; i < (size * size); i++)
            {
                //is rand.next inclusive or exclusive?
                int swap = rand.Next(i, blockArray.Length);
                indiceArray[i] = indiceArray[swap];
                indiceArray[swap] = i;
            }

            for (int i = 0; i < (size*size); i++)
            {
                //HARDCODED TO 4x4 SIZE RIGHT NOW
                Rectangle rect = new Rectangle((int)(boardStartVec.X) + (int)(i/size) * BLOCKSIZE, (int)(boardStartVec.Y) + (int)(i%size) * BLOCKSIZE, BLOCKSIZE, BLOCKSIZE);
                blockArray[(int)(i/size),(int)(i%size)] = new Block(x4_cubes[indiceArray[i]].Substring(rand.Next(0, CUBESIZE), 1), rect);
                blockArray[(int)(i / size), (int)(i % size)].Selected = false;
            }
        }

        public void CheckFakeCollision(Vector2 v)
        {
            Rectangle r = new Rectangle((int)v.X, (int)v.Y, 1, 1);
            for(int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (r.Intersects(blockArray[i, j].BlockRect)) blockArray[i, j].FakeSelected = true;
                    else blockArray[i, j].FakeSelected = false;
                }
        }

        public string CheckRealCollision(Vector2 v)
        {
            Rectangle r = new Rectangle((int)v.X, (int)v.Y, 1, 1);
            for(int i = 0; i<size;i++)
                for (int j = 0; j < size; j++)
                {
                    blockArray[i, j].FakeSelected = false;
                    if (r.Intersects(blockArray[i, j].BlockRect) && (!blockArray[i,j].Selected))
                    {
                        if (ValidSelection(i, j))
                        {
                            blockArray[i, j].Selected = true;
                            return blockArray[i, j].Letters;
                        }
                    }
                }
            return "";
        }
        /*
        private void MakeCharArray()
        {
            charArray = new char[size,size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    charArray[i, j] = blockArray[i, j].Letters.ToCharArray(0, 1)[0];
        }
        */
        public string CheckAutomated(int i, int j)
        {
            if(i>(size) || (i<0)) return "";
            if (j > (size) || (j < 0)) return "";
            return blockArray[i, j].Letters;
        }

        private bool ValidSelection(int i, int j)
        {
            if (freshBoard)
            {
                lastI = i;
                lastJ = j;
                freshBoard = false;
                return true;
            }
            else if (((lastI -1) == i) || (lastI == i) || ((lastI + 1) == i))
                if (((lastJ - 1) == j) || (lastJ == j) || ((lastJ + 1) == j))
                {
                    lastI = i;
                    lastJ = j;
                    return true;
                }
            return false;
        }

        public void ResetSelections()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    blockArray[i, j].Selected = false;
                    blockArray[i, j].FakeSelected = false;
                    freshBoard = true;
                }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < size; i++)
                for(int j = 0; j<size;j++)
                {
                    if (blockArray[i, j].Selected || blockArray[i,j].FakeSelected) spriteBatch.Draw(tex, blockArray[i,j].BlockRect, selectedSourceRect, Color.White);
                    else spriteBatch.Draw(tex, blockArray[i,j].BlockRect, unselectedSourceRect, Color.White);
                    spriteBatch.DrawString(font, blockArray[i, j].Letters, new Vector2(blockArray[i,j].BlockRect.X + 10, blockArray[i,j].BlockRect.Y + 3), Color.Black);
                }
        }

    }
}
