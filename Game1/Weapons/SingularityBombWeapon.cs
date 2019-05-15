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
    class SingularityBombWeapon : WeaponAbstract
    {
        //Will Walrond
        //implementation by Alex Sarnese
        //This bomb will do minimal damage, but drag enemies towards the point of detonation

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

        private int explosionXLocation;
        private int explosionYLocation;

        Random rng;

        public Circle WeaponDamageArea { get { return weaponDamageArea; } }
        

        public SingularityBombWeapon(Rectangle WeaponSpawnLocation, Texture2D bombTexture,
           Texture2D explosionTexture)
            : base("Singularity Bomb", TypeOfWeapon.Bomb, 2, 10, 11, 6f, WeaponSpawnLocation, bombTexture)
        {
            weaponDamageArea = new Circle(WeaponSpawnLocation.X, WeaponSpawnLocation.Y, Range);
            explosionTime = AttackPhaseDurationTime;
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
            rng = new Random();
        }

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

        protected override void Attacking(List<EnemiesAbstract> listOfEnemies, Player player)
        {
            //if we spent longer in attack phase (bomb waiting to explode) than its specified duration, 
            //end the attack phase and start the explosion
            if (!readyToExplode)
            {
                TimeThatHasPassed = 0;
                readyToExplode = true;
            }
            else if (exploded == false && TimeThatHasPassed < AttackPhaseDurationTime / 15)
            {
                //bomb falls downward
                location.Y += 1;
            }
            else if (exploded == false && TimeThatHasPassed < AttackPhaseDurationTime / 3)
            {
                //bomb does nothing and stands still
            }
            else if(exploded == false)
            {
                //starts the explosion
                exploded = true;
                explosionXLocation = location.X - (widthOfSingleExplosionSprite / 2) + widthOfSingleBombSprite / 2 - 10;
                explosionYLocation = location.Y - heightOfSingleExplosionSprite / 2;
            }
            else if (TimeThatHasPassed < AttackPhaseDurationTime)
            {
                //drags and damages enemies into prolonged explosion

                weaponDamageArea.X = location.X+65;
                weaponDamageArea.Y = location.Y+65;
                weaponDamageArea.Radius = Range;


                for (int i = 0; i < listOfEnemies.Count; i++)
                {
                    if (listOfEnemies[i].DropType != TypeOfWeapon.Gun)
                    {
                        //drags enemy closer to the singularity
                        listOfEnemies[i].EnemyPosition = new Vector2(listOfEnemies[i].EnemyPosition.X - Math.Sign(listOfEnemies[i].EnemyPosition.X - (weaponDamageArea.X - 65)) * 3.5f, listOfEnemies[i].EnemyPosition.Y - Math.Sign(listOfEnemies[i].EnemyPosition.Y - (weaponDamageArea.Y - 65)) * 3.5f);
                    }

                    //checks to see if enemy is damaged
                    if (weaponDamageArea.Intersects(listOfEnemies[i].EnemyHitBox))
                    {
                        listOfEnemies[i].TakeDamage(Damage);
                    }
                }

                //drag player towards center with less force than enemy faces
                player.Move(-Math.Sign((int)player.PlayerPosition.X - (weaponDamageArea.X - 65)) * 2, -Math.Sign((int)player.PlayerPosition.Y - (weaponDamageArea.Y - 65)) * 2);

                //damages player if player is inside explosion
                if (weaponDamageArea.Intersects(player.PlayerHitBox))
                {
                    player.TakeDamage(Damage*4);
                }

            }
            else {
                //ends attacking phase and removes explosion (bomb returns to player location at next Update frame)
                IsAttacking = false;
                exploded = false;
                readyToExplode = false;
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
            if (player.PreviousPlayerState == PlayerState.FaceUp)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 25;
                location.Y = player.PlayerHitBox.Y - 10;
            }

            //make weapon face downward if player is facing down
            else if (player.PreviousPlayerState == PlayerState.FaceDown)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 25;
                location.Y = player.PlayerHitBox.Y;
            }

            //makes the weapon face right if player is facing right
            else if (player.PreviousPlayerState == PlayerState.FaceRight)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 40;
                location.Y = player.PlayerHitBox.Y - 5;
            }

            //make weapon face left if player is facing left
            else if (player.PreviousPlayerState == PlayerState.FaceLeft)
            {
                //update location to the carrier's location and change direction
                location.X = player.PlayerHitBox.X - 50;
                location.Y = player.PlayerHitBox.Y - 5;
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
            float explosionLayer = explosionYLocation / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if(bombLayer <= 0)
            {
                bombLayer = 0.01f;
            }
            if(explosionLayer <= 0)
            {
                explosionLayer = 0.01f;
            }


            //If player is null, then this weapon is dropped and is not being held by the player. 
            //Thus does not change the weapon's direction
            if (player != null)
            {
                //change the bomb's animation every 1/4th second
                if (TimeThatHasPassed % 1 < 0.25f)
                {
                    bombAnimationFrame = 0;
                }
                else if (TimeThatHasPassed % 1 < 0.5f)
                {
                    bombAnimationFrame = 1;
                }
                else if (TimeThatHasPassed % 1 < 0.75f)
                {
                    bombAnimationFrame = 2;
                }
                else
                {
                    bombAnimationFrame = 3;
                }

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
                        new Vector2(explosionXLocation + 8, explosionYLocation + 60),
                        new Rectangle(0, 0, widthOfSingleExplosionSprite, heightOfSingleExplosionSprite),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        explosionLayer);
                    }
                    else if (TimeThatHasPassed < explosionTime / 2)
                    {
                        spriteBatch.Draw(
                        explosionTexture,
                        new Vector2(explosionXLocation + 8, explosionYLocation + 60),
                        new Rectangle(widthOfSingleExplosionSprite, 0, widthOfSingleExplosionSprite, heightOfSingleExplosionSprite),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        explosionLayer);
                    }
                    else if (TimeThatHasPassed < (explosionTime/4) * 3)
                    {
                        spriteBatch.Draw(
                        explosionTexture,
                        new Vector2(explosionXLocation + 8, explosionYLocation + 60),
                        new Rectangle(widthOfSingleExplosionSprite * 2, 0, widthOfSingleExplosionSprite, heightOfSingleExplosionSprite),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        explosionLayer);
                    }
                    else if (TimeThatHasPassed < explosionTime)
                    {
                        spriteBatch.Draw(
                        explosionTexture,
                        new Vector2(explosionXLocation + 8, explosionYLocation + 60),
                        new Rectangle(widthOfSingleExplosionSprite * 3, 0, widthOfSingleExplosionSprite, heightOfSingleExplosionSprite),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
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
    }
}
