using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Enemies;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1.Weapons
{
    class FeralClawsWeapon : WeaponAbstract
    {
        //created by Will Walrond
        //implementation by Alex Sarnese

        //Attacks very fast. low range. low-medium damage. If you have killed an enemy with it in the last 2 secs, damage doubles
        //Visual effect when double damage?
        
        bool rage = false;
        int ragetime;

        private Rectangle[] weaponDamageArea;
        
        Texture2D faceLeft;
        Texture2D faceRight;
        
        private bool readyToSwing;
        private int quarterRange;

        public FeralClawsWeapon(Rectangle WeaponSpawnLocation, Texture2D faceLeft, Texture2D faceRight) : 
            base("Feral Claws", TypeOfWeapon.Melee, 5, 44, 1f, 0.2f, WeaponSpawnLocation, faceRight)
        {
            this.faceLeft = faceLeft;
            this.faceRight = faceRight;

            readyToSwing = false;
            quarterRange = Range / 4;


            //the swings will be made up of 4 rectangles making a semi-circle
            weaponDamageArea = new Rectangle[] {
                new Rectangle(0, 0, 8, Range),
                new Rectangle(0, 0, 8, Range),
                new Rectangle(0, 0, 8, Range),
                new Rectangle(0, 0, 8, Range)
            };
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
                //update location to the carrier's center top part
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) - 10;
                location.Y = player.PlayerHitBox.Y + 35;

                WeaponFacing = WeaponDirection.Up;
            }

            //make weapon face downward if player is facing down
            else if (player.PreviousPlayerState == PlayerState.FaceDown)
            {
                //update location to the carrier's center bottom
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) + 25;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height) - 25;

                WeaponFacing = WeaponDirection.Down;
            }

            //makes the weapon face right if player is facing right
            else if (player.PreviousPlayerState == PlayerState.FaceRight)
            {
                //update location to the carrier's center right side
                location.X = player.PlayerHitBox.X + player.PlayerHitBox.Width - 15;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2);

                WeaponFacing = WeaponDirection.Right;
            }

            //make weapon face left if player is facing left
            else if (player.PreviousPlayerState == PlayerState.FaceLeft)
            {
                //update location to the carrier's center left side
                location.X = player.PlayerHitBox.X + 20;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2);

                WeaponFacing = WeaponDirection.Left;
            }
        }


        protected override void Attacking(List<EnemiesAbstract> listOfEnemies, Player player)
        {
            //prep for swinging and to help with syncing up the animation
            if (!readyToSwing)
            {
                TimeThatHasPassed = 0;
                readyToSwing = true;
            }

            //if we spent longer in attack phase than its specified duration, end the attack phase
            else if (TimeThatHasPassed < AttackPhaseDurationTime)
            {
                for (int i = 0; i < weaponDamageArea.Length; i++)
                {
                    //adds the 4 damaging area depending on which direction the weapon is facing
                    if (WeaponFacing == WeaponDirection.Up)
                    {
                        weaponDamageArea[i].X = location.X - Range + (quarterRange * i);
                        weaponDamageArea[i].Y = location.Y - quarterRange * (i + 1);
                        weaponDamageArea[i].Width = (Range - quarterRange * i) * 2;
                        weaponDamageArea[i].Height = quarterRange;
                    }
                    else if (WeaponFacing == WeaponDirection.Down)
                    {
                        weaponDamageArea[i].X = location.X - Range + (quarterRange * i);
                        weaponDamageArea[i].Y = location.Y + quarterRange * (i + 1);
                        weaponDamageArea[i].Width = (Range - quarterRange * i) * 2;
                        weaponDamageArea[i].Height = quarterRange;
                    }
                    else if (WeaponFacing == WeaponDirection.Right)
                    {
                        weaponDamageArea[i].X = location.X + quarterRange * (i + 1) + 10;
                        weaponDamageArea[i].Y = location.Y - Range + (quarterRange * i);
                        weaponDamageArea[i].Width = quarterRange;
                        weaponDamageArea[i].Height = (Range - quarterRange * i) * 2;
                    }
                    else
                    {
                        weaponDamageArea[i].X = location.X - quarterRange * (i + 1) - 10;
                        weaponDamageArea[i].Y = location.Y - Range + (quarterRange * i);
                        weaponDamageArea[i].Width = quarterRange;
                        weaponDamageArea[i].Height = (Range - quarterRange * i) * 2;
                    }

                    //loops through list of enemies and see if it hit one of them
                    //if so, damages the enemy
                    for (int b = 0; b < listOfEnemies.Count; b++)
                    {
                        if (weaponDamageArea[i].Intersects(listOfEnemies[b].EnemyHitBox))
                        {
                            if (rage) listOfEnemies[b].TakeDamage(Damage * 2);
                            else listOfEnemies[b].TakeDamage(Damage);

                            if (listOfEnemies[b].Health <= 0)
                            {
                                rage = true;
                                ragetime = 200;//need to make this equal to 2 secs
                                CooldownTime = 0.2f;
                            }
                        }
                    }
                }
            }
            else
            {
                //ends attacking phase
                IsAttacking = false;
                readyToSwing = false;
                TimeThatHasPassed = 0;
            }
        }

        /// <summary>
        /// Updates the weapon's location and where it faces 
        /// Calls up the Attacking method used for dealing damage
        /// Needs to be called in update loop in Player
        /// 
        /// Will also keep track of the rage timer and determine if the rage has ended 
        /// </summary>
        /// <param name="listOfEnemies">the list of enemies to check for collusion with</param>
        /// <param name="player">the player to check for location of</param>
        /// <param name="gameTime">the timer to be used to keep track of cooldown time</param>
        public override void Update(List<EnemiesAbstract> listOfEnemies, Player player, GameTime gameTime)
        {
            base.Update(listOfEnemies, player, gameTime);

            if (rage)
            {
                ragetime -= (int)TimeThatHasPassed;
                if(ragetime <= 0)
                {
                    rage = false;
                    CooldownTime = 1f;
                }
            }
        }


        /// <summary>
        /// Draws the weapon
        /// </summary>
        /// <param name="spriteBatch">spritebatch used for drawing</param>
        /// <param name="player">player to use to know if being held</param>
        public override void Draw(SpriteBatch spriteBatch, Player player)
        {
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
                        if (!IsAttacking)
                        {
                            DrawHelperWithOffset(spriteBatch, faceRight, 0, 2.3f, rage, 62, 55);
                        }
                        else
                        {
                            if (TimeThatHasPassed < AttackPhaseDurationTime * 0.125)
                            {
                                DrawHelper(spriteBatch, faceRight, 0, 1.6f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.25)
                            {
                                DrawHelper(spriteBatch, faceRight, 0, 2.0f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.375)
                            {
                                DrawHelper(spriteBatch, faceRight, 1, 2.4f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5)
                            {
                                DrawHelper(spriteBatch, faceRight, 1, 2.8f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.625)
                            {
                                DrawHelper(spriteBatch, faceRight, 2, 3.2f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75)
                            {
                                DrawHelper(spriteBatch, faceRight, 2, 3.6f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.875)
                            {
                                DrawHelper(spriteBatch, faceRight, 3, 4.0f, rage);
                            }
                            else
                            {
                                DrawHelper(spriteBatch, faceRight, 3, 4.4f, rage);
                            }
                        }
                    }
                    else if (WeaponFacing == WeaponDirection.Left)
                    {
                        if (!IsAttacking)
                        {
                            DrawHelperWithOffset(spriteBatch, faceLeft, 0, 4.9f, rage, -40, 15);
                        }
                        else
                        {
                            if (TimeThatHasPassed < AttackPhaseDurationTime * 0.125)
                            {
                                DrawHelper(spriteBatch, faceLeft, 0, 2.5f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.25)
                            {
                                DrawHelper(spriteBatch, faceLeft, 0, 2.2f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.375)
                            {
                                DrawHelper(spriteBatch, faceLeft, 1, 1.9f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5)
                            {
                                DrawHelper(spriteBatch, faceLeft, 1, 1.6f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.625)
                            {
                                DrawHelper(spriteBatch, faceLeft, 2, 1.2f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75)
                            {
                                DrawHelper(spriteBatch, faceLeft, 2, 0.8f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.875)
                            {
                                DrawHelper(spriteBatch, faceLeft, 3, 0.5f, rage);
                            }
                            else
                            {
                                DrawHelper(spriteBatch, faceLeft, 3, 0.2f, rage);
                            }
                        }

                    }
                    else if (WeaponFacing == WeaponDirection.Right)
                    {
                        if (!IsAttacking)
                        {
                            DrawHelperWithOffset(spriteBatch, faceRight, 0, 1.4f, rage, 34, 15);
                        }
                        else
                        {
                            if (TimeThatHasPassed < AttackPhaseDurationTime * 0.125)
                            {
                                DrawHelper(spriteBatch, faceRight, 0, 3.8f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.25)
                            {
                                DrawHelper(spriteBatch, faceRight, 0, 4.1f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.375)
                            {
                                DrawHelper(spriteBatch, faceRight, 1, 4.5f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5)
                            {
                                DrawHelper(spriteBatch, faceRight, 1, 4.9f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.625)
                            {
                                DrawHelper(spriteBatch, faceRight, 2, 5.2f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75)
                            {
                                DrawHelper(spriteBatch, faceRight, 2, 5.5f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.875)
                            {
                                DrawHelper(spriteBatch, faceRight, 3, 5.8f, rage);
                            }
                            else
                            {
                                DrawHelper(spriteBatch, faceRight, 3, 6.1f, rage);
                            }
                        }
                    }
                    //If reached here, the weapon must be facing downward
                    else
                    {
                        if (!IsAttacking)
                        {
                            DrawHelperWithOffset(spriteBatch, faceLeft, 0, 4.5f, rage, -90, 8);
                        }
                        else
                        {
                            if (TimeThatHasPassed < AttackPhaseDurationTime * 0.125)
                            {
                                DrawHelper(spriteBatch, faceLeft, 0, 1.6f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.25)
                            {
                                DrawHelper(spriteBatch, faceLeft, 0, 1.2f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.375)
                            {
                                DrawHelper(spriteBatch, faceLeft, 1, 0.8f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5)
                            {
                                DrawHelper(spriteBatch, faceLeft, 1, 0.4f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.625)
                            {
                                DrawHelper(spriteBatch, faceLeft, 2, 6.0f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75)
                            {
                                DrawHelper(spriteBatch, faceLeft, 2, 5.6f, rage);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.875)
                            {
                                DrawHelper(spriteBatch, faceLeft, 3, 5.2f, rage);
                            }
                            else
                            {
                                DrawHelper(spriteBatch, faceLeft, 3, 4.8f, rage);
                            }
                        }
                    }
                }
            }

            //updates the weapons rectangle to its location and draws it to the screen when not held by player
            else
            {
                DrawHelperWithOffset(spriteBatch, faceLeft, 0, 1.57f, false, 100, 60);
            }

            /*
            //used for checking hitboxes. Pass in the redTest texture.
            for(int i = 0; i < weaponDamageArea.Length; i++)
            {
                    spriteBatch.Draw(
                    faceUp,
                    weaponDamageArea[i],
                    new Rectangle(0, 0, faceUp.Width, faceUp.Height),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0.0f);
            }*/
        }

        //does the actual drawing and draws weapon red or white depending on if it's raging
        private void DrawHelper(SpriteBatch spriteBatch, Texture2D texture, int frame, float angle, bool rage)
        {
            if (rage)
            {
                spriteBatch.Draw(
                texture,
                new Vector2(location.X, location.Y),
                new Rectangle((texture.Width / 4) * frame, 0, texture.Width / 4, texture.Height),
                Color.Red,
                angle,
                new Vector2(64, 25),
                1.0f,
                SpriteEffects.None,
                0.0f);
            }
            else
            {
                spriteBatch.Draw(
               texture,
               new Vector2(location.X, location.Y),
               new Rectangle((texture.Width / 4) * frame, 0, texture.Width / 4, texture.Height),
               Color.White,
               angle,
               new Vector2(64, 25),
               1.0f,
               SpriteEffects.None,
               0.0f);
            }
        }

        //does the actual drawing and draws weapon red or white depending on if it's raging
        private void DrawHelperWithOffset(SpriteBatch spriteBatch, Texture2D texture, int frame, float angle, bool rage, int xOffset, int yOffset)
        {
            //set the layer for weapon
            float weaponLayer = location.Y / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (weaponLayer <= 0)
            {
                weaponLayer = 0.01f;
            }

            if (rage)
            {
                spriteBatch.Draw(
                texture,
                new Vector2(location.X + xOffset, location.Y + yOffset),
                new Rectangle((texture.Width / 4) * frame, 0, texture.Width / 4, texture.Height),
                Color.Red,
                angle,
                new Vector2(64, 25),
                1.0f,
                SpriteEffects.None,
                weaponLayer);
            }
            else
            {
                spriteBatch.Draw(
                texture,
                new Vector2(location.X + xOffset, location.Y + yOffset),
                new Rectangle((texture.Width / 4) * frame, 0, texture.Width / 4, texture.Height),
                Color.White,
                angle,
                new Vector2(64, 25),
                1.0f,
                SpriteEffects.None,
                weaponLayer);
            }
        }

    }
}
