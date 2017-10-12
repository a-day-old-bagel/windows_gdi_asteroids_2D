/**************************************************************************
 * Galen Cochrane 11/20/2014
 * AlienWuss.cs
 * 
 * The Wuss is by far the most common foe and the easiest to defeat.
 * It can flank, but it can't dodge.
 * It spends most of its time trying not to die, and occasionally shoots
 * at the player (though its shots mostly go wide)
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    class AlienWuss : AlienBase
    {
        Bitmap shieldImg;               // the image for the shields
        /// <summary>
        /// Constructor
        /// </summary>
        public AlienWuss() : base("alienStockPIC", (int)(Settings.bulletLife * Settings.bulletSpeed), 300, 1, 100, (float)Math.PI / 6)
        {
            CanDodge = false;
            IsCoward = true;
            shieldImg = (Bitmap)Image.FromFile(GameMedia.getDir["shieldPIC"]);
            PointValue = 1000;
        }
        /// <summary>
        /// what happens when the Wuss fires
        /// </summary>
        public override void fire()
        {
            if (BulletCoolDown == 0)
            {
                Bullets.Add(new NormalBullet(Position.X - Settings.bulletWidth / 2, Position.Y - Settings.bulletHeight / 2, Velocity.X, Velocity.Y, Rotation, "laser1PIC"));
                BulletCoolDown = 150;
                base.fire();
                try { GameSounds.effect(GameMedia.getDir["laser1SND"]); }
                catch { }
            }
        }
        /// <summary>
        /// overridden draw method allows for the drawing of the thruster fire.
        /// </summary>
        /// <param name="Initialg"></param>
        public override void DrawObj(Graphics Initialg)
        {
            base.DrawObj(Initialg);
            if (!IsDying)
            {
                if (ShieldLevel > 0 || Accelerating)
                {
                    Initialg.TranslateTransform((int)Position.X, (int)Position.Y);
                    Initialg.RotateTransform((float)Rotation); //degrees
                    if (ShieldLevel > 0)
                    {
                        Initialg.DrawImage(shieldImg, new Point(-(int)(shieldImg.Width / 2 + 9), -(int)(shieldImg.Height / 2 + 8)));
                    }
                    if (Accelerating)
                    {
                        Initialg.DrawEllipse(new Pen(Color.Beige, 3), -19, 15, 12, 5);
                        Initialg.FillEllipse(new SolidBrush(Color.DarkOrange), -16, 15, 6, 20);
                        Initialg.DrawEllipse(new Pen(Color.Beige, 3), 7, 15, 12, 5);
                        Initialg.FillEllipse(new SolidBrush(Color.DarkOrange), 10, 15, 6, 20);
                        Accelerating = false;
                    }
                    Initialg.ResetTransform();
                }
            }
            else
            {
                IsActive = Boom.ExplosionDraw(Initialg, new Rectangle((int)(this.Position.X - (this.Dimensions.X + Settings.explodeSizeAdd) / 2), (int)(this.Position.Y - (this.Dimensions.Y + Settings.explodeSizeAdd) / 2), (int)(this.Dimensions.X + Settings.explodeSizeAdd), (int)(this.Dimensions.Y + Settings.explodeSizeAdd)));
                if (HasBoomed == false)
                {
                    GameSounds.effect(GameMedia.getDir["explosion1SND"]);
                    HasBoomed = true;
                }
            }
        }
    }
}
