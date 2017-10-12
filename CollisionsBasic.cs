/************************************************************
 * Ted Delezene 11/17/2012
 * Collisions.cs
 * This class handles an object once a collision has been 
 * detected.
 * 
 * Very much updated and modified by
 * Galen Cochrane 11/20/2014
 ***********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace Asteroids2._0
{
    class Collisions
    {
        #region Variables

        private List<Meteor> DestroyedMeteors = new List<Meteor>();
        private List<AlienBase> DestroyedAliens = new List<AlienBase>();
        private List<Weapon> UsedBullets = new List<Weapon>();
        public MeteorSpawner meteorSpawner = new MeteorSpawner();
        private bool hasBouncedThisCycle = false;
        private bool hasHitMeteorThisCycle = false;

        #endregion

        #region Collision Detectors

        /// <summary>
        /// Checks if a collision occurred with the crude bounding rectangles (AABB)
        /// </summary>
        /// <param name="vehicle">First rectangle</param>
        /// <param name="meteor">Second rectangle</param>
        /// <returns>Returns ture if collision happens else false</returns>
        private bool Check(Rectangle FirstRect, Rectangle SecondRect)
        {
            return FirstRect.IntersectsWith(SecondRect);
        }
        /// <summary>
        /// Check if Collision occurs between Meteor and Bullet
        /// Also spawn addition Meteors
        /// </summary>
        /// <param name="Lmeteor">Meteor List</param>
        /// <param name="Lbullet">Weapon List</param>
        public void DidCollisionOccur(List<Meteor> Lmeteor, List<Weapon> Lbullet)        
        {
            hasHitMeteorThisCycle = false;
            for (int i = Lmeteor.Count - 1; i >= 0; i--)
            {
                if (!Lmeteor[i].IsDying)
                {
                    for (int j = Lbullet.Count - 1; j >= 0; j--)
                    {
                        if (Lbullet[j].IsDying)
                        {
                            Lbullet[j].IsActive = false;
                            UsedBullets.Add(Lbullet[j]);
                        }
                        else if (Check(Lmeteor[i].Bounds, Lbullet[j].Bounds))
                        {
                            if (Settings.debug)
                            {
                                Lmeteor[i].isCollided = true;
                                Lbullet[j].isCollided = true;                                
                            }
                            if (Settings.useFancyCollisions)
                            {
                                if (Settings.debug)
                                {
                                    Lmeteor[i].isCollidedFoReal = false;
                                    Lbullet[j].isCollidedFoReal = false;
                                }
                                if (FancyCollisions.isColliding(Lmeteor[i], Lbullet[j]))
                                {
                                    int p = i;
                                    int q = j;
                                    try { Threads.InternalCollTasks.Add(Task.Run(() => bulletHitMeteor(Lmeteor[p], Lbullet[q]))); }
                                    catch { }
                                    //bulletHitMeteor(Lmeteor[i], Lbullet[j]);
                                    hasHitMeteorThisCycle = true;
                                }
                            }
                            else
                            {
                                int p = i;
                                int q = j;
                                try { Threads.InternalCollTasks.Add(Task.Run(() => bulletHitMeteor(Lmeteor[p], Lbullet[q]))); }
                                catch { }
                                //bulletHitMeteor(Lmeteor[i], Lbullet[j]);
                                hasHitMeteorThisCycle = true;
                            }                            
                        }
                        if (hasHitMeteorThisCycle) 
                            break;
                    }
                }
                if (hasHitMeteorThisCycle) 
                    break;
            }
        }
        /// <summary>
        /// Handles the collision of a bullet and a meteor (meteor spawning called from here)
        /// </summary>
        /// <param name="m">The Meteor</param>
        /// <param name="b">The Weapon</param>
        private void bulletHitMeteor(Meteor m, Weapon b)
        {
            if (Settings.debug && Settings.useFancyCollisions)
            {
                m.isCollidedFoReal = true;
                b.isCollidedFoReal = true;
            }
            m.IsDying = true;
            b.IsActive = false;
            DestroyedMeteors.Add(m);
            UsedBullets.Add(b);
            MainGame.score += Settings.meteorScoreBase / (int)m.biggerDimension;
            MainGame.extraLivesCounter += Settings.meteorScoreBase / (int)m.biggerDimension;
            if (MainGame.extraLivesCounter > Settings.scoreToNextLife)
            {
                MainGame.extraLivesCounter = 0;
                MainGame.lives++;                
                MainGame.extraLifeAdded = true;
            }
            if (m.biggerDimension > Settings.meteorMinSize)
            {
                Meteor TmpMeteor = m;
                int potShotPenalty;
                if (Settings.meteorPotshotPenalty && m.Dimensions.X > Settings.meteorMinSize)
                {
                    potShotPenalty = (int)(Math.Abs(b.Velocity.getDirection() - b.Position.getDirectionTo(m.Position)) * Settings.meteorPotShotPenaltyMult);
                    if (potShotPenalty > Settings.meteorMaxExtraSpawns) potShotPenalty = Settings.meteorMaxExtraSpawns;
                    //Threads.MeteorSpawningTasks.Add(Task.Run(() => meteorSpawner.SpawnMeteors(TmpMeteor, potShotPenalty)));
                    meteorSpawner.SpawnMeteors(TmpMeteor, potShotPenalty);
                }
                else
                    meteorSpawner.SpawnMeteors(TmpMeteor, 0);
                    //Threads.MeteorSpawningTasks.Add(Task.Run(() => meteorSpawner.SpawnMeteors(TmpMeteor, 0)));                                        
            }
        }
        /// <summary>
        /// Checks collisions between the meteors and the alien bullets (they just bounce off)
        /// </summary>
        /// <param name="Lmeteor">Meteor list</param>
        /// <param name="Lbullet">Alien's bullet list</param>
        public void DidCollisionOccurNoDamage(List<Meteor> Lmeteor, List<Weapon> Lbullet)
        {
            hasBouncedThisCycle = false;
            for (int i = Lmeteor.Count - 1; i >= 0; i--)
            {
                if (!Lmeteor[i].IsDying)
                {
                    for (int j = Lbullet.Count - 1; j >= 0; j--)
                    {
                        if (Lbullet[j].IsDying)
                        {
                            Lbullet[j].IsActive = false;
                            UsedBullets.Add(Lbullet[j]);
                        }
                        else if (Check(Lmeteor[i].Bounds, Lbullet[j].Bounds))
                        {
                            if (Settings.debug)
                            {
                                Lmeteor[i].isCollided = true;
                                Lbullet[j].isCollided = true;
                            }
                            if (Settings.useFancyCollisions)
                            {
                                if (Settings.debug)
                                {
                                    Lmeteor[i].isCollidedFoReal = false;
                                    Lbullet[j].isCollidedFoReal = false;
                                }
                                if (FancyCollisions.isColliding(Lmeteor[i], Lbullet[j]))
                                {
                                    if (Settings.debug)
                                    {
                                        Lmeteor[i].isCollidedFoReal = true;
                                        Lbullet[j].isCollidedFoReal = true;
                                    }
                                    Lbullet[j].Velocity.setVector(Lbullet[j].Velocity.getMagnitude(), Lbullet[j].Position.getDirectionFrom(Lmeteor[i].Position));
                                }
                            }
                            else
                                Lbullet[j].Velocity.setVector(Lbullet[j].Velocity.getMagnitude(), Lbullet[j].Position.getDirectionFrom(Lmeteor[i].Position));

                            hasBouncedThisCycle = true;
                        }
                        if (hasBouncedThisCycle) break;
                    }
                }
                if (hasBouncedThisCycle) break;
            }
        }        
        /// <summary>
        /// Check if Collision occurs between Meteor and playerShip
        /// </summary>
        /// <param name="Lmeteor">Meteor List</param>
        /// <param name="ship">Vehicle Object</param>
        public void DidCollisionOccur(List<Meteor> Lmeteor, PlayerShip ship)
        {
            ship.isCollided = false;
            for (int i = Lmeteor.Count - 1; i >= 0; i--)
            {
                Lmeteor[i].isCollided = false;
                if (!Lmeteor[i].IsDying)
                {
                    if (Check(Lmeteor[i].Bounds, ship.Bounds))
                    {
                        if (Settings.debug)
                        {
                            Lmeteor[i].isCollided = true;
                            ship.isCollided = true;
                        }
                        if (Settings.useFancyCollisions)
                        {
                            if (Settings.debug)
                            {
                                Lmeteor[i].isCollidedFoReal = false;
                                ship.isCollidedFoReal = false;
                            }
                            if (FancyCollisions.isColliding(Lmeteor[i], ship))
                            {
                                if (Settings.debug)
                                {
                                    Lmeteor[i].isCollidedFoReal = true;
                                    ship.isCollidedFoReal = true;
                                }
                                Lmeteor[i].IsDying = true;
                                ship.IsDying = true;
                            }
                        }
                        else
                        {
                            Lmeteor[i].IsDying = true;
                            ship.IsDying = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Checks collisions between the aliens' bullets and the player's ship
        /// </summary>
        /// <param name="Lbullet">alien bullet list</param>
        /// <param name="ship">player's ship</param>
        public void DidCollisionOccur(List<Weapon> Lbullet, PlayerShip ship)
        {
            for (int i = Lbullet.Count - 1; i >= 0; i--)
            {
                if (!Lbullet[i].IsDying)
                {
                    if (Check(Lbullet[i].Bounds, ship.Bounds))
                    {
                        if (Settings.debug)
                        {
                            Lbullet[i].isCollided = true;
                            ship.isCollided = true;
                        }
                        if (Settings.useFancyCollisions)
                        {
                            if (Settings.debug)
                            {
                                Lbullet[i].isCollidedFoReal = false;
                                ship.isCollidedFoReal = false;
                            }
                            if (FancyCollisions.isColliding(Lbullet[i], ship))
                            {
                                if (Settings.debug)
                                {
                                    Lbullet[i].isCollidedFoReal = true;
                                    ship.isCollidedFoReal = true;
                                }
                                Lbullet[i].IsDying = true;
                                UsedBullets.Add(Lbullet[i]);
                                ship.IsDying = true;
                            }
                        }
                        else
                        {
                            Lbullet[i].IsDying = true;
                            UsedBullets.Add(Lbullet[i]);
                            ship.IsDying = true;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// checks collisions between the player's bullets and the alien ships
        /// </summary>
        /// <param name="Lbullet">player's bullets</param>
        /// <param name="ship">alien ship</param>
        public void DidCollisionOccur(List<Weapon> Lbullet, AlienBase ship)
        {
            for (int i = Lbullet.Count - 1; i >= 0; i--)
            {
                if (!Lbullet[i].IsDying)
                {
                    if (Check(Lbullet[i].Bounds, ship.Bounds))
                    {
                        if (Settings.debug)
                        {
                            Lbullet[i].isCollided = true;
                            ship.isCollided = true;
                        }
                        if (Settings.useFancyCollisions)
                        {
                            if (Settings.debug)
                            {
                                Lbullet[i].isCollidedFoReal = false;
                                ship.isCollidedFoReal = false;
                            }
                            if (FancyCollisions.isColliding(Lbullet[i], ship))
                            {
                                if (Settings.debug)
                                {
                                    Lbullet[i].isCollidedFoReal = true;
                                    ship.isCollidedFoReal = true;
                                }
                                bulletHitAlien(ship, Lbullet[i]);
                            }
                        }
                        else
                            bulletHitAlien(ship, Lbullet[i]);
                    }
                    else if (Check(Lbullet[i].Bounds, ship.Aware))
                        ship.Visibles.Add(Lbullet[i]);
                }
            }
        }
        /// <summary>
        /// Handles the collision between a player's bullet and an alien ship
        /// </summary>
        /// <param name="alien">the alien</param>
        /// <param name="bullet">the bullet</param>
        private void bulletHitAlien(AlienBase alien, Weapon bullet)
        {
            bullet.IsDying = true;
            UsedBullets.Add(bullet);
            alien.ShieldLevel -= 1;
            if (alien.ShieldLevel < 0)
            {
                alien.IsDying = true;
                DestroyedAliens.Add(alien);
                MainGame.score += alien.PointValue;
                MainGame.extraLivesCounter += alien.PointValue;
                if (MainGame.extraLivesCounter > Settings.scoreToNextLife)
                {
                    MainGame.lives++;
                    MainGame.extraLivesCounter = 0;
                    MainGame.extraLifeAdded = true;
                }
            }
            else
                try { GameSounds.effect(GameMedia.getDir["shieldsDownSND"]); }
                catch { }
        }
        /// <summary>
        /// Checks collisions between the meteors and an alien ship
        /// </summary>
        /// <param name="Lmeteor">list of meteors</param>
        /// <param name="ship">alien ship</param>
        public void DidCollisionOccur(List<Meteor> Lmeteor, AlienBase ship)
        {
            for (int i = Lmeteor.Count - 1; i >= 0; i--)
            {
                if (!Lmeteor[i].IsDying)
                {
                    if (Check(Lmeteor[i].Bounds, ship.Bounds))
                    {
                        if (Settings.debug)
                        {
                            Lmeteor[i].isCollided = true;
                            ship.isCollided = true;
                        }
                        if (Settings.useFancyCollisions)
                        {
                            if (Settings.debug)
                            {
                                Lmeteor[i].isCollidedFoReal = false;
                                ship.isCollidedFoReal = false;
                            }
                            if (FancyCollisions.isColliding(Lmeteor[i], ship))
                            {
                                if (Settings.debug)
                                {
                                    Lmeteor[i].isCollidedFoReal = true;
                                    ship.isCollidedFoReal = true;
                                }
                                ship.IsDying = true;
                                DestroyedAliens.Add(ship);
                            }
                        }
                        else
                        {
                            ship.IsDying = true;
                            DestroyedAliens.Add(ship);
                        }
                    }
                    else if (Check(Lmeteor[i].Bounds, ship.Aware))
                    {
                        ship.Visibles.Add(Lmeteor[i]);
                    }
                }
            }
        }
        /// <summary>
        /// detects mouse clicks on an object by detecting collisions between a rectangle centered at the pointer and all other objects
        /// </summary>
        /// <param name="aliens">list of aliens</param>
        /// <param name="meteors">list of meteors</param>
        /// <param name="clickLocation">location of clic</param>
        /// <param name="targeter">object doing the targeting (the player's ship)</param>
        public void DidCollisionOccur(List<AlienBase> aliens, List<Meteor> meteors, Coordinates clickLocation, GameObject targeter)
        {
            Rectangle clickRec = new Rectangle((int)clickLocation.X - 8, (int)clickLocation.Y - 8, 16, 16);
            for (int i = aliens.Count - 1; i >= 0; i--)
            {
                if (!aliens[i].IsDying)
                {
                    if (Check(aliens[i].Bounds, clickRec))
                    {
                        aliens[i].IsTargeted = true;
                        aliens[i].Targeter = targeter;
                    }
                    else aliens[i].IsTargeted = false;
                }
            }
            for (int i = meteors.Count - 1; i >= 0; i--)
            {
                if (!meteors[i].IsDying)
                {
                    if (Check(meteors[i].Bounds, clickRec))
                    {
                        meteors[i].IsTargeted = true;
                        meteors[i].Targeter = targeter;
                    }
                    else meteors[i].IsTargeted = false;
                }
            }
        }

        #endregion

        #region List Cleaners
        /// <summary>
        /// Removes meteors once they have been destoryed
        /// </summary>
        /// <param name="Lmeteor">Meteor List</param>
        public void CleanupList(List<Meteor> Lmeteor)
        {
            if (DestroyedMeteors != null)
                foreach (Meteor M in DestroyedMeteors)
                    if (!M.IsActive)
                    {
                        Lmeteor.Remove(M);
                        M.IsDying = false;
                    }
        }
        /// <summary>
        /// Removes bullet once they have been destoryed
        /// </summary>
        /// <param name="Lbullet">Weapon List</param>
        public void CleanupList(List<Weapon> Lbullet)
        {
                if (UsedBullets != null)
                    foreach (Weapon B in UsedBullets)
                        if (!B.IsActive)
                        {
                            Lbullet.Remove(B);
                            B.IsDying = false;
                        }
        }
        /// <summary>
        /// Removes dead aliens
        /// </summary>
        /// <param name="LShip"></param>
        public void CleanupList(List<AlienBase> LShip)
        {
            if (DestroyedAliens != null)
                foreach (AlienBase S in DestroyedAliens)
                    if (!S.IsActive)
                    {
                        LShip.Remove(S);
                        S.IsDying = false;
                    }
        }
        #endregion
    }
}