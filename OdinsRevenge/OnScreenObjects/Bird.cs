using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace OdinsRevenge
{
    class Bird : BaseAnimatedOnScreenObjects
    {

        public override void Update(GameTime gameTime, bool day)
        {
            animation.Position = Position;
            animation.Update(gameTime);
            MoveBird(day);
        }

        /// <summary>
        /// If the bird  is still on the screen it moves it across it, otherwise
        /// it will randomly teleport the  somewhere to the right of the screen
        /// </summary>


        private void MoveBird(bool day)
        {
            if (Position.X >= -50 && day == true)
            {
                Position.X = Position.X - 1;
            }
            else
            {
                Random rand1 = new Random();

                Position.X = rand1.Next(900, 1400);
                Position.Y = rand1.Next(50, 250);
            }
        }
    }
}
