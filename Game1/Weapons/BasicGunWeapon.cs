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
    class BasicGunWeapon : WeaponAbstract
    {
        // Alex Sarnese
        // Raman Mandavia

        //Field
        private List<BasicGunBullet> bulletList;
        private Texture2D bulletTexture;

        //Additional required textures for proper directional drawing
        private Texture2D faceDown;
        private Texture2D faceUp;
        private Texture2D faceRight;
        private Texture2D faceLeft;
        private int widthOfSingleSpriteHorizontal;
        private int widthOfSingleSpriteVertical;

        private bool fired;
        private int gunAnimationFrame;


        //Property
        //enemies must check the bullet's rectangles to see if they got hit. Also, remove bullet from list if it hit enemy or wall
        public List<BasicGunBullet> ListOfBullets { get { return bulletList; } set { bulletList = value; } }



        //Constructor

        /// <summary>
        /// Creates instance of a basic gun weapon with given base stats
        /// </summary>
        /// <param name="name">Name of the gun weapon</param>
        /// <param name="damage">Amount of damage the gun's bullet deals in one hit</param>
        /// <param name="range">How far the gun's bullet can travel in pixels</param>
        /// <param name="cooldownTimer">How much time in seconds that the gun must rest between firing</param>
        /// <param name="attackPhaseDurationTime">How long the gun attack takes in seconds</param>
        ///<param name="WeaponSpawnLocation">Location for the gun's spawn</param>
        /// <param name="weaponTexture">Texture of the gun</param>
        public BasicGunWeapon(string name, int damage, int range, float cooldownTimer, float attackPhaseDurationTime, Rectangle WeaponSpawnLocation, Texture2D faceDown,
            Texture2D faceUp, Texture2D faceRight, Texture2D faceLeft, Texture2D bulletTexture)
            : base(name, TypeOfWeapon.Gun, damage, range, cooldownTimer, attackPhaseDurationTime, WeaponSpawnLocation, faceRight)
        {
            TimeThatHasPassed = 10;
            this.faceDown = faceDown;
            this.faceUp = faceUp;
            this.faceRight = faceRight;
            this.faceLeft = faceLeft;
            fired = false;

            widthOfSingleSpriteHorizontal = faceLeft.Width / 4;
            widthOfSingleSpriteVertical = faceUp.Width / 4;

            this.bulletTexture = bulletTexture;
            bulletList = new List<BasicGunBullet>();
            gunAnimationFrame = 0;
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

            //loops through list of bullets made by this gun
            for (int i = 0; i < bulletList.Count; i++)
            {
                //bullet moves in their direction
                bulletList[i].Update();

                //stores x and y component of the current bullet
                int x = bulletList[i].BulletCircle.X;
                int y = bulletList[i].BulletCircle.Y;

                //removes the bullet if it goes out of bounds
                if (x < 0 || x > 800 || y < 0 || y > 650)
                {
                    bulletList.RemoveAt(i);
                }
                else
                {
                    //loops through list of enemies and see if it hit one of them
                    //if so, damages the enemy and removes the bullet
                    for (int b = 0; b < listOfEnemies.Count; b++)
                    {
                        bool stillaround = true;
                        if(listOfEnemies[b] is BossMech)
                        {
                            BossMech d = (BossMech)listOfEnemies[b];
                            if (bulletList[i].BulletCircle.Intersects(d.SheildHitBox))
                            {
                                stillaround = false;
                                bulletList.RemoveAt(i);
                                break;
                            }
                        }

                        if (bulletList[i].BulletCircle.Intersects(listOfEnemies[b].EnemyHitBox) && listOfEnemies[b].Health > 0 && stillaround)
                        {
                            listOfEnemies[b].TakeDamageButNoKnockback(Damage);
                            bulletList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }


            //checks to see if the weapon should go into attacking phase
            if (IsAttacking)
            {
                // change weapon's direction to face the mouse and 
                //progress through attacking sequence
                DetermineWeaponDirection(player);
                Attacking(listOfEnemies, player);

            }
            else
            {
                //If player is null, then this weapon is dropped and is not being held by the player. 
                //Thus does nothing to weapon's position
                if (player != null && player.CurrentWeapon == this)
                {
                    DetermineWeaponDirection(player);
                }
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
                //update location to the carrier's center top part
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) + 10;
                location.Y = player.PlayerHitBox.Y;

                WeaponFacing = WeaponDirection.Up;
            }

            //make weapon face downward if player is facing down
            else if (player.CurrentPlayerState == PlayerState.FaceDown)
            {
                //update location to the carrier's center bottom
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) + 5;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height);

                WeaponFacing = WeaponDirection.Down;
            }

            //makes the weapon face right if player is facing right
            else if (player.CurrentPlayerState == PlayerState.FaceRight)
            {
                //update location to the carrier's center right side
                location.X = player.PlayerHitBox.X + player.PlayerHitBox.Width + 20;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) - 15;

                WeaponFacing = WeaponDirection.Right;
            }

            //make weapon face left if player is facing left
            else if (player.CurrentPlayerState == PlayerState.FaceLeft)
            {
                //update location to the carrier's center left side
                location.X = player.PlayerHitBox.X - 20;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) - 15;

                WeaponFacing = WeaponDirection.Left;
            }
        }

        //Attack() method is defined in WeaponAbstract and can be called elsewhere to make this weapon attack



        /// <summary>
        ///                 ATTACKING METHOD FOR BASIC GUN WEAPONS
        /// 
        /// creates traveling collusion circle in front of character and checks if hit enemy.
        /// If they do, calls up the enemy's TakenDamage() method 
        /// </summary>
        /// <param name="listOfEnemies">the list of enemies to check for collusion with</param>
        /// <param name="player">the player to use for location of weapon</param>
        protected override void Attacking(List<EnemiesAbstract> listOfEnemies, Player player)
        {
            //shoots one bullet and exits attacking method
            if (!fired)
            {
                //creates a bullet going in direction that the gun is facing
                ListOfBullets.Add(new BasicGunBullet(WeaponFacing, location, bulletTexture));
                fired = true;
                TimeThatHasPassed = 0;
            }
            else if (TimeThatHasPassed < AttackPhaseDurationTime)
            {
                //do nothing here to force a pause between shots 
            }
            else
            {
                //ends attacking method
                IsAttacking = false;
                fired = false;
                TimeThatHasPassed = 0;
            }
        }


        /// <summary>
        /// Draws the weapon on the screen. 
        /// Needs to be called in the draw loop in Game1
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
        public override void Draw(SpriteBatch spriteBatch, Player player)
        {
            //set the layer for gun
            float weaponLayer = location.Y / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (weaponLayer <= 0)
            {
                weaponLayer = 0.01f;
            }

            //draws every bullet to the screen first before the weapon
            foreach (BasicGunBullet bullet in bulletList)
            {
                bullet.Draw(spriteBatch);
            }

            if (fired)
            {
                //show the gun firing
                if (TimeThatHasPassed < AttackPhaseDurationTime * 0.25f)
                {
                    gunAnimationFrame = 1;
                }
                else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5f)
                {
                    gunAnimationFrame = 2;
                }
                else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75f)
                {
                    gunAnimationFrame = 3;
                }
                else
                {
                    gunAnimationFrame = 0;
                }
            }

            //If player is null, then this weapon is dropped and is not being held by the player. 
            //Thus does not change the weapon's direction
            if (player != null)
            {
                //if this is the held weapon, draw it to the screen
                if (player.CurrentWeapon == this)
                {
                    //draws the weapon to the player's directions and position
                    if (WeaponFacing == WeaponDirection.Up)
                    {
                        spriteBatch.Draw(
                        faceUp,
                        new Vector2(location.X - 65, location.Y - 55),
                        new Rectangle(widthOfSingleSpriteVertical * gunAnimationFrame, 0, widthOfSingleSpriteVertical, faceUp.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        weaponLayer);
                    }
                    else if (WeaponFacing == WeaponDirection.Left)
                    {
                        spriteBatch.Draw(
                        faceLeft,
                        new Vector2(location.X - 65, location.Y - 60),
                        new Rectangle(widthOfSingleSpriteVertical * gunAnimationFrame, 0, widthOfSingleSpriteHorizontal, faceLeft.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        weaponLayer);
                    }
                    else if (WeaponFacing == WeaponDirection.Right)
                    {
                        spriteBatch.Draw(
                        faceRight,
                        new Vector2(location.X - 65, location.Y - 60),
                        new Rectangle(widthOfSingleSpriteVertical * gunAnimationFrame, 0, widthOfSingleSpriteHorizontal, faceRight.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        weaponLayer);
                    }
                    //If reached here, the weapon must be facing downward
                    else
                    {
                        spriteBatch.Draw(
                        faceDown,
                        new Vector2(location.X - 65, location.Y - 85),
                        new Rectangle(widthOfSingleSpriteVertical * gunAnimationFrame, 0, widthOfSingleSpriteVertical, faceDown.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        weaponLayer);
                    }
                }
            }

            //updates the weapons rectangle to its location and draws it to the screen
            else
            {
                    spriteBatch.Draw(
                    faceRight,
                    new Vector2(location.X, location.Y),
                    new Rectangle(0, 0, widthOfSingleSpriteHorizontal, faceRight.Height),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    1.0f,
                    SpriteEffects.None,
                    weaponLayer);
            }
        }


    }

    class BasicGunBullet
    {
        // Alex Sarnese
        // Raman Mandavia
        //field
        private Circle bulletCircle;
        private WeaponDirection bulletDirection;
        private Texture2D bulletTexture;
        private int bulletTextureWidth;
        private int bulletTextureHeight;
        private int bulletAnimationFrame;
        private int distanceTraveled;

        //property
        public Circle BulletCircle { get { return bulletCircle; } set { bulletCircle = value; } }


        //constructor
        /// <summary>
        /// creates instance of bullet facing gun's direction at time of it firing
        /// </summary>
        /// <param name="bulletDirection">gun's direction at time of firing bullet</param>
        public BasicGunBullet(WeaponDirection bulletDirection, Point gunLocation, Texture2D bulletTexture)
        {
            //create bullet collusion as a 4 by 4 square
            bulletCircle.X = gunLocation.X;
            bulletCircle.Y = gunLocation.Y;
            bulletCircle.Radius = 1;
            this.bulletDirection = bulletDirection;

            this.bulletTexture = bulletTexture;
            bulletTextureWidth = bulletTexture.Width / 4;
            bulletTextureHeight = bulletTexture.Height;
            bulletAnimationFrame = 0;
            distanceTraveled = 0;
        }


        //Method

        /// <summary>
        /// Makes bullet travel in a straight line in direction specified at creation of bullet 
        /// </summary>
        public void Update()
        {

            //moves bullet in direction that is is facing
            if (bulletDirection == WeaponDirection.Up)
            {
                bulletCircle.Y = bulletCircle.Y - 10;
            }
            else if (bulletDirection == WeaponDirection.Down)
            {
                bulletCircle.Y = bulletCircle.Y + 10;
            }
            else if (bulletDirection == WeaponDirection.Right)
            {
                bulletCircle.X = bulletCircle.X + 10;
            }
            else
            {
                bulletCircle.X = bulletCircle.X - 10;
            }

            distanceTraveled += 10;
        }


        /// <summary>
        /// Draws the bullet on the screen. 
        /// Needs to be called in the draw loop in Game1
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
        public void Draw(SpriteBatch spriteBatch)
        {

            //set the layer for bullet
            float bulletLayer = bulletCircle.Y / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (bulletLayer <= 0)
            {
                bulletLayer = 0.01f;
            }

            //change the bullet's animation after set distance
            if (distanceTraveled < 75)
            {
                bulletAnimationFrame = 0;
            }
            else if (distanceTraveled < 150)
            {
                bulletAnimationFrame = 1;
            }
            else if (distanceTraveled < 225)
            {
                bulletAnimationFrame = 2;
            }
            else
            {
                bulletAnimationFrame = 3;
            }


            spriteBatch.Draw(
            bulletTexture,
            new Rectangle(bulletCircle.X - 50, bulletCircle.Y - 50, 100, 100),
            new Rectangle(bulletTextureWidth * bulletAnimationFrame, 0, bulletTextureWidth, bulletTextureHeight),
            Color.White,
            0.0f,
            Vector2.Zero,
            SpriteEffects.None,
            bulletLayer);
        }

    }
}
