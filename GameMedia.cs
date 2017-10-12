/*****************************************************************
 * Galen Cochrane 11/20/2014
 * GameMedia.cs
 * Provides a dictionary for media filepath retreival, covering
 * images and sounds.  Used by many classes, so I made it static.
 * Makes it easier to update game graphics and sounds - this is
 * the only class that needs to be modified if file paths change.
 * Inspired by: (This has been completely redone)
 * 
 * Ted Delezene 11/17/2012
 * GameGraphics.cs
 * This class defines the images that will be used for the various
 * objects.
 *****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Asteroids2._0
{
    static class GameMedia
    {
        static public Dictionary<string, string> getDir = new Dictionary<string, string>();     // dictionary of file paths
        static public Dictionary<string, Image> getImg = new Dictionary<string, Image>();       // dictionary of pre-loaded imaages
        static public void InitializeMedia()
        {
            string MediaDirectory = System.IO.Directory.GetCurrentDirectory().Replace("\\bin\\Debug", "\\Graphics");
            getDir.Add("asteroidPIC0", MediaDirectory + "\\Asteroid1s.png");
            getDir.Add("asteroidPIC1", MediaDirectory + "\\Asteroid2s.png");
            getDir.Add("asteroidPIC2", MediaDirectory + "\\Asteroid3s.png");
            getDir.Add("spaceShipPIC", MediaDirectory + "\\shipWorkGimp.png");           
            getDir.Add("alienStockPIC", MediaDirectory + "\\alienWorkGimp.png");
            getDir.Add("alienHunterPIC", MediaDirectory + "\\GreenAlienWorkGimp.png");
            getDir.Add("alienSeekerPIC", MediaDirectory + "\\OrangeAlienWorkGimp.png");
            getDir.Add("alienCallerPIC", MediaDirectory + "\\PinkAlienWorkGimp.png");
            getDir.Add("shieldPIC", MediaDirectory + "\\shield.png");
            getDir.Add("callSigPIC", MediaDirectory + "\\callSignal.png");
            getDir.Add("laser0PIC", MediaDirectory + "\\weaponWork.png");
            getDir.Add("laser1PIC", MediaDirectory + "\\RedWeaponWork.png");
            getDir.Add("laser2PIC", MediaDirectory + "\\GreenWeaponWork.png");
            getDir.Add("laser3PIC", MediaDirectory + "\\SeekerWeaponWork.png");
            getDir.Add("laser0SND", MediaDirectory + "\\laser00Simple.wav");
            getDir.Add("laser1SND", MediaDirectory + "\\laser01Simple.wav");
            getDir.Add("laser2SND", MediaDirectory + "\\laser02Simple.wav");
            getDir.Add("explosionPIC", MediaDirectory + "\\Explosion");
            getDir.Add("explosion0SND", MediaDirectory + "\\Grenade.wav");
            getDir.Add("explosion1SND", MediaDirectory + "\\Gun_Shot.wav");
            getDir.Add("shieldsDownSND", MediaDirectory + "\\shieldsDown.wav");
            getDir.Add("shieldsUpSND", MediaDirectory + "\\shieldsUp.wav");
            getDir.Add("callSigSND", MediaDirectory + "\\callSignal.wav");
            getDir.Add("extraLifeSND", MediaDirectory + "\\PowerUp.wav");
            getDir.Add("warpSND", MediaDirectory + "\\Warp.wav");
            getDir.Add("backgroundSND", MediaDirectory + "\\Black Vortex mono.wav");
            getDir.Add("winSongSND", MediaDirectory + "\\Take a Chance simple.wav");
            getDir.Add("loseSongSND", MediaDirectory + "\\Vortex simple.wav");
            getDir.Add("backStoryTXT", MediaDirectory + "\\story.txt");
            getDir.Add("highScoresTXT", MediaDirectory + "\\highScores.txt");

            // These images are used a lot.  I didn't want to read the disk anew for each object that used them, so I open them once,
            // then store the opened images in the handy-dandy dictionary.
            getImg.Add("asteroidPIC0", Image.FromFile(getDir["asteroidPIC0"]));
            getImg.Add("asteroidPIC1", Image.FromFile(getDir["asteroidPIC1"]));
            getImg.Add("asteroidPIC2", Image.FromFile(getDir["asteroidPIC2"]));
            getImg.Add("spaceShipPIC", Image.FromFile(getDir["spaceShipPIC"]));
            getImg.Add("alienStockPIC", Image.FromFile(getDir["alienStockPIC"]));
            getImg.Add("alienHunterPIC", Image.FromFile(getDir["alienHunterPIC"]));
            getImg.Add("alienSeekerPIC", Image.FromFile(getDir["alienSeekerPIC"]));
            getImg.Add("alienCallerPIC", Image.FromFile(getDir["alienCallerPIC"]));
            getImg.Add("shieldPIC", Image.FromFile(getDir["shieldPIC"]));
            getImg.Add("callSigPIC", Image.FromFile(getDir["callSigPIC"]));
            getImg.Add("laser0PIC", Image.FromFile(getDir["laser0PIC"]));
            getImg.Add("laser1PIC", Image.FromFile(getDir["laser1PIC"]));
            getImg.Add("laser2PIC", Image.FromFile(getDir["laser2PIC"]));
            getImg.Add("laser3PIC", Image.FromFile(getDir["laser3PIC"]));
        }
    }
}
