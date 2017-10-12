/**************************************************************************
 * Galen Cochrane 11/20/2014
 * AlienHunter.cs
 * 
 * The Hunter is aggressive, fearless, and deadly accurate.  It lives by
 * the philosophy that the best defense is a good offense, and so has
 * no shields.  Instead, it sports dual cannons and a rapid rate of fire.
 * It can dodge, but it doesn't beleive in flanking (or running)
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    class AlienHunter : AlienBase
    {
        Coordinates GunRight, GunLeft;      // the Hunter has two guns, these are their object relative positions.
        /// <summary>
        /// Constructor
        /// </summary>
        public AlienHunter()
            : base("alienHunterPIC", (int)(Settings.bulletLife * Settings.bulletSpeed), 200, 0, 100, (float)Math.PI / 32)
        {
            CanDodge = true;
            IsCoward = false;
            GunLeft = new Coordinates();
            GunRight = new Coordinates();
            PointValue = 3000;
        }
        /// <summary>
        /// this is what happens when the Hunter's underlying logic decides to fire weapons
        /// </summary>
        public override void fire()
        {
            if (BulletCoolDown == 0)
            {
                GunLeft.setVector(Position);
                GunLeft.addVector(8, RadRotation);
                GunRight.setVector(Position);
                GunRight.addVector(-8, RadRotation);
                Bullets.Add(new NormalBullet(GunLeft.X - Settings.bulletWidth / 2, GunLeft.Y - Settings.bulletHeight / 2, Velocity.X, Velocity.Y, Rotation, "laser2PIC"));
                Bullets.Add(new NormalBullet(GunRight.X - Settings.bulletWidth / 2, GunRight.Y - Settings.bulletHeight / 2, Velocity.X, Velocity.Y, Rotation, "laser2PIC"));
                BulletCoolDown = 120;
                base.fire();
                try { GameSounds.effect(GameMedia.getDir["laser2SND"]); }
                catch { }
            }
        }
        /// <summary>
        /// overridden draw method allows for thruster fire to be drawn
        /// </summary>
        /// <param name="Initialg"></param>
        public override void DrawObj(Graphics Initialg)
        {
            base.DrawObj(Initialg);
            if (!IsDying)
            {
                if (Accelerating)
                {
                    Initialg.TranslateTransform((int)Position.X, (int)Position.Y);
                    Initialg.RotateTransform((float)Rotation); //degrees                    
                    if (Accelerating)
                    {
                        Initialg.DrawEllipse(new Pen(Color.Honeydew, 3), -6, 15, 12, 5);
                        Initialg.FillEllipse(new SolidBrush(Color.Yellow), -3, 15, 6, 20);
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
