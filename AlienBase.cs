/*********************************************************************
 * Galen Cochrane 11/20/2014
 * AlienBase.cs
 * Provides a parent class for all types of enemy ship to inherit.
 * Includes the logic for the behavior of enemy pilots.
 * Inspired by:
 * 
 * Ted Delezene 11/17/2012 (Ted only had the header before)
 * AlienShip.cs
 * This class defines the alienship, and its activities.
 ********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids2._0
{
    class AlienBase : GameObject
    {
        #region Variables
        int preferredMaxDist;                                   // farthest away the alien wants to be from player (firing range)
        int preferredMinDist;                                   // closest the alien wants to be to player (danger zone)
        float angleOfInaccuracy;                                // how many radians 'off' from its target is the alien willing to fire?
        int bulletCoolDown;                                     // minimum time between shots
        int shieldLevel;                                        // how many shields the alien has
        int awareness;                                          // how far away can the alien detect objects to avoid / dodge
        Rectangle areaOfAwareness;                              // a rectangle representing that detection area
        List<GameObject> visibleObjs;                           // a list of objects that fall within that detection area
        bool escaping;                                          // whether or not the alien is ready to leave/escape
        bool accelerating;                                      // whether the alien's thrusters are firing
        bool hasBoomed;                                         // whether it has exploded
        Explosions boom;                                        // to handle explosions
        private List<Weapon> bullets;                           // list of the alien's projectiles
        Coordinates target;                                     // what to aim at (usually the player)
        int plusOrMinus;                                        // switches between 1 and -1 to randomize some behaviors
        bool canDodge;                                          // whether or not this alien has access to the dodge routine
        bool isCoward;                                          // whether or not this alien performs the flanking routine
        float cowardAngle;                                      // how wide an angle from the player's rotation will the alien try to flank from
        int pointValue;                                         // how many points is the alien worth if destroyed
        #endregion
        #region Properties
        public int BulletCoolDown { get { return bulletCoolDown; } set { bulletCoolDown = value; } }
        public List<Weapon> Bullets { get { return bullets; } set { bullets = value; } }
        public float Inaccuracy { get { return angleOfInaccuracy; } set { angleOfInaccuracy = value; } }
        public Rectangle Aware { get { return areaOfAwareness; } set { areaOfAwareness = value; } }
        public List<GameObject> Visibles { get { return visibleObjs; } set { visibleObjs = value; } }
        public bool CanDodge { get { return canDodge; } set { canDodge = value; } }
        public bool IsCoward { get { return isCoward; } set { isCoward = value; } }
        public float CowardAngle { get { return cowardAngle; } set { cowardAngle = value; } }
        public Explosions Boom { get { return boom; } }
        public bool HasBoomed { get { return hasBoomed; } set { hasBoomed = value; } }
        public bool Accelerating { get { return accelerating; } set { accelerating = value; } }
        public int ShieldLevel { get { return shieldLevel; } set { shieldLevel = value; } }
        public bool Escaping { get { return escaping; } set { escaping = value; } }
        public int PreferredMinDist { get { return preferredMaxDist; } set { preferredMaxDist = value; } }
        public int PointValue { get { return pointValue; } set { pointValue = value; } }
        #endregion
        #region Constructor
        /// <summary>
        /// Constructor for alien base
        /// </summary>
        /// <param name="imgName">image of the alien</param>
        /// <param name="prefMax">preferred maximum distance from target (usually firing range)</param>
        /// <param name="prefMin">preferred minimum distance from target</param>
        /// <param name="numShields">how many shield levels - how many hits to kill it (-1)</param>
        /// <param name="awareness">how distant an object can the alien detect to avoid</param>
        /// <param name="inaccuracy">how wide an angle (radians) around the target will the alien be willing to fire</param>
        public AlienBase(string imgName, int prefMax, int prefMin, int numShields, int awareness, float inaccuracy)
            : base(Utilities.getScreenEdge(), imgName, new Coordinates[] {
            new Coordinates(0, -(Image.FromFile(GameMedia.getDir[imgName]).Height / Settings.boundDivisor) / 2 * (float)1.3),
            new Coordinates(-(Image.FromFile(GameMedia.getDir[imgName]).Width / Settings.boundDivisor) / 2, (Image.FromFile(GameMedia.getDir[imgName]).Height / Settings.boundDivisor) / 2),
            new Coordinates((Image.FromFile(GameMedia.getDir[imgName]).Width / Settings.boundDivisor) / 2, (Image.FromFile(GameMedia.getDir[imgName]).Height / Settings.boundDivisor) / 2)})
        {
            Velocity.setVector((float).1, Position.getDirectionTo(Utilities.screenCenter));
            RadRotation = Velocity.getDirection() + (float)Math.PI / 2;
            escaping = false;
            shieldLevel = numShields;
            preferredMaxDist = prefMax;
            preferredMinDist = prefMin;
            accelerating = false;
            hasBoomed = false;
            boom = new Explosions();
            this.awareness = awareness;
            angleOfInaccuracy = inaccuracy;
            bullets = new List<Weapon>();
            visibleObjs = new List<GameObject>();
            bulletCoolDown = 200;             // gives player time to see the alien before it shoots
            plusOrMinus = 1;
            canDodge = true;
            isCoward = false;
            cowardAngle = (float)Math.PI / 12;
            pointValue = 0;
        }
        #endregion
        #region Overloaded GameObject Methods
        /// <summary>
        /// Overloaded move function runs the logic (AI) of the alien and sets rotation = velocity
        /// </summary>
        /// <param name="player">the player's ship - used for AI</param>
        /// <param name="dt">the time step</param>
        public virtual void Move(PlayerShip player, float dt)
        {
            if (!IsDying)
            {
                if (Settings.shipUseDamper)
                    Velocity.addVector((float)-.01 * Velocity.getMagnitude(), Velocity.getDirection());
                alienLogic(player);
                RadRotation = Velocity.getDirection() + (float)Math.PI / 2;
            }
            base.Move(dt);
            areaOfAwareness = new Rectangle(Bounds.X - awareness, Bounds.Y - awareness, Bounds.Width + awareness * 2, Bounds.Height + awareness * 2);
            if (bulletCoolDown > 0) bulletCoolDown--;
        }
        /// <summary>
        /// wrap-around overloaded to allow for escaping the screen
        /// </summary>
        /// <param name="screenEdge">rectangle representing the screen</param>
        public override void WrapAround(Rectangle screenEdge)
        {
            if (!escaping)
                base.WrapAround(screenEdge);
            else                                    // if the alien is escaping, when it touches the edge it disappears instead
                if (Position.X < screenEdge.X ||
                    Position.X > screenEdge.Width ||
                    Position.Y < screenEdge.Y ||
                    Position.Y > screenEdge.Height)
                {
                    IsDying = true;
                    IsActive = false;
                }
        }
        #endregion
        #region Artificial Intelligence
        /// <summary>
        /// The main artificial intelligence algorithm for the aliens
        /// </summary>
        /// <param name="player">the player's ship</param>
        protected virtual void alienLogic(PlayerShip player)
        {
            try //One task at a time...
            {
                // Object avoidance logic first - don't let meteors or bullets hit me
                if (visibleObjs.Count > 0)
                    avoid();

                // Player stalking logic second - get in close enough to kill              
                else if (Position.getMagnitudeTo(player.Position) > preferredMaxDist)
                    approach(player.Position);

                // Prevent player from using the edge to trick me - don't sit on the edge
                else if (Math.Abs(Position.X - Utilities.screenBounds.X) < 60 || Math.Abs(Position.X - Utilities.screenBounds.Width) < 60
                    || Math.Abs(Position.Y - Utilities.screenBounds.Y) < 60 || Math.Abs(Position.Y - Utilities.screenBounds.Height) < 60)
                {
                    flank(player.RadRotation, player.Position);
                    getAwayFromEdge();
                }

                // If the player's getting too close, run away
                else if (Position.getMagnitudeTo(player.Position) < preferredMinDist && player.Velocity.getMagnitude() > .5)
                    flee(player.Position);

                // If I'm a coward, get around behind the player - don't let the player aim at me
                else if (Math.Abs(player.Position.getDirectionTo(Position) - (player.RadRotation - (float)Math.PI / 2)) < cowardAngle && isCoward)
                    flank(player.RadRotation, player.Position);                

                // Player shooting logic last - if all else is golden, blow the player up
                else
                    aim(player.Position, player.Velocity);
            }
            catch (Exception e) { MessageBox.Show(e.Message, "Alien Logic"); }
        }
        /// <summary>
        /// behavior to move away from the edge of the screen. It's dangerous to be near the edge where meteors appear.
        /// this may need some tweaking so that the aliens don't cross the edge when there's a meteor on the other side.
        /// </summary>
        protected virtual void getAwayFromEdge()
        {
            if (Math.Abs(Utilities.screenCenter.getDirectionTo(Position) - Velocity.getDirection()) < (float)Math.PI / 2)
            {
                Velocity.addVector((float).08, Position.getDirectionTo(Utilities.screenCenter));
                accelerating = true;
            }
        }
        /// <summary>
        /// behavior to avoid objects within the alien's rectangle of awareness
        /// </summary>
        private void avoid()
        {
            for (int i = visibleObjs.Count - 1; i >= 0; i--)
            {
                try
                {
                    float escapeSpeed = .04F + 2 / (Position.getMagnitudeTo(visibleObjs[i].Position) - visibleObjs[i].biggerDimension / 2 - 40);
                    if (escapeSpeed > 0.12 || escapeSpeed < 0) escapeSpeed = 0.12F;
                    Velocity.addVector(escapeSpeed, Position.getDirectionFrom(visibleObjs[i].Position));
                    if (canDodge && visibleObjs[i].Velocity.getMagnitude() > 5)
                        dodge(visibleObjs[i]);
                    accelerating = true;
                }
                catch { }
            }
            visibleObjs.Clear();
        }
        /// <summary>
        /// behavior to dodge fast moving objects within the alien's rectangle of awareness
        /// </summary>
        /// <param name="fastObject">GameObject to avoid</param>
        private void dodge(GameObject fastObject)
        {
            float approachAngle = fastObject.Velocity.getDirection() - fastObject.Position.getDirectionTo(Position);
            if (Math.Abs(approachAngle) < (float)Math.PI / 2)
            {
                float dodgeSpeed = fastObject.Velocity.getMagnitude() / approachAngle / 100;
                if (dodgeSpeed > .12) dodgeSpeed = (float).12;
                Velocity.addVector(dodgeSpeed, fastObject.Velocity.getDirection() - (float)Math.PI / 2);
            }
        }
        /// <summary>
        /// behavior to approach the player until within firing range
        /// </summary>
        /// <param name="playerPos">Coordinates of player's position</param>
        private void approach(Coordinates playerPos)
        {
            Velocity.addVector((float).08, Position.getDirectionTo(playerPos));
            accelerating = true;
        }
        /// <summary>
        /// behavior of the alien to distance itself from the player if the player is aggressive
        /// </summary>
        /// <param name="playerPos"></param>
        private void flee(Coordinates playerPos)
        {
            Velocity.addVector((float).08, Position.getDirectionFrom(playerPos) + (float)Math.PI / 4 * plusOrMinus);
            accelerating = true;
        }
        /// <summary>
        /// behavior to circle around the player if the player is pointing at the alien
        /// </summary>
        /// <param name="playerRot"></param>
        /// <param name="playerPos"></param>
        protected void flank(float playerRot, Coordinates playerPos)
        {
            Coordinates playerRotVec = new Coordinates();
            playerRotVec.setVector(200, playerRot - (float)Math.PI / 2);
            float escapeV = 1 / (playerPos.getDirectionTo(Position) - (playerRot - (float)Math.PI / 2));
            if (escapeV > .1)
                escapeV = (float).1;
            else if (escapeV < -.1)
                escapeV = (float)-.1;
            Velocity.addVector(escapeV, playerPos.getDirectionTo(Position) + (float)Math.PI / 2);// / 4 * plusOrMinus);
            accelerating = true;
        }
        /// <summary>
        /// behavior to aim (smoothly) at the player
        /// </summary>
        /// <param name="playerPos"></param>
        /// <param name="playerVel"></param>
        public void aim(Coordinates playerPos, Coordinates playerVel)
        { 
            target = playerPos.getVector();
            target.addVector(playerVel.getMagnitude() * (Position.getMagnitudeTo(playerPos) / Settings.bulletSpeed)
                * Math.Abs((float)Math.Sin(Position.getDirectionTo(playerPos) - playerVel.getDirection())), playerVel.getDirection());
            if (Math.Abs(Position.getDirectionTo(target) - (RadRotation - (float)Math.PI / 2)) < Inaccuracy)
            {
                Velocity.addVector((float).001, Position.getDirectionTo(target));
                fire();
            }
            else
            {
                Velocity.addVector(-Velocity.getMagnitude() / 8 / ((float)Math.Pow(Inaccuracy + 1, 2)), Velocity.getDirection());
                Velocity.addVector((float).001, Position.getDirectionTo(target));
            }
        }
        /// <summary>
        /// virtual behavior to fire weapons.  different for every alien. flips the 1/-1 randomizer
        /// </summary>
        public virtual void fire() { plusOrMinus *= -1; }
        #endregion
    }
}
