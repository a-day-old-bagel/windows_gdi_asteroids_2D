/*************************************************************************************************
 * Galen Cochrane 12/04/2014
 * MeteorSpawner.cs
 * 
 * This creates new meteors when a big one is destroyed.
 ************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids2._0
{
    class MeteorSpawner
    {
        private List<Meteor> newMeteors = new List<Meteor>();
        public List<Meteor> NewMeteors { get { return newMeteors; } }
        private int newSizeSubtractor;

        /// <summary>
        /// Spawns the addition meteors
        /// 
        /// </summary>
        /// <param name="Lmeteor">Meteor List</param>
        /// <param name="oldMeteor">Meteor object</param>
        public void SpawnMeteors(Meteor oldMeteor, int potShotPenalty)
        {
            for (int i = Settings.meteorPotshotPenalty ? -(Settings.meteorMinSpawns) : 0;
                i < (Settings.meteorPotshotPenalty ? potShotPenalty : Settings.meteorSpawnNumber); i++)
            {
                if (!Settings.useFancyCollisions)
                    newSizeSubtractor = 200 - Utilities.getRand10() * 10;
                newMeteors.Add(new Meteor(oldMeteor.Position.X, oldMeteor.Position.Y,
                    oldMeteor.Velocity.X + (Utilities.getRandBase(Settings.meteorSpawnSpeed) - Settings.meteorSpawnSpeed / 2) * (potShotPenalty + 1) / 10.0F,
                    oldMeteor.Velocity.Y + (Utilities.getRandBase(Settings.meteorSpawnSpeed) - Settings.meteorSpawnSpeed / 2) * (potShotPenalty + 1) / 10.0F,
                    Settings.useFancyCollisions ? oldMeteor.biggerDimension * (Utilities.getRandBase(Settings.meteorDimUpperLim) + Settings.meteorDimLowerLim) / Settings.meteorDimRandDivisor : Math.Abs(oldMeteor.Dimensions.X - newSizeSubtractor) + 30,
                    Settings.useFancyCollisions ? oldMeteor.biggerDimension * (Utilities.getRandBase(Settings.meteorDimUpperLim) + Settings.meteorDimLowerLim) / Settings.meteorDimRandDivisor : Math.Abs(oldMeteor.Dimensions.X - newSizeSubtractor) + 30,
                    oldMeteor.ImgPath));
            }                
        }
        /// <summary>
        /// This was going to be a cool method for having meteors split down the middle where the player shoots
        /// them, breaking into two semi-circular pieces instead of many smaller round meteors.  It is doable - I
        /// just didn't have time.  The drawImage method has overloads that allow you to just draw a portion of an image,
        /// and my fancyCollisions SAT algorithm would allow for non-ellipsoid meteors, too.  It was going to be awesome.
        /// </summary>
        /// <param name="meteor">the old meteor</param>
        /// <param name="bullet">the bullet that hit it</param>
        /// <param name="potShotPenalty">the inaccuracy penalty</param>
        public void SplitMeteor(Meteor meteor, Weapon bullet, int potShotPenalty) // Not yet implemented
        {
            float splitLineAngle = bullet.Position.getDirectionTo(meteor.Position);
        }
        /// <summary>
        /// clears the list of newly spawned meteors, once they've been copied to MainGame's list.
        /// </summary>
        public void clearNewMeteors()
        {
            newMeteors.Clear();
        }

    }
}
