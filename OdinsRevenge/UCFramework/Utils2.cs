//Utils2.cs
using System;
using System.IO;
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
    /// <summary>
    /// This is a class that is just 4 points.
    /// The points usually refer to a rotated rectange, or polygon, but not necessarily;
    /// </summary>
    public class Rect4
    {
        /// <summary>
        /// The data in the 4 points class called Rect4
        /// </summary>
        public Vector2[] point;

        /// <summary>
        /// Default constructor (0,0) (0,0) (0,0) (0,0)</summary>
        public Rect4()
        {
            point = new Vector2[4];
            for (int i=0; i<4; i++) 
            {
                point[i].X=0;
                point[i].Y=0;
            }
        }

        /// <summary>
        /// Construct from rectangle clockwise winding</summary>
        public Rect4(Rectangle r)
        {
            point = new Vector2[4];
            point[0].X = r.Left;
            point[0].Y = r.Top;

            point[1].X = r.Right;
            point[1].Y = r.Top;

            point[2].X = r.Right;
            point[2].Y = r.Bottom;

            point[3].X = r.Left;
            point[3].Y = r.Bottom;
        }

        Rect4(Rect4 r) // Copy Constructor
        {
            point = new Vector2[4];
            point[0].X = r.point[0].X;
            point[0].Y = r.point[0].Y;

            point[1].X = r.point[1].X;
            point[1].Y = r.point[1].Y;

            point[2].X = r.point[2].X;
            point[2].Y = r.point[2].Y;

            point[3].X = r.point[3].X;
            point[3].Y = r.point[3].Y;
        }
       
        /// <summary>
        /// Rotates the rect4 by a given angle in radians 
        /// </summary>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInRadians"></param>
        public void rotateRect(Vector2 centerOfRotation,float angleInRadians)
        {
            point[0]=Util.rotatePoint(point[0], centerOfRotation, -angleInRadians);
            point[1]=Util.rotatePoint(point[1], centerOfRotation, -angleInRadians);
            point[2]=Util.rotatePoint(point[2], centerOfRotation, -angleInRadians);
            point[3]=Util.rotatePoint(point[3], centerOfRotation, -angleInRadians);
        }

        /// <summary>
        /// Rotates the rect4 by a given angle in degrees 
        /// </summary>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInDegrees"></param>
        public void rotateRectDeg(Vector2 centerOfRotation, float angleInDegrees)
        {
            rotateRect(centerOfRotation, angleInDegrees * (float)Math.PI / 180);
        }

        /// <summary>
        /// This returns an axis aligned bounding box based on the four corners of Rect4.
        /// The points should be a convex polygon, but this routine will work in all cases
        /// (note it can probably be done faster using the Max and Min functions but it deliberately this way so students can understand it)
        /// </summary>        
        public Rectangle getAABoundingRect()
        {
            float Top = point[0].Y;
            float Left = point[0].X;
            float Bottom=point[0].Y;
            float Right=point[0].X;

            if (point[1].X < Left) Left = point[1].X;
            if (point[2].X < Left) Left = point[2].X;
            if (point[3].X < Left) Left = point[3].X;

            if (point[1].Y < Top) Top = point[1].Y;
            if (point[2].Y < Top) Top = point[2].Y;
            if (point[3].Y < Top) Top = point[3].Y;

            if (point[1].X > Right) Right = point[1].X;
            if (point[2].X > Right) Right = point[2].X;
            if (point[3].X > Right) Right = point[3].X;

            if (point[1].Y > Bottom) Bottom = point[1].Y;
            if (point[2].Y > Bottom) Bottom = point[2].Y;
            if (point[3].Y > Bottom) Bottom = point[3].Y;

            // now have bounds in Top, left bottomm and right - covert to rectangle

            return new Rectangle((int)Left,(int)Top,(int)(Right-Left),(int)(Bottom-Top));
        }
    }

    // ********************************************** WayPoint ******************************************* //

    /// <summary>
    ///  a single waypoint
    /// </summary>
    public class WayPoint
    {
        /// <summary>
        /// Its position
        /// </summary>
        public Vector2 pos;

        /// <summary>
        /// how ast to go
        /// </summary>
        public float speed;

        public WayPoint(Vector2 posZ, float speedZ)
        {
            pos = posZ;
            speed = speedZ;
        }

        public WayPoint(float x, float y, float speedZ)
        {
            pos = new Vector2(x,y);
            speed = speedZ;
        }
    }

    // ********************************************** WayPointList ******************************************* //

    /// <summary>
    /// A class for creating and managing waypoints
    /// </summary>
    public class WayPointList
    {
        public List<WayPoint> lst; 
        public int currentLeg;

        /// <summary>
        /// +1 = forward -1 = backward
        /// </summary>
        public int dir { set; get; }

        /// <summary>
        // 0=return false to nextLeg() but loop back to start
        // 1=return false to nextLeg() but reverse direction
        // 2=set invisible / inactive 
        /// </summary>
        public int wayFinished {set; get;} 
        
        /// <summary>
        /// construct an empty waypoint list
        /// </summary>
        public WayPointList()
        {
            wayFinished = 0;
            reset();
        }

        /// <summary>
        /// Construct a 2 position waypoint list 
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <param name="wayFinishedZ">you probably want this to be 1 ro get back forward actions</param>
        public WayPointList(WayPoint w1, WayPoint w2, int wayFinishedZ)
        {
            wayFinished = wayFinishedZ;
            reset();
            add(w1);
            add(w2);
        }

        /// <summary>
        /// Construct a 3 position walpoint list
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <param name="w3"></param>
        /// <param name="wayFinishedZ"></param>
        public WayPointList(WayPoint w1, WayPoint w2, WayPoint w3, int wayFinishedZ)
        {
            wayFinished = wayFinishedZ;
            reset();
            add(w1);
            add(w2); 
            add(w3);
        }


        /// <summary>
        /// rerst the list to none
        /// </summary>
        public void reset()
        {            
            lst = new List<WayPoint>();
            currentLeg = 0;
            dir = 1;
        }

        /// <summary>
        /// returns the index for subsequent use as leg
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public int add(WayPoint w)
        {
            lst.Add(w);
            return lst.Count() - 1;
        }

        /// <summary>
        /// returns the index for subsequent use as leg
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="speedZ"></param>
        /// <returns></returns>
        public int add(float x, float y, float speedZ)
        {
            lst.Add(new WayPoint(x,y,speedZ));
            return lst.Count() - 1;
        }

        public WayPoint currentWaypoint()
        {
            return lst[currentLeg];
        }

        public int getCurrentLeg()
        {
            return currentLeg;
        }

        public WayPoint getWayPoint(int i)
        {
            return lst[i];
        }

        /// <summary>
        /// returns false if the next leg is the end  
        /// </summary>
        /// <returns></returns>
        public bool nextLeg()
        {
            // 0=return false to nextLeg() but loop back to start
            // 1=return false to nextLeg() but reverse direction
  
            currentLeg=currentLeg+dir;
            if (currentLeg >= lst.Count())
            {
                if (wayFinished == 0)
                {
                    currentLeg = 0;
                    return false;
                }
                if (wayFinished >= 1)
                {
                    dir = dir * -1;
                    currentLeg = lst.Count()-1;
                    return false;
                }
            }

            if (currentLeg < 0)
            {
                if (wayFinished == 0)
                {
                    currentLeg = lst.Count()-1;
                    return false;
                }
                if (wayFinished >= 1)
                {
                    dir = dir * -1;
                    currentLeg = 0;
                    return false;
                }
            }
            return true;
        }

        public void setLeg(int i)
        {
            currentLeg = i;
        }
    
    
        /// <summary>
        /// Construct a circular path of waypoints 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="startAngle"></param>
        /// <param name="angleIncrement"></param>
        /// <param name="steps"></param>
        /// <param name="speed"></param>
        /// <param name="Xscale">usually 1 for a circle, vary from 1 to creat ovals</param>

        public void makePathCircle(Vector2 center, float radius, float startAngle, float angleIncrement, int steps, float speed, float Xscale)
        {
            lst = new List<WayPoint>();

            for (int i = 0; i < steps; i++)
            {
                Vector2 v = Util.moveByAngleDist(new Vector2(0, 0), startAngle + (i * angleIncrement), radius);
                v.X = v.X * Xscale;
                v = v + center;
                WayPoint w = new WayPoint(v, speed);
                add(w);
            }
        }

        /// <summary>
        /// Makes a zig zag path
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="zigZag"></param>
        /// <param name="steps"></param>
        /// <param name="speed"></param>
        public void makePathZigZag(Vector2 startPos, Vector2 endPos, Vector2 zigZag, int steps, float speed)
        {
            lst = new List<WayPoint>();
            float zig = 1;
            for (int i = 0; i < steps; i++)
            {
                Vector2 v=new Vector2(0,0);
                float lerp = (float)i / ((float)steps-1);
                v.X = MathHelper.Lerp(startPos.X, endPos.X, lerp);
                v.Y = MathHelper.Lerp(startPos.Y, endPos.Y, lerp);
                v = v + (zig * zigZag);
                WayPoint w = new WayPoint(v, speed);
                add(w);
                zig = zig * (-1);
            }
            wayFinished = 1; // typically you want this
        }

        /// <summary>
        /// Make a parabolic path changing velocity by delta to -/+ limits in 
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="initVelocity"></param>
        /// <param name="delta"></param>
        /// <param name="speedLimit"></param>
        /// <param name="limits"></param>
        public void makePathDelta(float speed, int maxSteps, Vector2 startPos, Vector2 initVelocity, Vector2 delta, Rectangle limits)
        {
            lst = new List<WayPoint>();
            Vector2 p = new Vector2(startPos.X, startPos.Y);  
            Vector2 v = new Vector2(initVelocity.X, initVelocity.Y);
            WayPoint w = new WayPoint(p, speed);
            add(w);
            for (int i = 0; i < maxSteps - 1; i++)
            {
                //Vector2 v = new Vector2(p.X+v.X,p.Y+v.X);
                p=p+v;
                v = v + delta;
                w = new WayPoint(p, speed);
                add(w);
                if (!limits.Contains((int)p.X, (int)p.Y))
                {
                    break;
                }
            }
            wayFinished = 2; // typically you want this
        }


        /// <summary>
        /// draw it so it helps
        /// </summary>
        /// <param name="r"></param>
        /// <param name="cPoints"> color of points (or null if no points wanted)</param>
        /// <param name="cLines"> color of lines (or null if no lines wanted)</param>
        public void Draw(SpriteBatch sb, Color cPoints, Color cLines, bool callBegin)
        {
            if (callBegin) sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            for (int i = 0; i < lst.Count(); i++)
            {
                WayPoint w = lst[i];
                if (cPoints != null) LineBatch.drawCrossX(sb, w.pos.X, w.pos.Y, 5, cPoints, cPoints);
                if (cLines != null && i > 0) 
                {
                    WayPoint ww = lst[i-1];
                    LineBatch.drawLine(sb, ww.pos.X, ww.pos.Y, w.pos.X, w.pos.Y, cLines);
                }
            }
            if (callBegin) sb.End();
        }

    }

    // *********************************** LimitSound ******************************************************

    /// <summary>
    /// Limits a sound to playing a certian number of instances of itself (stops muddy sound)
    /// </summary>
    public class LimitSound
    {
        SoundEffect se;
        SoundEffectInstance[] sei;
        int numOfSounds;
        int counter;

        /// <summary>
        /// Create it using a soundeffect
        /// </summary>
        /// <param name="soundEffect"></param>
        /// <param name="numOfSoundz"></param>
        public LimitSound(SoundEffect soundEffect, int numOfSoundz)
        {
            numOfSounds = numOfSoundz;
            se=soundEffect;
            init();
        }

        /// <summary>
        /// create it using a sound name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numOfSoundz"></param>
        public LimitSound(string name, int numOfSoundz, ContentManager c)
        {
            se= c.Load<SoundEffect>(name);
            numOfSounds = numOfSoundz;
            init();
        }

        private void init()
        {
            counter = 0;
            sei = new SoundEffectInstance[numOfSounds];
            for (int i = 0; i < numOfSounds; i++)
            {
                sei[i] = se.CreateInstance();
            }
        }

        /// <summary>
        /// play the sound from the start
        /// </summary>
        public void playSound()
        {
        // first find one not playing
        for (int i = 0; i < numOfSounds; i++)
            {
                if (sei[i].State == SoundState.Stopped)
                {
                sei[i].Play();
                return;
                }
            }
        // ok no free slots kill one that is playing
        sei[counter].Stop();
        sei[counter].Play();
        counter++;
        if (counter >= numOfSounds) counter = 0;
        }
    }

    /// <summary>
    /// Just a utility class for staic common usefull methods
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Rotate a single point about an arbitay center radians
        /// </summary>
        /// <param name="point"></param>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInRadians"></param>
        /// <returns></returns>
        public static Vector2 rotatePoint(Vector2 point, Vector2 centerOfRotation, float angleInRadians)
        {
            float tmpx, tmpy, tx, ty; // more temporaries than we really need but its very clear how it works with them
            Vector2 retv; // new value

            /* set to origin */
            tmpx = point.X - centerOfRotation.X;
            tmpy = point.Y - centerOfRotation.Y;

            // apply rotate
            tx = (tmpy * (float)Math.Sin(angleInRadians)) + (tmpx * (float)Math.Cos(angleInRadians));
            ty = (tmpy * (float)Math.Cos(angleInRadians)) - (tmpx * (float)Math.Sin(angleInRadians));

            retv.X = tx + centerOfRotation.X;
            retv.Y = ty + centerOfRotation.Y;
            return retv;
        }

        /// <summary>
        /// Rotate a single point about an arbitay center in degrees
        /// </summary>
        /// <param name="point"></param>
        /// <param name="centerOfRotation"></param>
        /// <param name="angleInDegrees"></param>
        /// <returns></returns>
        public static Vector2 rotatePointDeg(Vector2 point, Vector2 centerOfRotation, float angleInDegrees)
        {
            return rotatePoint(point, centerOfRotation, (float)(angleInDegrees * Math.PI / 180));

        }

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="angleInDegrees"></param>
        /// <returns></returns>
        public static float degToRad(float angleInDegrees) { return angleInDegrees * (float)Math.PI / 180; }

        /// <summary>
        /// convert radians to degrees
        /// </summary>
        /// <param name="angleInRadians"></param>
        /// <returns></returns>
        public static float radToDeg(float angleInRadians) { return angleInRadians * 180 / (float)Math.PI; }

        /// <summary>
        /// Move a single spot in x/y plane by a given distance at a given angle
        /// </summary>
        /// <param name="posZ"></param>
        /// <param name="angleInRadians"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector2 moveByAngleDist(Vector2 posZ, float angleInRadians, float distance)
        {
            Vector2 retv = posZ + (distance * new Vector2((float)Math.Cos(angleInRadians), (float)Math.Sin(angleInRadians)));
            return retv;
        }

        /// <summary>
        /// gets the angle in radians between p1 and p2
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float getAngle(Vector2 p1, Vector2 p2)
        {
            return (float)(Math.Atan2(p1.Y - p2.Y, p1.X - p2.X));
        }

        /// <summary>
        /// This will replace one colou with another in a 'Color' Format texture
        /// These apear to be the format read when a png 32 bit file is loaded
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="cToReplace"></param>
        /// <param name="cToWrite"></param>
        /// <returns></returns>
        public static int ChangeColourInTexturePNG(Texture2D tex, Color cToReplace, Color cToWrite)
        {
        uint[] pixelData;
        int i = 0;
        //int x,y = 0;
        //Color col;
        uint temp;
        uint c1;
        uint c2;
        
        byte[] cc1=new byte[4];
        byte[] cc2=new byte[4];
 
        if (tex.Format != SurfaceFormat.Color) return 0; // throwing an exception might be a better idea than return
        //Color format is ARGB - in paint.net you need to select 32bit when saving

        cc1[3]=cToReplace.A;
        cc1[2]=cToReplace.R;
        cc1[1]=cToReplace.G;
        cc1[0]=cToReplace.B;
        c1=BitConverter.ToUInt32(cc1,0);

        cc2[3] = cToWrite.A;
        cc2[2] = cToWrite.R;
        cc2[1] = cToWrite.G;
        cc2[0] = cToWrite.B;
        c2=BitConverter.ToUInt32(cc2,0);

        pixelData  = new uint[tex.Width * tex.Height];
        tex.GetData(pixelData, 0, tex.Width * tex.Height);

        for (int xx = 0; xx < tex.Width; xx++)
        {
            for (int yy = 0; yy <tex.Height; yy++)
            {
            temp=pixelData[xx+yy*tex.Width];
            if (temp == c1) pixelData[xx+yy*tex.Width]=c2;
            }
        }
        tex.SetData(pixelData);
        return i;
        }

        ///// <summary>
        ///// converts a bgr32 format texture to a color format texture
        ///// typically this is a bmp to png conversion in laymans terms
        ///// if in doubt make levels = 1
        ///// </summary>
        ///// <param name="gd"></param>
        ///// <param name="tex"></param>
        ///// <param name="AlphaVal"></param>
        ///// <param name="levels"></param>
        ///// <returns></returns>
        //public static Texture2D makeFormatColor(GraphicsDevice gd, Texture2D tex, byte AlphaVal, int levels)
        //{
        //    Texture2D retv = new Texture2D(gd, tex.Width, tex.Height, levels, TextureUsage.AutoGenerateMipMap, SurfaceFormat.Color);
            
        //    if (tex.Format== SurfaceFormat.Bgr32)
        //    {            
        //        uint[] pixelData = new uint[tex.Width * tex.Height];
        //        tex.GetData(pixelData, 0, tex.Width * tex.Height);

        //        for (int xx = 0; xx < tex.Width; xx++)
        //        {
        //            for (int yy = 0; yy < tex.Height; yy++)
        //            {
        //                uint temp;
        //                uint c2;
        //                byte[] cc1 = new byte[4];
        //                byte[] cc2 = new byte[4];
        //                temp = pixelData[xx + yy * tex.Width];
        //                cc1 = BitConverter.GetBytes(temp);
        //                //cc1[3] = unused
        //                //cc1[2] = r
        //                //cc1[1] = g
        //                //cc1[0] = b

        //                cc2[3] = AlphaVal; // a
        //                cc2[2] = cc1[2]; // r
        //                cc2[1] = cc1[1]; // g
        //                cc2[0] = cc1[0]; // b
        //                c2=BitConverter.ToUInt32(cc2,0);
        //                pixelData[xx + yy * tex.Width] = c2;
        //            }
        //        }
        //        retv.SetData(pixelData);
        //        return retv;
        //    }
        //    return tex; // unchanged - not right format
        //}

        ///// <summary>
        ///// Could work - if it does it will load a bmp - convert it Color format and add an alpha channel
        ///// </summary>
        ///// <param name="gd"></param>
        ///// <param name="fileName"></param>
        ///// <param name="transColor"></param>
        ///// <returns></returns>
        //public static Texture2D loadBmpWithTrans(GraphicsDevice gd, String fileName, Color transColor)
        //{
        //    Texture2D tempTex = Texture2D.FromFile(gd, fileName);
        //    Texture2D retv = Util.makeFormatColor(gd, tempTex, 255, 1);
        //    Util.ChangeColourInTexturePNG(retv, transColor, Color.TransparentBlack);
        //    return retv;
        //}

        public static Texture2D texFromFile(GraphicsDevice gd, String fName)
        {
            // note needs :using System.IO;
            Stream fs = new FileStream(fName, FileMode.Open);
            Texture2D rc = Texture2D.FromStream(gd, fs);
            fs.Close();
            return rc;
        }


        //public Vector2 closest_point_on_segment_AB_to_point_P(Vector2 A, Vector2 B, Vector2 P)
        //{
        //// code from Raymond_porter420 gamedev.net - hacked by RC 
        //Vector U = B-A;

        //float t = Vedot(P-A,U) / dot(U,U);

        //if ( t < 0 ) t = 0; if ( t > 1 ) t = 1;

        //return A + U * t;
        //}

        //public float PointDistanceFromLine(Vector2 A, Vector2 B, Vector2 P)
        //{
        //// code from Raymond_porter420 gamedev.net - hacked by RC 
        //Vector2 Pt = closest_point_on_segment_AB_to_point_P(A, B, P);

        //return (P-Pt).Length();
        //}
        //}

    }
}

// end utils2.cs
