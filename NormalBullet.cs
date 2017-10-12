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

namespace Asteroids2._0
{
    class NormalBullet : Weapon
    {
        /// <summary>
        /// Constructor - just calls GameObjects constructor - the normal bullet doesn't do anything fancy.
        /// </summary>        
        public NormalBullet(float posX, float posY, float velX, float velY, float rotation, string imgName)
            : base(posX, posY, Settings.bulletWidth, Settings.bulletHeight, imgName, Settings.bulletLife)
        {
            Rotation = rotation;
            Velocity.X = velX;
            Velocity.Y = velY;
            Velocity.addVector(Settings.bulletSpeed, RadRotation - (float)Math.PI / 2);
        }
    }
}
