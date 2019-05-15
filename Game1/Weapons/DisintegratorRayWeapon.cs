using Game1.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Weapons
{
    class DisintegratorRayWeapon : WeaponAbstract
    {
        // Alex Sarnese

        //Field
        Rectangle beamPath;

        //required textures for proper directional drawing
        private Texture2D faceDown;
        private Texture2D faceUp;
        private Texture2D faceRight;
        private Texture2D faceLeft;
        private Texture2D beamHorizontal;
        private Texture2D beamVertical;
        private int widthOfSingleSpriteHorizontal;
        private int HeightOfSingleSpriteVertical;

        private int damageIncrease;
        private bool engaged;
        private float timeToUseForCooldown;
        private int timeTillRampUp;
        public float timerUsedForCooldown;

        private EnemiesAbstract closestEnemy;
        private EnemiesAbstract enemyBeingAttacked;
        
        private int gunAnimationFrame;


        //Constructor

        /// <summary>
        /// Creates instance of a Disintegrator Ray gun weapon with given base stats
        /// </summary>
        /// <param name="WeaponSpawnLocation">Location for the gun's spawn</param>
        /// <param name="faceDown">texture of weapon facing downward</param>
        /// <param name="faceUp">texture of weapon facing upward</param>
        /// <param name="faceRight">texture of weapon facing right</param>
        /// <param name="faceLeft">texture of weapon facing left</param>
        /// <param name="beamHorizontal">texture of beam going horizontal</param>
        /// <param name="beamVertical">texture of the beam going vertical</param>
        public DisintegratorRayWeapon(Rectangle WeaponSpawnLocation, Texture2D faceDown,
                             Texture2D faceUp, Texture2D faceRight, Texture2D faceLeft,
                             Texture2D beamHorizontal, Texture2D beamVertical)
            : base("Disintegrator Ray", TypeOfWeapon.Gun, 1, 1, -1, 0, WeaponSpawnLocation, faceRight)
        {
            //The cooldown is -1 because this weapon does not uses the coolDown for actual cooldown time
            //Change timeToUseForCooldown instead to change how long this weapon rests between shots
            timeToUseForCooldown = 4;
            timerUsedForCooldown = 0;
            TimeThatHasPassed = 4;

            this.faceDown = faceDown;
            this.faceUp = faceUp;
            this.faceRight = faceRight;
            this.faceLeft = faceLeft;

            widthOfSingleSpriteHorizontal = faceLeft.Width / 2;
            HeightOfSingleSpriteVertical = faceUp.Height / 2;

            this.beamHorizontal = beamHorizontal;
            this.beamVertical = beamVertical;

            gunAnimationFrame = 0;
            beamPath = new Rectangle(0, 0, 0, 0);
            damageIncrease = 1;
            timeTillRampUp = 3;
            
            engaged = false;
        }



        //Methods


        /// <summary>
        /// Updates the weapon's location and where it faces 
        /// Calls up the Attacking method used for dealing damage
        /// Needs to be called in update loop in Game1
        /// </summary>
        /// <param name="characterPosition">the player's location as a rectangle</param>
        /// <param name="mouseXCoordinate">the X coordinate of the mouse needed for determing what direction the weapon should face</param>
        /// <param name="gameTime">the timer to be used to keep track of cooldown time</param>
        public override void Update(List<EnemiesAbstract> listOfEnemies, Player player, GameTime gameTime)
        {
            //stores how much time has passed since last update and stores it in a variable
            TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds;

            //checks to see if the weapon should go into attacking phase
            if (IsAttacking)
            {

                // change weapon's direction 
                DetermineWeaponDirection(player);

                if (timerUsedForCooldown >= 0)
                {
                    IsAttacking = false;
                    CooldownTime = timeToUseForCooldown;
                    timerUsedForCooldown = timeToUseForCooldown - (float)TimeThatHasPassed;
                }
                else
                {
                    CooldownTime = -1;
                    //progress through attacking sequence
                    Attacking(listOfEnemies, player);
                }
            }
            else
            {
                //keeps everything set to default value when not attacking anymore
                enemyBeingAttacked = null;
                damageIncrease = 1;
                engaged = false;

                if(timerUsedForCooldown >= 0)
                {
                    CooldownTime = timeToUseForCooldown;
                    timerUsedForCooldown = timeToUseForCooldown - (float)TimeThatHasPassed;
                }
                else
                {
                    CooldownTime = -1;
                    TimeThatHasPassed = 0;
                }

                //If player is null, then this weapon is dropped and is not being held by the player. 
                //Thus does nothing to weapon's position
                if (player != null)
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
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) - 15;
                location.Y = player.PlayerHitBox.Y - 25;

                WeaponFacing = WeaponDirection.Up;
            }

            //make weapon face downward if player is facing down
            else if (player.CurrentPlayerState == PlayerState.FaceDown)
            {
                //update location to the carrier's center bottom
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) - 25;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height) - 55;

                WeaponFacing = WeaponDirection.Down;
            }

            //makes the weapon face right if player is facing right
            else if (player.CurrentPlayerState == PlayerState.FaceRight)
            {
                //update location to the carrier's center right side
                location.X = player.PlayerHitBox.X + player.PlayerHitBox.Width - 41;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) - 73;

                WeaponFacing = WeaponDirection.Right;
            }

            //make weapon face left if player is facing left
            else if (player.CurrentPlayerState == PlayerState.FaceLeft)
            {
                //update location to the carrier's center left side
                location.X = player.PlayerHitBox.X - 55;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) - 45;

                WeaponFacing = WeaponDirection.Left;
            }
        }

        //Attack() method is defined in WeaponAbstract and can be called elsewhere to make this weapon attack



        /// <summary>
        ///                 ATTACKING METHOD FOR Disintegrator Ray WEAPON
        /// 
        /// creates a rectangle in a straight line and checks if hit enemy.
        /// If they do, calls up the enemy's TakenDamage() method 
        /// </summary>
        /// <param name="listOfEnemies">the list of enemies to check for collusion with</param>
        /// <param name="player">the player to use for location of weapon</param>
        protected override void Attacking(List<EnemiesAbstract> listOfEnemies, Player player)
        {
            closestEnemy = null;

            //creates a path going in direction that the gun is facing

            //make beam in direction that is is facing
            if (WeaponFacing == WeaponDirection.Up)
            {
                //shoots up
                beamPath.X = location.X + faceUp.Width / 2 - 5;
                beamPath.Y = 0;
                beamPath.Width = 4;
                beamPath.Height = location.Y + 5;

                //loops through list of enemies and see if it hit one of them
                //if so, damages the enemy
                for (int b = 0; b < listOfEnemies.Count; b++)
                {
                    if (beamPath.Intersects(listOfEnemies[b].EnemyHitBox) && listOfEnemies[b].Health > 0)
                    {
                        //stores enemy in closest enemy variable if it intersects the beam.
                        //replaces that stored enemy with the new enemy if the new one is closer to weapon
                        if (closestEnemy == null)
                        {
                            closestEnemy = listOfEnemies[b];
                        }
                        else if (listOfEnemies[b].EnemyHitBox.Y > closestEnemy.EnemyHitBox.Y)
                        {
                            closestEnemy = listOfEnemies[b];
                        }
                    }
                }

                //checks to see if we are staying focused on the same enemy and increase damage if we stayed focused long enough
                if (closestEnemy != null && enemyBeingAttacked == closestEnemy)
                {
                    if (TimeThatHasPassed > timeTillRampUp)
                    {
                        damageIncrease += 1;
                    }

                    closestEnemy.TakeDamageButNoKnockback(Damage + damageIncrease);
                }
                else
                {
                    Reset();
                }

                //modify beam path for the draw method later on
                if(closestEnemy != null)
                {
                    engaged = true;
                    beamPath.Y = closestEnemy.EnemyHitBox.Y + closestEnemy.EnemyHitBox.Height/2;
                    beamPath.Height -= closestEnemy.EnemyHitBox.Y + closestEnemy.EnemyHitBox.Height / 2;
                }
                else
                {
                    engaged = false;
                }
            }
            else if (WeaponFacing == WeaponDirection.Down)
            {
                //shoots down
                beamPath.X = location.X + faceDown.Width / 2 - 5;
                beamPath.Y = location.Y + faceDown.Height / 2 - 2;
                beamPath.Width = 4;
                beamPath.Height = 650 - location.Y;

                //loops through list of enemies and see if it hit one of them
                //if so, damages the enemy
                for (int b = 0; b < listOfEnemies.Count; b++)
                {
                    if (beamPath.Intersects(listOfEnemies[b].EnemyHitBox) && listOfEnemies[b].Health > 0)
                    {
                        //stores enemy in closest enemy variable if it intersects the beam.
                        //replaces that stored enemy with the new enemy if the new one is closer to weapon
                        if (closestEnemy == null)
                        {
                            closestEnemy = listOfEnemies[b];
                        }
                        else if (listOfEnemies[b].EnemyHitBox.Y < closestEnemy.EnemyHitBox.Y)
                        {
                            closestEnemy = listOfEnemies[b];
                        }
                    }
                }

                //checks to see if we are staying focused on the same enemy and increase damage if we stayed focused long enough
                if (closestEnemy != null && enemyBeingAttacked == closestEnemy)
                {
                    if (TimeThatHasPassed > timeTillRampUp)
                    {
                        damageIncrease += 1;
                    }

                    closestEnemy.TakeDamageButNoKnockback(Damage + damageIncrease);
                }
                else
                {
                    Reset();
                }

                //modify beam path for the draw method later on
                if (closestEnemy != null)
                {
                    engaged = true;
                    beamPath.Height = closestEnemy.EnemyHitBox.Y - closestEnemy.EnemyHitBox.Height/2 - location.Y;
                }
                else
                {
                    engaged = false;
                }
            }
            else if (WeaponFacing == WeaponDirection.Right)
            {
                //shoots right
                beamPath.X = location.X + faceLeft.Width / 2 + 12;
                beamPath.Y = location.Y + faceLeft.Height / 2 + 15;
                beamPath.Width = 800 - location.X;
                beamPath.Height = 4;

                //loops through list of enemies and see if it hit one of them
                //if so, damages the enemy
                for (int b = 0; b < listOfEnemies.Count; b++)
                {
                    if (beamPath.Intersects(listOfEnemies[b].EnemyHitBox) && listOfEnemies[b].Health > 0)
                    {
                        //stores enemy in closest enemy variable if it intersects the beam.
                        //replaces that stored enemy with the new enemy if the new one is closer to weapon
                        if (closestEnemy == null)
                        {
                            closestEnemy = listOfEnemies[b];
                        }
                        else if (listOfEnemies[b].EnemyHitBox.X < closestEnemy.EnemyHitBox.X)
                        {
                            closestEnemy = listOfEnemies[b];
                        }
                    }
                }

                //checks to see if we are staying focused on the same enemy and increase damage if we stayed focused long enough
                if (closestEnemy != null && enemyBeingAttacked == closestEnemy)
                {
                    if (TimeThatHasPassed > timeTillRampUp)
                    {
                        damageIncrease += 1;
                    }

                    closestEnemy.TakeDamageButNoKnockback(Damage + damageIncrease);
                }
                else
                {
                    Reset();
                }

                //modify beam path for the draw method later on
                if (closestEnemy != null)
                {
                    engaged = true;
                    beamPath.Width = closestEnemy.EnemyHitBox.X - closestEnemy.EnemyHitBox.Width - location.X;
                }
                else
                {
                    engaged = false;
                }
            }
            else
            {
                //shoots left
                beamPath.X = 0;
                beamPath.Y = location.Y + faceLeft.Height / 2 - 10;
                beamPath.Width = location.X + 11;
                beamPath.Height = 4;


                //loops through list of enemies and see if it hit one of them
                //if so, damages the enemy
                for (int b = 0; b < listOfEnemies.Count; b++)
                {
                    if (beamPath.Intersects(listOfEnemies[b].EnemyHitBox) && listOfEnemies[b].Health > 0)
                    {
                        //stores enemy in closest enemy variable if it intersects the beam.
                        //replaces that stored enemy with the new enemy if the new one is closer to weapon
                        if (closestEnemy == null)
                        {
                            closestEnemy = listOfEnemies[b];
                        }
                        else if (listOfEnemies[b].EnemyHitBox.X > closestEnemy.EnemyHitBox.X)
                        {
                            closestEnemy = listOfEnemies[b];
                        }
                    }
                }

                //checks to see if we are staying focused on the same enemy and increase damage if we stayed focused long enough
                if (closestEnemy != null && enemyBeingAttacked == closestEnemy)
                {
                    if (TimeThatHasPassed > timeTillRampUp)
                    {
                        damageIncrease += 1;
                    }

                    closestEnemy.TakeDamageButNoKnockback(Damage + damageIncrease);
                }
                else
                {
                    Reset();
                }

                
                //modify beam path for the draw method later on
                if (closestEnemy != null)
                {
                    engaged = true;
                    beamPath.X = closestEnemy.EnemyHitBox.X + closestEnemy.EnemyHitBox.Width / 2;
                    beamPath.Width -= closestEnemy.EnemyHitBox.X + closestEnemy.EnemyHitBox.Width / 2;
                }
                else
                {
                    engaged = false;
                }
            }
        }


        /// <summary>
        /// reset the weapon to go into cooldown
        /// </summary>
        public void Reset()
        {
            //resets the weapon's damage and timer as we are not focused on the same enemy anymore
            TimeThatHasPassed = 0;
            damageIncrease = 1;
            enemyBeingAttacked = closestEnemy;

            //set the cooldown when the player was shooting an enemy but missed or a new enemy get in the way
            if (engaged == true)
            {
                //triggers cooldown which will stop the weapon from firing next update cycle
                timerUsedForCooldown = timeToUseForCooldown;
            }
        }


        /// <summary>
        /// Draws the weapon on the screen. 
        /// Needs to be called in the draw loop in Game1
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
        /// <param name="player">the player used to know if this is being held or not</param>
        public override void Draw(SpriteBatch spriteBatch, Player player)
        {
            //set the layer for weapon
            float weaponLayer = location.Y / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (weaponLayer <= 0)
            {
                weaponLayer = 0.01f;
            }

            if (IsAttacking)
            {
                //draws the Disintegrator Ray beam path
                if (WeaponFacing == WeaponDirection.Up || WeaponFacing == WeaponDirection.Down)
                {
                    beamPath.Width += 15;

                    spriteBatch.Draw(
                    beamVertical,
                    beamPath,
                    new Rectangle(0, 0, beamVertical.Width, beamVertical.Height),
                    Color.White,
                    0.0f,
                    new Vector2(64, 25),
                    SpriteEffects.None,
                    weaponLayer);
                }
                //If reached here, must be facing left or right then
                else
                {
                    beamPath.Height += 15;

                    spriteBatch.Draw(
                    beamHorizontal,
                    beamPath,
                    new Rectangle(0, 0, beamHorizontal.Width, beamHorizontal.Height),
                    Color.White,
                    0.0f,
                    new Vector2(64, 25),
                    SpriteEffects.None,
                    weaponLayer);
                }

                //makes the gun draw it's on phase
                gunAnimationFrame = 1;
            }
            else
            {
                //make gun look like it is off
                gunAnimationFrame = 0;
            }


            //ends attacking phase here so the draw method can draw the beam
            IsAttacking = false;

            //draws the weapon to the player's directions and position

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
                        new Vector2(location.X, location.Y),
                        new Rectangle(0, HeightOfSingleSpriteVertical * gunAnimationFrame, faceUp.Width, HeightOfSingleSpriteVertical),
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
                        new Vector2(location.X, location.Y),
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
                        new Vector2(location.X, location.Y),
                        new Rectangle(faceRight.Width/4 * gunAnimationFrame, 0, faceRight.Width / 4, faceRight.Height),
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
                        new Vector2(location.X, location.Y),
                        new Rectangle(0, HeightOfSingleSpriteVertical * gunAnimationFrame, faceUp.Width, HeightOfSingleSpriteVertical),
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
                new Rectangle(0, 0, faceRight.Width / 4, faceRight.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                weaponLayer);
            }
        }

    }
}