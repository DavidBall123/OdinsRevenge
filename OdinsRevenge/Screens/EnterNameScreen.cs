#region File Description
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
        Keys[] oldKeys = new Keys[0];
        bool ended = false;
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
                if (ended == false)
                {
                    ScoreManagement scoreManager = new ScoreManagement();
                    string finalScore = messageString + " " + score.ToString();
                    scoreManager.WriteScore(finalScore);
                    LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                             new MainMenuScreen());
                    ended = true; 
                }
            }


            // the keys that are currently pressed
            Keys[] pressedKeys;
            pressedKeys = currentKeyboardState.GetPressedKeys();

            // work through each key that is presently pressed
            for (int i = 0; i < pressedKeys.Length; i++)
            {
                // set a flag to indicate we have not found the key
                bool foundIt = false;

                // work through each key in the old keys
                for (int j = 0; j < oldKeys.Length; j++)
                {
                    if (pressedKeys[i] == oldKeys[j])
                    {
                        // we found the key in the old keys
                        foundIt = true;
                    }
                }
                if (foundIt == false)
                {
                    // if we get here we didn't find the key in the old keys
                    // now decode the key value for use in the message
                    string keyString = ""; // initially this is an empty string
                    switch (pressedKeys[i])
                    {
                        case Keys.D0:
                            keyString = "0";
                            break;
                        case Keys.D1:
                            keyString = "1";
                            break;
                        case Keys.D2:
                            keyString = "2";
                            break;
                        case Keys.D3:
                            keyString = "3";
                            break;
                        case Keys.D4:
                            keyString = "4";
                            break;
                        case Keys.D5:
                            keyString = "5";
                            break;
                        case Keys.D6:
                            keyString = "6";
                            break;
                        case Keys.D7:
                            keyString = "7";
                            break;
                        case Keys.D8:
                            keyString = "8";
                            break;
                        case Keys.D9:
                            keyString = "9";
                            break;
                        case Keys.A:
                            keyString = "A";
                            break;
                        case Keys.B:
                            keyString = "B";
                            break;
                        case Keys.C:
                            keyString = "C";
                            break;
                        case Keys.D:
                            keyString = "D";
                            break;
                        case Keys.E:
                            keyString = "E";
                            break;
                        case Keys.F:
                            keyString = "F";
                            break;
                        case Keys.G:
                            keyString = "G";
                            break;
                        case Keys.H:
                            keyString = "H";
                            break;
                        case Keys.I:
                            keyString = "I";
                            break;
                        case Keys.J:
                            keyString = "J";
                            break;
                        case Keys.K:
                            keyString = "K";
                            break;
                        case Keys.L:
                            keyString = "L";
                            break;
                        case Keys.M:
                            keyString = "M";
                            break;
                        case Keys.N:
                            keyString = "N";
                            break;
                        case Keys.O:
                            keyString = "O";
                            break;
                        case Keys.P:
                            keyString = "P";
                            break;
                        case Keys.Q:
                            keyString = "Q";
                            break;
                        case Keys.R:
                            keyString = "R";
                            break;
                        case Keys.S:
                            keyString = "S";
                            break;
                        case Keys.T:
                            keyString = "T";
                            break;
                        case Keys.U:
                            keyString = "U";
                            break;
                        case Keys.W:
                            keyString = "W";
                            break;
                        case Keys.V:
                            keyString = "V";
                            break;
                        case Keys.X:
                            keyString = "X";
                            break;
                        case Keys.Y:
                            keyString = "Y";
                            break;
                        case Keys.Z:
                            keyString = "Z";
                            break;
                        case Keys.Space:
                            keyString = " ";
                            break;
                        case Keys.OemPeriod:
                            keyString = ".";
                            break;
                        case Keys.Enter:
                            keyString = "\n";
                            break;

                    }

                    if (currentKeyboardState.IsKeyUp(Keys.LeftShift) && currentKeyboardState.IsKeyUp(Keys.RightShift))
                    {
                        keyString = keyString.ToLower();
                    }

                    if (pressedKeys[i] == Keys.Back)
                    {
                        if (messageString.Length > 0)
                        {
                            messageString = messageString.Remove(messageString.Length - 1, 1);
                        }
                    }

                    messageString = messageString + keyString;
                }
            }

            // remember the keys for next time
            oldKeys = pressedKeys;
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



