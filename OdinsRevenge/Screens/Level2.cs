﻿ #region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace OdinsRevenge
{
  
    sealed class Level2 : OdinLevels
    {
        #region background & graphic variables

        // variables to control the bird

        Bird bird = new Bird();
        Animation birdAnimation = new Animation();
        Texture2D birdTexture;

        private int level2StartCounter = 150; 


       

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public Level2()
        {
            base.LevelNumber = 2; 
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
        }

        /// <summary>
        /// Loads content specific to the level 
        /// </summary>


        protected override void LevelSpecificContent()
        {                       
            
            
            Snow = new BackGround(content, "Backgrounds\\Snow");

            birdTexture = content.Load<Texture2D>("Backgrounds\\GreyBirdFly");
            birdAnimation.Initialize(birdTexture, Vector2.Zero, 33, 29, 4, 100, Color.White, 0.8f, true);
            bird.Initialize(birdTexture, position, birdAnimation, this);

            position.X = -150;
            position.Y = 350;

        

            position.X = 900;
            position.Y = 440;

            next1Spawn = enemy1Timer.Next(200, 500);
            next2Spawn = enemy2Timer.Next(100, 300); 
            
            
        }

       


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                bird.Update(gameTime, day);
                sun.Update(gameTime);
                
                Player.Update(gameTime);
                healthBar.Update(Player.PlayerResources.Health);
                manaBar.Update(Player.PlayerResources.Mana);
                energyBar.Update(Player.PlayerResources.Energy);
                if (distanceToTravel < 0)
                {
                    enemey1List.Clear();
                    enemey2List.Clear(); 
                }
                UpdateEnemy1(gameTime);
                UpdateEnemy2(gameTime);
                DetectCollision();
                if (playerHit > 0)
                {
                    player.PlayerHit = true;
                    playerHit--;
                }
                else
                {
                    player.PlayerHit = false;
                }
               
               
            }
        }

     

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            ground.Draw(spriteBatch);
            DrawBackground(spriteBatch);
            sun.Draw(spriteBatch);
            if (night == true)
            {
                stars.Draw(spriteBatch);
            }
            //bird.Draw(spriteBatch);
            Snow.Draw(spriteBatch);
            //DrawClouds(spriteBatch);
            Player.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);
            manaBar.Draw(spriteBatch);
            energyBar.Draw(spriteBatch);
            foreach (Enemy1 e in enemey1List)
            {
                e.Draw(spriteBatch);
            }
            foreach (Enemy2 e in enemey2List)
            {
                e.Draw(spriteBatch);
            }
            if (level2StartCounter > 0)
            {
                level2StartCounter--;
                spriteBatch.DrawString(gameFont, "Level 2", new Vector2(325, 250), Color.Black);
            }

            spriteBatch.DrawString(statsFont, "Distance: " + distanceToTravel, new Vector2(600, 25), Color.White);
            if (distanceToTravel < 0)
            {
                spriteBatch.DrawString(gameFont, "Level 2 Complete" , new Vector2(100, 200), Color.White);
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void UpdateEnemy1(GameTime gameTime)
        {
            enemy1Spawner++;
            if (enemy1Spawner == next1Spawn)
            {
                Animation dayEnemey1WalkingAnimation = new Animation();
                Texture2D dayEnemey1WalkingTexture;

                Animation dayEnemy1AttackAnimation = new Animation();
                Texture2D dayEnemey1AttackTexture;

                Animation dayEnemey1DeathAnimation = new Animation();
                Texture2D dayEnemey1DeathTexture;

                dayEnemey1WalkingTexture = content.Load<Texture2D>("DayEnemy1\\DayEnemy1Walking");
                dayEnemey1WalkingAnimation.Initialize(dayEnemey1WalkingTexture, Vector2.Zero, 49, 71, 4, 100, Color.White, 1f, true);

                dayEnemey1AttackTexture = content.Load<Texture2D>("DayEnemy1\\DayEnemy1Attack");
                dayEnemy1AttackAnimation.Initialize(dayEnemey1AttackTexture, Vector2.Zero, 83, 90, 6, 100, Color.White, 1f, true);

                dayEnemey1DeathTexture = content.Load<Texture2D>("DayEnemy1\\DayEnemy1Death");
                dayEnemey1DeathAnimation.Initialize(dayEnemey1DeathTexture, Vector2.Zero, 86, 90, 3, 400, Color.White, 1f, true);

                enemy1 = new Enemy1();
                enemy1.Initialize(dayEnemey1WalkingTexture, position, dayEnemy1AttackAnimation, dayEnemey1WalkingAnimation, dayEnemey1DeathAnimation, this);
                enemey1List.Add(enemy1);
                enemy1Spawner = 0;
                next1Spawn = enemy1Timer.Next(200, 500);
            }


            foreach (Enemy1 e in enemey1List)
            {
                e.Update(gameTime);
            }

            for (int i = 0; i < enemey1List.Count; i++)
            {
                if (enemey1List[i].Position.X < -200)
                {
                    enemey1List.RemoveAt(i);
                }
                else if (enemey1List[i].Death == true)
                {
                    enemey1List.RemoveAt(i);
                }
            }
        }

        private void UpdateEnemy2(GameTime gameTime)
        {
            enemy2Spawner++;
            if (enemy2Spawner == next2Spawn)
            {
                Animation dayEnemey2WalkingAnimation = new Animation();
                Texture2D dayEnemey2WalkingTexture;

                Animation dayEnemy2AttackAnimation = new Animation();
                Texture2D dayEnemey2AttackTexture;

                Animation dayEnemey2DeathAnimation = new Animation();
                Texture2D dayEnemey2DeathTexture;

                dayEnemey2WalkingTexture = content.Load<Texture2D>("Enemy2\\Walking");
                dayEnemey2WalkingAnimation.Initialize(dayEnemey2WalkingTexture, Vector2.Zero, 52, 79, 4, 100, Color.White, 1f, true);

                dayEnemey2AttackTexture = content.Load<Texture2D>("Enemy2\\Attacking");
                dayEnemy2AttackAnimation.Initialize(dayEnemey2AttackTexture, Vector2.Zero, 71, 81, 6, 100, Color.White, 1f, true);

                dayEnemey2DeathTexture = content.Load<Texture2D>("Enemy2\\Dying");
                dayEnemey2DeathAnimation.Initialize(dayEnemey2DeathTexture, Vector2.Zero, 104, 84, 6, 200, Color.White, 1f, true);

                Vector2 startPosition = new Vector2();
                startPosition.X = -50;
                startPosition.Y = 440; 

                enemy2 = new Enemy2();
                enemy2.Initialize(dayEnemey2WalkingTexture, startPosition, dayEnemy2AttackAnimation, dayEnemey2WalkingAnimation, dayEnemey2DeathAnimation, this);
                enemey2List.Add(enemy2);
                enemy2Spawner = 0;
                next2Spawn = enemy2Timer.Next(200, 500);
            }


            foreach (Enemy2 e in enemey2List)
            {
                e.Update(gameTime);
            }

            for (int i = 0; i < enemey2List.Count; i++)
            {
                if (enemey2List[i].Position.X > 900)
                {
                    enemey2List.RemoveAt(i);
                }
                else if (enemey2List[i].Death == true)
                {
                    enemey2List.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// Draws the coulds
        /// </summary>

        private void DrawClouds(SpriteBatch spriteBatch)
        {
            foreach (BaseStaticOnScreenObjects e in cloudList)
            {
                if (e.Position.X >= -150)
                {
                    e.Position.X--;
                    e.Draw(spriteBatch);
                }
                else
                {
                    Random rand1 = new Random();
                    e.Position.X = rand1.Next(800, 2000);
                    e.Position.Y = rand1.Next(0, 300);
                }
            }
        }

        /// <summary>
        /// Method checks to see if the players hit box & the bugs hit box are intersecting 
        /// </summary>

        private void DetectCollision()
        {
            for (int i = 0; i < enemey1List.Count; i++)
            {
                if (player.PlayerAnimationController.HitBox.Intersects(enemey1List[i].HitBox))
                {
                    if (player.Attacking == true)
                    {
                        enemey1List[i].Health = 0;
                    }
                    else if (enemey1List[i].Dying == false && enemey1List[i].Attacking == true || enemey1List[i].Death == false && enemey1List[i].Attacking == true)
                    {
                        if (playerHit == 0)
                        {
                            player.PlayerResources.ReduceHealth();
                            playerHit = 80;
                        }
                    }
                }
            }

            for (int i = 0; i < enemey2List.Count; i++)
            {
                if (player.PlayerAnimationController.HitBox.Intersects(enemey2List[i].HitBox))
                {
                    if (player.Attacking == true)
                    {
                        enemey2List[i].Health = 0;
                    }
                    else if (enemey2List[i].Dying == false && enemey2List[i].Attacking == true || enemey2List[i].Death == false && enemey2List[i].Attacking == true) 
                    {
                        if (playerHit == 0)
                        {
                            player.PlayerResources.ReduceHealth();  
                            playerHit = 80;
                        }
                    }
                }   
            }

        }


        #endregion
    }
}
