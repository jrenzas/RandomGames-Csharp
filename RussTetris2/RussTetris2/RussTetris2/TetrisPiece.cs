using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RussTetris2
{
    //operates entirely in world coordinates
    class TetrisPiece
    {
        enum PieceType { square, L, zee, sss, tee, revL, line };
        enum RotationPosition { zero, ninty, oneeighty, twoseventy };
        private PieceType pieceType;
        private RotationPosition rotPos;
        private Color color;
        private int boundX;
        private int boundY;
        private int baseX;
        private int baseY;
        private int[,] blockPositions;

        private PieceType nextPieceType;
        private Color nextColor;
        private Vector2[] nxtBlkPos = new Vector2[4];

        public TetrisPiece(int boundX, int boundY, Random rand)
        {
            this.boundX = boundX;
            this.boundY = boundY;
            ResetPieces(rand);
        }

        public Color GetPiece(int x, int y)
        {
            if (blockPositions[x, y] == 1) return color;
            return Color.Transparent;
        }
        public int GetBaseX()
        {
            return baseX;
        }
        public int GetBaseY()
        {
            return baseY;
        }

        public void ResetPieces(Random rand)
        {

            int type = rand.Next(0, 7);
            if (type == 0) nextPieceType = PieceType.square;
            else if (type == 1) nextPieceType = PieceType.L;
            else if (type == 2) nextPieceType = PieceType.zee;
            else if (type == 3) nextPieceType = PieceType.sss;
            else if (type == 4) nextPieceType = PieceType.revL;
            else if (type == 5) nextPieceType = PieceType.line;
            else nextPieceType = PieceType.tee;
            nextColor = new Color(rand.Next(0, 220), rand.Next(0, 220), rand.Next(0, 220));

            type = rand.Next(0, 7);
            if (type == 0) pieceType = PieceType.square;
            else if (type == 1) pieceType = PieceType.L;
            else if (type == 2) pieceType = PieceType.zee;
            else if (type == 3) pieceType = PieceType.sss;
            else if (type == 4) pieceType = PieceType.revL;
            else if (type == 5) pieceType = PieceType.line;
            else pieceType = PieceType.tee;
            color = new Color(rand.Next(0, 220), rand.Next(0, 220), rand.Next(0, 220));

            rotPos = RotationPosition.zero;
            baseX = 3;
            baseY = 0;
            SetBlockLocations();
            SetPreviewPosition();
        }

        public void MakeNewPiece(Random rand)
        {
            pieceType = nextPieceType;
            color = nextColor;

            int type = rand.Next(0, 7);
            if (type == 0) nextPieceType = PieceType.square;
            else if (type == 1) nextPieceType = PieceType.L;
            else if (type == 2) nextPieceType = PieceType.zee;
            else if (type == 3) nextPieceType = PieceType.sss;
            else if (type == 4) nextPieceType = PieceType.revL;
            else if (type == 5) nextPieceType = PieceType.line;
            else nextPieceType = PieceType.tee;
            nextColor = new Color(rand.Next(0, 220), rand.Next(0, 220), rand.Next(0, 220));

            rotPos = RotationPosition.zero;
            baseX = 3;
            baseY = 0;
            SetBlockLocations();
            SetPreviewPosition();
        }

        public Color[,] MigrateToMainGame(Color[,] blockMatrix)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (blockPositions[i, j] == 1)
                    {
                        //if((i+baseX)>-1 &&(i+baseX)<boundX)
                        blockMatrix[i + baseX, j + baseY] = color;
                    }
                }
            return blockMatrix;
        }

        private void SetPreviewPosition()
        {
            if(nextPieceType == PieceType.square)
            {
                nxtBlkPos[0].X = 1; nxtBlkPos[0].Y = 0;
                nxtBlkPos[1].X = 1; nxtBlkPos[1].Y = 1;
                nxtBlkPos[2].X = 2; nxtBlkPos[2].Y = 0;
                nxtBlkPos[3].X = 2; nxtBlkPos[3].Y = 1;
            }
                //finish filling out other piecetypes
            else if (nextPieceType == PieceType.L)
            {
                nxtBlkPos[0].X = 1; nxtBlkPos[0].Y = 0;
                nxtBlkPos[1].X = 1; nxtBlkPos[1].Y = 1;
                nxtBlkPos[2].X = 1; nxtBlkPos[2].Y = 2;
                nxtBlkPos[3].X = 2; nxtBlkPos[3].Y = 0;
            }
            else if (nextPieceType == PieceType.revL)
            {
                nxtBlkPos[0].X = 2; nxtBlkPos[0].Y = 0;
                nxtBlkPos[1].X = 2; nxtBlkPos[1].Y = 1;
                nxtBlkPos[2].X = 2; nxtBlkPos[2].Y = 2;
                nxtBlkPos[3].X = 1; nxtBlkPos[3].Y = 0;
            }
            else if (nextPieceType == PieceType.line)
            {
                nxtBlkPos[0].X = 2; nxtBlkPos[0].Y = 0;
                nxtBlkPos[1].X = 2; nxtBlkPos[1].Y = 1;
                nxtBlkPos[2].X = 2; nxtBlkPos[2].Y = 2;
                nxtBlkPos[3].X = 2; nxtBlkPos[3].Y = 3;
            }
            else if (nextPieceType == PieceType.sss)
            {
                nxtBlkPos[0].X = 1; nxtBlkPos[0].Y = 0;
                nxtBlkPos[1].X = 2; nxtBlkPos[1].Y = 0;
                nxtBlkPos[2].X = 2; nxtBlkPos[2].Y = 1;
                nxtBlkPos[3].X = 3; nxtBlkPos[3].Y = 1;
            }
            else if (nextPieceType == PieceType.zee)
            {
                nxtBlkPos[0].X = 1; nxtBlkPos[0].Y = 1;
                nxtBlkPos[1].X = 2; nxtBlkPos[1].Y = 1;
                nxtBlkPos[2].X = 2; nxtBlkPos[2].Y = 0;
                nxtBlkPos[3].X = 3; nxtBlkPos[3].Y = 0;
            }
            else //tee
            {
                nxtBlkPos[0].X = 1; nxtBlkPos[0].Y = 0;
                nxtBlkPos[1].X = 2; nxtBlkPos[1].Y = 0;
                nxtBlkPos[2].X = 3; nxtBlkPos[2].Y = 0;
                nxtBlkPos[3].X = 2; nxtBlkPos[3].Y = 1;
            }
        }

        public Vector2[] GetPreviewPieces()
        {
            return nxtBlkPos;
        }

        public Color GetPreviewColor()
        {
            return nextColor;
        }

        #region Attempt Movement
        public bool TryMoveDown(Color[,] blockMatrix)
        {
            //moves down 1 and returns true if it's okay, else returns false, doesn't move piece
            for(int i = 0; i<5; i++)
                for (int j = 0; j< 5; j++)
                {
                    if(blockPositions[i,j] == 1)
                    {
                            if (!((j + baseY) < boundY - 1)) return false;
                            //else if (!((baseX + i) > -1)) return false;
                            //else if (!((baseX + i) < boundX)) return false;
                            else if (blockMatrix[i + baseX, j + baseY + 1] != Color.Transparent) return false;
                    }
                }
            baseY++;
            return true;
        }

        public bool TryMoveLeft(Color[,] blockMatrix)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (blockPositions[i, j] == 1)
                    {
                        if (!((baseX+i)>0)) return false;
                        else if (blockMatrix[i + baseX-1, j + baseY] != Color.Transparent) return false;
                    }
                }
            baseX--;
            return true;
        }

        public bool TryMoveRight(Color[,] blockMatrix)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (blockPositions[i, j] == 1)
                    {
                        if (!((baseX + i) < boundX-1)) return false;
                        else if (blockMatrix[i + baseX+1, j + baseY] != Color.Transparent) return false;
                    }
                }
            baseX++;
            return true;
        }

        public void Rotate(bool clockwise, Color[,] blockMatrix)
        {

            RotationPosition originalPos = rotPos;
            if (clockwise)
            {
                if (rotPos == RotationPosition.zero) rotPos = RotationPosition.ninty;
                else if (rotPos == RotationPosition.ninty) rotPos = RotationPosition.oneeighty;
                else if (rotPos == RotationPosition.oneeighty) rotPos = RotationPosition.twoseventy;
                else rotPos = RotationPosition.zero;
            }
            else
            {
                if (rotPos == RotationPosition.zero) rotPos = RotationPosition.twoseventy;
                else if (rotPos == RotationPosition.ninty) rotPos = RotationPosition.zero;
                else if (rotPos == RotationPosition.oneeighty) rotPos = RotationPosition.ninty;
                else rotPos = RotationPosition.oneeighty;
            }
            SetBlockLocations();

            bool validRotation = true;
            //test whether rotation is valid
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (blockPositions[i, j] == 1 && validRotation)
                    {
                        if (baseX + i > 9) validRotation = false;
                        else if (baseX + i < 0) validRotation = false;
                        else if (baseY + j > 24) validRotation = false;
                        else if (blockMatrix[i + baseX, j + baseY] != Color.Transparent) validRotation = false;
                    }
                }

            if (!validRotation)
            {
                rotPos = originalPos;
                SetBlockLocations();
            }
        }

        #endregion

        #region Fixed Block Locations
        private void SetBlockLocations()
        {
            if (pieceType == PieceType.square)
            {
                blockPositions = new int[,] {
                { 0,0,0,0,0 },
                { 0,0,0,0,0 },
                { 0,0,1,1,0 },
                { 0,0,1,1,0 },
                { 0,0,0,0,0 }};

            }
            else if (pieceType == PieceType.line)
            {
                if (rotPos == RotationPosition.zero)
                {
                    blockPositions = new int[,] {
                    { 0,0,1,0,0 },
                    { 0,0,1,0,0 },
                    { 0,0,1,0,0 },
                    { 0,0,1,0,0 },
                    { 0,0,0,0,0 }};
                }
                else if (rotPos == RotationPosition.ninty)
                {
                    blockPositions = new int[,] {
                    { 0,0,0,0,0 },
                    { 0,0,0,0,0 },
                    { 0,1,1,1,1 },
                    { 0,0,0,0,0 },
                    { 0,0,0,0,0 }};
                }
                else if (rotPos == RotationPosition.oneeighty)
                {
                    blockPositions = new int[,] {
                    { 0,0,0,0,0 },
                    { 0,0,1,0,0 },
                    { 0,0,1,0,0 },
                    { 0,0,1,0,0 },
                    { 0,0,1,0,0 }};
                }
                else if (rotPos == RotationPosition.twoseventy)
                {
                    blockPositions = new int[,] {
                    { 0,0,0,0,0 },
                    { 0,0,0,0,0 },
                    { 1,1,1,1,0 },
                    { 0,0,0,0,0 },
                    { 0,0,0,0,0 }};
                }
            }
            else if (pieceType == PieceType.L)
            {
                if (rotPos == RotationPosition.zero)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,1,0,0 },
                        { 0,0,1,0,0 },
                        { 0,0,1,1,0 },
                        { 0,0,0,0,0 }};
                }
                else if (rotPos == RotationPosition.ninty)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 },
                        { 0,1,1,1,0 },
                        { 0,1,0,0,0 },
                        { 0,0,0,0,0 }};
                }
                else if (rotPos == RotationPosition.oneeighty)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,1,1,0,0 },
                        { 0,0,1,0,0 },
                        { 0,0,1,0,0 },
                        { 0,0,0,0,0 }};
                }
                else if (rotPos == RotationPosition.twoseventy)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,0,1,0 },
                        { 0,1,1,1,0 },
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 }};
                }
            }
            else if (pieceType == PieceType.revL)
            {
                if (rotPos == RotationPosition.zero)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,1,0,0 },
                        { 0,0,1,0,0 },
                        { 0,1,1,0,0 },
                        { 0,0,0,0,0 }};
                }
                else if (rotPos == RotationPosition.ninty)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,1,0,0,0 },
                        { 0,1,1,1,0 },
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 }};
                }
                else if (rotPos == RotationPosition.oneeighty)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,1,1,0 },
                        { 0,0,1,0,0 },
                        { 0,0,1,0,0 },
                        { 0,0,0,0,0 }};
                }
                else if (rotPos == RotationPosition.twoseventy)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 },
                        { 0,1,1,1,0 },
                        { 0,0,0,1,0 },
                        { 0,0,0,0,0 }};
                }
            }
            else if (pieceType == PieceType.zee)
            {
                if (rotPos == RotationPosition.zero || rotPos == RotationPosition.oneeighty)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 },
                        { 0,1,1,0,0 },
                        { 0,0,1,1,0 },
                        { 0,0,0,0,0 }};
                }
                else
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 },
                        { 0,0,0,1,0 },
                        { 0,0,1,1,0 },
                        { 0,0,1,0,0 }};
                }
            }
            else if (pieceType == PieceType.sss)
            {
                if (rotPos == RotationPosition.zero || rotPos == RotationPosition.oneeighty)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,1,1,0 },
                        { 0,1,1,0,0 },
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 }};
                }
                else
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,1,0,0 },
                        { 0,0,1,1,0 },
                        { 0,0,0,1,0 },
                        { 0,0,0,0,0 }};
                }
            }
            else if (pieceType == PieceType.tee)
            {
                if (rotPos == RotationPosition.zero)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 },
                        { 0,0,1,0,0 },
                        { 0,1,1,1,0 },
                        { 0,0,0,0,0 }};
                }
                else if (rotPos == RotationPosition.ninty)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 },
                        { 0,0,1,0,0 },
                        { 0,0,1,1,0 },
                        { 0,0,1,0,0 }};
                }
                else if (rotPos == RotationPosition.oneeighty)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 },
                        { 0,1,1,1,0 },
                        { 0,0,1,0,0 }};
                }
                else if (rotPos == RotationPosition.twoseventy)
                {
                    blockPositions = new int[,] {
                        { 0,0,0,0,0 },
                        { 0,0,0,0,0 },
                        { 0,0,1,0,0 },
                        { 0,1,1,0,0 },
                        { 0,0,1,0,0 }};
                }
            }
        }
        #endregion


    }
}
