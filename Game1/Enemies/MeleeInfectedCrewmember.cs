using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Enemies
{
    class MeleeInfectedCrewmember : EnemiesAbstract
    {
        // Alex Sarnese and Cail Umbaugh
        // Raman Mandavia
        //Field
        //this enemy just attacks by running into player so this
        //enemy does not need more fields. 

        private double deadEnemyDisappearTimer;
        private Random rng;

        /// <summary>
        /// Creates an instance of the infected crewmember. 
        /// 
        /// Notice how the height/width of this enemy, the damage, the health,
        /// and the speed are all hardcoded in for this specific enemy.
        /// 
        /// This helps keeps each instance of this enemy consistant. 
        /// </summary>
        /// <param name="x">X component of where this enemy will spawn</param>
        /// <param name="y">Y component of where this enemy will spawn</param>
        /// <param name="enemyTexture">Texture that this enemy uses</param>
        public MeleeInfectedCrewmember(int x, int y, Texture2D enemyTextureRight, Texture2D enemyTextureLeft, Texture2D deadTexture, Random rng) : 
            base(15, 10, 2f, new Vector2(x, y), enemyTextureRight, enemyTextureLeft, deadTexture)
        {
            enemyHitBox = new Rectangle((int)enemyPosition.X + 40, (int)enemyPosition.Y + 15, 55, 100);
            deadEnemyDisappearTimer = 0;
            dropType = TypeOfWeapon.Melee;
            this.rng = rng;
        }


        /// <summary>
        /// runs to player wherever the player is.
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

                if (Health > 0)
                {

                    //if the enemy is damaged, the enemy moved backwards and sets IsDamaged 
                    //to false when the enemy had enough time to recover from hit (determined by invicibilityTimer)
                    

                    //Also, if the enemy is taking damage and the attack caused knockback, move the enemy backwards
                    //Otherwise, move the enemy like normal unless it is stunned
                    if (IsDamaged)
                    {
                        if (damageCausedKnockback)
                        {
                            //moves enemy away from player by going 45 degrees diagonally opposite tp player
                            //unless it is exactly aligned with player, then it moves straight away from player
                            //Very basic and cheap movement system for when being damaged and taking knockback
                            if (EnemyFacing == EnemyDirection.Right)
                            {
                                //can move when it is not stunned
                                enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - (enemyPosition.X + (enemyHitBox.Width + 40))) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 30) * Speed);
                                enemyHitBox = new Rectangle((int)enemyPosition.X + 40, (int)enemyPosition.Y + 15, 55, 100);
                            }
                            else if (EnemyFacing == EnemyDirection.Left)
                            {
                                enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - enemyPosition.X) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 30) * Speed);
                                enemyHitBox = new Rectangle((int)enemyPosition.X + 40, (int)enemyPosition.Y + 15, 42, 100);
                            }
                        }
                        else
                        {
                            if (!stunned)
                            {
                                //can move when it is not stunned
                                if (EnemyFacing == EnemyDirection.Right)
                                {
                                    //can move when it is not stunned
                                    enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - (enemyPosition.X + (enemyHitBox.Width + 40))) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 30) * Speed);
                                    enemyHitBox = new Rectangle((int)enemyPosition.X + 40, (int)enemyPosition.Y + 15, 55, 100);
                                }
                                else if (EnemyFacing == EnemyDirection.Left)
                                {
                                    enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - enemyPosition.X) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 30) * Speed);
                                    enemyHitBox = new Rectangle((int)enemyPosition.X + 40, (int)enemyPosition.Y + 15, 42, 100);
                                }
                            }
                        }


                        //this determines how long the enemy is in knockback and invicible
                        //Always recommended that InvicibilityTimerAfterDamaged stays at 0.5 seconds
                        TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds;
                        
                        if (TimeThatHasPassed > InvicibilityTimerAfterDamaged)
                        {
                            IsDamaged = false;
                            TimeThatHasPassed = 0;
                        }

                    }
                    else
                    {
                        //Since this enemy doesn't have a timed attack, there is no attack method. 
                        //Other enemies may have distinct attacks and if so, make an Attack() method and call it periodically in Update().


                        //moves enemy closer to player by going 45 degrees diagonally to player
                        //unless it is exactly aligned with player, then it moves straight to player
                        //Very basic and cheap movement system
                        
                        if (!stunned)
                        {
                            //can move when it is not stunned
                            if (EnemyFacing == EnemyDirection.Right)
                            {
                                //can move when it is not stunned
                                enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - (enemyPosition.X + (enemyHitBox.Width + 40))) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 30) * Speed);
                                enemyHitBox = new Rectangle((int)enemyPosition.X + 40, (int)enemyPosition.Y + 15, 55, 100);
                            }
                            else if (EnemyFacing == EnemyDirection.Left)
                            {
                                enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - enemyPosition.X) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 30) * Speed);
                                enemyHitBox = new Rectangle((int)enemyPosition.X + 40, (int)enemyPosition.Y + 15, 42, 100);
                            }
                        }
                        else if(stunTime <= 0){
                            //removes stun effect after five seconds
                            stunned = false;
                        }
                    }

                    if (player.PlayerHitBox.X - enemyHitBox.X > 0)
                    {
                        EnemyFacing = EnemyDirection.Right;
                    }
                    else if (enemyHitBox.X + enemyHitBox.Width / 2 - player.PlayerHitBox.X > 0)
                    {
                        EnemyFacing = EnemyDirection.Left;
                    }


                    CheckAndDealDamage(player);
                }
                else
                {
                    deadEnemyDisappearTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (deadEnemyDisappearTimer > 2.0)
                    {
                        player.Score += 50;
                        exists = false;
                    }
                }
            }
        }

        public override void UpdateMovementFrame()
        {
            if (!stunned || Health <= 0)
            {
                walkCycleFrame++;
                if (walkCycleFrame > 7)
                {
                    walkCycleFrame = 0;
                    Speed = (float)rng.Next(10, 25) / 10;
                }
            }
        }


        /// <summary>
        /// Checks to see if this enemy made conact with the player. 
        /// If so, calls up the player's TakeDamage method and pass in this enemy's damage int
        /// </summary>
        public override void CheckAndDealDamage(Player player)
        {
            //condition that determines if player would take damage
            if (enemyHitBox.Intersects(player.PlayerHitBox) && !stunned)
            {
                player.TakeDamage(Damage);
            }
        }
        

        /// <summary>
        /// Draws the enemy on the screen. 
        /// Needs to be called in the draw loop in Game1
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (exists)
            {
                if (Health <= 0)
                {
                    if (walkCycleFrame % 2 == 0)
                        spriteBatch.Draw(
                            enemyTextureDead,
                            enemyPosition,
                            new Rectangle(0, 0, enemySpriteWidth, enemySpriteHeight),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            0.8f,
                            SpriteEffects.None,
                            enemyPosition.Y / 650 + 0.01f);
                }
                else
                {
                    switch (EnemyFacing)
                    {
                        case EnemyDirection.Right:
                            {

                                if (IsDamaged)
                                {
                                    if (walkCycleFrame < 4)
                                        spriteBatch.Draw(
                                            enemyTextureRight,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * walkCycleFrame, 0, enemySpriteWidth, enemySpriteHeight),
                                            Color.Red,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                    else if (walkCycleFrame >= 4)
                                        spriteBatch.Draw(
                                            enemyTextureRight,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * (walkCycleFrame - 4), enemySpriteHeight, enemySpriteWidth, enemySpriteHeight),
                                            Color.Red,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                }
                                else
                                {
                                    if (walkCycleFrame < 4)
                                        spriteBatch.Draw(
                                            enemyTextureRight,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * walkCycleFrame, 0, enemySpriteWidth, enemySpriteHeight),
                                            Color.White,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                    else if (walkCycleFrame >= 4)
                                        spriteBatch.Draw(
                                            enemyTextureRight,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * (walkCycleFrame - 4), enemySpriteHeight, enemySpriteWidth, enemySpriteHeight),
                                            Color.White,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                }
                            }
                            break;
                        case EnemyDirection.Left:
                            {
                                if (IsDamaged)
                                {
                                    if (walkCycleFrame < 4)
                                        spriteBatch.Draw(
                                            enemyTextureLeft,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * walkCycleFrame, 0, enemySpriteWidth, enemySpriteHeight),
                                            Color.Red,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                    else if (walkCycleFrame >= 4)
                                        spriteBatch.Draw(
                                            enemyTextureLeft,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * (walkCycleFrame - 4), enemySpriteHeight, enemySpriteWidth, enemySpriteHeight),
                                            Color.Red,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                }
                                else
                                {
                                    if (walkCycleFrame < 4)
                                        spriteBatch.Draw(
                                            enemyTextureLeft,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * walkCycleFrame, 0, enemySpriteWidth, enemySpriteHeight),
                                            Color.White,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                    else if (walkCycleFrame >= 4)
                                        spriteBatch.Draw(
                                            enemyTextureLeft,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * (walkCycleFrame - 4), enemySpriteHeight, enemySpriteWidth, enemySpriteHeight),
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
}
