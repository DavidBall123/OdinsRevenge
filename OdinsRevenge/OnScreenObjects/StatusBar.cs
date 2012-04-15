using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class StatusBar 
    {
        protected double barNumber;
        protected Texture2D firstTexture;
        protected Texture2D secondTexture;
        // Position of the object relative to the upper left side of the screen
        public Vector2 Position;

        public virtual void Initialize(Texture2D texture, Texture2D texture2, Vector2 position)
        {
            firstTexture = texture;
            secondTexture = texture2;

            // Set the starting position of the object around the middle of the screen and to the back
            Position = position;

            

        }

        /// </summary>
        /// <param name="health"></param>
        public void Update(double BarNumber)
        {
            this.barNumber = BarNumber; 
        }

        /// <summary>
        /// Draws the bar 
        /// </summary>
        /// <param name="spriteBatch"></param>

        public virtual void Draw(SpriteBatch spriteBatch)
        {

            double  barPerct;
            barPerct = ((double)barNumber / 100) * secondTexture.Width; //PUT THIS INTO CONSTANTS 

            Rectangle rect = new Rectangle(20,20,(int)barPerct,26);

            spriteBatch.Draw(firstTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(secondTexture, rect, Color.White);
            //spriteBatch.Draw(secondTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            

        }
    }
}
