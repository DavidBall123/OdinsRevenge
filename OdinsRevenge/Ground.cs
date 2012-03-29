using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Ground
    {
        Texture2D background;

        int iViewPortWidth = 800;
        int iViewPortHeight = 600;

        int groundWidth = 1200; 
        int groundHeight = 600;

        double groundOffset;

        public double GroundOffset
        {
            get { return groundOffset; }
            set
            {
                groundOffset = value;
                if (groundOffset < 0)
                {
                    groundOffset += groundWidth;
                }
                if (groundOffset > groundWidth)
                {
                    groundOffset -= groundWidth;
                }
            }
        }

        //constructor

        public Ground(ContentManager content, string sBackGround)
        {
            background = content.Load<Texture2D>(sBackGround);
            groundWidth = background.Width;
            groundHeight = background.Height; 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(-1 * (int)groundOffset,0,groundWidth, iViewPortHeight), Color.White);

            if (groundOffset > groundWidth - iViewPortWidth)
            {
                spriteBatch.Draw(background, new Rectangle((-1 * (int)groundOffset) + groundWidth, 0,iViewPortWidth, groundHeight), Color.White);
            }
        } 

    }
}
