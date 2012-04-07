using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    public abstract class AnimationBase
    {
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Draw(SpriteBatch spriteBatch, Direction direction) { }
        
        
        
    }
}
