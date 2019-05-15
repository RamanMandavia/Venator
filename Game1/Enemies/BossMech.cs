using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Enemies
{
    class BossMech : EnemiesAbstract
    {
        /*
         * Will Walrond
         * 
         * 
         * 
         * Boss Goals:
         * Has special front hitbox: blocks all attacks and bullets
         * Back is vulnerable to dmg
         * 
         * Behaviour flow chart:
         * 1- walks into screen
         * 2- boss 'sees' in 2 cardinal directional lines
         * 3-if player crosses one of these lines, boss charges
         * 4-if player has not crossed a line, boss will fire projectiles.
         * 
         * Charge
         * boss quickly crosses the screen, until he hits the edge of the screen.
         * After hitting the edge, he stands still facing the wall for a couple seconds
         * 
         * Projectiles
         * Fires basic bullets in patterns
         * Periodically fires a missile that blows up after a set amount of time
         * 
         */

        Rectangle sheildhitbox;
        Rectangle seeleft;
        Rectangle seeright;
        Texture2D bullettexture;

        Texture2D chargerightsprite;
        Texture2D chargeleftsprite;

        int bosssizex = 95, bosssizey = 192;
        int windowsizex;
        int windowsizey;
        bool charging = false;
        bool chargedirection = false;
        List<EnemyBullet> bullets;
        int windowx, windowy;
        bool facingleft = true;
        int chargeframes = 0;
        EnemyDirection prevdirection;
        int deathframes;
        double hurttime;
        double chargecooldown = 1;
        

        public Rectangle SheildHitBox { get { return sheildhitbox; } }

        public BossMech(int x, int y, Texture2D enemyTextureRight, Texture2D enemyTextureLeft, Texture2D enemyTextureDead,Texture2D chargerightsprite, Texture2D chargeleftsprite,Texture2D bullettexture, int windowx, int windowy) 
            : base(100, 15, 1, new Vector2(x,y), enemyTextureRight, enemyTextureLeft, enemyTextureDead)
        {

            this.bullettexture = bullettexture;
            sheildhitbox = new Rectangle((int)enemyPosition.X + 50 + bosssizex/2, (int)enemyPosition.Y + 25, bosssizex / 2, bosssizey);
            enemyHitBox = new Rectangle((int)enemyPosition.X + 50, (int)enemyPosition.Y + 25, bosssizex/2, bosssizey);
            bullets = new List<EnemyBullet>();
            this.windowsizex = windowx;
            this.windowsizey = windowy;
            prevdirection = EnemyFacing;
            deathframes = 0;
            this.chargerightsprite = chargerightsprite;
            this.chargeleftsprite = chargeleftsprite;
        }

        public override void CheckAndDealDamage(Player player)
        {
            if (enemyHitBox.Intersects(player.PlayerHitBox) || sheildhitbox.Intersects(player.PlayerHitBox))
            {
                
                player.TakeDamage(Damage);
                
            }

            for(int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].BulletCircle.Intersects(player.PlayerHitBox))
                {
                    player.TakeDamage(Damage);
                    bullets.RemoveAt(i);
                    i--;
                }//removes the bullet if it goes out of bounds
                else if (bullets[i].Bullet.X < -100 || bullets[i].Bullet.X > 900 || bullets[i].Bullet.Y < -100 || bullets[i].Bullet.Y > 850)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color reg = Color.White;

            if (IsDamaged) reg = Color.Red;

            if (exists)
            {
                if(Health > 0)
                {
                    if (!charging)
                    {
                        switch (EnemyFacing)
                        {
                            case EnemyDirection.Right:
                                if(walkCycleFrame >3)
                                {
                                    spriteBatch.Draw(
                                            enemyTextureRight,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * (walkCycleFrame - 4), enemySpriteHeight, enemySpriteWidth, enemySpriteHeight),
                                            reg,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                }
                                else
                                {
                                    spriteBatch.Draw(
                                            enemyTextureRight,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * walkCycleFrame, 0, enemySpriteWidth, enemySpriteHeight),
                                            reg,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                }
                                break;
                            case EnemyDirection.Left:
                                if (walkCycleFrame < 4)
                                {
                                    spriteBatch.Draw(
                                        enemyTextureLeft,
                                        enemyPosition,
                                        new Rectangle(enemySpriteWidth * walkCycleFrame, 0, enemySpriteWidth, enemySpriteHeight),
                                        reg,
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
                                        new Rectangle(enemySpriteWidth * (walkCycleFrame -4), enemySpriteHeight, enemySpriteWidth, enemySpriteHeight),
                                        reg,
                                        0.0f,
                                        Vector2.Zero,
                                        0.8f,
                                        SpriteEffects.None,
                                        enemyPosition.Y / 650 + 0.01f);
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (EnemyFacing)
                        {
                            case EnemyDirection.Right:
                                spriteBatch.Draw(
                                            chargerightsprite,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * chargeframes, 0, enemySpriteWidth, enemySpriteHeight),
                                            reg,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                break;
                            case EnemyDirection.Left:
                                spriteBatch.Draw(
                                            chargeleftsprite,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * chargeframes, 0, enemySpriteWidth, enemySpriteHeight),
                                            reg,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                                break;
                        }
                    }
                }
                else
                {
                    if(deathframes < 4)
                    {
                        spriteBatch.Draw(
                                            enemyTextureDead,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * deathframes, 0, enemySpriteWidth, enemySpriteHeight),
                                            Color.White,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                    }
                    else if(deathframes < 8)
                    {
                        spriteBatch.Draw(
                                            enemyTextureDead,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * (deathframes - 4), enemySpriteHeight, enemySpriteWidth, enemySpriteHeight),
                                            Color.White,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                    }
                    else if(deathframes < 12)
                    {
                        spriteBatch.Draw(
                                            enemyTextureDead,
                                            enemyPosition,
                                            new Rectangle(enemySpriteWidth * (deathframes - 8), enemySpriteHeight * 2, enemySpriteWidth, enemySpriteHeight),
                                            Color.White,
                                            0.0f,
                                            Vector2.Zero,
                                            0.8f,
                                            SpriteEffects.None,
                                            enemyPosition.Y / 650 + 0.01f);
                    }
                }

            }

            foreach (EnemyBullet s in bullets)
            {
               Console.WriteLine(bullets.Count + " " + s.Bullet.X + " " + s.Bullet.Y + " " + s.Bullet.Width + " " + s.Bullet.Height);


                spriteBatch.Draw(
                bullettexture,
                new Rectangle(s.BulletCircle.X - 100, s.BulletCircle.Y - 100, 200, 200),
                new Rectangle((bullettexture.Width / 4) * s.Frame, 0, (bullettexture.Width / 4), bullettexture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.1f
                );
            }
        }

        public override void Update(Player player, GameTime gameTime)
        {
            if (Health > 0)
            {
                CheckAndDealDamage(player);

                Console.WriteLine(EnemyFacing);

                if (IsDamaged)
                {
                    hurttime += gameTime.ElapsedGameTime.TotalSeconds;
                    if(hurttime > InvicibilityTimerAfterDamaged)
                    {
                        hurttime = 0;
                        IsDamaged = false;
                    }
                }



                foreach (EnemyBullet b in bullets)
                {
                    

                    b.Update(player);

                }

                if (!charging)
                {
                    if (player.PlayerPosition.X < enemyPosition.X) EnemyFacing = EnemyDirection.Left;
                    else EnemyFacing = EnemyDirection.Right;
                }

                if (bullets.Count > 0)
                {
                    Console.WriteLine(bullets[0].Bullet.X + "  " + bullets[0].Bullet.Y);
                }

                chargecooldown += gameTime.ElapsedGameTime.TotalSeconds;

                //checks if the player is directly to the left or right of the boss. If so, then charges in their direction
                if (player.PlayerPosition.Y > enemyPosition.Y - player.PlayerHitBox.Height && player.PlayerPosition.Y < enemyHitBox.Y + enemyHitBox.Height && !charging && chargecooldown >= .5)
                {
                    charging = true;
                    if (player.PlayerHitBox.X > enemyHitBox.X) chargedirection = false;
                    else chargedirection = true;
                    TimeThatHasPassed = 0;
                    
                }

                //the charge attack
                if (charging)
                {

                    TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds; // charge attack takes a set amount of time
                    if (TimeThatHasPassed >= .5) // after a short wind up
                    {

                        int speed = 8;

                        if (chargedirection) // left charge
                        {
                            if (enemyPosition.X - speed > 0)
                            {
                                enemyPosition.X -= speed;
                                enemyHitBox.X -= speed;
                                sheildhitbox.X -= speed;
                            }
                            else
                            {
                                chargecooldown = 0;
                                charging = false;
                            }
                        }
                        else // right charge
                        {
                            if (enemyPosition.X + speed + enemyHitBox.Width * 2 < windowsizex)
                            {
                                enemyPosition.X += speed;
                                enemyHitBox.X += speed;
                                sheildhitbox.X += speed;
                            }
                            else
                            {
                                chargecooldown = 0;
                                charging = false;
                            }
                        }
                    }
                }
                else
                {
                    

                    if (EnemyFacing == EnemyDirection.Right)
                    {
                        //can move when it is not stunned
                        enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - (enemyPosition.X + (enemyHitBox.Width + 40))) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 60) * Speed);
                        enemyHitBox = new Rectangle((int)enemyPosition.X + 40, (int)enemyPosition.Y, 110, 192);
                        sheildhitbox = new Rectangle((int)enemyPosition.X + 145, (int)enemyPosition.Y, 30, 192);

                    }
                    else if (EnemyFacing == EnemyDirection.Left)
                    {
                        enemyPosition = new Vector2(enemyPosition.X + Math.Sign(player.PlayerHitBox.X - enemyPosition.X) * Speed, enemyPosition.Y + Math.Sign(player.PlayerHitBox.Y - enemyPosition.Y - 60) * Speed);
                        enemyHitBox = new Rectangle((int)enemyPosition.X + 45, (int)enemyPosition.Y, 110, 192);
                        sheildhitbox = new Rectangle((int)enemyPosition.X + 15, (int)enemyPosition.Y, 30, 192);

                    }


                    TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds;

                    if (TimeThatHasPassed >= .6)
                    {
                        ShootBullet(player);
                        TimeThatHasPassed = 0;
                    }
                }
                prevdirection = EnemyFacing;
            }
            
            
        }

        public void ShootBullet(Player player)
        {
            if (EnemyFacing == EnemyDirection.Left)
            {
                if (player.PlayerPosition.X > enemyPosition.X)
                {
                    if (player.PlayerPosition.Y > enemyPosition.Y)
                    {
                        bullets.Add(new EnemyBullet(new Rectangle((int)enemyHitBox.X, (int)enemyHitBox.Y + enemyHitBox.Height / 2, 50, 50), 0, bullettexture));
                    }
                    else bullets.Add(new EnemyBullet(new Rectangle((int)enemyHitBox.X, (int)enemyHitBox.Y + enemyHitBox.Height / 2, 50, 50), 1, bullettexture));
                }
                else
                {
                    if (player.PlayerPosition.Y > enemyPosition.Y)
                    {
                        bullets.Add(new EnemyBullet(new Rectangle((int)enemyHitBox.X, (int)enemyHitBox.Y + enemyHitBox.Height / 2, 50, 50), 3, bullettexture));
                    }
                    else bullets.Add(new EnemyBullet(new Rectangle((int)enemyHitBox.X, (int)enemyHitBox.Y + enemyHitBox.Height / 2, 50, 50), 2, bullettexture));
                }
            }
            else if (EnemyFacing == EnemyDirection.Right)
            {
                if (player.PlayerPosition.X > enemyPosition.X)
                {
                    if (player.PlayerPosition.Y > enemyPosition.Y)
                    {
                        bullets.Add(new EnemyBullet(new Rectangle((int)enemyHitBox.X, (int)enemyHitBox.Y + enemyHitBox.Height / 2, 50, 50), 0, bullettexture));
                    }
                    else bullets.Add(new EnemyBullet(new Rectangle((int)enemyHitBox.X, (int)enemyHitBox.Y + enemyHitBox.Height / 2, 50, 50), 1, bullettexture));
                }
                else
                {
                    if (player.PlayerPosition.Y > enemyPosition.Y)
                    {
                        bullets.Add(new EnemyBullet(new Rectangle((int)enemyHitBox.X, (int)enemyHitBox.Y + enemyHitBox.Height / 2, 50, 50), 3, bullettexture));
                    }
                    else bullets.Add(new EnemyBullet(new Rectangle((int)enemyHitBox.X, (int)enemyHitBox.Y + enemyHitBox.Height / 2, 50, 50), 2, bullettexture));
                }
            }
        }

        public override void UpdateMovementFrame()
        {
            if (this.Health > 0)
            {
                if (charging)
                {
                    chargeframes++;
                    if(chargeframes > 3)
                        chargeframes = 0;
                }
                else
                {
                    walkCycleFrame++;
                    if (walkCycleFrame > 7)
                        walkCycleFrame = 0;
                    foreach(EnemyBullet b in bullets)
                    {
                        b.AnimationFrame++;
                        if (b.AnimationFrame > 3)
                            b.AnimationFrame = 0;
                    }
                }
            }
            else
            {
                deathframes++;
                if(deathframes == 13)
                {
                    Exists = false;
                }
            }
        }

        
    }

    class EnemyBullet
    {
        Rectangle bullet;
        int direction;
        Texture2D texture;
        int animationframe;
        int framelength;
        int speed = 5;
        private Circle bulletCircle;
        //int durability; I'll implement this later. Makes projectiles destroyable, since they would be homing

        public Rectangle Bullet { get { return bullet; } }

        public Texture2D Texture { get { return texture; } }

        public int Frame { get { return animationframe; } }

        public Circle BulletCircle { get { return bulletCircle; } set { bulletCircle = value; } }

        public int AnimationFrame { get { return animationframe; } set { animationframe = value; } }

        //direction:
        //0 for southeast
        //1 for northeast
        //2 for northwest
        //3 for southwest
        public EnemyBullet(Rectangle bullet, int direction, Texture2D texture)
        {
            this.bullet = bullet;
            animationframe = 0;

            this.direction = direction;
            
            this.texture = texture;

            framelength = texture.Width / 4;

            bulletCircle.X = bullet.X + 50;
            bulletCircle.Y = bullet.Y + 50;
            bulletCircle.Radius = 1;
        }

        public void Update(Player player)
        {
            switch (direction)
            {
                case 0:
                    bullet.X += speed;
                    bullet.Y += speed;
                    bulletCircle.X = bulletCircle.X + speed;
                    bulletCircle.Y = bulletCircle.Y + speed;
                    break;
                case 1:
                    bullet.X += speed;
                    bullet.Y -= speed;
                    bulletCircle.X = bulletCircle.X + speed;
                    bulletCircle.Y = bulletCircle.Y - speed;
                    break;
                case 2:
                    bullet.X -= speed;
                    bullet.Y -= speed;
                    bulletCircle.X = bulletCircle.X - speed;
                    bulletCircle.Y = bulletCircle.Y - speed;

                    break;
                case 3:
                    bullet.X -= speed;
                    bullet.Y += speed;
                    bulletCircle.X = bulletCircle.X - speed;
                    bulletCircle.Y = bulletCircle.Y + speed;
                    break;
            }
        }

        

    }


}
