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
    class Crate
    {
        // Raman Mandavia
        // Fields
        private int colorVal; //Determines what frame of the texture will be drawn, as each frame is one of four different colors
        private int durability;
        private int type; // 1 for rectangle crate, 2 for box crate
        private int explosionFrame;
        private int flickerFrame;
        private bool isExplosive;
        private bool hasExploded;
        private bool takingDamage;
        private bool hitBoxActive;
        private double damageTimer;
        private double disappearTimer;
        private bool exists;
        private Texture2D boxCrates;
        private Texture2D rectangleCrates;
        private Texture2D explosionTexture;
        private Rectangle hitBox;
        private Rectangle explosionRectangle;
        private Circle explosionHitCircle;
        private Vector2 position;
        private Random rng;

        // Properites
        public int ColorVal
        { get { return colorVal; } }

        public int Durability
        {
            get { return durability; }
            set { durability = value; }
        }

        public bool Exists
        { get { return exists; } }

        public bool IsExplosive
        { get { return isExplosive; } }

        public bool HasExploded
        { get { return hasExploded; } }

        public Rectangle HitBox
        { get { return hitBox; } }

        public Circle ExplosionHitCircle
        { get { return explosionHitCircle; } }

        public Vector2 Position
        { get { return position; } }

        // Constructor
        public Crate(int type, bool isExplosive, Texture2D boxCrates, Texture2D rectangleCrates, Texture2D explosionTexture, Vector2 position)
        {
            this.type = type;
            this.isExplosive = isExplosive;
            this.boxCrates = boxCrates;
            this.rectangleCrates = rectangleCrates;
            this.explosionTexture = explosionTexture;
            this.position = position;

            rng = new Random();

            durability = 20;
            hasExploded = false;
            exists = true;
            takingDamage = false;
            hitBoxActive = true;
            damageTimer = 0;
            disappearTimer = 0;
            explosionFrame = 0;
            flickerFrame = 0;

            if (this.type == 1)
            {
                if (this.isExplosive == true)
                {
                    colorVal = 1;
                }
                else
                {
                    int val = rng.Next(2, 5);
                    if (val == 4)            // Becuase the red texture for the explosive crate is on the second index, this is the easiest way to randomly get one of the other colors
                        val = 0;
                    colorVal = val;
                }
                hitBox = new Rectangle((int)this.position.X + 32, (int)this.position.Y + 40, 64, 52);
            }
            else if (this.type == 2)
            {
                if (this.isExplosive == true)
                {
                    colorVal = 1;
                }
                else
                {
                    int val = rng.Next(2, 5);
                    if (val == 4)
                        val = 0;
                    colorVal = val;
                }
                hitBox = new Rectangle((int)this.position.X + 44, (int)this.position.Y + 44, 40, 44);
            }

            explosionHitCircle = new Circle(0, 0, 45);
            explosionRectangle = new Rectangle(explosionHitCircle.X - 70, explosionHitCircle.Y - 85, 190, 190);
        }

        // Methods

        public void Update(GameTime gameTime, List<Enemies.EnemiesAbstract> listOfEnemies, Player player)
        {
            if (exists)
            {
                if (takingDamage)
                {
                    hitBoxActive = false;
                    damageTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (damageTimer >= .5)
                    {
                        takingDamage = false;
                        hitBoxActive = true;
                        damageTimer = 0;
                    }
                }

                if (durability <= 0 && !isExplosive)
                {
                    if (disappearTimer > 1.0)
                    {
                        exists = false;
                    }
                }
            }
            else if (!exists && isExplosive && !hasExploded)
            {
                explosionHitCircle.X = (int)(position.X + hitBox.Width / 2);
                explosionHitCircle.Y = (int)(position.Y + hitBox.Height / 2);
                explosionRectangle = new Rectangle(explosionHitCircle.X - 70, explosionHitCircle.Y - 85, 190, 190);

                for (int b = 0; b < listOfEnemies.Count; b++)
                {
                    if (explosionHitCircle.Intersects(listOfEnemies[b].EnemyHitBox))
                    {
                        listOfEnemies[b].TakeDamage(20);
                    }
                }

                if (explosionHitCircle.Intersects(player.PlayerHitBox))
                {
                    player.TakeDamage(40);
                }
            }
        }

        public void TakeDamage(int damage)
        {
            if (hitBoxActive)
            {
                takingDamage = true;
                durability -= damage;
                if (durability <= 0 && isExplosive)
                    exists = false;
            }

        }

        public void UpdateFlickerFrame()
        {
            flickerFrame++;
            if (flickerFrame > 3)
                flickerFrame = 0;
        }

        public void UpdateExplosionFrame()
        {
            if(!exists && isExplosive && !hasExploded)
            {
                explosionFrame++;
                if (explosionFrame > 3)
                {
                    explosionFrame = 0;
                    hasExploded = true;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (exists)
            {
                if (type == 1)
                {
                    if (!takingDamage)
                        sb.Draw(
                            rectangleCrates,
                            position,
                            new Rectangle(rectangleCrates.Width * colorVal, 0, rectangleCrates.Width / 4, rectangleCrates.Height),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
                    else if (takingDamage)
                        sb.Draw(
                            rectangleCrates,
                            position,
                            new Rectangle(rectangleCrates.Width * colorVal, 0, rectangleCrates.Width / 4, rectangleCrates.Height),
                            Color.Red,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
                    else if (durability <= 0 && (flickerFrame % 2 == 0))
                        sb.Draw(
                            rectangleCrates,
                            position,
                            new Rectangle(rectangleCrates.Width * colorVal, 0, rectangleCrates.Width / 4, rectangleCrates.Height),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
                }
                else if (type == 2)
                {
                    if (!takingDamage)
                        sb.Draw(
                            boxCrates,
                            position,
                            new Rectangle(boxCrates.Width * colorVal, 0, boxCrates.Width / 4, boxCrates.Height),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
                    else if (takingDamage)
                        sb.Draw(
                            boxCrates,
                            position,
                            new Rectangle(boxCrates.Width * colorVal, 0, boxCrates.Width / 4, boxCrates.Height),
                            Color.Red,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
                    else if (durability <= 0 && (flickerFrame % 2 == 0))
                        sb.Draw(
                            boxCrates,
                            position,
                            new Rectangle(boxCrates.Width * colorVal, 0, boxCrates.Width / 4, boxCrates.Height),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
                }
            }
            else if (isExplosive && !hasExploded)
            {
                sb.Draw(
                        explosionTexture,
                        explosionRectangle,
                        new Rectangle(explosionTexture.Height * explosionFrame, 0, explosionTexture.Width / 4, explosionTexture.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0.0f);
            }
        }
    }
}
