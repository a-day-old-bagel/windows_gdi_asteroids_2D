/**************************************************************************
 * Galen Cochrane 11/20/2014
 * NormalBullet.cs
 * This class defines a projectile that moves in a straight line,
 * with the speed defined in Settings, and with the lifetime defined
 * in Settings.  It is what most in-game bullets are, like the player's
 * bullets.  A key to GameMedia's dictionary must be provided for the
 * desired image in the constructor call.
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    class SeekingBullet : Weapon
    {
        GameObject target;          // the object that the bullet is homing to

        /// <summary>
        /// Constructor
        /// </summary>
        public SeekingBullet(float posX, float posY, float velX, float velY, float rotation, string imgName, GameObject target)
            : base(posX, posY, Settings.bulletWidth, Settings.bulletHeight, imgName, Settings.seekBulletLife)
        {
            Rotation = rotation;
            Velocity.X = velX;
            Velocity.Y = velY;
            Velocity.addVector(1, RadRotation - (float)Math.PI / 2);
            this.target = target;
        }

        /// <summary>
        /// overridden move allows the seeking bullet to apply a thrust vector toward its target.
        /// </summary>
        /// <param name="dt">time step</param>
        public override void Move(float dt)
        {
            base.Move(dt);
            Velocity.addVector(-Settings.seekBulletDamp * Velocity.getMagnitude(), Velocity.getDirection());
            Velocity.addVector(Settings.seekBulletThrust, Position.getDirectionTo(target.Position));
        }
    }
}
