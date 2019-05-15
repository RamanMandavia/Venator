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
    class RailgunWeapon : WeaponAbstract
    {
        // Alex Sarnese

        //Field
        Rectangle railgunPath;

        //required textures for proper directional drawing
        private Texture2D faceDown;
        private Texture2D faceUp;
        private Texture2D faceRight;
        private Texture2D faceLeft;
        private Texture2D railgunBeamHorizontal;
        private Texture2D railgunBeamVertical;
        private int widthOfSingleSpriteHorizontal;
        private int widthOfSingleSpriteVertical;

        private bool fired;
        private int gunAnimationFrame;

        //Constructor

        /// <summary>
        /// Creates instance of a railgun weapon with given base stats
        /// </summary>
        /// <param name="WeaponSpawnLocation">Location for the gun's spawn</param>
        /// <param name="faceDown">texture of weapon facing downward</param>
        /// <param name="faceUp">texture of weapon facing upward</param>
        /// <param name="faceRight">texture of weapon facing right</param>
        /// <param name="faceLeft">texture of weapon facing left</param>
        /// <param name="railgunBeamHorizontal">texture of beam going horizontal</param>
        /// <param name="railgunBeamVertical">texture of the beam going vertical</param>
        public RailgunWeapon(Rectangle WeaponSpawnLocation, Texture2D faceDown,
                             Texture2D faceUp, Texture2D faceRight, Texture2D faceLeft,
                             Texture2D railgunBeamHorizontal, Texture2D railgunBeamVertical)
            : base("Railgun", TypeOfWeapon.Gun, 35, 1, 8, 1f, WeaponSpawnLocation, faceRight)
        {
            TimeThatHasPassed = 20;

            this.faceDown = faceDown;
            this.faceUp = faceUp;
            this.faceRight = faceRight;
            this.faceLeft = faceLeft;
            fired = false;

            widthOfSingleSpriteHorizontal = faceLeft.Width / 4;
            widthOfSingleSpriteVertical = faceUp.Width / 4;

            this.railgunBeamHorizontal = railgunBeamHorizontal;
            this.railgunBeamVertical = railgunBeamVertical;

            gunAnimationFrame = 0;
            railgunPath = new Rectangle(0, 0, 0, 0);
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
                //progress through attacking sequence
                DetermineWeaponDirection(player);
                Attacking(listOfEnemies, player);

            }
            else
            {

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
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) - 55;
                location.Y = player.PlayerHitBox.Y - 55;

                WeaponFacing = WeaponDirection.Up;
            }

            //make weapon face downward if player is facing down
            else if (player.CurrentPlayerState == PlayerState.FaceDown)
            {
                //update location to the carrier's center bottom
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) - 60;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height) - 85;

                WeaponFacing = WeaponDirection.Down;
            }

            //makes the weapon face right if player is facing right
            else if (player.CurrentPlayerState == PlayerState.FaceRight)
            {
                //update location to the carrier's center right side
                location.X = player.PlayerHitBox.X + player.PlayerHitBox.Width - 45;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) - 75;

                WeaponFacing = WeaponDirection.Right;
            }

            //make weapon face left if player is facing left
            else if (player.CurrentPlayerState == PlayerState.FaceLeft)
            {
                //update location to the carrier's center left side
                location.X = player.PlayerHitBox.X - 85;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) - 75;

                WeaponFacing = WeaponDirection.Left;
            }
        }

        //Attack() method is defined in WeaponAbstract and can be called elsewhere to make this weapon attack



        /// <summary>
        ///                 ATTACKING METHOD FOR RAILGUN WEAPONS
        /// 
        /// creates a rectangle in a straight line and checks if hit enemy.
        /// If they do, calls up the enemy's TakenDamage() method 
        /// </summary>
        /// <param name="listOfEnemies">the list of enemies to check for collusion with</param>
        /// <param name="player">the player to use for location of weapon</param>
        protected override void Attacking(List<EnemiesAbstract> listOfEnemies, Player player)
        {
            //shoots once and exits attacking method
            if (!fired)
            {
                //creates a path going in direction that the gun is facing

                //make beam in direction that is is facing
                if (WeaponFacing == WeaponDirection.Up)
                {
                    //shoots up
                    railgunPath.X = location.X + faceUp.Width / 8 - 8;
                    railgunPath.Y = 0;
                    railgunPath.Width = 16;
                    railgunPath.Height = location.Y + faceUp.Height/2 + 30;

                    player.Move(0, 30);

                    //loops through list of enemies and see if it hit one of them
                    //if so, damages the enemy
                    for (int b = 0; b < listOfEnemies.Count; b++)
                    {
                        if (railgunPath.Intersects(listOfEnemies[b].EnemyHitBox))
                        {
                            listOfEnemies[b].TakeDamage(Damage);
                        }
                    }
                }
                else if (WeaponFacing == WeaponDirection.Down)
                {
                    //shoots down
                    railgunPath.X = location.X + faceUp.Width / 8 - 8;
                    railgunPath.Y = location.Y + faceUp.Height - 60;
                    railgunPath.Width = 16;
                    railgunPath.Height = 650;

                    player.Move(0, -30);

                    //loops through list of enemies and see if it hit one of them
                    //if so, damages the enemy
                    for (int b = 0; b < listOfEnemies.Count; b++)
                    {
                        if (railgunPath.Intersects(listOfEnemies[b].EnemyHitBox))
                        {
                            listOfEnemies[b].TakeDamage(Damage);
                        }
                    }
                }
                else if (WeaponFacing == WeaponDirection.Right)
                {
                    //shoots right
                    railgunPath.X = location.X + faceUp.Width / 8 - 25;
                    railgunPath.Y = location.Y + faceUp.Height / 2 - 10;
                    railgunPath.Width = 800;
                    railgunPath.Height = 16;

                    player.Move(-30, 0);

                    //loops through list of enemies and see if it hit one of them
                    //if so, damages the enemy
                    for (int b = 0; b < listOfEnemies.Count; b++)
                    {
                        if (railgunPath.Intersects(listOfEnemies[b].EnemyHitBox))
                        {
                            listOfEnemies[b].TakeDamage(Damage);
                        }
                    }
                }
                else
                {
                    //shoots left
                    railgunPath.X = 0;
                    railgunPath.Y = location.Y + faceUp.Height / 2 - 10;
                    railgunPath.Width = location.X + faceUp.Width / 8 + 25;
                    railgunPath.Height = 16;

                    player.Move(30, 0);

                    //loops through list of enemies and see if it hit one of them
                    //if so, damages the enemy
                    for (int b = 0; b < listOfEnemies.Count; b++)
                    {
                        if (railgunPath.Intersects(listOfEnemies[b].EnemyHitBox))
                        {
                            listOfEnemies[b].TakeDamage(Damage);
                        }
                    }
                }

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
            
            if (fired)
            {
                //set the railgun's animation
                //also draws the railgun bullet path for a short time
                if (TimeThatHasPassed < AttackPhaseDurationTime * 0.143f)
                {
                    gunAnimationFrame = 3;

                    if (WeaponFacing == WeaponDirection.Up)
                    {
                        spriteBatch.Draw(
                        railgunBeamVertical,
                        new Rectangle(railgunPath.X, railgunPath.Y + (location.Y / 2), railgunPath.Width, railgunPath.Height - (location.Y / 2)),
                        new Rectangle(0, 0, railgunBeamVertical.Width, railgunBeamVertical.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0.01f);
                    }
                    else if (WeaponFacing == WeaponDirection.Down)
                    {
                        spriteBatch.Draw(
                        railgunBeamVertical,
                        new Rectangle(railgunPath.X, railgunPath.Y, railgunPath.Width, railgunPath.Height/2),
                        new Rectangle(0, 0, railgunBeamVertical.Width, railgunBeamVertical.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0.01f);
                    }
                    else if (WeaponFacing == WeaponDirection.Right)
                    {
                        spriteBatch.Draw(
                        railgunBeamVertical,
                        new Rectangle(railgunPath.X, railgunPath.Y, railgunPath.Width/2, railgunPath.Height),
                        new Rectangle(0, 0, railgunBeamVertical.Width, railgunBeamVertical.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0.01f);
                    }
                    else
                    {
                        spriteBatch.Draw(
                        railgunBeamHorizontal,
                        new Rectangle(railgunPath.X + (location.X / 2), railgunPath.Y, railgunPath.Width - (location.X / 2), railgunPath.Height),
                        new Rectangle(0, 0, railgunBeamHorizontal.Width, railgunBeamHorizontal.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0.01f);
                    }
                }
                else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.286f)
                {
                    if (WeaponFacing == WeaponDirection.Up || WeaponFacing == WeaponDirection.Down)
                    {
                        spriteBatch.Draw(
                        railgunBeamVertical,
                        railgunPath,
                        new Rectangle(0, 0, railgunBeamVertical.Width, railgunBeamVertical.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0.01f);
                    }
                    else
                    {
                        spriteBatch.Draw(
                        railgunBeamHorizontal,
                        railgunPath,
                        new Rectangle(0, 0, railgunBeamHorizontal.Width, railgunBeamHorizontal.Height),
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0.01f);
                    }
                    gunAnimationFrame = 3;
                }
                else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.429f)
                {
                    gunAnimationFrame = 2;
                }
                else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.572f)
                {
                    gunAnimationFrame = 1;
                }
                else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.715f)
                {
                    gunAnimationFrame = 2;
                }
                else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.858f)
                {
                    gunAnimationFrame = 1;
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
                        new Vector2(location.X, location.Y),
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
                        new Vector2(location.X, location.Y),
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
                        new Vector2(location.X, location.Y),
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
                        new Vector2(location.X, location.Y),
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
}