/**************************************************************************
 * Galen Cochrane 11/20/2014
 * Settings.cs
 * Static class that keeps track of all the arbitrary numbers used
 * to calculate game behavior - Modify at your own risk.
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    static class Settings
    {

// GENERAL
        public static bool soundIsOn = true;                // use sound? Turn off for performance testing.
        public static bool useMouse = true;                 // whether or not to use the mouse (as opposed to the keyboard)
        public static bool useFancyCollisions = true;       // whether or not to use the generalized SAT polygon collision algorithm (slower but better)
        public static bool useTargetAssist = true;          // draw targeting markers to help noobs aim
        public const float boundDivisor = 1.3F;             // how much smaller an object's bounding box than its image
        public static bool debug = false;                   // show bounding boxes        
        public const int randPoolSize = 30;                 // the size of the pool of pre-generated random numbers made per level.
        public const int meteorScoreBase = 3000;            // higher number will make meteor hits worth more points. This is divided by the meteor's size for scoring.
        public const int scoreToNextLife = 10000;           // every how many points will the player be rewarded with another life?

// PERFOMANCE
        public const int graphicsInverseFPS = 12;           // 1000 divided by this number is the target frames per second for the graphics thread.
        public const int physicsInverseFPS = 16;            // 1000 divided by this number is the target frames per second for the all the physiscs engine.
        public const int collisionInverseFPS = 16;          // 1000 divided by this number is the target frames per second for the collisions engine.
        public const int gameStateInverseFPS = 60;
        public const int alienInverseFPS = 1000;            // 1000 divided by this number is the target frames per second for the alien spawner.
        public const int numSFXthreads = 4;                 // number of parallel sounds effects playable (don't change unless you also modify Threads.cs and GameSounds.cs accordingly)

// EXPLOSIONS
        public const int explodeSizeAdd = 50;               // how big are the booms (graphically)
        public const int explodeSlower = 3;                 // lower means explosions are more slo-mo (graphically).

// BULLETS       
        public const int bulletLife = 80;                   // how long the bullets last        
        public const float bulletSpeed = 8;                 // how fast the bullets go (pixels per physics frame)
        public static int bulletWidth = 15;                 // width of bullets (all bullets use this currently) (only applicable for fancyCollisions - otherwise both are 20)
        public static int bulletHeight = 60;                // length of bullets (all bullets use this currently) (only applicable for fancyCollisions - otherwise both are 20)
        public const int seekBulletLife = 500;              // how long the seeking bullents last
        public const float seekBulletThrust = .08F;         // the seeking bullets' thrust
        public const float seekBulletDamp = .01F;           // the braking factor on seeking bullets.  it helps them be deadly.

// SHIP
        public const float shipThrust = 0.08F;              // how powerful your engines are (acceleration in pixels per physics frame^2)
        public static bool shipUseDamper = true;            // will the ship slow down while not thrusting?
        public const float shipDamper = .008F;              // how strong 'space' resistance is. this is the damping force's relation to speed (notice that it's 1/10 of the ship's thrust)
        public const int shipBulletCooldown = 60;           // time between firing (in physics frames)
        public const int shipWarpCooldown = 100;            // time between warping (in physics frames) - as if you wanted to use this stupid "feature" repeatedly anyway... are you daft?
        public const int shipTurnRatio = 3;                 // how fast your ship turns (if using keyboard)

// METEORS
        /**/public static bool skinnyMeteors = true;        // oblong meteors possible? only activated if fancyCollisions are enabled.
        /****** Meteor skinny-ize-ation parameters ***/
        /**/public const int meteorDimLowerLim = 4;         // how skinny can one dimension of a newly spawned asteroid be, under normal splitting?
        /**/public const int meteorDimUpperLim = 7;         // other end of randomization spectrum for one dimension of an asteroid's size.  If it's greater than half of meteorDimRandDivisor, asteroids may grow when split.
        /**/public const float meteorDimRandDivisor = 16F;  // dimensions randomizer's divisor.
        /****** Rest of meteor stuff *****************/
        public const int meteorMinSize = 100;               // how small of a rock can split into more rocks? (atomic rock size)
        public const int meteorSpawnNumber = 3;             // how many peices come off of an asteroid when meteorPotshotPenalty = false?
        public const int meteorSpin = 20;                   // lower = faster asteroid spin on average
        public static int meteorInitAmount = 1;             // how many asteroids to start with
        public static int meteorStartSize = 200;            // about how big do the asteroids start (remember randomizer)
        public static int meteorSpawnSpeed = 6;             // max asteroid speed added at each spawn. unit: pixels per physics frame * 20.  Settings above 10 are effectively limited to 10 due to the way my random numbers work.
        /**/public static bool meteorPotshotPenalty = true; // fewer asteroids spawned when center of asteroid is hit?
        /***** ONLY FOR meteorPotshotPenalty = true*****/
        /**/public const int meteorMaxExtraSpawns = 3;      // max number of extra asteroids spawned if shot is inaccurate
        /**/public const int meteorMinSpawns = 3;           // how many asteroids MUST be spawned for each hit
        /**/public const int meteorPotShotPenaltyMult = 4;  // how much accuracy is required for few asteroid spawns? *unforgiving level*
        /***********************************************/

// ALIENS
        public static int chancePool = 100;                 // general chance for alien spawning greater when this is small. decreases with level
        public static int WussRemover = 0;                  // higher number = more mean aliens, fewer wuss aliens. increases with level to a max of 7.  higher will start preventing other types of aliens to spawn, too.
        public static int SpawnTimer = 10;                  // kind of redundant to have a timer as well as a regulated FPS for the alien spawner, but it helps prevent threading mayhem for some reason.

        // Other constants in the alien algorithms are modify-able, but it's probably unwise unless you really go through and figure out how they work...
        // plus I didn't want to go through and find all of them to put them here, but that would be a great idea for the future.
        
    }
}
