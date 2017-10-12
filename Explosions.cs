/********************************************************************
 * Ted Delezene 
 * Explosions.cs
 * This class handles explosions (all objects when destroyed will
 * have the same explosion animation.)
 * 
 * Slightly modified by:
 * Galen Cochrane 11/20/2014
 * This class remains mostly in the same state that I got it.
 * I thought the explosions were pretty good already.
 ******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    class Explosions
    {
        private Bitmap imgBoom = null;       
        private int iBoomTotal = 15;
        private int iBoomSquence = 0;
        private bool FinishedExploding = true;

        /// <summary>
        /// This function draws the explosion in multiple steps, then returns whether it is finished or not.
        /// </summary>
        /// <param name="initialG">Where the explosion will be drawn (graphics object)</param>
        /// <param name="Img">the rectangle in which the explosion will be drawn.</param>
        /// <returns></returns>
        public bool ExplosionDraw(Graphics initialG, Rectangle Img)
        {
            Rectangle rectPath = Img;

            //create or importimage for explosion

            imgBoom = (Bitmap)Image.FromFile(GameMedia.getDir["explosionPIC"] + (iBoomSquence / 4 + 1) + ".png");
            imgBoom.MakeTransparent(Color.Black);
            initialG.DrawImage(imgBoom, rectPath, 0, 0, imgBoom.Width, imgBoom.Height, GraphicsUnit.Pixel);

            iBoomSquence++;
            if (iBoomSquence > iBoomTotal * 4)
                FinishedExploding = false;

            return (FinishedExploding);
        }
    }
}
