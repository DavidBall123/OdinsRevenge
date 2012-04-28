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

        private int level1StartCounter = 150;
        

        BackGround ocean1;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public Level1()
        {
            base.LevelNumber = 1; 
            levelEndCounter = 100; 
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
        }

        /// <summary>
        /// Loads content specific to the level 
        /// </summary>


        protected override void LevelSpecificContent()
        {                       
            
            
            ocean1 = new BackGround(Content, "Backgrounds\\Ocean1");

            birdTexture = Content.Load<Texture2D>("Backgrounds\\GreyBirdFly");
            birdAnimation.Initialize(birdTexture, Vector2.Zero, 33, 29, 4, 100, Color.White, 0.8f, true);
            bird.Initialize(birdTexture, position, birdAnimation, this);

            position.X = -150;
            position.Y = 350;

            boatTexture = Content.Load<Texture2D>("Backgrounds\\Boat");
            boatAnimation.Initialize(boatTexture, Vector2.Zero, 63, 69, 4, 100, Color.White, 0.8f, true);
            boat.Initialize(boatTexture, position, boatAnimation, this);

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
            Content.Unload();
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
            level1StartCounter--;

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
            foreach (Enemy1 e in enemey1List)
            {
                e.Draw(spriteBatch);
            }
            foreach (Enemy2 e in enemey2List)
            {
                e.Draw(spriteBatch);
            }

            spriteBatch.DrawString(statsFont, "Distance: " + distanceToTravel, new Vector2(600, 25), Color.White);
            spriteBatch.DrawString(statsFont, "Score: " + Score, new Vector2(600, 45), Color.White);

            if (level1StartCounter > 0)
            {
                spriteBatch.DrawString(gameFont, "Level 1", new Vector2(325, 250), Color.White);
            }

            if (distanceToTravel < 0)
            {
                levelEndCounter--; 
                spriteBatch.DrawString(gameFont, "Level 1 Complete, Onwards to the tunrda" , new Vector2(100, 200), Color.White);
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

     

     


      

    


        #endregion

        #region Game logic

        protected override void FirstEnemy()
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

        }

        protected override void SecondEnemy()
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
        }

        #endregion
    }
}
