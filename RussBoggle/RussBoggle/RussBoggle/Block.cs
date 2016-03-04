using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RussBoggle
{
    class Block
    {
        string letters;
        bool selected;
        bool Fselected;
        Rectangle blockRect;

        public Block(string str, Rectangle rect)
        {
            letters = str;
            blockRect = rect;
            selected = false;
            Fselected = false;
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public bool FakeSelected
        {
            get { return Fselected; }
            set { Fselected = value; }
        }

        public string Letters
        {
            get { return letters; }
        }

        public Rectangle BlockRect
        {
            get { return blockRect; }
        }
    }
}
