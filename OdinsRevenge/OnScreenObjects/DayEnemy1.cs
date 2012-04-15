﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace OdinsRevenge
{
    class DayEnemy1 : Enemies
    {
        private const int ATTACK = -100;
        private const int TOO_FAR = 100;
        int deathCounter = 0;
        

        public override void Draw(SpriteBatch spriteBatch)
        {
            int distance;
            DrawHitBox(spriteBatch); 
            distance = (int)levelController.Player.PlayerPosition.X - (int)Position.X;

            if (Health == 100)
            {
                if (distance > ATTACK && distance < TOO_FAR)
                    
                    attackAnimation.Draw(spriteBatch);
                else
                    walkingAnimation.Draw(spriteBatch);
            }
            else
            {
                Dying = true; 
                deathCounter++;
                if (deathCounter <= 60)
                {
                    deathAnimation.Draw(spriteBatch);
                }
                else
                {
                    Death = true; 
                }

            }

        }
       
    }
}
