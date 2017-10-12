/**************************************************************************
 * Galen Cochrane 11/20/2014
 * AlienSeeker.cs
 * 
 * The Seeker is cowardly, like the Wuss.  Maybe even more so.  But unlike
 * the Wuss, the Seeker fires a steady stream of homing projectiles
 * (whether or not it's pointed in the right direction).
 * It can't dodge, so its strategy is to bombard the player with so many
 * deadly seeking missiles (from as far away as possible) that the player
 * has no time to focus on killing it.
 * If its shields go down, it tries to escape the screen.
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    class AlienSeeker : AlienBase
    {
        GameObject target;                  // the target for the homing missiles - usually the player
        Bitmap shieldImg;                   // the image for the shields
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        public AlienSeeker(GameObject target)
            : base("alienSeekerPIC", Utilities.screenBounds.Width, 400, 1, 100, (float)Math.PI / 6)
        {
            CanDodge = false;
            IsCoward = true;
            CowardAngle = (float)Math.PI / 2;
            this.target = target;
            shieldImg = (Bitmap)Image.FromFile(GameMedia.getDir["shieldPIC"]);
            PointValue = 2000;
        }
        /// <summary>
        /// overridden logic allows the Seeker to fire a constand barrage of homing missiles
        /// </summary>
        /// <param name="player"></param>
        protected override void alienLogic(PlayerShip player)
        {
            base.alienLogic(player);
            fire();
            if (ShieldLevel < 1)
            {
                Escaping = true;
                PreferredMinDist = Utilities.screenBounds.Width;
            }
        }
        /// <summary>
        /// what happens when the Seeker fires
        /// </summary>
        public override void fire()
        {
            if (BulletCoolDown == 0)
            {
                Bullets.Add(new SeekingBullet(Position.X - Settings.bulletWidth / 2, Position.Y - Settings.bulletHeight / 2, Velocity.X, Velocity.Y, Rotation, "laser3PIC", target));
                BulletCoolDown = 150;
                base.fire();
                try { GameSounds.effect(GameMedia.getDir["laser2SND"]); }
                catch { }
            }
        }
        /// <summary>
        /// overridden draw method allows for the drawing of the thruster fire
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
                        Initialg.DrawImage(shieldImg, new Point(-(int)(shieldImg.Width / 2 + 8), -(int)(shieldImg.Height / 2 + 9)));
                    }
                    if (Accelerating)
                    {
                        Initialg.DrawEllipse(new Pen(Color.PaleTurquoise, 3), -6, 15, 12, 5);
                        Initialg.FillEllipse(new SolidBrush(Color.Violet), -3, 15, 6, 20);
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
