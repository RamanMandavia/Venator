using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Enemies
{
    class Juggernaut : EnemiesAbstract
    {
        // Alex Sarnese
        // Raman Mandavia

        //Field
        bool isAttacking;
        Random rng;
        private double deadEnemyDisappearTimer;

        int attackAnimationFrame;

        // List of rectangles to hold multiple rectangles,
        // in order to properly generate the hitbox for the attack
        private Rectangle[] attackSwingArea;
        private Rectangle attackAreaCheck;
        private bool attackHitBoxesActive;

        // Textures for the attack animation
        Texture2D attackRight;
        Texture2D attackLeft;

        Texture2D rectangle;

        /// <summary>
        /// Creates an instance of the juggernaut that shoots
        /// This helps keeps each instance of this enemy consistant. 
        /// </summary>
        /// <param name="x">X component of where this enemy will spawn</param>
        /// <param name="y">Y component of where this enemy will spawn</param>
        /// <param name="enemyTexture">Texture that this enemy uses</param>
        public Juggernaut(int x, int y, Texture2D enemyTextureRight, Texture2D enemyTextureLeft, Texture2D enemyTextureDead, Texture2D attackRight, Texture2D attackLeft) :
            base(120, 40, 0.8f, new Vector2(x, y), enemyTextureRight, enemyTextureLeft, enemyTextureDead)
        {
            isAttacking = false;
            attackSwingArea = new Rectangle[] 
            {
                new Rectangle(0, 0, 10, 10),
                new Rectangle(0, 0, 10, 10),
                new Rectangle(0, 0, 10, 10),
                new Rectangle(0, 0, 10, 10)
            };
            rng = new Random();
            dropType = Weapons.TypeOfWeapon.Jug;

            attackAreaCheck = new Rectangle(0, 0, 20, 160);
            attackHitBoxesActive = true;

            enemyHitBox = new Rectangle((int)enemyPosition.X + 50, (int)enemyPosition.Y , 95, 192);
            deadEnemyDisappearTimer = 0;
            this.attackRight = attackRight;
            this.attackLeft = attackLeft;

            attackAnimationFrame = 0;
        }


        /// <summary>
        /// moves in diagonal directions and shoots at player
        /// Also checks to see if it is damaged by player's currently equipped weapon.
        /// </summary>
        /// <param name="playerPosition">Location of the player</param>
        /// <param name="gameTime">the timer that can be used to time attack methods or timed behaviors</param> <param name="gameTime"></param>
        public override void Update(Player player, GameTime gameTime)
        {
            if (exists)
            {
                if (isAttacking == false)
                {
                    if (Health > 0)
                    {

                        //if the enemy is damaged, the enemy moved backwards and sets IsDamaged 
                        //to false when the enemy had enough time to recover from hit (determined by invicibilityTimer)


                        //Also, if the enemy is taking damage and the attack caused knockback, move the enemy backwards
                        //Otherwise, move the enemy like normal unless it is stunned
                        if (IsDamaged)
                        {
                            if (EnemyFacing == EnemyDirection.Right)
                            {
                                //can move when it is not stunned
                                enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - (enemyPosition.X + (enemyHitBox.Width + 40))) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 60) * Speed);
                                enemyHitBox = new Rectangle((int)enemyPosition.X + 50, (int)enemyPosition.Y, 95, 192);
                                attackAreaCheck = new Rectangle((enemyHitBox.X + enemyHitBox.Width), (enemyHitBox.Y + 15), 20, 160);
                            }
                            else if (EnemyFacing == EnemyDirection.Left)
                            {
                                enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - enemyPosition.X) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 60) * Speed);
                                enemyHitBox = new Rectangle((int)enemyPosition.X + 50, (int)enemyPosition.Y, 95, 192);
                                attackAreaCheck = new Rectangle((enemyHitBox.X - 20), (enemyHitBox.Y + 15), 20, 160);
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
                            //can move when it is not stunned
                            if (EnemyFacing == EnemyDirection.Right)
                            {
                                //can move when it is not stunned
                                enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - (enemyPosition.X + (enemyHitBox.Width + 40))) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 60) * Speed);
                                enemyHitBox = new Rectangle((int)enemyPosition.X + 50, (int)enemyPosition.Y, 95, 192);
                                attackAreaCheck = new Rectangle((enemyHitBox.X + enemyHitBox.Width), (enemyHitBox.Y + 15), 20, 160);
                            }
                            else if (EnemyFacing == EnemyDirection.Left)
                            {
                                enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - enemyPosition.X) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 60) * Speed);
                                enemyHitBox = new Rectangle((int)enemyPosition.X + 50, (int)enemyPosition.Y, 95, 192);
                                attackAreaCheck = new Rectangle((enemyHitBox.X - 20), (enemyHitBox.Y + 15), 20, 160);
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

                        if (attackAreaCheck.Intersects(player.PlayerHitBox))
                        {
                            isAttacking = true;
                            attackHitBoxesActive = true;
                        }
                    }
                    else
                    {
                        deadEnemyDisappearTimer += gameTime.ElapsedGameTime.TotalSeconds;
                        if (deadEnemyDisappearTimer > 2.0)
                        {
                            player.Score += 150;
                            exists = false;
                        }
                    }
                }
                else if (isAttacking)
                {
                    TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds;

                    if (TimeThatHasPassed > InvicibilityTimerAfterDamaged)
                    {
                        IsDamaged = false;
                        TimeThatHasPassed = 0;
                    }
                    Attacking(player, gameTime);
                    CheckAndDealDamage(player);
                }
            }

        }


        /// <summary>
        /// shoots bullets towards player
        /// </summary>
        /// <param name="player">player to shoot at</param>
        /// <param name="gameTime">timer use to wait 5 seconds</param>
        public void Attacking(Player player, GameTime gameTime)
        {
            if (attackAnimationFrame < 2)
            {
                if (EnemyFacing == EnemyDirection.Right)
                {
                    attackSwingArea[0] = new Rectangle((enemyHitBox.X + enemyHitBox.Width - 10), enemyHitBox.Y, 10, 50);
                    attackSwingArea[1] = new Rectangle((enemyHitBox.X + enemyHitBox.Width), enemyHitBox.Y + 10, 10, 40);
                    attackSwingArea[2] = new Rectangle((enemyHitBox.X + enemyHitBox.Width + 10), enemyHitBox.Y + 20, 20, 30);
                    attackSwingArea[3] = new Rectangle((enemyHitBox.X + enemyHitBox.Width + 20), enemyHitBox.Y + 30, 30, 20);
                }
                else
                {
                    attackSwingArea[0] = new Rectangle((enemyHitBox.X ), enemyHitBox.Y, 10, 50);
                    attackSwingArea[1] = new Rectangle((enemyHitBox.X - 10), enemyHitBox.Y + 10, 10, 40);
                    attackSwingArea[2] = new Rectangle((enemyHitBox.X - 30), enemyHitBox.Y + 20, 20, 30);
                    attackSwingArea[3] = new Rectangle((enemyHitBox.X - 50), enemyHitBox.Y + 30, 30, 20);
                }
            }
            else if (attackAnimationFrame == 2)
            {
                if (EnemyFacing == EnemyDirection.Right)
                {
                    attackSwingArea[0] = new Rectangle((enemyHitBox.X + enemyHitBox.Width - 10), enemyHitBox.Y, 10, 100);
                    attackSwingArea[1] = new Rectangle((enemyHitBox.X + enemyHitBox.Width), enemyHitBox.Y + 10, 10, 90);
                    attackSwingArea[2] = new Rectangle((enemyHitBox.X + enemyHitBox.Width + 10), enemyHitBox.Y + 20, 20, 80);
                    attackSwingArea[3] = new Rectangle((enemyHitBox.X + enemyHitBox.Width + 20), enemyHitBox.Y + 30, 30, 60);
                }
                else
                {
                    attackSwingArea[0] = new Rectangle((enemyHitBox.X), enemyHitBox.Y, 10, 100);
                    attackSwingArea[1] = new Rectangle((enemyHitBox.X - 10), enemyHitBox.Y + 10, 10, 90);
                    attackSwingArea[2] = new Rectangle((enemyHitBox.X - 30), enemyHitBox.Y + 20, 20, 80);
                    attackSwingArea[3] = new Rectangle((enemyHitBox.X - 50), enemyHitBox.Y + 30, 30, 60);
                }
            }
            else if (attackAnimationFrame == 3)
            {
                if (EnemyFacing == EnemyDirection.Right)
                {
                    attackSwingArea[0] = new Rectangle((enemyHitBox.X + enemyHitBox.Width - 10), enemyHitBox.Y, 10, 140);
                    attackSwingArea[1] = new Rectangle((enemyHitBox.X + enemyHitBox.Width), enemyHitBox.Y + 10, 10, 130);
                    attackSwingArea[2] = new Rectangle((enemyHitBox.X + enemyHitBox.Width + 10), enemyHitBox.Y + 20, 20, 115);
                    attackSwingArea[3] = new Rectangle((enemyHitBox.X + enemyHitBox.Width + 20), enemyHitBox.Y + 30, 30, 100);
                }
                else
                {
                    attackSwingArea[0] = new Rectangle((enemyHitBox.X), enemyHitBox.Y, 10, 140);
                    attackSwingArea[1] = new Rectangle((enemyHitBox.X - 10), enemyHitBox.Y + 10, 10, 130);
                    attackSwingArea[2] = new Rectangle((enemyHitBox.X - 30), enemyHitBox.Y + 20, 20, 115);
                    attackSwingArea[3] = new Rectangle((enemyHitBox.X - 50), enemyHitBox.Y + 30, 30, 100);
                }
            }
            else if (attackAnimationFrame == 4)
            {
                if (EnemyFacing == EnemyDirection.Right)
                {
                    attackSwingArea[0] = new Rectangle((enemyHitBox.X + enemyHitBox.Width - 10), enemyHitBox.Y, 10, 170);
                    attackSwingArea[1] = new Rectangle((enemyHitBox.X + enemyHitBox.Width), enemyHitBox.Y + 10, 10, 160);
                    attackSwingArea[2] = new Rectangle((enemyHitBox.X + enemyHitBox.Width + 10), enemyHitBox.Y + 20, 20, 140);
                    attackSwingArea[3] = new Rectangle((enemyHitBox.X + enemyHitBox.Width + 20), enemyHitBox.Y + 30, 30, 110);
                }
                else
                {
                    attackSwingArea[0] = new Rectangle((enemyHitBox.X), enemyHitBox.Y, 10, 170);
                    attackSwingArea[1] = new Rectangle((enemyHitBox.X - 10), enemyHitBox.Y + 10, 10, 160);
                    attackSwingArea[2] = new Rectangle((enemyHitBox.X - 30), enemyHitBox.Y + 20, 20, 140);
                    attackSwingArea[3] = new Rectangle((enemyHitBox.X - 50), enemyHitBox.Y + 30, 30, 110);
                }
            }
        }


        /// <summary>
        /// Checks to see if this enemy's projectiles made contact with the player. 
        /// If so, calls up the player's TakeDamage method and pass in this enemy's damage int
        /// </summary>
        public override void CheckAndDealDamage(Player player)
        {
            //condition that determines if player would take damage
            foreach (Rectangle damageArea in attackSwingArea)
            {
                if (attackHitBoxesActive)
                {
                    if (damageArea.Intersects(player.PlayerHitBox))
                    {
                        player.TakeDamage(Damage);
                        attackHitBoxesActive = false;
                    }
                }
            }
        }

        public override void UpdateMovementFrame()
        {
            if (isAttacking == false)
            {
                walkCycleFrame++;
                if (walkCycleFrame > 7)
                    walkCycleFrame = 0;
            }
            else if (isAttacking)
            {
                UpdateAttackFrame();
            }
        }

        private void UpdateAttackFrame()
        {
            attackAnimationFrame++;
            if (attackAnimationFrame > 4)
            {
                attackAnimationFrame = 0;
                isAttacking = false;
                attackHitBoxesActive = true;
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
                    if (isAttacking == false)
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
                    else if (isAttacking)
                    {
                        switch (EnemyFacing)
                        {
                            case EnemyDirection.Right:
                                {
                                    if (attackAnimationFrame == 0)
                                    {
                                        if (IsDamaged)
                                        {
                                            spriteBatch.Draw(
                                                    attackRight,
                                                    enemyPosition,
                                                    new Rectangle(enemySpriteWidth * (attackAnimationFrame), 0, enemySpriteWidth, enemySpriteHeight),
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
                                                    attackRight,
                                                    enemyPosition,
                                                    new Rectangle(enemySpriteWidth * (attackAnimationFrame), 0, enemySpriteWidth, enemySpriteHeight),
                                                    Color.White,
                                                    0.0f,
                                                    Vector2.Zero,
                                                    0.8f,
                                                    SpriteEffects.None,
                                                    enemyPosition.Y / 650 + 0.01f);
                                        }
                                    }
                                    else
                                    {
                                        if (IsDamaged)
                                        {
                                            spriteBatch.Draw(
                                                    attackRight,
                                                    enemyPosition,
                                                    new Rectangle(enemySpriteWidth * (attackAnimationFrame - 1), 0, enemySpriteWidth, enemySpriteHeight),
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
                                                    attackRight,
                                                    enemyPosition,
                                                    new Rectangle(enemySpriteWidth * (attackAnimationFrame - 1), 0, enemySpriteWidth, enemySpriteHeight),
                                                    Color.White,
                                                    0.0f,
                                                    Vector2.Zero,
                                                    0.8f,
                                                    SpriteEffects.None,
                                                    enemyPosition.Y / 650 + 0.01f);
                                        }
                                    }
                                }
                                break;
                            case EnemyDirection.Left:
                                {
                                    if (attackAnimationFrame == 0)
                                    {
                                        if (IsDamaged)
                                        {
                                            spriteBatch.Draw(
                                                    attackLeft,
                                                    enemyPosition,
                                                    new Rectangle(enemySpriteWidth * (attackAnimationFrame), 0, enemySpriteWidth, enemySpriteHeight),
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
                                                    attackLeft,
                                                    enemyPosition,
                                                    new Rectangle(enemySpriteWidth * (attackAnimationFrame), 0, enemySpriteWidth, enemySpriteHeight),
                                                    Color.White,
                                                    0.0f,
                                                    Vector2.Zero,
                                                    0.8f,
                                                    SpriteEffects.None,
                                                    enemyPosition.Y / 650 + 0.01f);
                                        }
                                    }
                                    else
                                    {
                                        if (IsDamaged)
                                        {
                                            spriteBatch.Draw(
                                                    attackLeft,
                                                    enemyPosition,
                                                    new Rectangle(enemySpriteWidth * (attackAnimationFrame - 1), 0, enemySpriteWidth, enemySpriteHeight),
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
                                                    attackLeft,
                                                    enemyPosition,
                                                    new Rectangle(enemySpriteWidth * (attackAnimationFrame - 1), 0, enemySpriteWidth, enemySpriteHeight),
                                                    Color.White,
                                                    0.0f,
                                                    Vector2.Zero,
                                                    0.8f,
                                                    SpriteEffects.None,
                                                    enemyPosition.Y / 650 + 0.01f);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}