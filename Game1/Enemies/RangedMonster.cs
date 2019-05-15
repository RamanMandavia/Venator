using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Enemies
{
    // Ranged enemy requires its own enum, to track facing up and down
    enum RangedEnemyDirection { Up, Down, Right, Left }

    class RangedMonster : EnemiesAbstract
    {
        // Alex Sarnese
        // Raman Mandavia

        //Field
        bool isAttacking;
        Random rng;
        Rectangle enemyBullet;
        private bool spawning;
        private bool dying;
        private RangedEnemyDirection direction;
        private double attackTimer;

        private int spawnAnimationFrame;
        private int deathAnimationFrame;

        private Texture2D faceRight;
        private Texture2D faceUp;
        private Texture2D faceDown;
        private Texture2D spawnTexture;

        private List<Weapons.BasicGunBullet> projectiles;
        private Texture2D projectileRight;
        private Texture2D projectileLeft;
        private Texture2D projectileUp;
        private Texture2D projectileDown;


        /// <summary>
        /// Creates an instance of the infected crewmember that shoots
        /// This helps keeps each instance of this enemy consistant. 
        /// </summary>
        /// <param name="x">X component of where this enemy will spawn</param>
        /// <param name="y">Y component of where this enemy will spawn</param>
        /// <param name="enemyTexture">Texture that this enemy uses</param>
        public RangedMonster(int x, int y, Texture2D enemyTextureUp, Texture2D enemyTextureDown, Texture2D enemyTextureRight, Texture2D enemyTextureLeft, Texture2D enemyTextureSpawn, 
            Texture2D enemyTextureDeath, RangedEnemyDirection direction, Texture2D projectileRight, Texture2D projectileLeft, Texture2D projectileUp, Texture2D projectileDown) :
            base(12, 10, 0f, new Vector2(x, y), enemyTextureSpawn, enemyTextureLeft, enemyTextureDeath)
        {
            isAttacking = false;
            enemyHitBox = new Rectangle((int)enemyPosition.X + 30, (int)enemyPosition.Y + 30, 60, 60);
            enemyBullet = new Rectangle(-10, -10, 4, 4);
            rng = new Random();
            dropType = Weapons.TypeOfWeapon.Gun;
            this.direction = direction;
            spawning = true;
            dying = false;
            attackTimer = 0;
            

            spawnAnimationFrame = 0;
            deathAnimationFrame = 0;
            faceRight = enemyTextureRight;
            faceUp = enemyTextureUp;
            faceDown = enemyTextureDown;
            spawnTexture = enemyTextureSpawn;

            this.projectileRight = projectileRight;
            this.projectileLeft = projectileLeft;
            this.projectileUp = projectileUp;
            this.projectileDown = projectileDown;

            projectiles = new List<Weapons.BasicGunBullet>();
        }


        /// <summary>
        /// moves in diagonal directions but stops when shooting at player
        /// Also checks to see if it is damaged by player's currently equipped weapon.
        /// </summary>
        /// <param name="playerPosition">Location of the player</param>
        /// <param name="gameTime">the timer that can be used to time attack methods or timed behaviors</param> <param name="gameTime"></param>
        public override void Update(Player player, GameTime gameTime)
        {
            

            if (exists)
            {
                //reduce stun timer over time
                stunTime -= gameTime.ElapsedGameTime.TotalSeconds;

                if (stunTime <= 0)
                {
                    //removes stun effect after five seconds
                    stunned = false;
                }

                if (Health <= 0)
                {
                    EnemyPosition = new Vector2(EnemyPosition.X, EnemyPosition.Y);
                    dying = true;
                }
                //if the enemy is damaged, the enemy moved backwards and sets IsDamaged 
                //to false when the enemy had enough time to recover from hit (determined by invicibilityTimer)
                else if (IsDamaged)
                {
                    //this determines how long the enemy is in knockback and invicible
                    //Always recommended that InvicibilityTimerAfterDamaged stays at 0.5 seconds
                    TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds;

                    if (TimeThatHasPassed > InvicibilityTimerAfterDamaged)
                    {
                        IsDamaged = false;
                        TimeThatHasPassed = 0;
                    }

                    if (walkCycleFrame == 3 && !stunned && attackTimer > 0.5)
                    {
                        isAttacking = true;
                        attackTimer = 0.0;
                    }

                    if (isAttacking)
                    {
                        if (direction == RangedEnemyDirection.Down)
                        {
                            projectiles.Add(new Weapons.BasicGunBullet(Weapons.WeaponDirection.Down, new Point(enemyHitBox.X + (enemyHitBox.Width / 2) + 5, enemyHitBox.Y - (enemyHitBox.Height / 5)), projectileDown));
                            isAttacking = false;
                        }
                        else if (direction == RangedEnemyDirection.Up)
                        {
                            projectiles.Add(new Weapons.BasicGunBullet(Weapons.WeaponDirection.Up, new Point(enemyHitBox.X + (enemyHitBox.Width / 2) + 5, enemyHitBox.Y + (enemyHitBox.Height / 2)), projectileUp));
                            isAttacking = false;
                        }
                        else if (direction == RangedEnemyDirection.Right)
                        {
                            projectiles.Add(new Weapons.BasicGunBullet(Weapons.WeaponDirection.Right, new Point(enemyHitBox.X + enemyHitBox.Width, enemyHitBox.Y + (enemyHitBox.Height / 4)), projectileRight));
                            isAttacking = false;
                        }
                        else if (direction == RangedEnemyDirection.Left)
                        {
                            projectiles.Add(new Weapons.BasicGunBullet(Weapons.WeaponDirection.Left, new Point(enemyHitBox.X, enemyHitBox.Y + (enemyHitBox.Height / 4)), projectileLeft));
                            isAttacking = false;
                        }
                    }

                }
                else
                {
                    if (walkCycleFrame == 3 && !stunned && attackTimer > .5)
                    {
                        isAttacking = true;
                        attackTimer = 0.0;
                    }

                    if (isAttacking)
                    {
                        if (direction == RangedEnemyDirection.Down)
                        {
                            projectiles.Add(new Weapons.BasicGunBullet(Weapons.WeaponDirection.Down, new Point(enemyHitBox.X + (enemyHitBox.Width / 2) + 5, enemyHitBox.Y - (enemyHitBox.Height / 5)), projectileDown));
                            isAttacking = false;
                        }
                        else if (direction == RangedEnemyDirection.Up)
                        {
                            projectiles.Add(new Weapons.BasicGunBullet(Weapons.WeaponDirection.Up, new Point(enemyHitBox.X + (enemyHitBox.Width / 2) + 5, enemyHitBox.Y + (enemyHitBox.Height / 2)), projectileUp));
                            isAttacking = false;
                        }
                        else if (direction == RangedEnemyDirection.Right)
                        {
                            projectiles.Add(new Weapons.BasicGunBullet(Weapons.WeaponDirection.Right, new Point(enemyHitBox.X + enemyHitBox.Width, enemyHitBox.Y + (enemyHitBox.Height / 4)), projectileRight));
                            isAttacking = false;
                        }
                        else if (direction == RangedEnemyDirection.Left)
                        {
                            projectiles.Add(new Weapons.BasicGunBullet(Weapons.WeaponDirection.Left, new Point(enemyHitBox.X, enemyHitBox.Y + (enemyHitBox.Height / 4)), projectileLeft));
                            isAttacking = false;
                        }
                    }
                }

                //loops through list of bullets made by this gun
                for (int i = 0; i < projectiles.Count; i++)
                {
                    //bullet moves in their direction
                    projectiles[i].Update();

                    //stores x and y component of the current bullet
                    int x = projectiles[i].BulletCircle.X;
                    int y = projectiles[i].BulletCircle.Y;

                    //removes the bullet if it goes out of bounds
                    if (x < 0 || x > 800 || y < 0 || y > 650)
                    {
                        projectiles.RemoveAt(i);
                    }
                    else
                    {
                        if (projectiles[i].BulletCircle.Intersects(player.PlayerHitBox))
                        {
                            CheckAndDealDamage(player);
                            projectiles.RemoveAt(i);
                            break;
                        }
                    }
                }

                attackTimer += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
                player.Score += 75;
        }


        /// <summary>
        /// shoots bullet towards player
        /// </summary>
        /// <param name="player">player to shoot at</param>
        /// <param name="gameTime">timer use to wait 5 seconds</param>
        public void Attacking(Player player, GameTime gameTime)
        {
            TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds;
        }


        /// <summary>
        /// Checks to see if this enemy's projectiles made contact with the player. 
        /// If so, calls up the player's TakeDamage method and pass in this enemy's damage int
        /// </summary>
        public override void CheckAndDealDamage(Player player)
        {
            // If this is called, the player takes damage, in this case, as the condition is already met
            player.TakeDamage(Damage);
        }

        public override void UpdateMovementFrame()
        {
            if(spawning)
            {
                spawnAnimationFrame++;
                if (spawnAnimationFrame > 15)
                    spawning = false;
            }
            else if (dying)
            {
                deathAnimationFrame++;
                if (deathAnimationFrame > 7)
                    exists = false;
            }
            else if (Health > 0 && !stunned)
            {
                walkCycleFrame++;
                if (walkCycleFrame > 7)
                    walkCycleFrame = 0;
            }
        }

        /// <summary>
        /// Draws the enemy on the screen. 
        /// Needs to be called in the draw loop in Game1
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(rectangle, enemyHitBox, Color.Blue);

            foreach (Weapons.BasicGunBullet enemyProjectile in projectiles)
            {
                enemyProjectile.Draw(spriteBatch);
            }

            if (spawning)
            {
                if (spawnAnimationFrame < 8)
                    spriteBatch.Draw(
                        spawnTexture,
                        enemyPosition,
                        new Rectangle(enemySpriteWidth * (spawnAnimationFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        0.8f,
                        SpriteEffects.None,
                        enemyPosition.Y / 650 + 0.01f);
                else if (spawnAnimationFrame >= 4)
                    spriteBatch.Draw(
                        spawnTexture,
                        enemyPosition,
                        new Rectangle(enemySpriteWidth * ((spawnAnimationFrame - 8) / 2), enemySpriteHeight, enemySpriteWidth, enemySpriteHeight),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        0.8f,
                        SpriteEffects.None,
                        enemyPosition.Y / 650 + 0.01f);
            }
            else if (dying)
            {
                spriteBatch.Draw(
                    enemyTextureDead,
                    enemyPosition,
                    new Rectangle(enemySpriteWidth * (deathAnimationFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    0.8f,
                    SpriteEffects.None,
                    enemyPosition.Y / 650 + 0.01f);
            }
            else
            {
                switch (this.direction)
                {
                    case RangedEnemyDirection.Right:
                        {

                            if (IsDamaged)
                            {
                                spriteBatch.Draw(
                                        faceRight,
                                        enemyPosition,
                                        new Rectangle(enemySpriteWidth * (walkCycleFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                                        Color.Red,
                                        0.0f,
                                        Vector2.Zero,
                                        0.8f,
                                        SpriteEffects.None,
                                        enemyPosition.Y / 650 + 0.01f);
                            }
                            else
                            {
                                spriteBatch.Draw(
                                        faceRight,
                                        enemyPosition,
                                        new Rectangle(enemySpriteWidth * (walkCycleFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                                        Color.White,
                                        0.0f,
                                        Vector2.Zero,
                                        0.8f,
                                        SpriteEffects.None,
                                        enemyPosition.Y / 650 + 0.01f);
                            }
                        }
                        break;

                    case RangedEnemyDirection.Left:
                        {
                            if (IsDamaged)
                            {
                                    spriteBatch.Draw(
                                        enemyTextureLeft,
                                        enemyPosition,
                                        new Rectangle(enemySpriteWidth * (walkCycleFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                                        Color.Red,
                                        0.0f,
                                        Vector2.Zero,
                                        0.8f,
                                        SpriteEffects.None,
                                        enemyPosition.Y / 650 + 0.01f);
                            }
                            else
                            {
                                    spriteBatch.Draw(
                                        enemyTextureLeft,
                                        enemyPosition,
                                        new Rectangle(enemySpriteWidth * (walkCycleFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                                        Color.White,
                                        0.0f,
                                        Vector2.Zero,
                                        0.8f,
                                        SpriteEffects.None,
                                        enemyPosition.Y / 650 + 0.01f);
                            }
                        }
                        break;

                    case RangedEnemyDirection.Up:
                        {
                            if (IsDamaged)
                            {
                                    spriteBatch.Draw(
                                        faceUp,
                                        enemyPosition,
                                        new Rectangle(enemySpriteWidth * (walkCycleFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                                        Color.Red,
                                        0.0f,
                                        Vector2.Zero,
                                        0.8f,
                                        SpriteEffects.None,
                                        enemyPosition.Y / 650 + 0.01f);
                            }
                            else
                            {
                                    spriteBatch.Draw(
                                        faceUp,
                                        enemyPosition,
                                        new Rectangle(enemySpriteWidth * (walkCycleFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                                        Color.White,
                                        0.0f,
                                        Vector2.Zero,
                                        0.8f,
                                        SpriteEffects.None,
                                        enemyPosition.Y / 650 + 0.01f);
                            }
                        }
                        break;

                    case RangedEnemyDirection.Down:
                        {
                            if (IsDamaged)
                            {
                                    spriteBatch.Draw(
                                        faceDown,
                                        enemyPosition,
                                        new Rectangle(enemySpriteWidth * (walkCycleFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                                        Color.Red,
                                        0.0f,
                                        Vector2.Zero,
                                        0.8f,
                                        SpriteEffects.None,
                                        enemyPosition.Y / 650 + 0.01f);
                            }
                            else
                            {
                                    spriteBatch.Draw(
                                        faceDown,
                                        enemyPosition,
                                        new Rectangle(enemySpriteWidth * (walkCycleFrame / 2), 0, enemySpriteWidth, enemySpriteHeight),
                                        Color.White,
                                        0.0f,
                                        Vector2.Zero,
                                        0.8f,
                                        SpriteEffects.None,
                                        enemyPosition.Y / 650 + 0.01f);
                            }
                        }
                        break;
                }
            }
        }
    }
}
