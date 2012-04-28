// Linebatch.cs
// originally written by David Amador and made available on 26th jan 2010 on http://www.david-amador.com/2010/01/drawing-lines-in-xna/
// Modified by R.Cox March 2011 for use in UC_Framework
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OdinsRevenge
{
    /// <summary>
    /// Line Batch
    /// For drawing lines in a spritebatch
    /// </summary>
    static public class LineBatch
    {
        static private Texture2D _empty_texture;
        static private bool      _set_data = false;

        static public void init(GraphicsDevice device)
        {
            _empty_texture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
        }

        static public void drawLine(SpriteBatch batch, Color color,
                                    Vector2 point1, Vector2 point2)
        {

            drawLine(batch, color, point1, point2, 0);
        }

        static public void drawLine(SpriteBatch batch, float x1, float y1, float x2, float y2, Color color)
        {

            drawLine(batch, color, new Vector2(x1,y1), new Vector2(x2,y2), 0);
        }

        static public void drawLineRectangle(SpriteBatch batch, Rectangle r, Color c)
        {
            drawLine(batch, c, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y), 0);
            drawLine(batch, c, new Vector2(r.X, r.Y), new Vector2(r.X , r.Y+r.Height), 0);
            drawLine(batch, c, new Vector2(r.X+ r.Width, r.Y), new Vector2(r.X + r.Width, r.Y+r.Height), 0);
            drawLine(batch, c, new Vector2(r.X, r.Y+r.Height), new Vector2(r.X + r.Width, r.Y+r.Height), 0);
        }

        static public void drawCrossX(SpriteBatch batch, float x1, float y1, float size, Color c1, Color c2)
        {
            drawLine(batch, c1, new Vector2(x1 - size, y1 - size), new Vector2(x1+size, y1+size), 0);
            drawLine(batch, c2, new Vector2(x1 + size, y1 - size), new Vector2(x1-size, y1+size), 0);
        }

        static public void drawCross(SpriteBatch batch, float x1, float y1, float size, Color c1, Color c2)
        {
            drawLine(batch, c1, new Vector2(x1 , y1 - size), new Vector2(x1 , y1+size), 0);
            drawLine(batch, c2, new Vector2(x1 + size, y1), new Vector2(x1-size, y1), 0);
        }

        static public void drawRect4(SpriteBatch sb, Rect4 r, Color c)
        {
            drawLine(sb, c, new Vector2(r.point[0].X, r.point[0].Y), new Vector2(r.point[1].X, r.point[1].Y), 0);
            drawLine(sb, c, new Vector2(r.point[1].X, r.point[1].Y), new Vector2(r.point[2].X, r.point[2].Y), 0);
            drawLine(sb, c, new Vector2(r.point[2].X, r.point[2].Y), new Vector2(r.point[3].X, r.point[3].Y), 0);
            drawLine(sb, c, new Vector2(r.point[3].X, r.point[3].Y), new Vector2(r.point[0].X, r.point[0].Y), 0);
        }

        //static public void drawLine(SpriteBatch batch, float X1, float Y1, float X2, float Y2, Color cLine)
        //{
        //    DrawLine(batch, cLine, new Vector2(X1,Y1), new Vector2(X2,Y2), 0);
        //}

        /// <summary>
        /// Draw a line into a SpriteBatch
        /// </summary>
        /// <param name="batch">SpriteBatch to draw line</param>
        /// <param name="color">The line color</param>
        /// <param name="point1">Start Point</param>
        /// <param name="point2">End Point</param>
        /// <param name="Layer">Layer or Z position</param>
        static public void drawLine(SpriteBatch batch, Color color, Vector2 point1,
                                    Vector2 point2, float Layer)
        {
            //Check if data has been set for texture
            //Do this only once otherwise
            if (!_set_data)
            {
                //_empty_texture = new Texture2D();
                _empty_texture.SetData(new[] { Color.White });
                _set_data = true;
            }


            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = (point2 - point1).Length();

            batch.Draw(_empty_texture, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, 1),
                       SpriteEffects.None, Layer);
        }
    }
}