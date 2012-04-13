using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class ManaBar : StatusBar
    {

        public override void Draw(SpriteBatch spriteBatch)
        {

            double barPerct;
            barPerct = ((double)barNumber / 100) * secondTexture.Width; //PUT THIS INTO CONSTANTS 

            Rectangle rect = new Rectangle(20, 80, (int)barPerct, 26);
            spriteBatch.Draw(firstTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(secondTexture, rect, Color.White);
            


        }
    }
}
