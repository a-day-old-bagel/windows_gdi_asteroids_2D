/*********************************************************************************************
 * Galen Cochrane 11/20/2014
 * Form1.cs
 * Welcome to the latest version of ASTEROIDS!
 * This has been heavily modified, but a few of the underlying structural ideas remain.
 * 
 * Ted Delezene 11/14/2012
 * Form1.CS
 * This is the form that runs the rest of the game.
*********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Media;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Asteroids2._0
{
    /// <summary>
    /// Game will be painted on the form
    /// </summary>
    public partial class Form1 : Form
    {
        #region DLL imports

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int vKey);        // for controls

        [DllImport("user32.dll")]
        public static extern bool HideCaret(IntPtr hwnd);           // for scrolling story ala star wars

        #endregion

        #region Fields

        private MainGame Game = new MainGame();     // here's the main game object that runs the whole show.
        Graphics Canvas;                            // the graphics object on which the game will be drawn.
        private int lvlSplashCycler;                // used for displaying level load screens
        private List<PictureBox> livesPics;         // used for displaying extra lives

        #endregion

        #region Game Initialization

        /// <summary>
        /// this method initializes the game and plays the opening sequence.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            Utilities.populateRandPool();
            GameMedia.InitializeMedia();
            Game.IntializeGame(this);

            GameSounds.initializeMusic();
            GameSounds.backGroundLoop();

            lbl_level.Hide();
            lbl_score.Hide();

            lvlSplashCycler = -1;
            
            //private WriteableBitmap bmp
        }

        /// <summary>
        /// Game starts when you click the button, this is how!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_2(object sender, EventArgs e)
        {
            this.BackgroundImage = global::Asteroids2._0.Properties.Resources.ag;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            Threads.Physics_Engine = Task.Run(() => physicsEngine());
            Threads.Alien_Engine = Task.Run(() => alienSpawningEngine());
            Threads.Collisions_Engine = Task.Run(() => collisionEngine());
            Threads.Graphics_Engine = Task.Run(() => graphicsEngine());
            Threads.Game_Engine = Task.Run(() => gameStateEngine());
            if (Settings.soundIsOn) GameSounds.InitializeSFX();
            StartGame();
            populateLivesPics();
        }

        #endregion

        #region Game Engines

        /// <summary>
        /// checks if the level has been won or lost, writes game state to a string stored in MainGame, which
        /// string is accessed by the graphics engine.
        /// </summary>
        private void gameStateEngine()
        {
            int currentTime = 0;
            int previousTime = Environment.TickCount;
            int deltaTime = 0;
            while (true)
            {
                currentTime = Environment.TickCount;
                while (!MainGame.isRunning) ;
                deltaTime = currentTime - previousTime;
                if (deltaTime < Settings.gameStateInverseFPS)
                    Thread.Sleep(Settings.gameStateInverseFPS - deltaTime);
                previousTime = currentTime;

                if (MainGame.isRunning)
                {
                    if (Game.isLevelWon())
                    {
                        MainGame.stateDescription = "LevelWon";
                        MainGame.isRunning = false;
                        GameSounds.credits();
                    }
                    else if (Game.isLevelLost())
                    {
                        GameSounds.loser();
                        if (MainGame.lives <= 0)
                        {
                            MainGame.stateDescription = "GameLost";
                            MainGame.isRunning = false;
                            return;
                        }
                        else
                        {
                            MainGame.stateDescription = "LevelLost";
                            MainGame.isRunning = false;
                        }
                    }
                    else if (MainGame.extraLifeAdded)
                    {                        
                        GameSounds.effect(GameMedia.getDir["extraLifeSND"]);
                        MainGame.stateDescription = "extraLife";
                        MainGame.isRunning = false;                        
                    }
                    try
                    {
                        lbl_score.Text = MainGame.score.ToString();
                        lbl_level.Text = MainGame.level.ToString();
                    }
                    catch { }
                }
                else if (MainGame.needsToExit)
                    return;
            }
        }

        /// <summary>
        /// does all of the operating on the form, so that there isn't more than one thread operating on the form
        /// controls at once.  Also calls all the objects' draw methods on form.refresh.
        /// </summary>
        private void graphicsEngine()
        {
            int currentTime = 0;
            int previousTime = Environment.TickCount;
            int deltaTime = 0;
            while (true)
            {
                currentTime = Environment.TickCount;
                deltaTime = currentTime - previousTime;
                if (deltaTime < Settings.graphicsInverseFPS)
                    Thread.Sleep(Settings.graphicsInverseFPS - deltaTime);
                previousTime = currentTime;
                if (MainGame.isRunning)
                    this.Refresh();
                else if (MainGame.needsToExit)
                    return;
                else
                {
                    switch (MainGame.stateDescription)
                    {
                        case "LevelWon":
                            showLevelSplash();
                            lbl_LvlScr.Text = "LEVEL: " + MainGame.level + "\nSCORE: " + MainGame.score;
                            lbl_LvlScr.Show();
                            lbl_level.Hide();
                            lbl_score.Hide();

                            MessageBox.Show("Next Level!", "Good Job!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                            hideLevelSplash();
                            lbl_LvlScr.Hide();
                            lbl_level.Show();
                            lbl_score.Show();
                            MainGame.score += MainGame.level * 100;
                            MainGame.level++;
                            Game.InitializeNextLevel(this);
                            GameSounds.backGroundLoop();
                            break;
                        case "LevelLost":
                            MainGame.lives--;
                            this.BeginInvoke((MethodInvoker)delegate()//() =>
                            {
                                foreach (PictureBox p in livesPics)
                                {
                                    this.Controls.Remove(p);
                                }
                                populateLivesPics();
                            });
                            MessageBox.Show("Switch to next miner?", "Died!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                            Game.InitializeNextLevel(this);
                            GameSounds.backGroundLoop();
                            break;
                        case "GameLost":
                            showHighScores();
                            MainGame.needsToExit = true;
                            this.Refresh();
                            MessageBox.Show("Press ok to exit.", "Play again soon!", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                            Exit();
                            return;
                        case "extraLife":
                            MainGame.extraLifeAdded = false;
                            this.BeginInvoke((MethodInvoker)delegate()
                            {
                                foreach (PictureBox p in livesPics)
                                {
                                    this.Controls.Remove(p);
                                }
                                populateLivesPics();
                            });
                            break;
                        case "Paused":
                            MessageBox.Show("Game is Paused!!", "PAUSE", MessageBoxButtons.OK);
                            break;
                        case "Help":
                            MessageBox.Show("Up Arrow: Accelerate \n\nLeft Arrow: Rotate Left \n\nRight Arrow: Rotate Right \n\nSpace Bar: Shoot\n\nCTRL: Warp\n\nLeft Click: Target" +
                                "\n\n______________\n\nFOR MOUSE:\n______________\n\nShip points at mouse.\n\nLeft click to fire\n\nRight click to accelerate" +
                                "\n\nMiddle or Side Click to target\n\nSpace Bar: Warp\n\n______________\n\nESC: QUIT", "Help", MessageBoxButtons.OK);
                            break;
                    }
                    MainGame.isRunning = true;
                }
            }
        }

        /// <summary>
        /// runs the main game's collision loop.
        /// </summary>
        private void collisionEngine()
        {
            try
            {
                int currentTime = 0;
                int previousTime = Environment.TickCount;
                int deltaTime = 0;
                while (true)
                {
                    currentTime = Environment.TickCount;
                    while (!MainGame.isRunning) ;
                    deltaTime = currentTime - previousTime;
                    if (deltaTime < Settings.collisionInverseFPS)
                        Thread.Sleep(Settings.collisionInverseFPS - deltaTime);
                    previousTime = currentTime;

                    if (MainGame.isRunning)
                    {
                        Game.runGameCollisions();
                    }
                    else if (MainGame.needsToExit)
                        return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Collisions Loop");
            }
        }

        /// <summary>
        /// runs the main game's collision loops.
        /// </summary>
        private void alienSpawningEngine()
        {
            try
            {
                int currentTime = 0;
                int previousTime = Environment.TickCount;
                int deltaTime = 0;
                while (true)
                {
                    currentTime = Environment.TickCount;
                    while (!MainGame.isRunning) ;
                    deltaTime = currentTime - previousTime;
                    if (deltaTime < Settings.alienInverseFPS)
                        Thread.Sleep(Settings.alienInverseFPS - deltaTime);
                    previousTime = currentTime;

                    if (MainGame.isRunning)
                    {
                        Game.runGameAliens();
                    }
                    else if (MainGame.needsToExit)
                        return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Alien Loop");
            }
        }

        /// <summary>
        /// runs the main game's physics loops.
        /// </summary>
        private void physicsEngine()
        {
            try 
            {
                int currentTime = 0;
                int previousTime = Environment.TickCount;
                int deltaTime = 0;
                float dtMult;
                while (true)
                {
                    currentTime = Environment.TickCount;
                    while (!MainGame.isRunning) ;
                    deltaTime = currentTime - previousTime;
                    if (deltaTime < Settings.physicsInverseFPS)
                        Thread.Sleep(Settings.physicsInverseFPS - deltaTime);
                    previousTime = currentTime;

                    if (deltaTime <= Settings.physicsInverseFPS) dtMult = 1;
                    else dtMult = (float)deltaTime / (float)Settings.physicsInverseFPS;

                    if (MainGame.isRunning)
                    {
                        Game.runGamePhysics(this, dtMult);                        
                    }
                    else if (MainGame.needsToExit)
                        return;

                    if (Settings.useMouse)
                        MousePressProcess();
                    else
                        KeyPressProcess();
                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Physics Loop");
            }
        }

        #endregion

        #region Game Engine General Methods

        /// <summary>
        /// This is the form being painted on
        /// </summary>
        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            try
            {
                Canvas = e.Graphics;
            }
            catch { }
            Game.runGameGraphics(Canvas);
        }

        /// <summary>
        /// Currently not in use (becuase of bugs).  This same method is used to regulate the framerate of the
        /// engines in each individual loop, though.
        /// </summary>
        private void waitFps(ref int currentTime, ref int previousTime, ref int deltaTime, int inverseFPMS)
        {
            currentTime = Environment.TickCount;
            deltaTime = currentTime - previousTime;
            if (deltaTime < inverseFPMS)
                Thread.Sleep(inverseFPMS - deltaTime);
            previousTime = currentTime;
        }

        #endregion

        #region Game Engine Control Methods

        /// <summary>
        /// defines the virtual keys that are used for controls
        /// </summary>
        enum myKeys
        {
            thrust = 0x26,
            backThrust = 0x28,
            turnLeft = 0x25,
            turnRight = 0x27,
            space = 0x20,
            enter = 0x0D,
            help = 0x48,
            pause = 0x50,
            lClick = 0x01,
            rClick = 0x02,
            mClick = 0x03,
            sClick = 0x05,
            escape = 0x1B,
            CTRL = 0x11
        }        

        /// <summary>
        /// listens for mouse controls
        /// </summary>
        private void MousePressProcess()
        {
            Coordinates mouse = new Coordinates(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
            Game.playerMouseTurn(mouse);
            if (GetAsyncKeyState((int)myKeys.lClick) != 0)
                Game.playerShoot();
            if (GetAsyncKeyState((int)myKeys.rClick) != 0)
                Game.playerThrust();
            if (GetAsyncKeyState((int)myKeys.space) != 0)
                Game.playerWarp();
            if (GetAsyncKeyState((int)myKeys.help) != 0)
                help();
            if (GetAsyncKeyState((int)myKeys.pause) != 0)
                Pause();
            if (GetAsyncKeyState((int)myKeys.mClick) != 0 || GetAsyncKeyState((int)myKeys.sClick) != 0)
                if (Settings.useTargetAssist)
                    Game.playerTarget(mouse);
            if (GetAsyncKeyState((int)myKeys.escape) != 0)
                Quit();
        }

        /// <summary>
        /// listens for keyboard controls
        /// </summary>
        private void KeyPressProcess()
        {
            if (GetAsyncKeyState((int)myKeys.space) != 0)
                Game.playerShoot();
            if (GetAsyncKeyState((int)myKeys.turnLeft) != 0)
                Game.playerTurnLeft();
            if (GetAsyncKeyState((int)myKeys.turnRight) != 0)
                Game.playerTurnRight();
            if (GetAsyncKeyState((int)myKeys.thrust) != 0)
                Game.playerThrust();
            if (GetAsyncKeyState((int)myKeys.CTRL) != 0)
                Game.playerWarp();
            if (GetAsyncKeyState((int)myKeys.help) != 0)
                help();
            if (GetAsyncKeyState((int)myKeys.pause) != 0)
                Pause();
            if (GetAsyncKeyState((int)myKeys.lClick) != 0)
                if (Settings.useTargetAssist)
                    Game.playerTarget(new Coordinates(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y));
            if (GetAsyncKeyState((int)myKeys.escape) != 0)
                Quit();
        }

        #endregion

        #region GUI Display Methods

        /// <summary>
        /// makes the backstory scroll upwards "star wars style" at the beginning.
        /// </summary>
        private void scrollingStory()
        {
            string[] storyLines = System.IO.File.ReadAllLines(GameMedia.getDir["backStoryTXT"]);

            for (int i = 0; i < 20; i++)
                txt_Story.Text += "\n";
            foreach (string line in storyLines)
            {
                txt_Story.Text += '\n' + line;
                txt_Story.SelectionStart = txt_Story.Text.Length;
                txt_Story.ScrollToCaret();
                Thread.Sleep(40);
            }
            txt_Story.Text = System.IO.File.ReadAllText(GameMedia.getDir["backStoryTXT"]);
        }

        /// <summary>
        /// draws the little space ships that represent your lives.
        /// </summary>
        private void populateLivesPics()
        {
            livesPics = new List<PictureBox>();
            for (int i = 0; i < MainGame.lives; i++)
            {
                PictureBox lifePic = new PictureBox
                {
                    Name = "LifePic" + i,
                    Size = new Size(24, 24),
                    Location = new Point(i * 50 + 450, 10),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                //lifePic.ImageLocation = GameMedia.getDir["spaceShipPIC"];
                lifePic.Image = Image.FromFile(GameMedia.getDir["spaceShipPIC"]);
                livesPics.Add(lifePic);
            }
            foreach (PictureBox p in livesPics)
            {
                this.Controls.Add(p);
            }

            //PictureBox livesPic = new PictureBox();
            //livesPic.SetBounds(400, 30, 30, 30);
            //livesPic.Image = Image.FromFile(GameMedia.getDir["spaceShipPIC"]);
        }

        /// <summary>
        /// shows the high scores page
        /// </summary>
        private void showHighScores()
        {
            string[] scoreLines = System.IO.File.ReadAllLines(GameMedia.getDir["highScoresTXT"]);
            lbl_highScores.Text = System.IO.File.ReadAllText(GameMedia.getDir["highScoresTXT"]);
            lbl_highScores.Show();
            SortedDictionary<int, List<string>> scores = new SortedDictionary<int, List<string>>();
            foreach (string s in scoreLines)
            {
                string[] scoreAndName = Regex.Split(s, @"\s");
                int scoreKey = Int32.Parse(scoreAndName[0]);
                string scoreName = scoreAndName[1];
                for (int i = 2; i < scoreAndName.Length; i++)
                    scoreName += scoreAndName[i];
                if (!scores.ContainsKey(scoreKey))
                {
                    scores[scoreKey] = new List<string>();
                }
                scores[scoreKey].Add(scoreName);
            }
            if (!scores.ContainsKey(MainGame.score))
            {
                scores[MainGame.score] = new List<string>();
            }
            string currentName = Microsoft.VisualBasic.Interaction.InputBox("Type your name:", "Record your score!", "");
            if (currentName != "")
                scores[MainGame.score].Add(currentName);

            string newScores = "";
            foreach (KeyValuePair<int, List<string>> pair in scores.Reverse())
            {
                foreach(string s in pair.Value)
                {
                    newScores += pair.Key.ToString() + "    " + s + Environment.NewLine;
                }
            }
            lbl_highScores.Text = "HIGH SCORES:\n________________\n\n" + newScores;
            System.IO.File.WriteAllText(GameMedia.getDir["highScoresTXT"], newScores);
            lbl_Credits.Show();
        }

        /// <summary>
        /// shows 1 of 2 level loading screens (more should be added)
        /// </summary>
        private void showLevelSplash()
        {
            MainGame.isRunning = false;
            lvlSplashCycler++;
            if (lvlSplashCycler >= 2) lvlSplashCycler = 0;
            switch(lvlSplashCycler)
            {
                case 0:
                    pic_levelLoad0.Show();
                    txt_levelLoad0.Show();
                    break;
                case 1:
                    pic_levelLoad1.Show();
                    txt_levelLoad1.Show();
                    break;
            }
            
        }

        /// <summary>
        /// hides the level loading screens
        /// </summary>
        private void hideLevelSplash()
        {
            MainGame.isRunning = true;
            pic_levelLoad0.Hide();
            txt_levelLoad0.Hide();
            pic_levelLoad1.Hide();
            txt_levelLoad1.Hide();
        }

        #endregion

        #region GUI Control Methods

        /// <summary>
        /// asks if the user wants to quit
        /// </summary>
        private void Quit()
        {
            if (DialogResult.Yes == MessageBox.Show("Quit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000))
            {
                Exit();                
            }
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        private void Exit()
        {
            MainGame.needsToExit = true;
            GameSounds.CloseSFX();
            this.Close();
            Application.Exit();
        }

        /// <summary>
        /// This shows all key used in the game
        /// </summary>
        private void help()
        {
            if (MainGame.isRunning)
            {
                MainGame.stateDescription = "Help";
                MainGame.isRunning = false;
            }
        }

        /// <summary>
        /// This pauses game if user desires
        /// </summary>
        private void Pause()
        {
            if (MainGame.isRunning)
            {
                MainGame.stateDescription = "Paused";
                MainGame.isRunning = false;
            }
        }

        #endregion

        #region Form Events

        /// <summary>
        /// this happens when the form loads
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            Threads.scrollingStoryTask = Task.Run(() =>
                {
                    scrollingStory();
                });
        }

        /// <summary>
        /// this happens when the form closes.
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Settings.soundIsOn) GameSounds.CloseSFX();
            if (Settings.soundIsOn) GameSounds.CloseMusic();
            //Threads.initializeTasks();
            //Application.Exit();
        }

        #endregion
    }
}
