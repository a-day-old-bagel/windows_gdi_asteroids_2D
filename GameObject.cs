/***********************************************************************
 * Galen Cochrane 11/20/2014
 * GameObject.cs
 * This is the base class for all objects in the game
 * Inspired by: (Nearly completely modified version of:)
 * 
 * Ted Delezene 11/17/2012
 * GameObjects.cs
 * This is the base class for the sprites.
 **********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    class GameObject
    {
        #region Fields

        public Coordinates Position;
        public Coordinates Velocity;
        public Coordinates Dimensions;
        public Coordinates[] ObjectRelativeCorners;
        public Coordinates[] collisionCorners;
        private Rectangle bounds;
        public bool isCollided;
        public bool isCollidedFoReal;
        public int numberOfSignificantSides;
        public float biggerDimension;
        private float rotation;
        private Image objImage;
        private bool isActive;
        private bool isDying;
        private bool isTargeted;
        private GameObject targeter;

        #endregion

        #region Constructors

        /// <summary>
        /// This happens at the beggining of object construction (for any constructor)
        /// </summary>
        /// <param name="imageName">object's picture</param>
        private void initGameObjectBefore(string imageName)
        {
            Position = new Coordinates();
            Velocity = new Coordinates();
            rotation = 0;
            Dimensions = new Coordinates();
            isActive = true;
            isDying = false;
            isTargeted = false;
            if (objImage == null && imageName != null)
                objImage = GameMedia.getImg[imageName];
                //objImage = Image.FromFile(GameMedia.getDir[imageName]);
            isCollided = false;
            isCollidedFoReal = false;
        }
        /// <summary>
        /// This happens at the end of construction (for any constructor)
        /// </summary>
        /// <param name="posX">x posision</param>
        /// <param name="posY">y position</param>
        /// <param name="verteces">object relative verteces that define the bounding polygon</param>
        private void initGameObjectAfter(float posX, float posY, Coordinates[] verteces)
        {
            Position.X = posX + Dimensions.X / 2;
            Position.Y = posY + Dimensions.Y / 2;
            biggerDimension = Dimensions.X > Dimensions.Y ? Dimensions.X : Dimensions.Y;
            if (Settings.useFancyCollisions)
            {
                bounds = new Rectangle((int)(Position.X - biggerDimension / 2), (int)(Position.Y - biggerDimension / 2), (int)(biggerDimension), (int)(biggerDimension));
                ObjectRelativeCorners = verteces;
                collisionCorners = new Coordinates[verteces.Length];
                for (int i = 0; i < verteces.Length; i++)
                    collisionCorners[i] = new Coordinates();
                if (verteces.Length == 2)
                    numberOfSignificantSides = 1;
                else
                    numberOfSignificantSides = verteces.Length;
            }
            else
                bounds = new Rectangle((int)(Position.X - biggerDimension / 2 / Settings.boundDivisor), (int)(Position.Y - biggerDimension / 2 / Settings.boundDivisor), (int)(biggerDimension / Settings.boundDivisor), (int)(biggerDimension / Settings.boundDivisor));
        }
        /// <summary>
        /// Here are four different overloads for the constructor for gameObject.
        /// two of them take the object's dimensions, and the other two generate them
        /// based off of the image.
        /// </summary>
        /**/public GameObject(Coordinates pos, string imageName, Coordinates[] verteces) : this(pos.X, pos.Y, imageName, verteces) { }
        /**/public GameObject(float posX, float posY, string imageName, Coordinates[] verteces)
            {
                initGameObjectBefore(imageName);            
                Dimensions.X = objImage.Width;
                Dimensions.Y = objImage.Height;
                initGameObjectAfter(posX, posY, verteces);
            }
        /**/public GameObject(Coordinates pos, float dim, string imageName, Coordinates[] verteces) : this(pos.X, pos.Y, dim, dim, imageName, verteces) { }
        /**/public GameObject(float posX, float posY, float dimX, float dimY, string imageName, Coordinates[] verteces)
        {
            initGameObjectBefore(imageName);            
            Dimensions.X = dimX;
            Dimensions.Y = dimY;
            initGameObjectAfter(posX, posY, verteces);
        }

        #endregion

        #region Properties

        public Image ObjImage { get { return objImage; } set { objImage = value; } }
        public Rectangle Bounds { get { return bounds; } set { bounds = value; } }
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public float RadRotation { get { return rotation * ((float)Math.PI / 180); } set { rotation = value * (180 / (float)Math.PI); } }
        public bool IsDying { get { return isDying; } set { isDying = value; } }
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        public bool IsTargeted { get { return isTargeted; } set { isTargeted = value; } }
        public GameObject Targeter { get { return targeter; } set { targeter = value; } }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Moves the game object given a time step
        /// </summary>
        /// <param name="dt">amound of time to multiply by velocity to add to posision</param>
        public virtual void Move(float dt)
        {
            Position.X += Velocity.X * dt;
            Position.Y += Velocity.Y * dt;
            if (Settings.useFancyCollisions)
                bounds = new Rectangle((int)(Position.X - biggerDimension / 2), (int)(Position.Y - biggerDimension / 2), (int)(biggerDimension), (int)(biggerDimension));
            else
                bounds = new Rectangle((int)(Position.X - biggerDimension / 2 / Settings.boundDivisor), (int)(Position.Y - biggerDimension / 2 / Settings.boundDivisor), (int)(biggerDimension / Settings.boundDivisor), (int)(biggerDimension / Settings.boundDivisor));
        }

        /// <summary>
        /// "wraps the ship around the screen if it goes off the board"
        /// </summary>
        /// <param name="screenEdge">the defined borders of the game.</param>
        public virtual void WrapAround(Rectangle screenEdge)
        {
            if (Position.X < screenEdge.X)
                Position.X = screenEdge.Right - -(Position.X);

            if (Position.X > screenEdge.Width)
                Position.X = screenEdge.X + (screenEdge.Right - Position.X);

            if (Position.Y < screenEdge.Y)
                Position.Y = screenEdge.Bottom - -(Position.Y);

            if (Position.Y > screenEdge.Height)
                Position.Y = screenEdge.Y + (screenEdge.Bottom - Position.Y);
        }
        
        /// <summary>
        /// This function will be used to draw the various game objects.
        /// </summary>
        /// <param name="initialG">the graphics plane we are using.</param>
        public virtual void DrawObj(Graphics Initialg)
        {
            if (!IsDying)
            {
                //center world on vehicle then rotate
                Initialg.TranslateTransform((int)Position.X, (int)Position.Y);
                Initialg.RotateTransform((float)Rotation); //degrees               

                try
                {
                    Initialg.DrawImage(ObjImage, (float)-(Dimensions.X / 2), (float)-(Dimensions.Y / 2), (float)Dimensions.X, (float)Dimensions.Y);
                }
                catch { }

                Initialg.ResetTransform();

                try
                {
                    if (Settings.debug)
                    {
                        Initialg.DrawRectangle(new Pen(isCollided ? Color.Red : Color.Cyan), Bounds);
                        if (isCollided)
                            Initialg.DrawPolygon(new Pen(isCollidedFoReal ? Color.Red : Color.Cyan), collisionCorners.Select(x => x.getPointF()).ToArray());
                    }
                }
                catch { }

                try
                {
                    if (isTargeted)
                    {
                        Initialg.DrawEllipse(new Pen(Color.Orange), Bounds);
                        Coordinates targetLocation = Position.getVector();
                        float targetLocMagnitude = Velocity.getMagnitude() * (targeter.Position.getMagnitudeTo(Position) / Settings.bulletSpeed)
                            * Math.Abs((float)Math.Sin(Position.getDirectionFrom(targeter.Position) - Velocity.getDirection()));
                        targetLocation.addVector(targetLocMagnitude, Velocity.getDirection());
                        Coordinates velocityCompensator = new Coordinates();
                        velocityCompensator.setVector(targeter.Velocity);
                        velocityCompensator.X *= -1 * targeter.Position.getMagnitudeTo(targetLocation) / Settings.bulletSpeed;
                        velocityCompensator.Y *= -1 * targeter.Position.getMagnitudeTo(targetLocation) / Settings.bulletSpeed;
                        targetLocation.addVector(velocityCompensator);
                        Initialg.DrawLine(new Pen(Color.Orange, 3), (float)Position.X, (float)Position.Y, (float)targetLocation.X, (float)targetLocation.Y);
                        if (targeter.Position.getMagnitudeTo(targetLocation) - Dimensions.X / 2 < Settings.bulletLife * Settings.bulletSpeed)
                            Initialg.DrawEllipse(new Pen(Color.Cyan, 3), (float)targetLocation.X - 7, (float)targetLocation.Y - 7, 14, 14);
                        else
                            Initialg.DrawEllipse(new Pen(Color.DarkRed, 3), (float)targetLocation.X - 7, (float)targetLocation.Y - 7, 14, 14);
                    }
                }
                catch { }
            }
        }

        #endregion
    }
}
