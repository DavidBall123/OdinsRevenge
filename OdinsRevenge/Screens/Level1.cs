 #region File Description
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
  
    sealed class Level1 : OdinLevels
    {
        #region background & graphic variables

        // variables to control the bird

        Bird bird = new Bird();
        Animation birdAnimation = new Animation();
        Texture2D birdTexture;

        // variables to contorl the boat

        Boat boat = new Boat(); 
        Animation boatAnimation = new Animation();
        Texture2D boatTexture;

        BackGround ocean1;

        #endregion

        #region Enemey Variables

        int day1EnemySpawner; 
        DayEnemy1 dayEnemy1;
        Random enemyTimer = new Random();
        int nextSpawn; 
        
        List<DayEnemy1> dayEnemey1List = new List<DayEnemy1>(); 


        #endregion

        #region Properties

      

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public Level1()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
        }

        /// <summary>
        /// Loads content specific to the level 
        /// </summary>


        protected override void LevelSpecificContent()
        {                       
            Player.Initialize(content.Load<Texture2D>("Hero\\Hero"), playerPostion, walkingAnimation, strikingAnimation, spellCastingAnimation, spells, this);
            
            ocean1 = new BackGround(content, "Backgrounds\\Ocean1");

            birdTexture = content.Load<Texture2D>("Backgrounds\\GreyBirdFly");
            birdAnimation.Initialize(birdTexture, Vector2.Zero, 33, 29, 4, 100, Color.White, 0.8f, true);
            bird.Initialize(birdTexture, position, birdAnimation, this);

            position.X = -150;
            position.Y = 350;

            boatTexture = content.Load<Texture2D>("Backgrounds\\Boat");
            boatAnimation.Initialize(boatTexture, Vector2.Zero, 63, 69, 4, 100, Color.White, 0.8f, true);
            boat.Initialize(boatTexture, position, boatAnimation, this);

            position.X = 900;
            position.Y = 440;

            nextSpawn = enemyTimer.Next(200, 500); 
            
            
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
                boat.Update(gameTime);
                Player.Update(gameTime);
                healthBar.Update(Player.Health);
                manaBar.Update(Player.Mana);
                energyBar.Update(Player.Energy); 
                UpdateDayEnemy1(gameTime);
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
            ocean1.Draw(spriteBatch);
            DrawBackground(spriteBatch);
            sun.Draw(spriteBatch);
            if (night == true)
            {
                stars.Draw(spriteBatch);
            }
            bird.Draw(spriteBatch);
            DrawClouds(spriteBatch);
            boat.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);
            manaBar.Draw(spriteBatch);
            energyBar.Draw(spriteBatch); 
            foreach (DayEnemy1 e in dayEnemey1List)
            {
                e.Draw(spriteBatch);
            }
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void UpdateDayEnemy1(GameTime gameTime)
        {
            day1EnemySpawner++;
            if (day1EnemySpawner == nextSpawn)
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
                
                dayEnemy1 = new DayEnemy1(); 
                dayEnemy1.Initialize(dayEnemey1WalkingTexture, position, dayEnemy1AttackAnimation, dayEnemey1WalkingAnimation, dayEnemey1DeathAnimation, this);
                dayEnemey1List.Add(dayEnemy1);
                day1EnemySpawner = 0;
                nextSpawn = enemyTimer.Next(200, 500); 
            }

            

            foreach (DayEnemy1 e in dayEnemey1List)
            {
                e.Update(gameTime);
            }

            for (int i = 0; i < dayEnemey1List.Count; i++)
            {
                if (dayEnemey1List[i].Position.X < -200)
                {
                    dayEnemey1List.RemoveAt(i);
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
            for (int i = 0; i < dayEnemey1List.Count; i++)
            {
                if (player.HitBox.Intersects(dayEnemey1List[i].HitBox))
                {
                    if (player.Action == PlayerActions.Striking)
                    {
                        dayEnemey1List[i].Health = 0;
                    }
                    else if (dayEnemey1List[i].Dying == false && dayEnemey1List[i].Attacking == true || dayEnemey1List[i].Death == false && dayEnemey1List[i].Attacking == true) 
                    {
                        if (playerHit == 0)
                        {
                            player.Health = player.Health - 10;
                            playerHit = 80;
                        }
                    }
                }   
            }

        }


        #endregion
    }
}
