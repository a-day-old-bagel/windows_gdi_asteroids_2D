/****************************************************************************
 * Galen Cochrane 11/20/2014
 * Weapon.cs
 * This is the base class for all other kinds of bullets or missiles used
 * in the game.
 * Based off of:
 * 
 * Ted Delezene 11/17/2012
 * Weapon.cs
 * This class handles the bullet object, if one were to add different types
 * of bullets, this would be used as the base class, hence the name weapon
 * instead of bullet.
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    class Weapon : GameObject
    {
        int lifeTimer;
        int lifeSpan;

        /// <summary>
        /// Constructor
        /// </summary>
        public Weapon(float posX, float posY, float dimX, float dimY, string imgName, int life)
            : base(posX, posY, dimX, dimY, imgName, new Coordinates[] {
            new Coordinates(0, (dimY / Settings.boundDivisor) / 2 * (float)1.3),
            new Coordinates(0, -(dimY / Settings.boundDivisor) / 2 * (float)1.3)})
        {
            lifeTimer = 0;
            lifeSpan = life;
        }
        
        /// <summary>
        /// overridden move allows for the weapon to 'die' after a certain amount of time, giving it limited range.
        /// </summary>
        /// <param name="dt"></param>
        public override void Move(float dt)
        {
            base.Move(dt);
            RadRotation = Velocity.getDirection() + (float)Math.PI / 2;
            if (lifeTimer > lifeSpan)
            {
                IsDying = true;
            }
            lifeTimer++;
        }
        
    }
}
