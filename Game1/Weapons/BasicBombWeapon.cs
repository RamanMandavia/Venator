﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Weapons
{
    class BasicBombWeapon : WeaponAbstract
    {
        // Alex Sarnese
        // Raman Mandavia

        //Field
        private Circle weaponDamageArea;
        private float explosionTime;
        private bool readyToExplode;
        private bool exploded;

        private Texture2D bombTexture;
        private Texture2D explosionTexture;
        private int widthOfSingleBombSprite;
        private int heightOfSingleBombSprite;
        private int widthOfSingleExplosionSprite;
        private int heightOfSingleExplosionSprite;
        private int bombAnimationFrame;

        private float downwardDelta;
        private float horizontalDelta;

        Rectangle explosionRectangle;

        //Property
        //enemies must check this to see if they got hit
        public Circle WeaponDamageArea { get { return weaponDamageArea; } set { weaponDamageArea = value; } }
        

        //Constructor

        /// <summary>
        /// Creates instance of a basic bomb weapon with given base stats
        /// </summary>
        /// <param name="name">Name of the bomb weapon</param>
        /// <param name="damage">Amount of damage the bomb deals in one hit</param>
        /// <param name="range">How big the radius of the bomb's explosion is in pixels</param>
        /// <param name="cooldownTimer">How much time in seconds that the bomb cannot be used between attacks</param>
        /// <param name="attackPhaseDurationTime">How long the bomb's takes before it explodes in seconds</param>
        ///<param name="WeaponSpawnLocation">Location for the bomb's spawn</param>
        /// <param name="weaponTexture">Texture of the bomb</param>
        public BasicBombWeapon(string name, int damage, int range, float cooldownTimer, float attackPhaseDurationTime, Rectangle WeaponSpawnLocation, Texture2D bombTexture, Texture2D explosionTexture)
            : base(name, TypeOfWeapon.Bomb, damage, range, cooldownTimer, attackPhaseDurationTime, WeaponSpawnLocation, bombTexture)
        {
            WeaponDamageArea = new Circle(0, 0, range);
            explosionTime = 0.8f;
            readyToExplode = false;
            exploded = false;
            TimeThatHasPassed = 20;

            this.bombTexture = bombTexture;
            this.explosionTexture = explosionTexture;
            bombAnimationFrame = 0;

            widthOfSingleBombSprite = bombTexture.Width / 4;
            heightOfSingleBombSprite = bombTexture.Height;
            widthOfSingleExplosionSprite = explosionTexture.Width / 4;
            heightOfSingleExplosionSprite = explosionTexture.Height;

            explosionRectangle = new Rectangle(weaponDamageArea.X - Range * 2 - 20, weaponDamageArea.Y - Range * 2 - 5, Range * 4 + 10, Range * 4 + 10);
        }



        //Methods



        /// <summary>
        /// Updates the weapon's location and where it faces 
        /// Calls up the Attacking method used for dealing damage
        /// Needs to be called in update loop in Game1
        /// </summary>
        /// <param name="listOfEnemies">the list of enemies to check for collusion with</param>
        /// <param name="player">the player to check for location of</param>
        /// <param name="gameTime">the timer to be used to keep track of cooldown time</param>
        public override void Update(List<EnemiesAbstract> listOfEnemies, Player player, GameTime gameTime)
        {
            //stores how much time has passed since last update and stores it in a variable
            TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds;
            
            //checks to see if the weapon should go into attacking phase
            if (IsAttacking)
            {
                //change weapon's direction to face the mouse and 
                //progress through attacking sequence
                Attacking(listOfEnemies, player);
            }
            else
            {
                //If player is null, then this weapon is dropped and is not being held by the player. 
                //Thus does nothing to weapon's position
                if (player != null)
                {
                    //If it is being held, update location to the carrier's location (move to player's hands)
                    DetermineWeaponDirection(player);
                }
            }
        }

        /// <summary>
        /// for bomb to be thrown in correct direction
        /// </summary>
        /// <param name="player">player to get direction from</param>
        private void DetermineWeaponDirectionWithoutMoving(Player player)
        {
            //make weapon face upward if player is facing upward
            if (player.CurrentPlayerState == PlayerState.FaceUp)
            {
                WeaponFacing = WeaponDirection.Up;
            }

            //make weapon face downward if player is facing down
            else if (player.CurrentPlayerState == PlayerState.FaceDown)
            {
                WeaponFacing = WeaponDirection.Down;
            }

            //makes the weapon face right if player is facing right
            else if (player.CurrentPlayerState == PlayerState.FaceRight)
            {
                WeaponFacing = WeaponDirection.Right;
            }

            //make weapon face left if player is facing left
            else if (player.CurrentPlayerState == PlayerState.FaceLeft)
            {
                WeaponFacing = WeaponDirection.Left;
            }
            
        }

        /// <summary>
        /// makes weapon face the same direction as player
        /// </summary>
        /// <param name="playerState">the direction player is facing</param>
        public override void DetermineWeaponDirection(Player player)
        {
            //make weapon face upward if player is facing upward
            if (player.PreviousPlayerState == PlayerState.FaceUp)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 50;
                location.Y = player.PlayerHitBox.Y - 10;

                WeaponFacing = WeaponDirection.Up;
            }

            //make weapon face downward if player is facing down
            else if (player.PreviousPlayerState == PlayerState.FaceDown)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 25;
                location.Y = player.PlayerHitBox.Y;

                WeaponFacing = WeaponDirection.Down;
            }

            //makes the weapon face right if player is facing right
            else if (player.PreviousPlayerState == PlayerState.FaceRight)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 40;
                location.Y = player.PlayerHitBox.Y - 5;
                
                WeaponFacing = WeaponDirection.Right;
            }

            //make weapon face left if player is facing left
            else if (player.PreviousPlayerState == PlayerState.FaceLeft)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 50;
                location.Y = player.PlayerHitBox.Y - 5;
                
                WeaponFacing = WeaponDirection.Left;
            }
        }

        /// <summary>
        ///                 ATTACKING METHOD FOR BASIC BOMB WEAPONS
        /// 
        /// creates collusion circle where character was before that other enemies must check to see if they take damage.
        /// If they do, they need to get amount of damage from this weapon's Damage property
        /// Also sees if palyer damaged himself
        /// </summary>
        /// <param name="listOfEnemies">the list of enemies to check for collusion with</param>
        /// <param name="player">the player to use for both location of weapon and if player damaged himself</param>
        protected override void Attacking(List<EnemiesAbstract> listOfEnemies, Player player)
        {

            UpdateAnimationFrames();

            //if we spent longer in attack phase (bomb waiting to explode) than its specified duration, 
            //end the attack phase and start the explosion
            if (!readyToExplode)
            {
                TimeThatHasPassed = 0;
                readyToExplode = true;
                
                DetermineWeaponDirectionWithoutMoving(player);

                if (WeaponFacing == WeaponDirection.Right)
                {
                    location.X += 8;
                    location.Y -= 25;

                    downwardDelta = 90/(AttackPhaseDurationTime / 0.0169f);
                    horizontalDelta = (195 * AttackPhaseDurationTime) / (AttackPhaseDurationTime / 0.0169f);
                }
                else if (WeaponFacing == WeaponDirection.Left)
                {
                    location.X -= 10;
                    location.Y -= 25;

                    downwardDelta = 90 / (AttackPhaseDurationTime / 0.0169f);
                    horizontalDelta = (195 * AttackPhaseDurationTime) / (AttackPhaseDurationTime / 0.0169f);
                }
            }
            else if (TimeThatHasPassed < AttackPhaseDurationTime)
            {
                
                if (TimeThatHasPassed < (AttackPhaseDurationTime * 0.6))
                {
                    if (WeaponFacing == WeaponDirection.Up)
                    {
                        location.Y -= 4;
                    }
                    else if (WeaponFacing == WeaponDirection.Down)
                    {
                        location.Y += 4;
                    }
                    else if (WeaponFacing == WeaponDirection.Right)
                    {
                        location.X += (int)horizontalDelta;
                        location.Y += (int)(downwardDelta + 0.5);
                    }
                    else if (WeaponFacing == WeaponDirection.Left)
                    {
                        location.X -= (int)horizontalDelta;
                        location.Y += (int)(downwardDelta + 0.5);
                    }
                }
            }
            else
            {
                //bomb explodes and creates a damaging area for one frame
                weaponDamageArea.X = location.X + 70;
                weaponDamageArea.Y = location.Y + 30;

                //loops through list of enemies and see if it hit one of them
                //if so, damages the enemy
                for (int b = 0; b < listOfEnemies.Count; b++)
                {
                    if (weaponDamageArea.Intersects(listOfEnemies[b].EnemyHitBox))
                    {
                        listOfEnemies[b].TakeDamage(Damage);
                    }

                }

                //checks to see if it damaged player with explosion
                if (weaponDamageArea.Intersects(player.PlayerHitBox))
                {
                    player.TakeDamage(Damage*2);
                }

                //ends attacking phase and removes explosion (bomb returns to player location at next Update frame)
                IsAttacking = false;
                readyToExplode = false;
                exploded = true;
                TimeThatHasPassed = 0;

                explosionRectangle = new Rectangle(weaponDamageArea.X - Range * 2 - 20, weaponDamageArea.Y - Range * 2 - 5, Range * 4 + 10, Range * 4 + 10);
            }
        }


        /// <summary>
        /// Draws the weapon on the screen. 
        /// Needs to be called in the draw loop in Game1
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
        public override void Draw(SpriteBatch spriteBatch, Player player)
        {
            //set the layer for bomb and explosion
            float bombLayer = location.Y / (float)650;
            float explosionLayer = explosionRectangle.Y / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (bombLayer <= 0)
            {
                bombLayer = 0.01f;
            }
            if (explosionLayer <= 0)
            {
                explosionLayer = 0.01f;
            }


            //If player is null, then this weapon is dropped and is not being held by the player. 
            //Thus does not change the weapon's direction
            if (player != null)
            {
                //if this is the held weapon, draw it to the screen
                if (player.CurrentWeapon == this)
                {
                    //draws the weapon to the player's directions and position
                    spriteBatch.Draw(
                    bombTexture,
                    new Vector2(location.X, location.Y),
                    new Rectangle(widthOfSingleBombSprite * bombAnimationFrame, 0, widthOfSingleBombSprite, heightOfSingleBombSprite),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    bombLayer);
                }
                else if (readyToExplode)
                {
                    spriteBatch.Draw(
                    bombTexture,
                    new Vector2(location.X, location.Y),
                    new Rectangle(widthOfSingleBombSprite * 3, 0, widthOfSingleBombSprite, heightOfSingleBombSprite),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    bombLayer);
                }

                //draw the explosion of bomb
                if (exploded)
                {
                    if (TimeThatHasPassed < explosionTime / 4)
                    {
                        spriteBatch.Draw(
                        explosionTexture,
                        explosionRectangle,
                        new Rectangle(0, 0, widthOfSingleExplosionSprite, heightOfSingleExplosionSprite),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        explosionLayer);
                    }
                    else if (TimeThatHasPassed < explosionTime / 2)
                    {
                        spriteBatch.Draw(
                        explosionTexture,
                        explosionRectangle,
                        new Rectangle(widthOfSingleExplosionSprite, 0, widthOfSingleExplosionSprite, heightOfSingleExplosionSprite),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        explosionLayer);
                    }
                    else if (TimeThatHasPassed < (explosionTime * 3) / 4)
                    {
                        spriteBatch.Draw(
                        explosionTexture,
                        explosionRectangle,
                        new Rectangle(widthOfSingleExplosionSprite * 2, 0, widthOfSingleExplosionSprite, heightOfSingleExplosionSprite),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        explosionLayer);
                    }
                    else if (TimeThatHasPassed < explosionTime)
                    {
                        spriteBatch.Draw(
                        explosionTexture,
                        explosionRectangle,
                        new Rectangle(widthOfSingleExplosionSprite * 3, 0, widthOfSingleExplosionSprite, heightOfSingleExplosionSprite),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        explosionLayer);
                    }
                    else
                    {
                        exploded = false;
                    }
                }
            }

            //updates the weapons rectangle to its location and draws it to the screen
            else
            {
                spriteBatch.Draw(
                bombTexture,
                new Vector2(location.X, location.Y),
                new Rectangle(0, 0, widthOfSingleBombSprite, heightOfSingleBombSprite),
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                bombLayer);
            }
        }

        // Method to control updating animation frames, so that the bomb doesn't look "active" when in the player's hand
        private void UpdateAnimationFrames()
        {
            //change the bomb's animation every 1/4th second
            if (TimeThatHasPassed % 1 < 0.25)
            {
                bombAnimationFrame = 0;
            }
            else if (TimeThatHasPassed % 1 < 0.5)
            {
                bombAnimationFrame = 1;
            }
            else if (TimeThatHasPassed % 1 < 0.75)
            {
                bombAnimationFrame = 2;
            }
            else
            {
                bombAnimationFrame = 3;
            }
        }

    }
}
