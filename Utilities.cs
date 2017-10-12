/**************************************************************************
 * Galen Cochrane 11/20/2012
 * Utilities.cs
 * Static class containing a single Random class for all the game's
 * random calculations and methods to get the center and edges of the
 * screen.
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Asteroids2._0
{
    static class Utilities
    {
        #region Field for rand generation

        public static Random rand = new Random();
        private static int[] randPool;
        private static int randCycler;

        #endregion

        #region rand Methods

        public static void populateRandPool()
        {
            randCycler = 0;
            randPool = new int[Settings.randPoolSize];
            for (int i = 0; i < Settings.randPoolSize; i++)
            {
                randPool[i] = rand.Next(10);
                Thread.Sleep(randPool[i]);
            }
        }
        public static int getRand10()
        {
            randCycler += 1;
            return randPool[randCycler % Settings.randPoolSize];            
        }
        public static int getRandBool()
        {
            randCycler += 1;
            return randPool[randCycler % Settings.randPoolSize] % 2;
        }
        public static int getRandBase(int numBase)
        {
            randCycler += 1;
            return randPool[randCycler % Settings.randPoolSize] % numBase;
        }

        #endregion


        #region Fields for screen edge fetching

        public static Rectangle screenBounds;
        public static Coordinates screenCenter;

        #endregion

        /// <summary>
        /// returns a random point on the edge of the screen, mostly for aliens' use.
        /// </summary>
        /// <returns>point on edge of screen</returns>
        public static Coordinates getScreenEdge()
        {
            switch (Utilities.rand.Next(3))
            {
                case 0:
                    return new Coordinates(0, Utilities.rand.Next(screenBounds.Bottom));
                case 1:
                    return new Coordinates(screenBounds.Right, Utilities.rand.Next(screenBounds.Bottom));
                case 2:
                    return new Coordinates(Utilities.rand.Next(screenBounds.Right), 0);
                case 3:
                    return new Coordinates(Utilities.rand.Next(screenBounds.Right), screenBounds.Bottom);
            }
            return new Coordinates(0, 0);
        }

        /// <summary>
        /// get random image of meteor
        /// </summary>
        /// <returns>a grey, brown, or green meteor image</returns>
        public static string randomMeteorImage()
        {
            switch (Utilities.getRandBase(3))
            {
                case 0:
                    return "asteroidPIC0";
                case 1:
                    return "asteroidPIC1";
                case 2:
                    return "asteroidPIC2";
            }
            return "callSigPIC";            // As an error image
        }
    }
}
