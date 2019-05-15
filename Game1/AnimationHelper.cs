using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class AnimationHelper
    {
        // Raman Mandavia
        // Alex Sarnese (Made text glow lol. That's it)
        // Cail Umbaugh

        // Fields
        private int helmetAnimationFrame;
        private int enterPromtFlickerTimer;
        private int weaponSelectIconFlickerTimer;
        private int deathAnimationFrame;
        private int spaceBackgroundAnimFrame;
        private float alpha; // Alpha channel value for the background fade-in
        private float textAlpha; // Alpha channel value for the text
        private bool helmetLoaded;
        private bool titleLoaded;
        private bool enterPromptFlicker; // If true, the enter prompt is drawn, otherwise it is not
        private bool displayEnterPrompt;
        private bool controlsDisplay;
        private bool weaponSelectIconFlicker;
        private bool displaySelectionScreen;
        private bool playDeathAnimation;
        private bool youAreReallyDeadNow;
        private bool displayGameOver;
        private int slotSelection;
        private int selection1;
        private int selection2;
        private int pauseTimer;
        private double gameOverScreenTimer;
        private Vector2 pointerPos;
        private Color instructionKeyColor;
        private Color instructionTextColor;
        private Color TextShadowColor;

        // Properties
        public int HelmetAnimationFrame
        { get { return helmetAnimationFrame; } }

        public int DeathAnimationFrame
        { get { return deathAnimationFrame; } }

        public float Alpha
        { get { return alpha; } }

        public float TextAlpha
        { get { return textAlpha; } }

        public bool HelmetLoaded
        { get { return helmetLoaded; } }

        public bool TitleLoaded
        { get { return titleLoaded; } }

        public bool EnterPromptFlicker
        { get { return enterPromptFlicker; } }

        public bool DisplayEnterPrompt
        { get { return displayEnterPrompt; } }

        public bool WeaponSelectIconFlicker
        { get { return weaponSelectIconFlicker; } }

        public bool ControlsDisplay
        {
            get { return controlsDisplay; }
            set { controlsDisplay = value; }
        }

        public bool DisplaySelectionScreen
        {
            get { return displaySelectionScreen; }
            set { displaySelectionScreen = value; }
        }

        public bool PlayDeathAnimation
        {
            get { return playDeathAnimation; }
            set { playDeathAnimation = value; }
        }

        public bool DisplayGameOver
        {
            get { return displayGameOver; }
            set { displayGameOver = value; }
        }

        public bool YouAreReallyDeadNow
        { get { return youAreReallyDeadNow; } }

        public int Selection1
        {
            get { return selection1; }
            set { selection1 = value; }
        }

        public int Selection2
        {
            get { return selection2; }
            set { selection2 = value; }
        }

        public int SlotSelection
        {
            get { return slotSelection; }
            set { slotSelection = value; }
        }

        public int PauseTimer
        {
            get { return pauseTimer; }
            set { pauseTimer = value; }
        }

        public double GameOverScreenTimer
        { get { return gameOverScreenTimer; } }

        // Constructor
        public AnimationHelper()
        {
            helmetAnimationFrame = 0;
            enterPromtFlickerTimer = 0;
            weaponSelectIconFlickerTimer = 0;
            spaceBackgroundAnimFrame = 0;
            slotSelection = 0;
            selection1 = 0;
            selection2 = 0;
            pauseTimer = 0;
            gameOverScreenTimer = 0;
            alpha = 1.3f;
            textAlpha = 2.0f;
            helmetLoaded = false;
            titleLoaded = false;
            enterPromptFlicker = false;
            displayEnterPrompt = false;
            controlsDisplay = false;
            weaponSelectIconFlicker = false;
            displaySelectionScreen = true;
            playDeathAnimation = false;
            youAreReallyDeadNow = false;
            displayGameOver = false;

            //This holds what color the shadow/outline should be
            TextShadowColor = Color.Multiply(Color.DarkBlue, 0.2f);
        }

        // Methods
        public void FadeInUpdate()
        {
            if (alpha > 0)
                alpha -= .003f;
        }

        public void FadeInTitleUpdate()
        {
            if (textAlpha > 0)
                textAlpha -= .01f;
            else
                titleLoaded = true;
        }

        public void UpdateHelmetAnimFrame()
        {
            if (helmetAnimationFrame < 16)
                helmetAnimationFrame++;
            else
                helmetLoaded = true;
        }

        public void UpdateSpaceBackground()
        {
            spaceBackgroundAnimFrame++;
            if (spaceBackgroundAnimFrame == 47)
                spaceBackgroundAnimFrame = 0;
        }

        public void UpdateEnterPromptFlicker()
        {
            enterPromtFlickerTimer++;
            displayEnterPrompt = true;
            if (enterPromtFlickerTimer == 5)
            {
                if (enterPromptFlicker == false)
                    enterPromptFlicker = true;
                else if (enterPromptFlicker == true)
                    enterPromptFlicker = false;
                enterPromtFlickerTimer = 0;
            }
        }

        public void UpdateWeaponSelectIconFlicker()
        {
            weaponSelectIconFlickerTimer++;
            if (weaponSelectIconFlickerTimer == 5)
            {
                if (weaponSelectIconFlicker == false)
                    weaponSelectIconFlicker = true;
                else if (weaponSelectIconFlicker == true)
                    weaponSelectIconFlicker = false;
                weaponSelectIconFlickerTimer = 0;
            }
        }

        public void UpdateDeathAnimationFrame()
        {
            if (playDeathAnimation)
            {
                deathAnimationFrame++;
                if (deathAnimationFrame == 15)
                {
                    deathAnimationFrame = 0;
                    playDeathAnimation = false;
                    youAreReallyDeadNow = true;
                }
            }
        }

        public void ResetFadeInTimer()
        {
            alpha = 1.3f;
        }

        public void ResetTitleFadeInTimer()
        {
            textAlpha = 2.0f;
        }

        public void ResetHelemtAnimationFrame()
        {
            helmetAnimationFrame = 0;
        }

        public void FadeInDraw(SpriteBatch sb, Texture2D image, Texture2D curtain)
        {
            //sb.Draw(image, new Rectangle(0, 0, 800, 650), Color.White);
            if (alpha <= 1.0f)
                sb.Draw(curtain, new Rectangle(0, 0, 800, 650), Color.Black * alpha);
            else
                sb.Draw(curtain, new Rectangle(0, 0, 800, 650), Color.Black);
        }

        public void DrawHelmetOverlayAnimation(SpriteBatch sb, Texture2D helmetOverlayAnim)
        {
            if (helmetAnimationFrame < 4)
            {
                sb.Draw(helmetOverlayAnim, new Rectangle(0, 0, 800, 650), new Rectangle(900 * helmetAnimationFrame, 0, 900, 900), Color.White);
            }
            else if (helmetAnimationFrame < 8)
            {
                sb.Draw(helmetOverlayAnim, new Rectangle(0, 0, 800, 650), new Rectangle(900 * (helmetAnimationFrame - 4), 900, 900, 900), Color.White);
            }
            else if (helmetAnimationFrame < 12)
            {
                sb.Draw(helmetOverlayAnim, new Rectangle(0, 0, 800, 650), new Rectangle(900 * (helmetAnimationFrame - 8), 1800, 900, 900), Color.White);
            }
            else if (helmetAnimationFrame >= 12)
            {
                sb.Draw(helmetOverlayAnim, new Rectangle(0, 0, 800, 650), new Rectangle(900 * (helmetAnimationFrame - 12), 2700, 900, 900), Color.White);
            }
        }

        public void DrawTitleFadeIn(SpriteBatch sb, SpriteFont titleFont)
        {
            if (textAlpha > 1.0f)
            {
                //add glow behind text
                for (int i = -5; i < 6; i = i + 2)
                {
                    for (int b = -5; b < 6; b = b + 2)
                    {
                        sb.DrawString(titleFont, "Venator", new Vector2(133 + i, 145 + b), Color.Multiply(Color.DarkRed, 0.075f) * 0.0f);
                    }
                }
                sb.DrawString(titleFont, "Venator", new Vector2(133, 145), Color.Firebrick * 0.0f);
            }
            else
            {
                //add glow behind text
                for (int i = -5; i < 6; i = i + 2)
                {
                    for (int b = -5; b < 6; b = b + 2)
                    {
                        sb.DrawString(titleFont, "Venator", new Vector2(133 + i, 145 + b), Color.Multiply(Color.DarkRed, 0.075f) * (1.0f - textAlpha));
                    }
                }
                sb.DrawString(titleFont, "Venator", new Vector2(133, 145), Color.Firebrick * (1.0f - textAlpha));
            }
        }

        public void DrawEnterPrompt(SpriteBatch sb, SpriteFont textFont)
        {
            if (displayEnterPrompt == true)
            {
                if (enterPromptFlicker == true)
                {
                    for (int i = -5; i < 6; i = i + 2)
                    {
                        for (int b = -5; b < 6; b = b + 2)
                        {
                            sb.DrawString(textFont, "Press Enter to Begin", new Vector2(257 + i, 430 + b), TextShadowColor);
                        }
                    }
                    sb.DrawString(textFont, "Press Enter to Begin", new Vector2(257, 430), Color.DeepSkyBlue);
                }
            }
        }

        // It's called controls display, but it will also show general instructions alongside that
        public void DrawControlsDisplay(SpriteBatch sb, SpriteFont textFont)
        {
            //Adds shadow/outline behind text so the stars do not make it hard to read text or make text look fancy
            for (int i = -5; i < 6; i = i + 2)
            {
                for (int b = -5; b < 6; b = b + 2)
                {
                    sb.DrawString(textFont, " - Use W,.A,.S, and D to move.", new Vector2(100 + i, 160 + b), TextShadowColor);
                    sb.DrawString(textFont, " - Use the arrow keys to use your equipped", new Vector2(100 + i, 180 + b), TextShadowColor);
                    sb.DrawString(textFont, "   weapon in the respective direction.", new Vector2(100 + i, 200 + b), TextShadowColor);
                    sb.DrawString(textFont, " - Use Q to pick up dropped weapons", new Vector2(100 + i, 220 + b), TextShadowColor);
                    sb.DrawString(textFont, "   and interact with certain parts of the", new Vector2(100 + i, 240 + b), TextShadowColor);
                    sb.DrawString(textFont, "   environment.", new Vector2(100 + i, 260 + b), TextShadowColor);
                    sb.DrawString(textFont, " - Use.LShift to switch between weapons.", new Vector2(100 + i, 280 + b), TextShadowColor);
                    sb.DrawString(textFont, " - Use the Spacebar to dodge.", new Vector2(100 + i, 300 + b), TextShadowColor);
                    sb.DrawString(textFont, " - Use the arrow keys and the Enter button", new Vector2(100 + i, 320 + b), TextShadowColor);
                    sb.DrawString(textFont, "   to select weapons on the next screen.", new Vector2(100 + i, 340 + b), TextShadowColor);
                    sb.DrawString(textFont, " - Fight your way through the derelict ship,", new Vector2(100 + i, 380 + b), TextShadowColor);
                    sb.DrawString(textFont, "   taking on waves of aliens, infected crew", new Vector2(100 + i, 400 + b), TextShadowColor);
                    sb.DrawString(textFont, "   members, and who knows what else,", new Vector2(100 + i, 420 + b), TextShadowColor);
                    sb.DrawString(textFont, "   to find and destroy the source.", new Vector2(100 + i, 440 + b), TextShadowColor);
                    sb.DrawString(textFont, "   Press Enter to Continue.", new Vector2(200 + i, 460 + b), TextShadowColor);
                }
            }

            //Draw the actual instructions with the player keys using a different color
            //These two variables are here for quick color changes to see what looks good
            instructionKeyColor = Color.DarkOrange;
            instructionTextColor = Color.DodgerBlue;


            sb.DrawString(textFont, " - Use    ,   ,   , and    to move.", new Vector2(100, 160), instructionTextColor);
            sb.DrawString(textFont, "W", new Vector2(178, 160), instructionKeyColor);
            sb.DrawString(textFont, "A", new Vector2(206, 160), instructionKeyColor);
            sb.DrawString(textFont, "S", new Vector2(232, 160), instructionKeyColor);
            sb.DrawString(textFont, "D", new Vector2(316, 160), instructionKeyColor);
            sb.DrawString(textFont, " - Use the                         to use your equipped", new Vector2(100, 180), instructionTextColor);
            sb.DrawString(textFont, "arrow keys", new Vector2(232, 180), instructionKeyColor);
            sb.DrawString(textFont, "   weapon in the respective direction.", new Vector2(100, 200), instructionTextColor);
            sb.DrawString(textFont, " - Use    to pick up dropped weapons", new Vector2(100, 220), instructionTextColor);
            sb.DrawString(textFont, "Q", new Vector2(177, 220), instructionKeyColor);
            sb.DrawString(textFont, "   and interact with certain parts of the", new Vector2(100, 240), instructionTextColor);
            sb.DrawString(textFont, "   environment.", new Vector2(100, 260), instructionTextColor);
            sb.DrawString(textFont, " - Use             to switch between weapons.", new Vector2(100, 280), instructionTextColor);
            sb.DrawString(textFont, "LShift", new Vector2(175, 280), instructionKeyColor);
            sb.DrawString(textFont, " - Use the                    to dodge.", new Vector2(100, 300), instructionTextColor);
            sb.DrawString(textFont, "Spacebar", new Vector2(230, 300), instructionKeyColor);
            sb.DrawString(textFont, " - Use the                         and the              button", new Vector2(100, 320), instructionTextColor);
            sb.DrawString(textFont, "arrow keys", new Vector2(232, 320), instructionKeyColor);
            sb.DrawString(textFont, "Enter", new Vector2(511, 320), instructionKeyColor);
            sb.DrawString(textFont, "   to select weapons on the next screen.", new Vector2(100, 340), instructionTextColor);
            sb.DrawString(textFont, " - Fight your way through the derelict ship,", new Vector2(100, 380), instructionTextColor);
            sb.DrawString(textFont, "   taking on waves of aliens, infected crew", new Vector2(100, 400), instructionTextColor);
            sb.DrawString(textFont, "   members, and who knows what else,", new Vector2(100, 420), instructionTextColor);
            sb.DrawString(textFont, "   to find and destroy the source.", new Vector2(100, 440), instructionTextColor);
            sb.DrawString(textFont, "   Press             to Continue.", new Vector2(200, 460), instructionTextColor);
            sb.DrawString(textFont, "Enter", new Vector2(310, 460), instructionKeyColor);
        }

        // This is a placeholder weapon select screen, catered for the weapons that we currently have available
        // Very much subject to change.
        public void DrawWeaponSelectScreen(SpriteBatch sb, SpriteFont textFont, SpriteFont largerTextFont, Texture2D displayItemBox, Texture2D weaponSelection1, Texture2D weaponSelection2, List<Weapons.WeaponAbstract> gunUnlocks, List<Weapons.WeaponAbstract> meleeUnlocks, List<Weapons.WeaponAbstract> bombUnlocks, List<Weapons.WeaponAbstract> currentRow, Weapons.WeaponAbstract weapon1, Weapons.WeaponAbstract weapon2)
        {
            //add glow behind text
            for (int i = -5; i < 6; i = i + 2)
            {
                for (int b = -5; b < 6; b = b + 2)
                {
                    sb.DrawString(largerTextFont, "Choose your weapons", new Vector2(110 + i, 160 + b), TextShadowColor);
                    sb.DrawString(textFont, "Guns", new Vector2(85 + i, 230 + b), TextShadowColor);
                    sb.DrawString(textFont, "Melee Weapons", new Vector2(85 + i, 350 + b), TextShadowColor);
                    sb.DrawString(textFont, "Bombs", new Vector2(85 + i, 470 + b), TextShadowColor);
                }
            }

            sb.DrawString(largerTextFont, "Choose your weapons", new Vector2(110, 160), Color.DodgerBlue);
            sb.DrawString(textFont, "Guns", new Vector2(85, 230), Color.RoyalBlue);
            sb.DrawString(textFont, "Melee Weapons", new Vector2(85, 350), Color.RoyalBlue);
            sb.DrawString(textFont, "Bombs", new Vector2(85, 470), Color.RoyalBlue);

            for (int i = 0; i < gunUnlocks.Count; i++)
            {
                if (weapon1 == gunUnlocks[i])
                    sb.Draw(displayItemBox, new Rectangle(65 + (114 * i), 240, 100, 100), Color.Blue);
                else if (weapon2 == gunUnlocks[i])
                    sb.Draw(displayItemBox, new Rectangle(65 + (114 * i), 240, 100, 100), Color.Red);
                else
                    sb.Draw(displayItemBox, new Rectangle(65 + (114 * i), 240, 100, 100), Color.White);
            }

            for (int i = 0; i < meleeUnlocks.Count; i++)
            {
                if (weapon1 == meleeUnlocks[i])
                    sb.Draw(displayItemBox, new Rectangle(65 + (114 * i), 360, 100, 100), Color.Blue);
                else if (weapon2 == meleeUnlocks[i])
                    sb.Draw(displayItemBox, new Rectangle(65 + (114 * i), 360, 100, 100), Color.Red);
                else
                    sb.Draw(displayItemBox, new Rectangle(65 + (114 * i), 360, 100, 100), Color.White);
            }

            for (int i = 0; i < bombUnlocks.Count; i++)
            {
                if (weapon1 == bombUnlocks[i])
                    sb.Draw(displayItemBox, new Rectangle(65 + (114 * i), 480, 100, 100), Color.Blue);
                else if (weapon2 == bombUnlocks[i])
                    sb.Draw(displayItemBox, new Rectangle(65 + (114 * i), 480, 100, 100), Color.Red);
                else
                    sb.Draw(displayItemBox, new Rectangle(65 + (114 * i), 480, 100, 100), Color.White);
            }

            if (currentRow == gunUnlocks)
                pointerPos = new Vector2(65 + (114 * slotSelection), 258);
            else if (currentRow == meleeUnlocks)
                pointerPos = new Vector2(65 + (114 * slotSelection), 378);
            else if (currentRow == bombUnlocks)
                pointerPos = new Vector2(65 + (114 * slotSelection), 498);

            DrawArrowFlicker(sb, largerTextFont);
        }

        /// <summary>
        /// makes arrow flicker to help see selection
        /// </summary>
        /// <param name="sb">ised for drawing</param>
        /// <param name="largerTextFont">used for text font</param>
        private void DrawArrowFlicker(SpriteBatch sb, SpriteFont largerTextFont)
        {
            if (weaponSelectIconFlicker == true)
            {
                //add glow behind text
                for (int i = -5; i < 6; i = i + 2)
                {
                    for (int b = -5; b < 6; b = b + 2)
                    {
                        sb.DrawString(largerTextFont, ">", new Vector2(pointerPos.X + i, pointerPos.Y + b), TextShadowColor);
                        sb.DrawString(largerTextFont, ">", new Vector2(pointerPos.X + i, pointerPos.Y + 30 + b), TextShadowColor);
                    }
                }
                sb.DrawString(largerTextFont, ">", pointerPos, Color.RoyalBlue);
                sb.DrawString(largerTextFont, ">", new Vector2(pointerPos.X, pointerPos.Y + 30), Color.RoyalBlue);
            }

        }

        // Draws the animation for the death of the player
        public void DrawPlayerDeath(SpriteBatch sb, Texture2D playerDeath)
        {
            if (playDeathAnimation == true)
            {
                if (deathAnimationFrame < 8)
                {
                    sb.Draw(
                        playerDeath,
                        new Rectangle(0, 0, 800, 650),
                        new Rectangle(900 * (deathAnimationFrame / 2), 0, 900, 900),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        1.0f);
                }
                else if (deathAnimationFrame >= 8)
                {
                    sb.Draw(
                        playerDeath,
                        new Rectangle(0, 0, 800, 650),
                        new Rectangle(900 * ((deathAnimationFrame - 8) / 2), 900, 900, 900),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        1.0f);
                }
            }
        }

        // Draws the animated space background
        public void DrawAnimatedBackground(SpriteBatch sb, Texture2D animatedBackground)
        {
            if (spaceBackgroundAnimFrame < 24)
            {
                sb.Draw(
                    animatedBackground,
                    new Rectangle(0, 0, 800, 650),
                    new Rectangle(900 * (spaceBackgroundAnimFrame / 6), 0, 900, 900),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    1.0f);
            }
            else if (spaceBackgroundAnimFrame >= 24)
            {
                sb.Draw(
                    animatedBackground,
                    new Rectangle(0, 0, 800, 650),
                    new Rectangle(900 * ((spaceBackgroundAnimFrame - 24) / 6), 900, 900, 900),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    1.0f);
            }
        }

        // This is what is displayed if the Game reaches the GameEnd state and the player is dead
        public void DrawGameOver(SpriteBatch sb, Texture2D helmetOverlayDeath, SpriteFont largerTextFont, SpriteFont textFont, GameTime gameTime)
        {
            if (displayGameOver)
            {
                if (gameOverScreenTimer <= 8)
                {
                    gameOverScreenTimer += gameTime.ElapsedGameTime.TotalSeconds;
                }

                //add glow behind text
                for (int i = -5; i < 6; i = i + 2)
                {
                    for (int b = -5; b < 6; b = b + 2)
                    {
                        sb.DrawString(largerTextFont, "Mission", new Vector2(200 + i, 200 + b), Color.Multiply(Color.Red, 0.02f) * ((float)(gameOverScreenTimer / 8)));
                        sb.DrawString(largerTextFont, "Failed", new Vector2(250 + i, 250 + b), Color.Multiply(Color.Red, 0.02f) * ((float)(gameOverScreenTimer / 8)));
                    }
                }

                sb.Draw(helmetOverlayDeath, new Rectangle(0, 0, 800, 650), Color.White * (float)(gameOverScreenTimer / 8));
                sb.DrawString(largerTextFont, "Mission", new Vector2(200, 200), Color.Red * (float)(gameOverScreenTimer / 8));
                sb.DrawString(largerTextFont, "Failed", new Vector2(250, 250), Color.Red * (float)(gameOverScreenTimer / 8));

                if (gameOverScreenTimer > 8)
                {
                    //add glow behind text
                    for (int i = -5; i < 6; i = i + 2)
                    {
                        for (int b = -5; b < 6; b = b + 2)
                        {
                            sb.DrawString(textFont, "Press Enter to Return", new Vector2(220 + i, 300 + b), Color.Red * 0.04f);
                            sb.DrawString(textFont, "to the Main Menu", new Vector2(235 + i, 320 + b), Color.Red * 0.04f);
                        }
                    }

                    sb.DrawString(textFont, "Press Enter to Return", new Vector2(220, 300), Color.Red);
                    sb.DrawString(textFont, "to the Main Menu", new Vector2(235, 320), Color.Red);
                }
            }
        }

        // Draws the game's final screen, showing the scoring system and final score
        public void DrawGameComplete(SpriteBatch sb, int gameTimerScore, int damageTaken, int score, int highScore, Texture2D helmetOverlay, SpriteFont textFont, SpriteFont largerTextFont)
        {
            int finalScore = score - gameTimerScore - damageTaken;

            for (int i = -5; i < 6; i = i + 2)
            {
                for (int b = -5; b < 6; b = b + 2)
                {
                    sb.DrawString(largerTextFont, "Mission", new Vector2(325 + i, 120 + b), TextShadowColor);
                    sb.DrawString(largerTextFont, "Complete", new Vector2(295 + i, 150 + b), TextShadowColor);
                    sb.DrawString(textFont, "Time:", new Vector2(310 + i, 270 + b), TextShadowColor);
                    sb.DrawString(textFont, "Damage:", new Vector2(310 + i, 310 + b), TextShadowColor);
                    sb.DrawString(textFont, "Kills:", new Vector2(310 + i, 350 + b), TextShadowColor);
                    sb.DrawString(textFont, "Final Score:", new Vector2(260 + i, 410 + b), TextShadowColor);
                    sb.DrawString(textFont, "High Score:", new Vector2(260 + i, 450 + b), TextShadowColor);
                    sb.DrawString(textFont, "-" + gameTimerScore, new Vector2(430 + i, 270 + b), TextShadowColor);
                    sb.DrawString(textFont, "-" + damageTaken, new Vector2(450 + i, 310 + b), TextShadowColor);
                    sb.DrawString(textFont, "" + score, new Vector2(430 + i, 350 + b), TextShadowColor);
                    sb.DrawString(textFont, "" + finalScore, new Vector2(470 + i, 410 + b), TextShadowColor);
                    sb.DrawString(textFont, "" + highScore, new Vector2(470 + i, 450 + b), TextShadowColor);
                    sb.DrawString(textFont, "Press Enter to Return", new Vector2(240 + i, 500 + b), TextShadowColor);
                    sb.DrawString(textFont, "to the Main Menu", new Vector2(270 + i, 515 + b), TextShadowColor);
                }
            }

            sb.DrawString(largerTextFont, "Mission", new Vector2(325, 120), Color.DodgerBlue);
            sb.DrawString(largerTextFont, "Complete", new Vector2(295, 150), Color.DodgerBlue);
            sb.DrawString(textFont, "Time:", new Vector2(310, 270), Color.DodgerBlue);
            sb.DrawString(textFont, "Damage:", new Vector2(310, 310), Color.DodgerBlue);
            sb.DrawString(textFont, "Kills:", new Vector2(310, 350), Color.DodgerBlue);
            sb.DrawString(textFont, "Final Score:", new Vector2(260, 410), Color.DarkOrange);
            sb.DrawString(textFont, "High Score:", new Vector2(260, 450), Color.MonoGameOrange);
            sb.DrawString(textFont, "-" + gameTimerScore, new Vector2(430, 270), Color.DodgerBlue);
            sb.DrawString(textFont, "-" + damageTaken, new Vector2(450, 310), Color.DodgerBlue);
            sb.DrawString(textFont, "" + score, new Vector2(430, 350), Color.DodgerBlue);
            sb.DrawString(textFont, "" + finalScore, new Vector2(470, 410), Color.DarkOrange);
            sb.DrawString(textFont, "" + highScore, new Vector2(470, 450), Color.MonoGameOrange);
            sb.DrawString(textFont, "Press Enter to Return", new Vector2(240, 500), Color.DodgerBlue);
            sb.DrawString(textFont, "to the Main Menu", new Vector2(270, 515), Color.DodgerBlue);
            sb.Draw(helmetOverlay, new Rectangle(0, 0, 800, 650), Color.White);
        }

        // Resets all values, except for helmet and title loaded, so that it goes to the main menu and skips the into
        public void Reset()
        {
            helmetAnimationFrame = 0;
            enterPromtFlickerTimer = 0;
            weaponSelectIconFlickerTimer = 0;
            slotSelection = 0;
            selection1 = 0;
            selection2 = 0;
            pauseTimer = 0;
            gameOverScreenTimer = 0;
            alpha = 1.3f;
            textAlpha = 2.0f;
            helmetLoaded = true;
            titleLoaded = true;
            enterPromptFlicker = false;
            displayEnterPrompt = false;
            controlsDisplay = false;
            weaponSelectIconFlicker = false;
            displaySelectionScreen = true;
            playDeathAnimation = false;
            youAreReallyDeadNow = false;
            displayGameOver = false;
        }
    }
}
