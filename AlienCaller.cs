/**************************************************************************
 * Galen Cochrane 11/20/2014
 * AlienCaller.cs
 * 
 * The Caller can both dodge and flank, but only flanks once its shields
 * are down.  It has the special ability to call in reinforcements and
 * recharge its own shields.  It is not very powerful offensively.
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    class AlienCaller : AlienBase
    {
        int abilityCoolDown;            // the Caller has a special ability to call in other aliens and recharge its own shields
        List<AlienBase> allies;         // the list of active aliens, passed from mainGame
        GameObject seekerTarget;        // the target to give alien Seekers should they come to help
        Bitmap shieldImg;               // the image of the shield
        Bitmap callSigImg;              // the image that appears when the Caller calls in other aliens
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="allies">list of active aliens</param>
        /// <param name="target">target to provide for allied Seekers - usually the player's ship</param>
        public AlienCaller(List<AlienBase> allies, GameObject target)
            : base("alienCallerPIC", 800, 300, 1, 100, (float)Math.PI / 8)
        {
            CanDodge = true;
            IsCoward = false;
            abilityCoolDown = 3000;
            BulletCoolDown = 400;
            this.allies = allies;
            seekerTarget = target;
            shieldImg = (Bitmap)Image.FromFile(GameMedia.getDir["shieldPIC"]);
            callSigImg = (Bitmap)Image.FromFile(GameMedia.getDir["callSigPIC"]);
            PointValue = 4000;
        }
        /// <summary>
        /// overridded behavior routine allows the Caller to change to a 'coward' when it's shields go down.
        /// </summary>
        /// <param name="player">the player's ship</param>
        protected override void alienLogic(PlayerShip player)
        {
            base.alienLogic(player);
            abilityCoolDown--;
            if (abilityCoolDown == 195)
                GameSounds.effect(GameMedia.getDir["callSigSND"]);
            if (abilityCoolDown <= 0)
            {
                if (ShieldLevel < 1)
                {
                    ShieldLevel++;
                    IsCoward = false;
                    try { GameSounds.effect(GameMedia.getDir["shieldsUpSND"]); }
                    catch { }
                }
                callAllies();
                abilityCoolDown = 3000;
            }
        }
        /// <summary>
        /// special ability to call in reinforcements
        /// </summary>
        private void callAllies()
        {
            switch(Utilities.rand.Next(8))
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    allies.Add(new AlienWuss());
                    break;
                case 4:
                case 5:
                    allies.Add(new AlienHunter());
                    break;
                case 6:
                case 7:
                    allies.Add(new AlienSeeker(seekerTarget));
                    break;
            }
        }
        /// <summary>
        /// what happens when the Caller fires its main weapon
        /// </summary>
        public override void fire()
        {
            if (BulletCoolDown == 0)
            {
                Bullets.Add(new NormalBullet(Position.X - Settings.bulletWidth / 2, Position.Y - Settings.bulletHeight / 2, Velocity.X, Velocity.Y, Rotation, "laser1PIC"));
                BulletCoolDown = 400;
                base.fire();
                try { GameSounds.effect(GameMedia.getDir["laser1SND"]); }
                catch { }
            }
        }
        /// <summary>
        /// overridden draw method allows for the drawing of thruster fire
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
                        Initialg.DrawImage(shieldImg, new Point(-(int)(shieldImg.Width / 2 + 8), -(int)(shieldImg.Height / 2 + 7)));
                    }
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
