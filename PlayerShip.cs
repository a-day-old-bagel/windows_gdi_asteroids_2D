/***************************************************************************
 * Galen Cochrane 11/20/2014
 * PlayerShip.cs
 * Based on:
 * 
 * Ted Delezene 11/17/2012
 * SpaceShip.cs
 * This class defines the player controlled spaceship, it contains methods
 * for movement, attack, and rotation. 
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Asteroids2._0
{
    class PlayerShip : GameObject
    {
        #region Fields

        private List<Weapon> bullets;
        int bulletCoolDown;
        int warpCoolDown;
        private float thrust;
        private float turnRatio;
        private bool accelerating;
        private bool hasBoomed;
        private Explosions boom;

        #endregion

        #region Properties

        public List<Weapon> Bullets { get { return bullets; } set { bullets = value; } }
        public float Thrust { get { return thrust; } set { thrust = value; } }
        public float TurnRatio { get { return turnRatio; } set { turnRatio = value; } }
        public bool Accelerating { get { return accelerating; } set { accelerating = value; } }

        #endregion

        /// <summary>
        /// Constructor - Creates the vehicle
        /// </summary>
        /// <param name="initialX">X coordinate of vehicle</param>
        /// <param name="initialY">Y Coordinate of vehicle</param>
        public PlayerShip(int initialX, int initialY)
            : base((float)initialX, (float)initialY, 32, 32, "spaceShipPIC", new Coordinates[] {
            new Coordinates(0, -(32 / Settings.boundDivisor) / 2 * (float)1.3),
            new Coordinates(-(32 / Settings.boundDivisor) / 2, (32 / Settings.boundDivisor) / 2),
            new Coordinates((32 / Settings.boundDivisor) / 2, (32 / Settings.boundDivisor) / 2)})
        {
            Thrust = Settings.shipThrust;
            TurnRatio = Settings.shipTurnRatio;
            bullets = new List<Weapon>();
            bulletCoolDown = 0;
            warpCoolDown = 0;
            hasBoomed = false;
            boom = new Explosions();
        }

        /// <summary>
        /// Draws the vehicle on the given grahpics
        /// </summary>
        /// <param name="Initialg">Graphics vehicle is drawn on</param>
        public override void DrawObj(Graphics Initialg)
        {
            if (!IsDying)
            {
                base.DrawObj(Initialg);

                //center world on vehicle then rotate
                Initialg.TranslateTransform((int)Position.X, (int)Position.Y);
                Initialg.RotateTransform((float)Rotation); //degrees

                if (Accelerating)
                {
                    Initialg.DrawEllipse(new Pen(Color.AliceBlue, 3), -6, 15, 12, 5);
                    Initialg.FillEllipse(new SolidBrush(Color.DeepSkyBlue), -3, 15, 6, 20);
                    Accelerating = false;
                }

                Initialg.ResetTransform();
            }
            else
            {
                IsActive = boom.ExplosionDraw(Initialg, new Rectangle((int)(this.Position.X - (this.Dimensions.X + Settings.explodeSizeAdd) / 2), (int)(this.Position.Y - (this.Dimensions.Y + Settings.explodeSizeAdd) / 2), (int)(this.Dimensions.X + Settings.explodeSizeAdd), (int)(this.Dimensions.Y + Settings.explodeSizeAdd)));
                if (hasBoomed == false)
                {
                    GameSounds.effect(GameMedia.getDir["explosion1SND"]);
                    hasBoomed = true;
                }
            }
        }
        /// <summary>
        /// Turn the ship left using angles in degrees
        /// </summary>
        public void TurnLeft() { Rotation -= TurnRatio; }
        /// <summary>
        /// Turn the ship right using angles in degrees
        /// </summary>
        public void TurnRight() { Rotation += TurnRatio; }
        /// <summary>
        /// Called when the ship fires thrusters.
        /// </summary>
        public void Accelerate()
        {
            if (!IsDying)
            {
                Velocity.addVector(thrust, RadRotation - (float)Math.PI / 2);
                accelerating = true;
            }
        }
        /// <summary>
        /// overridden move allows the thruster fire to be drawn, as well as explosions.
        /// </summary>
        /// <param name="dt"></param>
        public override void Move(float dt)
        {
            if (Settings.shipUseDamper)
                Velocity.addVector(Settings.shipDamper * -Velocity.getMagnitude(), Velocity.getDirection());
            base.Move(dt);
            if (bulletCoolDown > 0) bulletCoolDown--;
            if (warpCoolDown > 0) warpCoolDown--;
        }
        /// <summary>
        /// this function shoots 
        /// </summary>
        public void Shoot()
        {
            if (bulletCoolDown <= 0)
            {
                try { GameSounds.effect(GameMedia.getDir["laser0SND"]); }
                catch { }
                bullets.Add(new NormalBullet(Position.X - Settings.bulletWidth / 2, Position.Y - Settings.bulletHeight / 2, Velocity.X, Velocity.Y, Rotation, "laser0PIC"));
                bulletCoolDown = Settings.shipBulletCooldown;
            }
        }
        /// <summary>
        /// warps the player's ship to a random location on the screen.
        /// </summary>
        public void Warp()
        {
            if (warpCoolDown <= 0)
            {
                GameSounds.effect(GameMedia.getDir["warpSND"]);
                Position.X = (Utilities.screenBounds.Width * Utilities.getRand10() / 10);
                Position.Y = (Utilities.screenBounds.Height * Utilities.getRand10() / 10);
                warpCoolDown = Settings.shipWarpCooldown;
            }
        }
    }
}
