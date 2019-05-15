using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Enemies;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1.Weapons
{
    class RocketLauncherWeapon : WeaponAbstract
    {
        //created by Will Walrond
        //implementation by Alex Sarnese
        
        Texture2D bulletTexture;
        List<RocketLauncherBullet> bulletList;
        Texture2D explosion;
        bool fired;


        //Additional required textures for proper directional drawing
        private Texture2D faceDown;
        private Texture2D faceUp;
        private Texture2D faceRight;
        private Texture2D faceLeft;
        private int widthOfSingleSpriteHorizontal;
        private int widthOfSingleSpriteVertical;

        private int gunAnimationFrame;

        public RocketLauncherWeapon(Rectangle WeaponSpawnLocation, Texture2D faceDown, Texture2D faceUp, 
            Texture2D faceRight, Texture2D faceLeft, Texture2D bulletTexture, Texture2D explosionTexture) 
            : base("Rocket Launcher", TypeOfWeapon.Gun, 30, 80, 15f, 0.3f, WeaponSpawnLocation, faceRight)
        {
            //range is the size of the explosion. Not how far the rocket travels

            this.faceDown = faceDown;
            this.faceUp = faceUp;
            this.faceRight = faceRight;
            this.faceLeft = faceLeft;

            fired = false;
            TimeThatHasPassed = 30;

            widthOfSingleSpriteHorizontal = faceLeft.Width / 4;
            widthOfSingleSpriteVertical = faceUp.Width / 4;

            bulletList = new List<RocketLauncherBullet>();
            this.bulletTexture = bulletTexture;
            explosion = explosionTexture;

            gunAnimationFrame = 0;
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
                if (bulletList[i].ReadyToBeRemoved || (x < 0 || x > 800 || y < 0 || y > 650))
                {
                    bulletList.RemoveAt(i);
                }
                else
                {
                    bool stillhere = true;
                    //loops through list of enemies and see if it hit one of them
                    //if so, explodes and damages enemies and player
                    for (int b = 0; b < listOfEnemies.Count; b++)
                    {
                        if (listOfEnemies[b] is BossMech)
                        {
                            BossMech d = (BossMech)listOfEnemies[b];
                            if (bulletList[i].BulletCircle.Intersects(d.SheildHitBox))
                            {
                                stillhere = false;
                                bulletList.RemoveAt(i);
                                break;
                            }
                        }


                        if (bulletList[i].BulletCircle.Intersects(listOfEnemies[b].EnemyHitBox) && listOfEnemies[b].Health > 0 && stillhere)
                        {

                            if (!bulletList[i].Exploding)
                            {
                                bulletList[i].Exploding = true;
                            }
                            else if(bulletList[i].ExplosionTime > 0)
                            {
                                listOfEnemies[b].TakeDamage(Damage);
                            }
                        }
                    }


                    if (stillhere)
                    {
                        //checks to see if it damaged player with explosion
                        if (bulletList[i].BulletCircle.Intersects(player.PlayerHitBox))
                        {
                            if (bulletList[i].Exploding)
                            {
                                player.TakeDamage((int)(Damage * 2));
                            }
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

        protected override void Attacking(List<EnemiesAbstract> listOfEnemies, Player player)
        {

            //shoots one bullet and exits attacking method
            if (!fired)
            {
                if (WeaponFacing == WeaponDirection.Up)
                {
                    player.Move(0, 30);
                    location.Y = player.PlayerHitBox.Y;
                }
                else if (WeaponFacing == WeaponDirection.Down)
                {
                    player.Move(0, -30);
                    location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height);
                }
                else if (WeaponFacing == WeaponDirection.Left)
                {
                    player.Move(30, 0);
                    location.X = player.PlayerHitBox.X - 20;
                }
                //weapon must be facing right when this is reached
                else
                {
                    player.Move(-30, 0);
                    location.X = player.PlayerHitBox.X + player.PlayerHitBox.Width + 20;
                }

                //creates a bullet going in direction that the gun is facing
                bulletList.Add(new RocketLauncherBullet(WeaponFacing, location, bulletTexture, explosion, Range));
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


        public override void Draw(SpriteBatch spriteBatch, Player player)
        {
            //set the layer for weapon
            float weaponLayer = location.Y / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (weaponLayer <= 0)
            {
                weaponLayer = 0.01f;
            }

            //loops through list of bullets made by this gun
            for (int i = 0; i < bulletList.Count; i++)
            {
                bulletList[i].Draw(spriteBatch);
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

    class RocketLauncherBullet
    {
        private int bulletspeed;
        private int explosionTime;
        private Circle bulletCircle;
        private WeaponDirection bulletDirection;
        private Texture2D bulletTexture;
        private Texture2D explosionTexture;
        private bool exploding;
        private bool readyToBeRemoved;
        private float rotation;
        private int xOffset;
        private int yOffset;
        private int explosionRange;
        private int distanceTraveled;
        private int bulletAnimationFrame;

        public bool ReadyToBeRemoved { get { return readyToBeRemoved; }}

        public bool Exploding { get { return exploding; } set { exploding = value; } }

        public int ExplosionTime { get { return explosionTime; } }

        public Circle BulletCircle { get { return bulletCircle; } }

        public RocketLauncherBullet(WeaponDirection bulletDirection, Point gunLocation, Texture2D bulletTexture, Texture2D explosion, int radius)
        {
            explosionRange = radius;
            bulletCircle = new Circle(gunLocation.X, gunLocation.Y, 8);
            this.bulletDirection = bulletDirection;
            bulletspeed = 7;
            this.bulletTexture = bulletTexture;
            explosionTexture = explosion;
            explosionTime = 40;
            readyToBeRemoved = false;
            exploding = false;
            distanceTraveled = 0;
            bulletAnimationFrame = 0;

            if (bulletDirection == WeaponDirection.Down)
            {
                rotation = 1.57f;
                xOffset =  80;
                yOffset = -90;
            }
            else if (bulletDirection == WeaponDirection.Left)
            {
                rotation = 3.14f;
                xOffset = 25;
                yOffset = 80;
            } 
            else if (bulletDirection == WeaponDirection.Up)
            {
                rotation = 4.71f;
                xOffset = -80;
                yOffset = 25;
            }
            //facing right
            else
            {
                rotation = 0f;
                xOffset = -50;
                yOffset = -80;
            }
        }

        public void Update()
        {
            //moves rocket in desired direction
            if (!exploding)
            {
                if (bulletDirection == WeaponDirection.Up)
                {
                    bulletCircle.Y = bulletCircle.Y - bulletspeed;
                }
                else if (bulletDirection == WeaponDirection.Down)
                {
                    bulletCircle.Y = bulletCircle.Y + bulletspeed;
                }
                else if (bulletDirection == WeaponDirection.Right)
                {
                    bulletCircle.X = bulletCircle.X + bulletspeed;
                }
                else
                {
                    bulletCircle.X = bulletCircle.X - bulletspeed;
                }

                distanceTraveled += bulletspeed;
            }
            else
            {
                explosionTime--;
                bulletCircle.Radius = explosionRange;
            }
        }


        /// <summary>
        /// Draws the bullet on the screen. 
        /// Needs to be called in the draw loop in Game1
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            float explosionLayer = bulletCircle.Y / (float)650;

            if (explosionLayer <= 0)
            {
                explosionLayer = 0.01f;
            }


            //show the gun firing
            if (distanceTraveled < 40)
            {
                bulletAnimationFrame = 0;
            }
            else if (distanceTraveled % 160 < 80)
            {
                bulletAnimationFrame = 1;
            }
            else if (distanceTraveled % 160 < 120)
            {
                bulletAnimationFrame = 2;
            }
            else
            {
                bulletAnimationFrame = 3;
            }

            if (!exploding)
            {
               
                spriteBatch.Draw(
                bulletTexture,
                new Rectangle(bulletCircle.X + xOffset, bulletCircle.Y + yOffset, 160, 160),
                new Rectangle((bulletTexture.Width / 4) * bulletAnimationFrame, 0, bulletTexture.Width / 4, bulletTexture.Height),
                Color.White,
                rotation,
                Vector2.Zero,
                SpriteEffects.None,
                explosionLayer);
            }
            else
            {

                if (explosionTime > 30)
                {
                    spriteBatch.Draw(
                    explosionTexture,
                    new Rectangle(bulletCircle.X - bulletCircle.Radius * 2 - 20, bulletCircle.Y - bulletCircle.Radius * 2 - 5, bulletCircle.Radius * 4 + 10, bulletCircle.Radius * 4 + 10),
                    new Rectangle(0, 0, explosionTexture.Width/4, explosionTexture.Height),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    explosionLayer);
                }
                else if (explosionTime > 20)
                {
                    spriteBatch.Draw(
                    explosionTexture,
                    new Rectangle(bulletCircle.X - bulletCircle.Radius * 2 - 20, bulletCircle.Y - bulletCircle.Radius * 2 - 5, bulletCircle.Radius * 4 + 10, bulletCircle.Radius * 4 + 10),
                    new Rectangle(explosionTexture.Width/4, 0, explosionTexture.Width/4, explosionTexture.Height),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    explosionLayer);
                }
                else if (explosionTime > 10)
                {
                    spriteBatch.Draw(
                    explosionTexture,
                    new Rectangle(bulletCircle.X - bulletCircle.Radius * 2 - 20, bulletCircle.Y - bulletCircle.Radius * 2 - 5, bulletCircle.Radius * 4 + 10, bulletCircle.Radius * 4 + 10),
                    new Rectangle((explosionTexture.Width/4)*2, 0, explosionTexture.Width/4, explosionTexture.Height),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    explosionLayer);
                }
                else if (explosionTime > 0)
                {
                    spriteBatch.Draw(
                    explosionTexture,
                    new Rectangle(bulletCircle.X - bulletCircle.Radius * 2 - 20, bulletCircle.Y - bulletCircle.Radius * 2 - 5, bulletCircle.Radius * 4 + 10, bulletCircle.Radius * 4 + 10),
                    new Rectangle((explosionTexture.Width/4)*3, 0, explosionTexture.Width/4, explosionTexture.Height),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    explosionLayer);
                }
                else
                {
                    readyToBeRemoved = true;
                }
            }
        }

    }

}
