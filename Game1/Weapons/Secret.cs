using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Weapons
{
    class Secret : WeaponAbstract
    {
        // Secret

        //Field
        private Circle weaponDamageArea;
        private bool readyToExplode;
        private bool firstThrow;

        private Texture2D bombTexture;
        private Texture2D explosionTexture;
        private int widthOfSingleBombSprite;
        private int heightOfSingleBombSprite;
        private int widthOfSingleExplosionSprite;
        private int heightOfSingleExplosionSprite;

        private List<Weapons.Secret> secrets;
        private Player player;

        //Property
        //enemies must check this to see if they got hit
        public Circle WeaponDamageArea { get { return weaponDamageArea; } set { weaponDamageArea = value; } }
        public bool ReadyToExplode { get { return readyToExplode; } set { readyToExplode = value; } }




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
        public Secret(string name, int damage, int range, float cooldownTimer, float attackPhaseDurationTime, Rectangle WeaponSpawnLocation, Texture2D bombTexture, Texture2D explosionTexture, List<Weapons.Secret> secrets, Player player)
            : base(name, TypeOfWeapon.Secret, damage, range, cooldownTimer, attackPhaseDurationTime, WeaponSpawnLocation, bombTexture)
        {
            WeaponDamageArea = new Circle(0, 0, range);
            readyToExplode = false;
            firstThrow = true;
            TimeThatHasPassed = 20;

            this.bombTexture = bombTexture;
            this.explosionTexture = explosionTexture;

            widthOfSingleBombSprite = bombTexture.Width;
            heightOfSingleBombSprite = bombTexture.Height;
            widthOfSingleExplosionSprite = explosionTexture.Width;
            heightOfSingleExplosionSprite = explosionTexture.Height;

            this.secrets = secrets;
            this.player = player;
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
        private void DetermineWeaponDirection2(Player player)
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
            if (player.CurrentPlayerState == PlayerState.FaceUp)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 50;
                location.Y = player.PlayerHitBox.Y - 10;

                WeaponFacing = WeaponDirection.Up;
            }

            //make weapon face downward if player is facing down
            else if (player.CurrentPlayerState == PlayerState.FaceDown)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 25;
                location.Y = player.PlayerHitBox.Y;
                
                WeaponFacing = WeaponDirection.Down;
            }

            //makes the weapon face right if player is facing right
            else if (player.CurrentPlayerState == PlayerState.FaceRight)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 40;
                location.Y = player.PlayerHitBox.Y - 5;

                WeaponFacing = WeaponDirection.Right;
            }

            //make weapon face left if player is facing left
            else if (player.CurrentPlayerState == PlayerState.FaceLeft)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 50;
                location.Y = player.PlayerHitBox.Y - 5;

                WeaponFacing = WeaponDirection.Left;
            }
        }

        //Attack() method is defined in WeaponAbstract and can be called elsewhere to make this weapon attack



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
            if(firstThrow)
            {
                player.Score += 10000;
                firstThrow = false;
            }

            //if we spent longer in attack phase (bomb waiting to explode) than its specified duration, 
            //end the attack phase and start the explosion
            if (!readyToExplode)
            {
                DetermineWeaponDirection2(player);

                TimeThatHasPassed = 0;
                readyToExplode = true;
                secrets.Add(this);

                if (WeaponFacing == WeaponDirection.Right)
                {
                    location.X += 8;
                    location.Y -= 25;
                }
                else if (WeaponFacing == WeaponDirection.Left)
                {
                    location.X -= 10;
                    location.Y -= 25;
                }
            }
            else if (TimeThatHasPassed < AttackPhaseDurationTime)
            {
                if (TimeThatHasPassed < (AttackPhaseDurationTime * 0.8))
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
                        location.X += 2;
                        location.Y += 2;
                    }
                    else if (WeaponFacing == WeaponDirection.Left)
                    {
                        location.X -= 2;
                        location.Y += 3;
                    }

                    //bounce off edge of screen
                    if(location.X < 1)
                    {
                        WeaponFacing = WeaponDirection.Right;
                    }
                    else if(location.X > 749)
                    {
                        WeaponFacing = WeaponDirection.Left;
                    }

                    if (location.Y < 1)
                    {
                        WeaponFacing = WeaponDirection.Down;
                    }
                    else if (location.Y > 599)
                    {
                        WeaponFacing = WeaponDirection.Up;
                    }
                }
            }
            else
            {
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
            float bombLayer = location.Y / (float)650 + 0.01f;

            //make sure the layer is not equal or less than zero as layer will not show
            if (bombLayer <= 0)
            {
                bombLayer = 0.01f;
            }

            if (IsAttacking)
            {
                //draws the weapon to the player's directions and position
                spriteBatch.Draw(
                bombTexture,
                new Rectangle(location.X, location.Y, 50, 50),
                new Rectangle(0, 0, widthOfSingleBombSprite, heightOfSingleBombSprite),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                bombLayer);
            }
            else
            {
                //draws the weapon to the player's directions and position
                spriteBatch.Draw(
                bombTexture,
                new Rectangle(location.X + 40, location.Y + 40, 50, 50),
                new Rectangle(0, 0, widthOfSingleBombSprite, heightOfSingleBombSprite),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                bombLayer);
            }
        }
    }
}
