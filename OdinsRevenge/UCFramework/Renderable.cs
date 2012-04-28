using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



namespace OdinsRevenge
{

    // ------------------------------------------------ UC_Renderable ----------------------------------------    
    /// <summary>
    /// Parent class for all renderables has active flag and and draw/update functions
    /// </summary>
    public class UC_Renderable
    {

        /// <summary>
        ///        
        /// this variable called active is available for use by user code and is used to identify
        /// active objects
        /// it is used in conjunction with renderable list to manage activity renderables notably 
        /// inactive renderables can be deleted and space re-used
        /// inactive renderables dont receive draw or update events (or any events)
        /// </summary>
        public bool active {set; get;}
        public Color colour { get; set; }

        public UC_Renderable()
        {
            active = true;
            colour = Color.White;
        }

        public virtual void Draw(SpriteBatch sb)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void LoadContent() // probably quite rarely used
        {
        }

        public virtual void Reset()
        {
        }

        /// <summary>
        /// Events in the GUI class can be consumed so defaut behavious must be return false
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="previousMs"></param>
        /// <returns></returns>
        public virtual bool MouseMoveEvent(MouseState ms, MouseState previousMs) { return false; }
        public virtual bool MouseDownEventLeft(float mouse_x, float mouse_y) { return false; }
        public virtual bool MouseUpEventLeft(float mouse_x, float mouse_y) { return false; }
        public virtual bool MouseDownEventRight(float mouse_x, float mouse_y) { return false; }
        public virtual bool MouseUpEventRight(float mouse_x, float mouse_y) { return false; }
        public virtual bool KeyHitEvent(Keys keyHit) { return false; }
        public virtual bool MouseOver(float mouse_x, float mouse_y) { return false; } // for tool tips

    }

    // ------------------------------------------------ UC_RenderableBounded ----------------------------------

    /// <summary>
    /// This class is just a renderable class with a rectangle for bounds
    /// </summary>
    public class UC_RenderableBounded : UC_Renderable
    {

        public Rectangle bounds { get; set; }

        /// <summary>
        /// Returns true if the renderable contains the point x,y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(int x, int y)
        {
            return bounds.Contains(x, y);
        }

        /// <summary>
        /// Returns a rectangle which is the intersection of this renderable with another rectangle
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Rectangle Intersect(Rectangle r)
        {
            return Rectangle.Intersect(bounds, r);
        }

        /// <summary>
        /// Returns true if the rectange intersects this this renderable
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        bool Intersects(Rectangle r)
        {
            return bounds.Intersects(r);
        }


    }



// ------------------------------------------------ UC_RenderableList ----------------------------------------
    /// <summary>
    /// a list of renderables
    /// </summary>
    public class UC_RenderableList
    {

        public List<UC_Renderable> rlist;

        public UC_RenderableList()
        {
            rlist=new List<UC_Renderable>();
        }

        public virtual void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i].active) rlist[i].Draw(sb);
            }
        }

        public virtual void Update(GameTime gameTime)
        {            
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i].active) rlist[i].Update(gameTime);
            }
        }

        public virtual void LoadContent()
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                if (rlist[i].active) rlist[i].LoadContent();
            }
        }

        public void addToEnd(UC_Renderable r)
        {
            rlist.Add(r);
        }

        /// <summary>
        /// Adds a new renderable - make the assumption that inactive renderables 
        /// are able to be destryoyed
        /// </summary>
        /// <param name="r"></param>
        public void addReuse(UC_Renderable r)
        {
            int i = findInactive();
            if (i==-1) rlist.Add(r);
            else rlist[i] = r;
        }

        public int findInactive()
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                if (!rlist[i].active) return i;
            }
            return -1;
        }

        /// <summary>
        /// replaces returning the old value
        /// </summary>
        /// <param name="num"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public UC_Renderable replace(int num, UC_Renderable r)
        {
            UC_Renderable retv = rlist[num];
            rlist[num] = r;
            return retv;
        }

    }

// ------------------------------------------------ TextRenderable ----------------------------------------

    class TextRenderable : UC_Renderable
    {
        public string text { get; set; }
        public Vector2 pos { get; set; }
        public SpriteFont font { get; set; }
        //public Color colour { get; set; }

        public TextRenderable(string textZ, Vector2 posZ, SpriteFont fontZ, Color colourZ)
        {
            active = true;
            text = textZ;
            pos = posZ;
            font = fontZ;
            colour = colourZ;
        }

        public override void Draw(SpriteBatch sp)
        {
            sp.DrawString(font, text, pos, colour);
        }



    }

