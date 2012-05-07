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
using System.Collections.Generic; 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    class HighScoreScreen : MenuScreen
    {
        #region Fields

        private ContentManager content;
        private Texture2D backgroundTexture;
        private const int MAX = 1000;

        private Dictionary<String, int> scores = new Dictionary<String, int>();
        ScoreManagement scoreManagement = new ScoreManagement();

        #endregion

        #region Initialization
        

        /// <summary>
        /// Constructor.
        /// </summary>
        public HighScoreScreen() : base("High Score Table")
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            scores = scoreManagement.ReadScores();

            
            MenuEntry back = new MenuEntry("Back");
            back.Selected += OnCancel;
            MenuEntries.Add(back);
            
            
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

            backgroundTexture = content.Load<Texture2D>("Odin");
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
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();
            DrawScores(spriteBatch);
            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime, true);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;

            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, Color.OrangeRed, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public void DrawScores(SpriteBatch spriteBatch)
        {
           
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Draw(backgroundTexture, fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            int x = 250;
            int xTwo = 500;
            int y = 150;
            spriteBatch.DrawString(ScreenManager.Font, "Name        Score", new Vector2(250, 100), Color.Red);
            int count = 0;
            foreach (KeyValuePair<string, int> pair in scores)
            {

                string key = pair.Key;
                key = key.Remove(key.Length - 1, 1);
                if (count < 5)
                {
                    spriteBatch.DrawString(ScreenManager.Font, key, new Vector2(x, y), Color.Red);
                    spriteBatch.DrawString(ScreenManager.Font, pair.Value.ToString(), new Vector2(xTwo, y), Color.Red);
                }
                count++; 
                y = y + 60;
            }

            
        }

       


        #endregion
    }
}

