using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class EnergyBar : StatusBar
    {
        public override void Draw(SpriteBatch spriteBatch)
        {

            double barPerct;
            barPerct = ((double)barNumber / 100) * secondTexture.Width; 

            Rectangle rect = new Rectangle(20, 100, (int)barPerct, 26);
            spriteBatch.Draw(secondTexture, rect, Color.White);
            spriteBatch.Draw(firstTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }
    }
    
    
}
