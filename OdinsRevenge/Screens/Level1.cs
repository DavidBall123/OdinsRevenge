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
  
    class Level1 : OdinLevels
    {
        #region background & graphic variables

      
        Bird bird = new Bird();
        Animation birdAnimation = new Animation();
        Texture2D birdTexture;

        Boat boat = new Boat(); 
        Animation boatAnimation = new Animation();
        Texture2D boatTexture;

        BackGround ocean1;

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
            ocean1 = new BackGround(content, "Backgrounds\\Ocean1");

            birdTexture = content.Load<Texture2D>("Backgrounds\\GreyBirdFly");
            birdAnimation.Initialize(birdTexture, Vector2.Zero, 33, 29, 4, 100, Color.White, 0.8f, true);
            bird.Initialize(birdTexture, position, birdAnimation);

            position.X = -150;
            position.Y = 350;

            boatTexture = content.Load<Texture2D>("Backgrounds\\Boat");
            boatAnimation.Initialize(boatTexture, Vector2.Zero, 63, 69, 4, 100, Color.White, 0.8f, true);
            boat.Initialize(boatTexture, position, boatAnimation);
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
                player.Update(gameTime);
                healthBar.Update(player.health); 
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
            player.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
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


        #endregion
    }
}