// ------------------------------------------------ TextRenderableFade --------------------------------------

    class TextRenderableFade : TextRenderable
    {
        Color finalColour;
        int ticks;
        int fadeTicks;
        public Vector2 drift;
        Vector2 curPos;
        Color curColour;

        public TextRenderableFade(string textZ, Vector2 posZ, SpriteFont fontZ, Color colourZ, Color finalColourZ, int fadeTicksZ)
            : base(textZ, posZ, fontZ, colourZ)
        {
        finalColour=finalColourZ;
        ticks=0;
        fadeTicks=fadeTicksZ;
        drift.X = 0;
        drift.Y = 0;
        curPos.X = posZ.X;
        curPos.Y = posZ.Y;
        }
        
        public override void Draw(SpriteBatch sb)
        {
            if (!active) return;
            sb.DrawString(font, text, curPos, curColour);
        }
        
        public override void Update(GameTime gameTime)
        {
            if (!active) return;
            ticks++;
            if (ticks > fadeTicks)
            {
                active = false;
                return;
            }
            curColour=Color.Lerp(colour, finalColour, (float)ticks/(float)fadeTicks);
            curPos = curPos + drift;
        }

        public override void Reset()
        {
            ticks=0;
            curPos.X = pos.X;
            curPos.Y = pos.Y;
        }
    }

