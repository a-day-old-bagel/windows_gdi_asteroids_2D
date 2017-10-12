/************************************************************************
 * Galen Cochrane 11/20/2014
 * Meteor.cs
 * This class is based on Ted's meteor class only in name, really.
 * His was huge - I redid this to inherit from game object, which
 * already handles movement and drawing, so the class is pretty
 * small now - only explosions and a constant rotation are added
 * to the base class, in addition to several constructor overloads.
 * Inspired by:
 * 
 * Ted Delezene 11/17/2012
 * Meteor.cs
 * This class defines the basic asteroid.
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Asteroids2._0
{
    class Meteor : GameObject
    {
        public int deathFlag = 0;
        private float rotateRate;
        private Explosions boom;
        private string imgPath;
        public string ImgPath { get { return imgPath; } set { imgPath = value; } }

        #region Constructors

        /// <summary>
        /// Creates the Meteor object
        /// This constructor is called to make meteors at the beginning of levels
        /// </summary>
        public Meteor(Coordinates pos, int Size, Coordinates vel)
            : base(pos, Size, "callSigPIC", new Coordinates[] {
            new Coordinates((Settings.meteorStartSize / Settings.boundDivisor) / (float)1.732, (Settings.meteorStartSize / Settings.boundDivisor) / 3),
            new Coordinates(0, (Settings.meteorStartSize / Settings.boundDivisor) / (float)1.5),
            new Coordinates(-(Settings.meteorStartSize / Settings.boundDivisor) / (float)1.732, (Settings.meteorStartSize / Settings.boundDivisor) / 3),
            new Coordinates(-(Settings.meteorStartSize / Settings.boundDivisor) / (float)1.732, -(Settings.meteorStartSize / Settings.boundDivisor) / 3),
            new Coordinates(0, -(Settings.meteorStartSize / Settings.boundDivisor) / (float)1.5),
            new Coordinates((Settings.meteorStartSize / Settings.boundDivisor) / (float)1.732, -(Settings.meteorStartSize / Settings.boundDivisor) / 3)})
        {
            imgPath = Utilities.randomMeteorImage();
            ObjImage = Image.FromFile(GameMedia.getDir[imgPath]);
            Velocity = vel;
            initializeMeteor();
        }
        /// <summary>
        /// This constructor is called to make child meteors when a big meteor splits.
        /// </summary>
        public Meteor(float posX, float posY, float velX, float velY, float dimX, float dimY, string image)
            : base(posX, posY, dimX, dimY, image, new Coordinates[] {
            new Coordinates((dimX / Settings.boundDivisor) / (float)1.732, (dimY / Settings.boundDivisor) / 3),
            new Coordinates(0, (dimY / Settings.boundDivisor) / (float)1.5),
            new Coordinates(-(dimX / Settings.boundDivisor) / (float)1.732, (dimY / Settings.boundDivisor) / 3),
            new Coordinates(-(dimX / Settings.boundDivisor) / (float)1.732, -(dimY / Settings.boundDivisor) / 3),
            new Coordinates(0, -(dimY / Settings.boundDivisor) / (float)1.5),
            new Coordinates((dimX / Settings.boundDivisor) / (float)1.732, -(dimY / Settings.boundDivisor) / 3)})
        {
            Velocity.X = velX;
            Velocity.Y = velY;
            initializeMeteor();
            imgPath = image;
        }
        /// <summary>
        /// This is called by both constructors.
        /// </summary>
        private void initializeMeteor()
        {
            rotateRate = (float)(Utilities.rand.Next(20) - 10) / Settings.meteorSpin;
            boom = new Explosions();
        }

        #endregion

        #region Overloaded GameObject Methods

        /// <summary>
        /// overloaded move allows the meteors to rotate at a constant rate
        /// </summary>
        /// <param name="dt"></param>
        public override void Move(float dt)
        {
            Rotation += rotateRate;
            base.Move(dt);
        }      
        /// <summary>
        /// overloaded draw allows the meteors to explode upon death
        /// </summary>
        /// <param name="Initialg"></param>
        public override void DrawObj(Graphics Initialg)
        {
            base.DrawObj(Initialg);
            if (IsDying)
            {
                deathFlag++;
                IsActive = boom.ExplosionDraw(Initialg, new Rectangle((int)(this.Position.X - (this.Dimensions.X + Settings.explodeSizeAdd) / 2), (int)(this.Position.Y - (this.Dimensions.Y + Settings.explodeSizeAdd) / 2), (int)(this.Dimensions.X + Settings.explodeSizeAdd), (int)(this.Dimensions.Y + Settings.explodeSizeAdd)));
            }
            if (deathFlag == 1)
            {
                try { GameSounds.effect(GameMedia.getDir["explosion0SND"]); }
                catch { }
            }
        }

        #endregion
    }
}
