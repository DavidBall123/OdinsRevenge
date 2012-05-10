using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OdinsRevenge
{
    class PlayerAnimationController
    {
        private Texture2D playerTexture;
        private PlayerAnimation walkingAnimation;
        private PlayerAnimation strikingAnimation;
        private PlayerAnimation spellCastingAnimation;
        private PlayerAnimation deathAnimation;
        private Rectangle playerHitBox;
        private Texture2D playerHitBoxTexture;
        private int bw = 2; // Border width
        Texture2D borderLine;
        private Texture2D smoke;
        private List<ParticleSystem> particleList;
        private int partCounter = 0; 

        WayPointList wl = null;
        Rectangle rectangle = new Rectangle(0, 0, 800, 600);


        public int Bw
        {
            get { return bw; }
            set { bw = value; }
        }

        public Texture2D BorderLine
        {
            get { return borderLine; }
            set { borderLine = value; }
        }

        public Texture2D PlayerHitBoxTexture
        {
            get { return playerHitBoxTexture; }
            set { playerHitBoxTexture = value; }
        }

        

        public Rectangle PlayerHitBox
        {
            get { return playerHitBox; }
            set { playerHitBox = value; }
        }
        

        internal PlayerAnimation DeathAnimation
        {
            get { return deathAnimation; }
            set { deathAnimation = value; }
        } 

        internal PlayerAnimation SpellCastingAnimation
        {
            get { return spellCastingAnimation; }
            set { spellCastingAnimation = value; }
        }

        internal PlayerAnimation StrikingAnimation
        {
            get { return strikingAnimation; }
            set { strikingAnimation = value; }
        }

        internal PlayerAnimation WalkingAnimation
        {
            get { return walkingAnimation; }
            set { walkingAnimation = value; }
        }

        public Texture2D PlayerTexture
        {
            get { return playerTexture; }
            set { playerTexture = value; }
        }

        // Get the width of the player 
        public int Width
        {
            get { return playerTexture.Width; }
        }

        // Height of the  object
        public int Height
        {
            get { return playerTexture.Height; }
        }

        // Get the width of the player 
        public int WalkingAnimationWidth
        {
            get { return walkingAnimation.FrameWidth; }
        }

        // Get the height of the player
        public int WalkingAnimationHeight
        {
            get { return walkingAnimation.FrameHeight; }
        }

        // Gets the hitbox

        public Rectangle HitBox
        {
            get { return playerHitBox; }
        }

        public void Intialize(Texture2D texture, PlayerAnimation walkingAnimate, PlayerAnimation strikingAnimate, PlayerAnimation spellAnimate, PlayerAnimation deathAnimate, Texture2D Smoke)
        {
            particleList = new List<ParticleSystem>(); 
            playerTexture = texture;
            walkingAnimation = walkingAnimate;
            strikingAnimation = strikingAnimate;
            spellCastingAnimation = spellAnimate;
            deathAnimation = deathAnimate;
            smoke = Smoke; 
        }

        public void Update(GameTime gameTime, Vector2 PlayerPosition)
        {
            walkingAnimation.Position = PlayerPosition;
            walkingAnimation.Update(gameTime);
            strikingAnimation.Position = PlayerPosition;
            strikingAnimation.Update(gameTime);
            deathAnimation.Position = PlayerPosition;
            deathAnimation.Update(gameTime);
            spellCastingAnimation.Position = PlayerPosition;
            spellCastingAnimation.Update(gameTime);

            foreach (ParticleSystem p in particleList)
            {
                p.Update(gameTime); 
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(playerHitBoxTexture, playerHitBox, Color.Green);
            //spriteBatch.Draw(borderLine, new Rectangle(playerHitBox.Left, playerHitBox.Top, bw, playerHitBox.Height), Color.Purple);
            //spriteBatch.Draw(borderLine, new Rectangle(playerHitBox.Right, playerHitBox.Top, bw, playerHitBox.Height), Color.Purple);
            //spriteBatch.Draw(borderLine, new Rectangle(playerHitBox.Left, playerHitBox.Top, playerHitBox.Width, bw), Color.Purple);
            //spriteBatch.Draw(borderLine, new Rectangle(playerHitBox.Left, playerHitBox.Bottom, playerHitBox.Width, bw), Color.Purple);

            if (partCounter < 0)
            {
                particleList.Clear(); 
            }

            foreach (ParticleSystem p in particleList)
            {
                partCounter--; 
                p.Draw(spriteBatch);
            }


        }

        public void DrawDeath(SpriteBatch spriteBatch)
        {
            DeathAnimation.Draw(spriteBatch);
        }

        /// <summary>
        /// The draw method for when the player is standing still 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="direction"></param>

        public void DrawStanding(SpriteBatch spriteBatch, Direction direction, Vector2 PlayerPosition)
        {
            if (direction == Direction.Right)
            {
                spriteBatch.Draw(playerTexture, PlayerPosition, null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(playerTexture, PlayerPosition, null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.FlipHorizontally, 0f);
            }
        }

        /// <summary>
        /// Draws the striking animation
        /// </summary>
        /// <param name="spriteBatch"></param>

        public void DrawStriking(SpriteBatch spriteBatch, Direction direction)
        {
            StrikingAnimation.Draw(spriteBatch, direction);
        }

        /// <summary>
        /// Draws the player casting a spell
        /// </summary>
        /// <param name="spriteBatch"></param>

        public void DrawSpellCasting(SpriteBatch spriteBatch, Direction direction, Dictionary<string, Texture2D> spells, List<Enemy1> enemey1List, List<Enemy2> enemey2List)
        {
            partCounter = 250;

            foreach (Enemy1 e in enemey1List)
            {
                Vector2 particlePos = new Vector2(e.Position.X, (e.Position.Y + 50));
                ParticleSystem p = new ParticleSystem(particlePos, 40, 999);
                p.tex = smoke;
                p.setMandatory1(smoke, new Vector2(5, 5), new Vector2(32, 32), Color.Black, Color.White);
                p.setMandatory2(-1, 1, 1, 5, 0);
                rectangle = new Rectangle(0, 0, 800, 600);
                p.setMandatory3(120, rectangle);
                p.setMandatory4(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 0));
                p.randomDelta = new Vector2(0.1f, 0.1f);
                p.moveTowards = 3;
                p.moveTowardsPos = new Vector2(50, 50);
                p.moveToDrift = 0.1f;
                p.activate();
                wl = null;
                particleList.Add(p); 
            }

            foreach (Enemy2 e in enemey2List)
            {
                Vector2 particlePos = new Vector2(e.Position.X, (e.Position.Y + 50));
                ParticleSystem p = new ParticleSystem(particlePos, 40, 999);
                p.tex = smoke;
                p.setMandatory1(smoke, new Vector2(5, 5), new Vector2(32, 32), Color.Black, Color.White);
                p.setMandatory2(-1, 1, 1, 5, 0);
                rectangle = new Rectangle(0, 0, 800, 600);
                p.setMandatory3(120, rectangle);
                p.setMandatory4(new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 0));
                p.randomDelta = new Vector2(0.1f, 0.1f);
                p.moveTowards = 3;
                p.moveTowardsPos = new Vector2(50, 50);
                p.moveToDrift = 0.1f;
                p.activate();
                wl = null;
                particleList.Add(p); 
            }

            spellCastingAnimation.Draw(spriteBatch, direction);
            spriteBatch.Draw(spells["Power of Thor"], new Vector2(0, 0), Color.White);
        }

        /// <summary>
        /// Draws the player walking 
        /// </summary>
        /// <param name="spriteBatch"></param>

        public void DrawAnimation(SpriteBatch spriteBatch, Direction direction)
        {
            walkingAnimation.Draw(spriteBatch, direction);
        }
    }
}
