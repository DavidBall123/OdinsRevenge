using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    class Boat : BaseAnimatedOnScreenObjects
    {

        public virtual void Update(GameTime gameTime)
        {
            animation.Position = Position;
            animation.Update(gameTime);
            UpdateBoat();
        }

        /// <summary>
        /// If the boat  is still on the screen it moves it across it, otherwise
        /// it will randomly teleport the  somewhere to the right of the screen
        /// </summary>


        private void UpdateBoat()
        {
            if (Position.X <= 950)
            {
                Position.X = Position.X + 1;
            }
            else
            {
                Random rand1 = new Random();

                Position.X = rand1.Next(-1100, -400);

            }
        }
    }
}
