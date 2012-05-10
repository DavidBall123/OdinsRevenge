using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace OdinsRevenge
{
    class Ghost : BaseStaticOnScreenObjects
    {
        protected Rectangle enemyHitBox;
        protected float attackingScale = 0.8f;
        
        public int waypointCounter = 1;
        Vector2 Wp1 = new Vector2(700, 250);
        Vector2 Wp2 = new Vector2(400, 150);
        Vector2 Wp3 = new Vector2(100, 250);
        Vector2 Wp4 = new Vector2(-200, 150);
   

        // Gets the hitbox

        public Rectangle HitBox
        {
            get { return enemyHitBox; }
        }


        public void Initialize(Texture2D ghost)
        {
            objectTexture = ghost;
            Position.X = 850;
            Position.Y = 250;
            
        }

      

        public void Update(GameTime gameTime)
        {
            enemyHitBox = new Rectangle((int)(Position.X - 30), (int)(Position.Y), (int)((Width - 130) * attackingScale), (int)(Height * attackingScale));

            switch (waypointCounter)
            {
                case 1:
                    if (Wp1.X < Position.X)
                        Position.X--;
                    else if (Wp1.Y > Position.Y)
                        Position.Y++;
                    else
                        waypointCounter = 2; 
                    break;
                case 2:
                    if (Wp2.X < Position.X && Wp2.Y < Position.Y)
                    {
                        Position.X--;
                        Position.Y--;
                    }
                    else if (Wp2.X < Position.X)
                        Position.X--;
                    else if (Wp2.Y < Position.Y)
                        Position.Y++;
                    else
                        waypointCounter = 3;
                    break;
                case 3:
                    if (Wp3.X < Position.X && Wp3.Y > Position.Y)
                    {
                        Position.X--;
                        Position.Y++;
                    }
                    else if (Wp3.X < Position.X)
                        Position.X--;
                    else if (Wp3.Y < Position.Y)
                        Position.Y++;
                    else
                        waypointCounter = 4;
                    break;
                case 4:
                    if (Wp4.X < Position.X && Wp4.Y < Position.Y)
                    {
                        Position.X--;
                        Position.Y--;
                    }
                    else if (Wp4.X < Position.X)
                        Position.X--;
                    else if (Wp4.Y < Position.Y)
                        Position.Y++;
                    else
                    {
                        waypointCounter = 1;
                        Position.X = 800;
                    }

                    break;
            }

            //if (Position.X > 0)
            //    Position.X--;

            //if (Position.X < -50)
            //    Position.X = 850;  
         
        }
    }
}
