using System.Windows.Forms;
namespace Asteroids2._0
{
    partial class Form1
    {
        int speedcheck = 0;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            try
            {
                base.Dispose(disposing);
            }
            catch { }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.sel_mouse = new System.Windows.Forms.RadioButton();
            this.sel_keys = new System.Windows.Forms.RadioButton();
            this.chk_debug = new System.Windows.Forms.CheckBox();
            this.txt_Story = new System.Windows.Forms.RichTextBox();
            this.chk_Damper = new System.Windows.Forms.CheckBox();
            this.chk_potShot = new System.Windows.Forms.CheckBox();
            this.lbl_score = new System.Windows.Forms.Label();
            this.lbl_level = new System.Windows.Forms.Label();
            this.chk_targetAssist = new System.Windows.Forms.CheckBox();
            this.pic_levelLoad0 = new System.Windows.Forms.PictureBox();
            this.txt_levelLoad0 = new System.Windows.Forms.Label();
            this.txt_levelLoad1 = new System.Windows.Forms.Label();
            this.pic_levelLoad1 = new System.Windows.Forms.PictureBox();
            this.lbl_LvlScr = new System.Windows.Forms.Label();
            this.lbl_highScores = new System.Windows.Forms.Label();
            this.lbl_Credits = new System.Windows.Forms.Label();
            this.chk_useSound = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chk_fancyColl = new System.Windows.Forms.CheckBox();
            this.lbl_survMiners = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_levelLoad0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_levelLoad1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(37, 685);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "\'p\' for pause";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(37, 710);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "\'h\' for help";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Copperplate Gothic Bold", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Gold;
            this.label3.Location = new System.Drawing.Point(293, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(771, 107);
            this.label3.TabIndex = 4;
            this.label3.Text = "METEOROIDS";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.WindowText;
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Gold;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.button1.Location = new System.Drawing.Point(564, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(251, 68);
            this.button1.TabIndex = 5;
            this.button1.Text = "READY!";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // sel_mouse
            // 
            this.sel_mouse.AutoSize = true;
            this.sel_mouse.Checked = true;
            this.sel_mouse.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.sel_mouse.Location = new System.Drawing.Point(178, 235);
            this.sel_mouse.Name = "sel_mouse";
            this.sel_mouse.Size = new System.Drawing.Size(93, 17);
            this.sel_mouse.TabIndex = 6;
            this.sel_mouse.TabStop = true;
            this.sel_mouse.Text = "Mouse Control";
            this.sel_mouse.UseVisualStyleBackColor = true;
            // 
            // sel_keys
            // 
            this.sel_keys.AutoSize = true;
            this.sel_keys.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.sel_keys.Location = new System.Drawing.Point(178, 258);
            this.sel_keys.Name = "sel_keys";
            this.sel_keys.Size = new System.Drawing.Size(106, 17);
            this.sel_keys.TabIndex = 6;
            this.sel_keys.Text = "Keyboard Control";
            this.sel_keys.UseVisualStyleBackColor = true;
            // 
            // chk_debug
            // 
            this.chk_debug.AutoSize = true;
            this.chk_debug.Cursor = System.Windows.Forms.Cursors.Cross;
            this.chk_debug.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.chk_debug.Location = new System.Drawing.Point(178, 438);
            this.chk_debug.Name = "chk_debug";
            this.chk_debug.Size = new System.Drawing.Size(170, 17);
            this.chk_debug.TabIndex = 7;
            this.chk_debug.Text = "Show bounding boxes (debug)";
            this.chk_debug.UseVisualStyleBackColor = true;
            // 
            // txt_Story
            // 
            this.txt_Story.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txt_Story.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_Story.Cursor = System.Windows.Forms.Cursors.Cross;
            this.txt_Story.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Story.ForeColor = System.Drawing.Color.Gold;
            this.txt_Story.Location = new System.Drawing.Point(444, 235);
            this.txt_Story.Name = "txt_Story";
            this.txt_Story.ReadOnly = true;
            this.txt_Story.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txt_Story.ShortcutsEnabled = false;
            this.txt_Story.Size = new System.Drawing.Size(499, 543);
            this.txt_Story.TabIndex = 8;
            this.txt_Story.Text = "";
            // 
            // chk_Damper
            // 
            this.chk_Damper.AutoSize = true;
            this.chk_Damper.Checked = true;
            this.chk_Damper.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Damper.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.chk_Damper.Location = new System.Drawing.Point(178, 318);
            this.chk_Damper.Name = "chk_Damper";
            this.chk_Damper.Size = new System.Drawing.Size(141, 17);
            this.chk_Damper.TabIndex = 9;
            this.chk_Damper.Text = "Air resistance in space...";
            this.chk_Damper.UseVisualStyleBackColor = true;
            // 
            // chk_potShot
            // 
            this.chk_potShot.AutoSize = true;
            this.chk_potShot.Checked = true;
            this.chk_potShot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_potShot.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.chk_potShot.Location = new System.Drawing.Point(178, 358);
            this.chk_potShot.Name = "chk_potShot";
            this.chk_potShot.Size = new System.Drawing.Size(221, 17);
            this.chk_potShot.TabIndex = 9;
            this.chk_potShot.Text = "More and faster rocks if you\'re a bad shot";
            this.chk_potShot.UseVisualStyleBackColor = true;
            // 
            // lbl_score
            // 
            this.lbl_score.AutoSize = true;
            this.lbl_score.BackColor = System.Drawing.Color.Transparent;
            this.lbl_score.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_score.ForeColor = System.Drawing.Color.Gold;
            this.lbl_score.Location = new System.Drawing.Point(92, 9);
            this.lbl_score.Name = "lbl_score";
            this.lbl_score.Size = new System.Drawing.Size(73, 25);
            this.lbl_score.TabIndex = 10;
            this.lbl_score.Text = "Score";
            // 
            // lbl_level
            // 
            this.lbl_level.AutoSize = true;
            this.lbl_level.BackColor = System.Drawing.Color.Transparent;
            this.lbl_level.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_level.ForeColor = System.Drawing.Color.Gold;
            this.lbl_level.Location = new System.Drawing.Point(12, 9);
            this.lbl_level.Name = "lbl_level";
            this.lbl_level.Size = new System.Drawing.Size(69, 25);
            this.lbl_level.TabIndex = 10;
            this.lbl_level.Text = "Level";
            // 
            // chk_targetAssist
            // 
            this.chk_targetAssist.AutoSize = true;
            this.chk_targetAssist.Checked = true;
            this.chk_targetAssist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_targetAssist.ForeColor = System.Drawing.SystemColors.Control;
            this.chk_targetAssist.Location = new System.Drawing.Point(178, 398);
            this.chk_targetAssist.Name = "chk_targetAssist";
            this.chk_targetAssist.Size = new System.Drawing.Size(100, 17);
            this.chk_targetAssist.TabIndex = 11;
            this.chk_targetAssist.Text = "Targeting assist";
            this.chk_targetAssist.UseVisualStyleBackColor = true;
            // 
            // pic_levelLoad0
            // 
            this.pic_levelLoad0.BackColor = System.Drawing.Color.Transparent;
            this.pic_levelLoad0.Image = ((System.Drawing.Image)(resources.GetObject("pic_levelLoad0.Image")));
            this.pic_levelLoad0.Location = new System.Drawing.Point(949, 134);
            this.pic_levelLoad0.Name = "pic_levelLoad0";
            this.pic_levelLoad0.Size = new System.Drawing.Size(374, 503);
            this.pic_levelLoad0.TabIndex = 12;
            this.pic_levelLoad0.TabStop = false;
            this.pic_levelLoad0.Visible = false;
            // 
            // txt_levelLoad0
            // 
            this.txt_levelLoad0.AutoSize = true;
            this.txt_levelLoad0.BackColor = System.Drawing.Color.Transparent;
            this.txt_levelLoad0.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_levelLoad0.ForeColor = System.Drawing.Color.Gold;
            this.txt_levelLoad0.Location = new System.Drawing.Point(39, 90);
            this.txt_levelLoad0.Name = "txt_levelLoad0";
            this.txt_levelLoad0.Size = new System.Drawing.Size(502, 528);
            this.txt_levelLoad0.TabIndex = 14;
            this.txt_levelLoad0.Text = resources.GetString("txt_levelLoad0.Text");
            this.txt_levelLoad0.Visible = false;
            // 
            // txt_levelLoad1
            // 
            this.txt_levelLoad1.AutoSize = true;
            this.txt_levelLoad1.BackColor = System.Drawing.Color.Transparent;
            this.txt_levelLoad1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_levelLoad1.ForeColor = System.Drawing.Color.Gold;
            this.txt_levelLoad1.Location = new System.Drawing.Point(39, 90);
            this.txt_levelLoad1.Name = "txt_levelLoad1";
            this.txt_levelLoad1.Size = new System.Drawing.Size(319, 288);
            this.txt_levelLoad1.TabIndex = 14;
            this.txt_levelLoad1.Text = "\r\n\r\n\r\n\r\n\r\n\r\nTry to hit the meteoroids dead on\r\n(in the center). If you hit one at" +
    " a \r\nglancing angle, it will split into\r\nmany more peices than usual,\r\nand they " +
    "will be moving\r\nfaster, too.";
            this.txt_levelLoad1.Visible = false;
            // 
            // pic_levelLoad1
            // 
            this.pic_levelLoad1.BackColor = System.Drawing.Color.Transparent;
            this.pic_levelLoad1.Image = ((System.Drawing.Image)(resources.GetObject("pic_levelLoad1.Image")));
            this.pic_levelLoad1.Location = new System.Drawing.Point(968, 134);
            this.pic_levelLoad1.Name = "pic_levelLoad1";
            this.pic_levelLoad1.Size = new System.Drawing.Size(374, 503);
            this.pic_levelLoad1.TabIndex = 12;
            this.pic_levelLoad1.TabStop = false;
            this.pic_levelLoad1.Visible = false;
            // 
            // lbl_LvlScr
            // 
            this.lbl_LvlScr.AutoSize = true;
            this.lbl_LvlScr.BackColor = System.Drawing.Color.Transparent;
            this.lbl_LvlScr.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_LvlScr.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbl_LvlScr.Location = new System.Drawing.Point(584, 9);
            this.lbl_LvlScr.Name = "lbl_LvlScr";
            this.lbl_LvlScr.Size = new System.Drawing.Size(108, 37);
            this.lbl_LvlScr.TabIndex = 15;
            this.lbl_LvlScr.Text = "label4";
            this.lbl_LvlScr.Visible = false;
            // 
            // lbl_highScores
            // 
            this.lbl_highScores.AutoSize = true;
            this.lbl_highScores.BackColor = System.Drawing.Color.Transparent;
            this.lbl_highScores.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_highScores.ForeColor = System.Drawing.Color.Gold;
            this.lbl_highScores.Location = new System.Drawing.Point(117, 90);
            this.lbl_highScores.Name = "lbl_highScores";
            this.lbl_highScores.Size = new System.Drawing.Size(682, 216);
            this.lbl_highScores.TabIndex = 14;
            this.lbl_highScores.Text = "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nhighScores highScores highScores highScores highScores highScores" +
    " ";
            this.lbl_highScores.Visible = false;
            // 
            // lbl_Credits
            // 
            this.lbl_Credits.AutoSize = true;
            this.lbl_Credits.BackColor = System.Drawing.Color.Black;
            this.lbl_Credits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Credits.ForeColor = System.Drawing.Color.Gold;
            this.lbl_Credits.Location = new System.Drawing.Point(736, 18);
            this.lbl_Credits.Name = "lbl_Credits";
            this.lbl_Credits.Size = new System.Drawing.Size(606, 676);
            this.lbl_Credits.TabIndex = 16;
            this.lbl_Credits.Text = resources.GetString("lbl_Credits.Text");
            this.lbl_Credits.Visible = false;
            // 
            // chk_useSound
            // 
            this.chk_useSound.AutoSize = true;
            this.chk_useSound.Checked = true;
            this.chk_useSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_useSound.Cursor = System.Windows.Forms.Cursors.Cross;
            this.chk_useSound.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.chk_useSound.Location = new System.Drawing.Point(178, 474);
            this.chk_useSound.Name = "chk_useSound";
            this.chk_useSound.Size = new System.Drawing.Size(92, 17);
            this.chk_useSound.TabIndex = 7;
            this.chk_useSound.Text = "Sound effects";
            this.chk_useSound.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label4.Location = new System.Drawing.Point(37, 735);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "\'esc\' exit";
            // 
            // chk_fancyColl
            // 
            this.chk_fancyColl.AutoSize = true;
            this.chk_fancyColl.Checked = true;
            this.chk_fancyColl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_fancyColl.Cursor = System.Windows.Forms.Cursors.Cross;
            this.chk_fancyColl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.chk_fancyColl.Location = new System.Drawing.Point(179, 509);
            this.chk_fancyColl.Name = "chk_fancyColl";
            this.chk_fancyColl.Size = new System.Drawing.Size(101, 17);
            this.chk_fancyColl.TabIndex = 7;
            this.chk_fancyColl.Text = "Fancy Collisions";
            this.chk_fancyColl.UseVisualStyleBackColor = true;
            // 
            // lbl_survMiners
            // 
            this.lbl_survMiners.AutoSize = true;
            this.lbl_survMiners.BackColor = System.Drawing.Color.Transparent;
            this.lbl_survMiners.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_survMiners.ForeColor = System.Drawing.Color.Gold;
            this.lbl_survMiners.Location = new System.Drawing.Point(211, 9);
            this.lbl_survMiners.Name = "lbl_survMiners";
            this.lbl_survMiners.Size = new System.Drawing.Size(195, 25);
            this.lbl_survMiners.TabIndex = 10;
            this.lbl_survMiners.Text = "Surviving Miners:";
            this.lbl_survMiners.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1354, 733);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_Credits);
            this.Controls.Add(this.chk_targetAssist);
            this.Controls.Add(this.txt_Story);
            this.Controls.Add(this.lbl_level);
            this.Controls.Add(this.lbl_survMiners);
            this.Controls.Add(this.lbl_score);
            this.Controls.Add(this.chk_potShot);
            this.Controls.Add(this.chk_Damper);
            this.Controls.Add(this.chk_fancyColl);
            this.Controls.Add(this.chk_useSound);
            this.Controls.Add(this.chk_debug);
            this.Controls.Add(this.sel_keys);
            this.Controls.Add(this.sel_mouse);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pic_levelLoad1);
            this.Controls.Add(this.pic_levelLoad0);
            this.Controls.Add(this.lbl_highScores);
            this.Controls.Add(this.txt_levelLoad1);
            this.Controls.Add(this.txt_levelLoad0);
            this.Controls.Add(this.lbl_LvlScr);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asteroids Version 3.0 By Galen Cochrane and Ted Delezene";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_levelLoad0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_levelLoad1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// This function starts the game, it needs to be here to make sure the graphics get painted in the correct place.
        /// </summary>
        private void StartGame()
        {
            if (speedcheck < 1)
            {
                //this.Load += new System.EventHandler(this.button1_Click);
                this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
                //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
                speedcheck++;

                if (sel_mouse.Checked)
                    Settings.useMouse = true;
                else
                    Settings.useMouse = false;

                if (chk_debug.Checked)
                    Settings.debug = true;
                else
                    Settings.debug = false;

                if (chk_Damper.Checked)
                    Settings.shipUseDamper = true;
                else
                    Settings.shipUseDamper = false;

                if (chk_potShot.Checked)
                    Settings.meteorPotshotPenalty = true;
                else
                    Settings.meteorPotshotPenalty = false;

                if (chk_targetAssist.Checked)
                    Settings.useTargetAssist = true;
                else
                    Settings.useTargetAssist = false;

                if (chk_useSound.Checked)
                    Settings.soundIsOn = true;
                else
                    Settings.soundIsOn = false;

                if (chk_fancyColl.Checked)
                {
                    Settings.useFancyCollisions = true;                    
                }
                else
                {
                    Settings.useFancyCollisions = false;
                    Settings.bulletHeight = 20;
                    Settings.bulletWidth = 20;
                    Settings.skinnyMeteors = false;
                }

                label3.Hide();
                button1.Hide();
                sel_keys.Hide();
                sel_mouse.Hide();
                chk_debug.Hide();
                chk_Damper.Hide();
                chk_potShot.Hide();
                chk_targetAssist.Hide();
                txt_Story.Hide();
                lbl_score.Show();
                lbl_level.Show();
                lbl_survMiners.Show();
                chk_useSound.Hide();
                chk_fancyColl.Hide();
            }
        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private RadioButton sel_mouse;
        private RadioButton sel_keys;
        private CheckBox chk_debug;
        private RichTextBox txt_Story;
        private CheckBox chk_Damper;
        private CheckBox chk_potShot;
        private Label lbl_score;
        private Label lbl_level;
        private CheckBox chk_targetAssist;
        private PictureBox pic_levelLoad0;
        private Label txt_levelLoad0;
        private Label txt_levelLoad1;
        private PictureBox pic_levelLoad1;
        private Label lbl_LvlScr;
        private Label lbl_highScores;
        private Label lbl_Credits;
        private CheckBox chk_useSound;
        private Label label4;
        private CheckBox chk_fancyColl;
        private Label lbl_survMiners;
    }
}

