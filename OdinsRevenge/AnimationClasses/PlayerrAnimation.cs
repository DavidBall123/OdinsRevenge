using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class PlayerAnimation : Animation 
    {   

        // Draw the Animation Strip
        public override void Draw(SpriteBatch spriteBatch, Direction direction)
        {
            // Only draw the animation when we are active
            if (Active)
            {
                if (direction == Direction.Left)
                {
                    Texture2D tempForFlip; 
                    tempForFlip = Flip(spriteStrip, false, true);
                    spriteBatch.Draw(tempForFlip, destinationRect, sourceRect, frameColor);
                }
                else
                {
                    spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, frameColor);
                }
                
            }
        }

        /// <summary>
        /// used to flip animations backwards 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="vertical"></param>
        /// <param name="horizontal"></param>
        /// <returns></returns>

        public static Texture2D Flip(Texture2D source, bool vertical, bool horizontal)
        {
            Texture2D flipped = new Texture2D(source.GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] flippedData = new Color[data.Length];

            source.GetData<Color>(data);

            for (int x = 0; x < source.Width; x++)
                for (int y = 0; y < source.Height; y++)
                {
                    int idx = (horizontal ? source.Width - 1 - x : x) + ((vertical ? source.Height - 1 - y : y) * source.Width);
                    flippedData[x + y * source.Width] = data[idx];
                }

            flipped.SetData<Color>(flippedData);

            return flipped;
        }  

    }
}
