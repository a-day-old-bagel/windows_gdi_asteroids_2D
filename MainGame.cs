/*********************************************************************
 * Ted Delezene 11/17/2012
 * MainGame.cs
 * This class runs the game.
 * 
 * Galen Cochrane 11/20/2014
 * I have heavily modified pretty much everything here.
 * Some of Ted's functions remain, but even those have
 * been changed significantly.
 ********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Asteroids2._0
{
    class MainGame
    {
        #region Fields

        static public int level;
        static public int score;
        static public int extraLivesCounter;
        static public int lives;
        static public bool extraLifeAdded;
        static public bool isRunning;
        static public bool needsToExit;
        static public string stateDescription;
        private List<Meteor> inGameMeteors;
        private List<AlienBase> inGameAlienShips;
        private PlayerShip playerShip;
        private Collisions aCollision;
        public List<Meteor> InGameMeteors { get { return inGameMeteors; } set { inGameMeteors = value; } }
        public List<AlienBase> InGameAlienShips { get { return inGameAlienShips; } set { inGameAlienShips = value; } }
        public PlayerShip PlayerShip { get { return playerShip; } set { playerShip = value; } }
        public Collisions ACollision { get { return aCollision; } set { aCollision = value; } }
        private int alienSpawnTimer;

        #endregion

        #region Initiators

        /// <summary>
        /// Intializes the game creating everything that is needed
        /// </summary>
        /// <param name="Display">The form being drawn on</param>
        public void IntializeGame(Form1 Display)
        {            
            Threads.initializeTasks();
            level = 1;
            score = 0;
            lives = 3;
            extraLivesCounter = 0;
            Utilities.screenBounds = Screen.PrimaryScreen.Bounds;//Display.ClientRectangle;
            Utilities.screenCenter = new Coordinates(Utilities.screenBounds.Width / 2, Utilities.screenBounds.Height / 2);
            isRunning = true;
            PlayerShip = new PlayerShip(Utilities.screenBounds.Width / 2, Utilities.screenBounds.Height / 2);
            InGameMeteors = new List<Meteor>();
            inGameAlienShips = new List<AlienBase>();
            alienSpawnTimer = Settings.SpawnTimer;
            ACollision = new Collisions();
            stateDescription = "fine";
            needsToExit = false;
            
            int x; 
            int y; 
            int i = 0; 

            while (i < Settings.meteorInitAmount)
            {
                //create asteriod away from middle
                x = (Utilities.getRand10() * (Utilities.screenBounds.Width / 10));
                while (x > Utilities.screenBounds.Width / 2 - 300 && x < Utilities.screenBounds.Width / 2 + 300)
                    x = (Utilities.getRand10() * (Utilities.screenBounds.Width / 10));

                y = (Utilities.getRand10() * (Utilities.screenBounds.Height / 10));
                while (y > Utilities.screenBounds.Height / 2 - 300 && y < Utilities.screenBounds.Height / 2 + 300)
                    y = (Utilities.getRand10() * (Utilities.screenBounds.Height / 10));

                InGameMeteors.Add(new Meteor(new Coordinates(x, y),
                    Settings.meteorStartSize, new Coordinates((Utilities.getRandBase(Settings.meteorSpawnSpeed) - Settings.meteorSpawnSpeed / 2) / (float)10.0,
                        (Utilities.getRandBase(Settings.meteorSpawnSpeed) - Settings.meteorSpawnSpeed / 2) / (float)10.0)));
                i++;
            }
        }
        /// <summary>
        /// initialized the next level
        /// </summary>
        /// <param name="Display">the form on which to operate</param>
        public void InitializeNextLevel(Form Display)
        {
            //GameSounds.CloseSFX();
            //GameSounds.InitializeSFX();
            //Task.WaitAll()

            Threads.initializeTasks();
            InGameMeteors = new List<Meteor>();
            inGameAlienShips = new List<AlienBase>();
            alienSpawnTimer = Settings.SpawnTimer;
            ACollision = new Collisions();
            PlayerShip = new PlayerShip(Display.ClientRectangle.Width / 2, Display.ClientRectangle.Height / 2);
            PlayerShip.Position = new Coordinates(Display.ClientRectangle.Width / 2, Display.ClientRectangle.Height / 2);
            Utilities.populateRandPool();

            Settings.chancePool = 160 - level * 10;                         // at level 15 an alien should spawn every time the alien spawner loops
            if (Settings.chancePool <= 10) Settings.chancePool = 10;
            Settings.WussRemover = level / 5;                               // at level 35 only the tough aliens will spawn
            if (Settings.WussRemover >= 7) Settings.WussRemover = 7;
            int totalMeteorMaterial = 200 + level * 75;
            if (totalMeteorMaterial >= 2100) totalMeteorMaterial = 2100;    // 10 large asteroids will be the max ever spawned
            Settings.meteorInitAmount = totalMeteorMaterial / 200;
            Settings.meteorStartSize = totalMeteorMaterial / Settings.meteorInitAmount;
            Settings.meteorSpawnSpeed = 6 + level / 12;                     // every 12 levels meteor speed will increase (can't go over 10)
            if (Settings.meteorSpawnSpeed <= 10) Settings.meteorSpawnSpeed = 10;

            int x;
            int y;
            int i = 0;

            while (i < Settings.meteorInitAmount)
            {
                //create asteriod away from middle
                x = (Utilities.getRand10() * (Utilities.screenBounds.Width / 10));
                while (x > Utilities.screenBounds.Width / 2 - 300 && x < Utilities.screenBounds.Width / 2 + 300)
                    x = (Utilities.getRand10() * (Utilities.screenBounds.Width / 10));

                y = (Utilities.getRand10() * (Utilities.screenBounds.Height / 10));
                while (y > Utilities.screenBounds.Height / 2 - 300 && y < Utilities.screenBounds.Height / 2 + 300)
                    y = (Utilities.getRand10() * (Utilities.screenBounds.Height / 10));

                InGameMeteors.Add(new Meteor(new Coordinates(x, y),
                    Settings.meteorStartSize, new Coordinates((Utilities.getRandBase(Settings.meteorSpawnSpeed) - Settings.meteorSpawnSpeed / 2) / (float)10.0,
                        (Utilities.getRandBase(Settings.meteorSpawnSpeed) - Settings.meteorSpawnSpeed / 2) / (float)10.0)));
                i++;
            }
        }

        #endregion

        #region Engine Loops

        /// <summary>
        /// loops to run the physics simulations in the game
        /// </summary>
        public void runGamePhysics(Form Display, float dt)
        {
            Threads.physTasks[0] = Task.Run(() =>
            {
                try
                {
                    if (playerShip.IsActive)
                    {                    
                        playerShip.Move(dt);
                        playerShip.WrapAround(Display.ClientRectangle);
                    }                    
                }
                catch{}
                //catch { MessageBox.Show("move"); }
            });

            Threads.physTasks[1] = Task.Run(() =>
            {
                for (int i = PlayerShip.Bullets.Count - 1; i >= 0; i--) 
                {
                    try
                    {
                        if (PlayerShip.Bullets[i].IsActive)
                        {                        
                            PlayerShip.Bullets[i].Move(dt);
                            PlayerShip.Bullets[i].WrapAround(Display.ClientRectangle);
                        }                        
                    }
                    catch { }
                    //catch { MessageBox.Show("move"); }
                }
            });

            Threads.physTasks[2] = Task.Run(() =>
            {
                for (int i = inGameMeteors.Count - 1; i >= 0; i--)
                {
                    //int p = i;
                    //Threads.InternalPhysTasks.Add(Task.Run(() =>
                    //{
                        try
                        {
                            if (inGameMeteors[i].IsActive)
                            {
                                inGameMeteors[i].Move(dt);
                                inGameMeteors[i].WrapAround(Display.ClientRectangle);
                            }                            
                        }
                        catch { }//(Exception e) { MessageBox.Show(e.Message,"InternalPhysTasks"); }
                    //}));
                }
            });

            Threads.physTasks[3] = Task.Run(() =>
                {
                    for (int i = inGameAlienShips.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            if (inGameAlienShips[i].IsActive)
                            {                            
                                inGameAlienShips[i].Move(PlayerShip, dt);
                                inGameAlienShips[i].WrapAround(Display.ClientRectangle);
                            }                            
                        }
                        catch { MessageBox.Show("move"); }
                        for (int j = inGameAlienShips[i].Bullets.Count - 1; j >= 0; j--)
                        {
                            try
                            {
                                if (inGameAlienShips[i].Bullets[j].IsActive)
                                {                                
                                    inGameAlienShips[i].Bullets[j].Move(dt);
                                    inGameAlienShips[i].Bullets[j].WrapAround(Display.ClientRectangle);
                                }                               
                            }
                            catch { }
                            //catch { MessageBox.Show("move"); }
                        }
                    }
                });

            try
            {
                if (Threads.physTasks != null)
                    Task.WaitAll(Threads.physTasks);
            }
            catch { }
            //if (Threads.InternalPhysTasks != null)
            //    Task.WaitAll(Threads.InternalPhysTasks.ToArray());
        }
        /// <summary>
        /// loops to run the collisions simulations in the game
        /// </summary>
        public void runGameCollisions()
        {
            Threads.collTasks[0] = Task.Run(() =>
                {
                    try
                    {
                        aCollision.DidCollisionOccur(inGameMeteors, PlayerShip.Bullets);
                    }
                    catch { }//{ MessageBox.Show("coll"); }
                });

            Threads.collTasks[1] = Task.Run(() =>
                {
                    try
                    {
                        aCollision.DidCollisionOccur(inGameMeteors, PlayerShip);
                    }
                    catch { }//{ MessageBox.Show("coll"); }
                });

            Threads.collTasks[2] = Task.Run(() =>
                {
                    for (int i = inGameAlienShips.Count - 1; i >= 0; i--)
                        try
                        {
                            aCollision.DidCollisionOccurNoDamage(inGameMeteors, inGameAlienShips[i].Bullets);
                        }
                        catch { }//{ MessageBox.Show("coll"); }
                });

            Threads.collTasks[3] = Task.Run(() =>
                {
                    for (int i = inGameAlienShips.Count - 1; i >= 0; i--)
                        try
                        {
                            aCollision.DidCollisionOccur(inGameMeteors, inGameAlienShips[i]);
                        }
                        catch { }//{ MessageBox.Show("coll"); }
                });
            
            Threads.collTasks[4] = Task.Run(() =>
                {
                    for (int i = inGameAlienShips.Count - 1; i >= 0; i--)
                        try
                        {
                            aCollision.DidCollisionOccur(inGameAlienShips[i].Bullets, PlayerShip);
                        }
                        catch { }//{ MessageBox.Show("coll"); }
                });

            Threads.collTasks[5] = Task.Run(() =>
                {
                    for (int i = inGameAlienShips.Count - 1; i >= 0; i--)
                        try
                        {
                            aCollision.DidCollisionOccur(PlayerShip.Bullets, inGameAlienShips[i]);
                        }
                        catch { }//{ MessageBox.Show("coll"); }
                });

            try
            {
                foreach (Meteor m in aCollision.meteorSpawner.NewMeteors)
                    InGameMeteors.Add(m);
                aCollision.meteorSpawner.clearNewMeteors();
            }
            catch (Exception e) { MessageBox.Show(e.Message, "METEOR SPAWNING"); }

            try
            {
                if (Threads.collTasks != null)
                    Task.WaitAll(Threads.collTasks);
                if (Threads.InternalCollTasks != null)
                    Task.WaitAll(Threads.InternalCollTasks.ToArray());
            }
            catch { }

            Threads.cleanTasks[0] = Task.Run(() =>
                {
                    for (int i = inGameAlienShips.Count - 1; i >= 0; i--)
                        try
                        {
                            aCollision.CleanupList(inGameAlienShips[i].Bullets);
                        }
                        catch { MessageBox.Show("clean"); }
                });

            Threads.cleanTasks[1] = Task.Run(() =>
                {
                    try
                    {
                        aCollision.CleanupList(inGameAlienShips);
                    }
                    catch { MessageBox.Show("clean"); }
                });

            //Remove Meteors that got destroyed
            Threads.cleanTasks[2] = Task.Run(() =>
                {
                    try
                    {
                        aCollision.CleanupList(inGameMeteors);
                    }
                    catch { MessageBox.Show("clean"); }
                });

            //Remove bullets that expired or hit something
            Threads.cleanTasks[3] = Task.Run(() =>
                {
                    try
                    {
                        aCollision.CleanupList(playerShip.Bullets);
                    }
                    catch { MessageBox.Show("clean"); }
                });

            try
            {
                if (Threads.cleanTasks != null)
                    Task.WaitAll(Threads.cleanTasks);
            }
            catch { }
        }
        /// <summary>
        /// loops to run the alien spawner in the game
        /// </summary>
        public void runGameAliens()
        {
            try
            {
                if (alienSpawnTimer <= 0)
                {
                    int dieRoll = Utilities.rand.Next(Settings.chancePool - Settings.WussRemover) + Settings.WussRemover;         // chancePool size is decreased every level, making a spawn more likely.
                    if (dieRoll < 7) inGameAlienShips.Add(new AlienWuss());                      // 70% for Alien Wuss to spawn
                    else if (dieRoll == 7) inGameAlienShips.Add(new AlienHunter());             //  10% for each other variety
                    else if (dieRoll == 8) inGameAlienShips.Add(new AlienSeeker(PlayerShip));
                    else if (dieRoll == 9) inGameAlienShips.Add(new AlienCaller(inGameAlienShips, PlayerShip));
                    alienSpawnTimer = Settings.SpawnTimer;           // every 10 frames alien spawn chance is evaluated. Currently @ 1fps.
                }
                else alienSpawnTimer--;
            }
            catch (Exception e) { MessageBox.Show(e.Message + e.Source + e.Data, "AlienSpawn"); }
        }
        /// <summary>
        /// loops to run the draw each game object each frame
        /// </summary>
        public void runGameGraphics(Graphics gameGraphics)
        {
            List<Weapon> playersBullets = PlayerShip.Bullets.ToList();
            for (int i = playersBullets.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (playersBullets[i].IsActive)
                    {
                        playersBullets[i].DrawObj(gameGraphics);
                    }
                }
                catch { }
            }

            List<Meteor> meteors = inGameMeteors.ToList();
            for (int i = meteors.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (meteors[i].IsActive)
                    {
                        meteors[i].DrawObj(gameGraphics);
                    }
                }
                catch { }
            }

            List<AlienBase> aliens = inGameAlienShips.ToList();
            for (int i = aliens.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (aliens[i].IsActive)
                    {
                        aliens[i].DrawObj(gameGraphics);
                    }
                }
                catch (Exception e) { MessageBox.Show(e.Message, "Alien Ship Draw"); }
                for (int j = aliens[i].Bullets.Count - 1; j >= 0; j--)
                {
                    try
                    {
                        if (aliens[i].Bullets[j].IsActive)
                        {
                            aliens[i].Bullets[j].DrawObj(gameGraphics);
                        }
                    }
                    catch (Exception e) { MessageBox.Show(e.Message, "Alien Bullet Draw"); }
                }                
            }
            
            if (playerShip != null)
            {
                if (PlayerShip.IsActive)
                {
                    PlayerShip.DrawObj(gameGraphics);
                }
            }            
        }

        #endregion

        #region Game State Checkers

        /// <summary>
        /// Check if the level has been won.
        /// </summary>
        public bool isLevelWon() { return (InGameMeteors != null && InGameMeteors.Count == 0 && inGameAlienShips != null && inGameAlienShips.Count == 0); }
        /// <summary>
        /// checks to see if the level has been lost
        /// </summary>
        public bool isLevelLost() { return (PlayerShip != null && !PlayerShip.IsActive); }

        #endregion

        #region Control Methods

        /// <summary>
        /// Turns the ship left if it still active
        /// </summary>
        public void playerTurnLeft()
        {
            if ((PlayerShip != null) && (PlayerShip.IsActive))
                PlayerShip.TurnLeft();
        }
        /// <summary>
        /// Turns the ship right if still active
        /// </summary>
        public void playerTurnRight()
        {
            if ((PlayerShip != null) && (PlayerShip.IsActive))
                PlayerShip.TurnRight();
        }
        /// <summary>
        /// points the ship at the pointer location (if using mouse controls)
        /// </summary>
        /// <param name="mouse">location of pointer</param>
        public void playerMouseTurn(Coordinates mouse)
        {
            PlayerShip.RadRotation = mouse.getDirectionFrom(PlayerShip.Position) + ((float)Math.PI / 2);
        }
        /// <summary>
        /// Accelerate ship if still active
        /// </summary>
        public void playerThrust()
        {
            if ((PlayerShip != null) && (PlayerShip.IsActive))
                PlayerShip.Accelerate();
        }
        /// <summary>
        /// engages the player's ship's targeting computer (or you can just use the force)
        /// </summary>
        /// <param name="clickLocation">location of click</param>
        public void playerTarget(Coordinates clickLocation)
        {
            aCollision.DidCollisionOccur(inGameAlienShips, inGameMeteors, clickLocation, PlayerShip);
        }
        /// <summary>
        /// Shoots Bullets if ship is still active
        /// </summary>
        public void playerShoot()
        {
            if ((PlayerShip != null) && (PlayerShip.IsActive))
                PlayerShip.Shoot();
        }
        /// <summary>
        /// warps the player to a random location (what an awful feature...)
        /// </summary>
        public void playerWarp()
        {
            if ((PlayerShip != null) && (PlayerShip.IsActive))
                PlayerShip.Warp();
        }

        #endregion
    }
}
