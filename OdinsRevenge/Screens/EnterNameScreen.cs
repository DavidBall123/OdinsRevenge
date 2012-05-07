﻿#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 
#endregion

namespace OdinsRevenge
{
    /// <summary>
    /// The following class is modfied on a class found at this location
    /// http://create.msdn.com/en-US/education/catalog/sample/game_state_management
    /// 
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class EnterNameScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        KeyboardState currentKeyboardState;
        string messageString;
        private int score;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public EnterNameScreen(int score)
        {
            this.score = score;
            messageString= "";
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            currentKeyboardState = new KeyboardState();
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            
            
            
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            currentKeyboardState = Keyboard.GetState();


            if (currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                ExitScreen();
            }

            Keys[] pressedKeys;
            pressedKeys = currentKeyboardState.GetPressedKeys();
            for (int i = 0; i < pressedKeys.Length; i++)
            {
                messageString = messageString + pressedKeys[i].ToString() + " ";
            }
            
            
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.DrawString(ScreenManager.Font, "Please enter your name", new Vector2(50, 50), Color.GhostWhite);
            spriteBatch.DrawString(ScreenManager.Font, messageString, new Vector2(50, 150),Color.White);
            spriteBatch.End();
        }


        #endregion
    }
}



