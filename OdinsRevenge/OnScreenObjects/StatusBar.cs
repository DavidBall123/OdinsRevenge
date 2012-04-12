using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class StatusBar : BaseStaticOnScreenObjects
    {
        private int barNumber;
        
        
        /// </summary>
        /// <param name="health"></param>
        public void Update(int BarNumber)
        {
            this.barNumber = BarNumber; 
        }

        /// <summary>
        /// Draws the health bar 
        /// </summary>
        /// <param name="spriteBatch"></param>

        public override void Draw(SpriteBatch spriteBatch)
        {

            double  barPerct;
            barPerct = ((double)barNumber / 100) * 217; //PUT THIS INTO CONSTANTS 

            Rectangle rect = new Rectangle(20,20,(int)barPerct,26);

            spriteBatch.Draw(firstTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(secondTexture, rect, Color.Red);
            //spriteBatch.Draw(secondTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            

        }
    }
}