// ------------------------------------------------ TextureFade --------------------------------------

    /// <summary>
    /// This renderable changes its size and colour over time
    /// When its finished depending on a user setting (in loop) it can reverse,  loop or go inactive
    /// </summary>
    class TextureFade : UC_Renderable
    {
        Color finalColour;
        Color initColour;
        Rectangle initFrame;
        Rectangle finalFrame;
        int fadeTicks;
        public int loop {get; set;} // 0=end (go inactive), 1=Loop, 2=reverse
        int ticks;
        bool reverse;
        float lerp;
        public Rectangle sourceFrame {get; set;}

        Rectangle curFrame;
        Color curColour;
        Texture2D tex;

        public TextureFade(Texture2D texZ, Rectangle initFrameZ, Rectangle finalFrameZ, Color initColourZ, Color finalColourZ, int fadeTicksZ)
            : base()
        {
            finalColour = finalColourZ;
            initColour = initColourZ;
            fadeTicks = fadeTicksZ;
            tex = texZ;
            sourceFrame = new Rectangle(0, 0, tex.Width, tex.Height);
            initFrame = initFrameZ;
            finalFrame=finalFrameZ;
            Reset();
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!active) return;
            sb.Draw(tex, curFrame,sourceFrame, curColour);
        }

        public override void Update(GameTime gameTime)
        {
            if (!active) return;
            ticks++;
            if (ticks > fadeTicks)
            {
                if (loop == 0)
                {
                    active = false;
                    return;
                }
                if (loop == 1)
                {
                    ticks=0;
                    return;
                }
                if (loop == 2)
                {
                    ticks = 0;
                    reverse = !reverse;
                    return;
                }


            }
            lerp = (float)ticks / (float)fadeTicks;
            if (reverse) lerp = 1 - lerp;

            curColour = Color.Lerp(initColour, finalColour, lerp);
            curFrame.X = (int)MathHelper.Lerp(initFrame.X, finalFrame.X, lerp);
            curFrame.Y = (int)MathHelper.Lerp(initFrame.Y, finalFrame.Y, lerp);
            curFrame.Width = (int)MathHelper.Lerp(initFrame.Width, finalFrame.Width, lerp);
            curFrame.Height = (int)MathHelper.Lerp(initFrame.Height, finalFrame.Height, lerp);
        }

        public override void Reset()
        {
            ticks = 0;
            reverse = false;
        }
    }

 // ------------------------------------------------ ScrollBackGround --------------------------------------

    class ScrollBackGround : UC_Renderable
    {
        Rectangle screenRectangle;
        Rectangle sourceRectangle;

        Rectangle source1;
        Rectangle source2;

        Rectangle screen1;
        Rectangle screen2;
        
        float scrollV;
        float scrollH;
        float scrollDelta;

        Texture2D tex;
        int direction; // 0=none 1=vertical 2=horizontal
       
        public ScrollBackGround()
        {  
        scrollDelta=1;
        colour=Color.White;
        Reset();
        }
        /// <summary>
        /// direction  0=none 1=vertical 2=horizontal
        /// </summary>
        /// <param name="texZ"></param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="delta"></param>
        /// <param name="directionZ"></param>
        public ScrollBackGround(Texture2D texZ, Rectangle source, Rectangle dest, float delta, int directionZ)
        {
        scrollDelta=delta;
        direction=directionZ;
        tex=texZ;
        screenRectangle=dest;
        sourceRectangle=source;
        colour=Color.White;
        }
        
        public override void Draw(SpriteBatch sb)
        {
            if (direction == 0) sb.Draw(tex, screenRectangle, sourceRectangle, Color.White);

            if (direction == 1) //Vertical scroll
            {
                int s = (int)(sourceRectangle.Height - scrollV);
                float ratio = (float)screenRectangle.Height/(float)sourceRectangle.Height;
                
                source1 = new Rectangle(sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height -(int)scrollV);
                screen1 = new Rectangle(screenRectangle.X, screenRectangle.Y+(int)(scrollV*ratio), screenRectangle.Width, screenRectangle.Height - (int)(scrollV*ratio));
                
                source2 = new Rectangle(sourceRectangle.X, sourceRectangle.Y+s, sourceRectangle.Width, (int)scrollV); 
                screen2 = new Rectangle(screenRectangle.X, screenRectangle.Y, screenRectangle.Width, (int)(scrollV*ratio));
                
                sb.Draw(tex, screen1, source1, colour); 
                sb.Draw(tex, screen2, source2, colour);

                //spriteBatch.Draw(tex3, new Rectangle(0, scrollV, 800, 600 - scrollV), new Rectangle(0, 0, 800, 600 - scrollV), Color.White);
                //spriteBatch.Draw(tex3, new Rectangle(0, 0, 800, scrollV), new Rectangle(0, s, 800, scrollV), Color.White);
            }

            if (direction == 2) //Horizontal scroll
            {
                int s = (int)(sourceRectangle.Width - scrollH);
                float ratio = (float)screenRectangle.Width/(float)sourceRectangle.Width;
                
                source1 = new Rectangle(sourceRectangle.X, sourceRectangle.Y, sourceRectangle.Width-(int)scrollH, sourceRectangle.Height);
                screen1 = new Rectangle(screenRectangle.X+(int)(scrollH*ratio), screenRectangle.Y, screenRectangle.Width - (int)(scrollH*ratio), screenRectangle.Height );
                
                source2 = new Rectangle(sourceRectangle.X+s, sourceRectangle.Y, (int)scrollH, sourceRectangle.Height ); 
                screen2 = new Rectangle(screenRectangle.X, screenRectangle.Y, (int)(scrollH*ratio), screenRectangle.Height);
                
                sb.Draw(tex, screen1, source1, colour); 
                sb.Draw(tex, screen2, source2, colour);
                
            }

        }

        public override void Update(GameTime gameTime)
        {
        scrollV = scrollV+scrollDelta;
        if (scrollV > sourceRectangle.Height) scrollV = 0;
        if (scrollV < 0) scrollV = sourceRectangle.Height;
            
        scrollH = scrollH+scrollDelta; 
        if (scrollH > sourceRectangle.Width) scrollH = 0;
        if (scrollH < 0) scrollH = sourceRectangle.Width;
        }

        public override void Reset()
        {
        scrollV = 0;
        scrollH = 0;
        } 
    }

    // ------------------------------------------------ ImageBackground --------------------------------------

    class ImageBackground : UC_RenderableBounded
    {
        public Texture2D tex {set; get;} 
        public Rectangle source {set; get;} 
        //public Rectangle dest {set; get;}

        public ImageBackground(Texture2D texZ, Rectangle sourceZ, Rectangle destZ, Color colourZ)
        {
            tex = texZ;
            source = sourceZ;
            if (source == null)
            {
                source = new Rectangle(0, 0, tex.Width, tex.Height);
            }
            bounds = destZ;
            colour = colourZ;
        }

        public ImageBackground(Texture2D texZ, Color colourZ, GraphicsDevice g)
        {
            tex = texZ;
            source = new Rectangle(0, 0, tex.Width, tex.Height);
            bounds = new Rectangle(0, 0, g.Viewport.Width, g.Viewport.Height); 
            colour = colourZ;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, bounds, source, colour);
        }
    }

    // --------------------- Pan and mzoom system --------------------------------------------------

    public class PanZoomStage : UC_Renderable
    {

        public Texture2D tex { set; get; }
        public Rectangle initDest { set; get; }
        public Rectangle finalDest { set; get; }
        public Rectangle initSource { set; get; }
        public Rectangle finalSource { set; get; }
        public int ticksToTransit { set; get; }
        public int cntTicksToTransit { set; get; }
        public Color initColour { set; get; }
        public Color finalColour { set; get; }
        public bool done { set; get; }

        public PanZoomStage()
        {
            tex = null;
            reset();
        }

        public void reset()
        {
            cntTicksToTransit = 0;
            done = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (cntTicksToTransit >= ticksToTransit)
            {
                done = true;
                return;
            }
            cntTicksToTransit++;
        }

        public override void Draw(SpriteBatch sb)
        {
            float lerpV = (float)cntTicksToTransit / (float)ticksToTransit;
            Rectangle src = new Rectangle((int)MathHelper.Lerp(initSource.X, finalSource.X, lerpV),
                                            (int)MathHelper.Lerp(initSource.Y, finalSource.Y, lerpV),
                                            (int)MathHelper.Lerp(initSource.Width, finalSource.Width, lerpV),
                                            (int)MathHelper.Lerp(initSource.Height, finalSource.Height, lerpV));
            Rectangle dest = new Rectangle((int)MathHelper.Lerp(initDest.X, finalDest.X, lerpV),
                                            (int)MathHelper.Lerp(initDest.Y, finalDest.Y, lerpV),
                                            (int)MathHelper.Lerp(initDest.Width, finalDest.Width, lerpV),
                                            (int)MathHelper.Lerp(initDest.Height, finalDest.Height, lerpV));
            Color c = Color.Lerp(initColour, finalColour, lerpV);
            sb.Draw(tex, dest, src, c);
        }
    }

    public class PanZoomSequence : UC_Renderable
    {
        public List<PanZoomStage> lst; // my list of transitions
        public Texture2D defaultTex { set; get; }
        public Rectangle defaultDest { set; get; }
        public Color defaultColour { set; get; }
        int currentStage;
        bool done = true;
        int ticks;

        public PanZoomSequence(Rectangle destZ, Texture2D defaultTexZ, Color defaultColorZ)
        {
            defaultTex = defaultTexZ;
            defaultDest = destZ;
            defaultColour = defaultColorZ;
            lst = new List<PanZoomStage>();
            reset();
        }

        public void reset()
        {
            currentStage = 0;
            done = false;
            ticks = 0;
        }

        public void addStage(PanZoomStage s)
        {
            lst.Add(s);
            reset();
        }

        public void addStage(Texture2D texZ, int ticksToTransitZ, Color initColourZ, Color finalColourZ,
                             Rectangle initDestZ, Rectangle finalDestZ, Rectangle initSourceZ, Rectangle finalSourceZ)
        {
            PanZoomStage p = new PanZoomStage();
            p.tex = texZ;
            p.ticksToTransit = ticksToTransitZ;
            p.initColour = initColourZ;
            p.finalColour = finalColourZ;
            p.initDest = initDestZ;
            p.finalDest = finalDestZ;
            p.initSource = initSourceZ;
            p.finalSource = finalSourceZ;
            p.reset();
            addStage(p);
        }

        public void addStage(int ticksToTransitZ, Rectangle initSourceZ)
        {
            PanZoomStage p = new PanZoomStage();
            p.tex = defaultTex;
            p.ticksToTransit = ticksToTransitZ;
            p.initColour = defaultColour;
            p.finalColour = defaultColour;
            p.initDest = defaultDest;
            p.finalDest = defaultDest;
            p.initSource = initSourceZ;
            p.finalSource = initSourceZ;
            p.reset();
            addStage(p);
        }

        public void addStage(int ticksToTransitZ, Rectangle initSourceZ, Rectangle finalSourceZ)
        {
            PanZoomStage p = new PanZoomStage();
            p.tex = defaultTex;
            p.ticksToTransit = ticksToTransitZ;
            p.initColour = defaultColour;
            p.finalColour = defaultColour;
            p.initDest = defaultDest;
            p.finalDest = defaultDest;
            p.initSource = initSourceZ;
            p.finalSource = finalSourceZ;
            p.reset();
            addStage(p);
        }


        public bool Done()
        {
            return done;
        }

        public override void Update(GameTime gameTime)
        {
            if (lst.Count == 0) return;

            PanZoomStage p = lst[currentStage];

            if (ticks == 0)
            {
                p.reset();
            }
            ticks++;
            p.Update(gameTime);
            if (p.done)
            {
                if (currentStage >= lst.Count() - 1)
                {
                    done = true;
                    return;
                }
                currentStage++;
            }

        }

        public override void Draw(SpriteBatch sb)
        {
            if (lst.Count == 0) return;

            PanZoomStage p = lst[currentStage];

            p.Draw(sb);

        }

    }


}
