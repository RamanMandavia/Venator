using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Game1.Enemies;

namespace Game1.Weapons
{
    class ShotgunWeapon : WeaponAbstract
    {
        // Created by Will Walrond
        // Integerated and working with player class by Alex Sarnese and Raman Mandavia
        
        List<ShotgunBullet> ListOfBullets = new List<ShotgunBullet>();
        
        private Texture2D bulletTexture;
        private Texture2D faceDown;
        private Texture2D faceUp;
        private Texture2D faceRight;
        private Texture2D faceLeft;
        private int widthOfSingleSpriteHorizontal;
        private int widthOfSingleSpriteVertical;
        private bool fired = false;

        private int gunAnimationFrame;


        public ShotgunWeapon(int damage, int range, float cooldown, Rectangle WeaponSpawnLocation, Texture2D faceDown,
            Texture2D faceUp, Texture2D faceRight, Texture2D faceLeft, Texture2D bulletTexture)
            : base("Shotgun", TypeOfWeapon.Gun, damage, range, cooldown, 0.2f, WeaponSpawnLocation, faceRight)
        {
            TimeThatHasPassed = 20;

            this.bulletTexture = bulletTexture;
            this.faceDown = faceDown;
            this.faceUp = faceUp;
            this.faceRight = faceRight;
            this.faceLeft = faceLeft;

            widthOfSingleSpriteHorizontal = faceLeft.Width / 4;
            widthOfSingleSpriteVertical = faceUp.Width / 4;

            gunAnimationFrame = 0;
        }



        public override void Update(List<EnemiesAbstract> listOfEnemies, Player player, GameTime gameTime)
        {
            TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds;


            //loops through list of bullets made by this gun
            for (int i = 0; i < ListOfBullets.Count; i++)
            {
                //bullet moves in their direction
                ListOfBullets[i].Update();

                //stores x and y component of the current bullet
                int x = ListOfBullets[i].BulletCircle.X;
                int y = ListOfBullets[i].BulletCircle.Y;

                //removes the bullet if it goes out of bounds
                if (x < 0 || x > 800 || y < 0 || y > 650)
                {
                    ListOfBullets.RemoveAt(i);
                }
                //removes bullet when it traveled too far
                else if (ListOfBullets[i].DistanceTraveled > Range)
                {
                    ListOfBullets.RemoveAt(i);
                }
                else
                {
                    //loops through list of enemies and see if it hit one of them
                    //if so, damages the enemy and removes the bullet
                    for (int b = 0; b < listOfEnemies.Count; b++)
                    {
                        bool still = true;
                        if (listOfEnemies[b] is BossMech)
                        {
                            BossMech d = (BossMech)listOfEnemies[b];
                            if (ListOfBullets[i].BulletCircle.Intersects(d.SheildHitBox))
                            {
                                still = false;
                                ListOfBullets.RemoveAt(i);
                                break;
                            }
                        }


                        if (ListOfBullets[i].BulletCircle.Intersects(listOfEnemies[b].EnemyHitBox) && listOfEnemies[b].Health > 0 && still == true && listOfEnemies[b].IsDamaged == false)
                        {
                            listOfEnemies[b].TakeDamage(Damage);
                            ListOfBullets.RemoveAt(i);
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


        protected override void Attacking(List<EnemiesAbstract> listOfEnemies, Player player)
        {
            //shoots one bullet and exits attacking method
            if (!fired)
            {
                //creates a bullet going in direction that the gun is facing
                ListOfBullets.Add(new ShotgunBullet(WeaponFacing, location, 3, bulletTexture));
                ListOfBullets.Add(new ShotgunBullet(WeaponFacing, location, 1, bulletTexture));
                ListOfBullets.Add(new ShotgunBullet(WeaponFacing, location, 0, bulletTexture));
                ListOfBullets.Add(new ShotgunBullet(WeaponFacing, location, -1, bulletTexture));
                ListOfBullets.Add(new ShotgunBullet(WeaponFacing, location, -3, bulletTexture));
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
        /// makes weapon face the same direction as player
        /// </summary>
        /// <param name="playerState">the direction player is facing</param>
        public override void DetermineWeaponDirection(Player player)
        {
            //make weapon face upward if player is facing upward
            if (player.CurrentPlayerState == PlayerState.FaceUp)
            {
                //update location to the carrier's center top part
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2);
                location.Y = player.PlayerHitBox.Y;

                WeaponFacing = WeaponDirection.Up;
            }

            //make weapon face downward if player is facing down
            else if (player.CurrentPlayerState == PlayerState.FaceDown)
            {
                //update location to the carrier's center bottom
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2);
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height);

                WeaponFacing = WeaponDirection.Down;
            }

            //makes the weapon face right if player is facing right
            else if (player.CurrentPlayerState == PlayerState.FaceRight)
            {
                //update location to the carrier's center right side
                location.X = player.PlayerHitBox.X + player.PlayerHitBox.Width + 20;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) - 10;

                WeaponFacing = WeaponDirection.Right;
            }

            //make weapon face left if player is facing left
            else if (player.CurrentPlayerState == PlayerState.FaceLeft)
            {
                //update location to the carrier's center left side
                location.X = player.PlayerHitBox.X - 20;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) - 10;

                WeaponFacing = WeaponDirection.Left;
            }
        }


        //draws weapon and bullets to the screen
        public override void Draw(SpriteBatch spriteBatch, Player player)
        {
            //set the layer for weapon
            float weaponLayer = location.Y / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (weaponLayer <= 0)
            {
                weaponLayer = 0.01f;
            }

            //draws every bullet to the screen first before the weapon
            foreach (ShotgunBullet bullet in ListOfBullets)
            {
                bullet.Draw(spriteBatch);
            }


            if (fired)
            {
                //change the shotgun's animation every 1/4th second
                if (TimeThatHasPassed< AttackPhaseDurationTime * 0.25)
                {
                    gunAnimationFrame = 1;
                }
                else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5)
                {
                    gunAnimationFrame = 2;
                }
                else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75)
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
                        new Vector2(location.X - 55, location.Y - 55),
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
                        new Vector2(location.X - 65, location.Y - 65),
                        new Rectangle(widthOfSingleSpriteHorizontal * gunAnimationFrame, 0, widthOfSingleSpriteHorizontal, faceLeft.Height),
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
                        new Vector2(location.X - 65, location.Y - 65),
                        new Rectangle(widthOfSingleSpriteHorizontal * gunAnimationFrame, 0, widthOfSingleSpriteHorizontal, faceRight.Height),
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
                        new Vector2(location.X - 60, location.Y - 85),
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

    class ShotgunBullet
    {
        WeaponDirection direction;

        int bulletspeed = 10;

        public Circle bulletcircle;

        public Circle BulletCircle { get { return bulletcircle; } }

        Vector2 trajectory;


        private Texture2D bulletTexture;
        private int bulletTextureWidth;
        private int bulletTextureHeight;
        private int distanceTraveled;
        private int bulletAnimationFrame;

        public int DistanceTraveled { get { return distanceTraveled; } }

        //input rise as if the bullet is being shot to the RIGHT
        public ShotgunBullet(WeaponDirection direction, Point gunlocation, int rise, Texture2D bulletTexture)
        {
            bulletcircle = new Circle(gunlocation.X, gunlocation.Y, 4);
            trajectory.Y = rise;
            trajectory.X = bulletspeed;

            this.direction = direction;
            distanceTraveled = 0;

            this.bulletTexture = bulletTexture;
            bulletTextureWidth = bulletTexture.Width / 4;
            bulletTextureHeight = bulletTexture.Height;
            bulletAnimationFrame = 0;
        }

        public void Update()
        {
            switch (direction)
            {
                case WeaponDirection.Left:
                    bulletcircle.X -= (int)trajectory.X;
                    bulletcircle.Y += (int)trajectory.Y;
                    break;
                case WeaponDirection.Right:
                    bulletcircle.X += (int)trajectory.X;
                    bulletcircle.Y += (int)trajectory.Y;
                    break;
                case WeaponDirection.Down:
                    bulletcircle.X += (int)trajectory.Y;
                    bulletcircle.Y += (int)trajectory.X;
                    break;
                case WeaponDirection.Up:
                    bulletcircle.X -= (int)trajectory.Y;
                    bulletcircle.Y -= (int)trajectory.X;
                    break;
            }

            distanceTraveled += bulletspeed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //set the layer for bullet
            float bulletLayer = BulletCircle.Y / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (bulletLayer <= 0)
            {
                bulletLayer = 0.01f;
            }

            //change the gun's bullet animation
            if (distanceTraveled < 20)
            {
                bulletAnimationFrame = 0;
            }
            else if (distanceTraveled < 60)
            {
                bulletAnimationFrame = 1;
            }
            else if (distanceTraveled < 100)
            {
                bulletAnimationFrame = 2;
            }
            else
            {
                bulletAnimationFrame = 3;
            }

            spriteBatch.Draw(
            bulletTexture,
            new Vector2(bulletcircle.X - (bulletTextureWidth / 2), bulletcircle.Y - (bulletTextureHeight / 2)),
            new Rectangle(bulletTextureWidth * bulletAnimationFrame, 0, bulletTextureWidth, bulletTextureHeight),
            Color.White,
            0.0f,
            Vector2.Zero,
            1.0f,
            SpriteEffects.None,
            bulletLayer);
        }
    }

}
