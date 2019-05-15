using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Weapons
{
    class StunWandWeapon : WeaponAbstract
    {
        //Will Walrond
        //This weapon wont do much damage, but will stun the enemy in place for a short time
        //implementation by Alex Sarnese

        private Rectangle[] weaponDamageArea;
        private Texture2D rightAndDownSwing;
        private Texture2D leftAndUpSwing;
        private int widthOfSwordSprite;
        private int heightOfSwordSprite;
        private bool readyToSwing;
        private int quarterRange;
        private int oneFifthRange;
        private bool stunnedAnEnemy;

        private Vector2 position;
        

        public StunWandWeapon(Rectangle WeaponSpawnLocation, Texture2D faceRight, Texture2D faceLeft)
            : base("Stun Wand", TypeOfWeapon.Melee, 5, 45, 6, 0.3f, WeaponSpawnLocation, faceRight)
        {
            leftAndUpSwing = faceLeft;
            rightAndDownSwing = faceRight;

            //the swings will be made up of 4 rectangles making a semi-circle
            weaponDamageArea = new Rectangle[] {
                new Rectangle(0, 0, 8, Range),
                new Rectangle(0, 0, 8, Range),
                new Rectangle(0, 0, 8, Range),
                new Rectangle(0, 0, 8, Range)
            };

            stunnedAnEnemy = false;
            IsAttacking = false;
            readyToSwing = false;
            TimeThatHasPassed = 20;

            widthOfSwordSprite = faceLeft.Width / 4;
            heightOfSwordSprite = faceLeft.Height;
            quarterRange = Range / 4;
            oneFifthRange = Range / 5;
            position = new Vector2(location.X, location.Y);
        }


        //Methods


        //Update() method is defined in WeaponAbstract and needs to be called in Game1's Update method 
        //or called up in another class's Update method that is called up in Game1's Update method 


        /// <summary>
        ///                 ATTACKING METHOD FOR Stun Wand WEAPONS
        /// 
        /// creates collusion circle in front of character that check to see if they deal damage to enemies.
        /// If they do, they need to get amount of damage from this weapon's Damage property 
        /// </summary>
        /// <param name="listOfEnemies">the list of enemies to check for collusion with</param>
        /// <param name="player">the player to use for location of weapon</param>
        protected override void Attacking(List<EnemiesAbstract> listOfEnemies, Player player)
        {
            //prep for swinging and to help with syncing up the animation
            if (!readyToSwing)
            {
                TimeThatHasPassed = 0;
                readyToSwing = true;
                stunnedAnEnemy = false;
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
                        weaponDamageArea[i].Y = location.Y - oneFifthRange * (i + 1) - 15;
                        weaponDamageArea[i].Width = (Range - quarterRange * i) * 2;
                        weaponDamageArea[i].Height = oneFifthRange;
                    }
                    else if (WeaponFacing == WeaponDirection.Down)
                    {
                        weaponDamageArea[i].X = location.X - Range + (quarterRange * i);
                        weaponDamageArea[i].Y = location.Y + oneFifthRange * (i + 1);
                        weaponDamageArea[i].Width = (Range - quarterRange * i) * 2;
                        weaponDamageArea[i].Height = oneFifthRange;
                    }
                    else if (WeaponFacing == WeaponDirection.Right)
                    {
                        weaponDamageArea[i].X = location.X + oneFifthRange * (i + 1) + 5;
                        weaponDamageArea[i].Y = location.Y - Range + (quarterRange * i);
                        weaponDamageArea[i].Width = oneFifthRange + 20;
                        weaponDamageArea[i].Height = (Range - quarterRange * i) + 5;
                    }
                    else
                    {
                        weaponDamageArea[i].X = location.X - oneFifthRange * (i + 1) - 40;
                        weaponDamageArea[i].Y = location.Y - Range + (quarterRange * i);
                        weaponDamageArea[i].Width = oneFifthRange + 20;
                        weaponDamageArea[i].Height = (Range - quarterRange * i) + 5;
                    }

                    //loops through list of enemies and see if it hit one of them
                    //if so, damages the enemy
                    for (int b = 0; b < listOfEnemies.Count; b++)
                    {
                        if (weaponDamageArea[i].Intersects(listOfEnemies[b].EnemyHitBox))
                        {
                            listOfEnemies[b].TakeDamage(Damage);
                            listOfEnemies[b].Stunned();
                            stunnedAnEnemy = true;
                        }
                    }
                }

            }
            else
            {
                //If we hit an enemy, engaged cooldown. Otherwise, have no cooldown
                if (stunnedAnEnemy)
                {
                    TimeThatHasPassed = 0;
                }
                else
                {
                    TimeThatHasPassed = 20;
                }
                
                //ends attacking phase
                IsAttacking = false;
                readyToSwing = false;
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
                //update location to the carrier's center top part
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) + 17;
                location.Y = player.PlayerHitBox.Y + 44;

                WeaponFacing = WeaponDirection.Up;
            }

            //make weapon face downward if player is facing down
            else if (player.PreviousPlayerState == PlayerState.FaceDown)
            {
                //update location to the carrier's center bottom
                location.X = player.PlayerHitBox.X + (player.PlayerHitBox.Width / 2) - 22;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height) - 30;

                WeaponFacing = WeaponDirection.Down;
            }

            //makes the weapon face right if player is facing right
            else if (player.PreviousPlayerState == PlayerState.FaceRight)
            {
                //update location to the carrier's center right side
                location.X = player.PlayerHitBox.X + player.PlayerHitBox.Width - 22;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) + 15;

                WeaponFacing = WeaponDirection.Right;
            }

            //make weapon face left if player is facing left
            else if (player.PreviousPlayerState == PlayerState.FaceLeft)
            {
                //update location to the carrier's center left side
                location.X = player.PlayerHitBox.X + 23;
                location.Y = player.PlayerHitBox.Y + (player.PlayerHitBox.Height / 2) + 15;

                WeaponFacing = WeaponDirection.Left;
            }
        }

        /// <summary>
        /// Draws the weapon on the screen. 
        /// Needs to be called in the draw loop in Game1
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
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
                            position = new Vector2(location.X, location.Y);
                            DrawHelper(spriteBatch, leftAndUpSwing, 0, 3.3f, position);
                        }
                        else
                        {
                            if (TimeThatHasPassed < AttackPhaseDurationTime * 0.125)
                            {
                                DrawHelper(spriteBatch, leftAndUpSwing, 0, 1.6f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.25)
                            {
                                DrawHelper(spriteBatch, leftAndUpSwing, 0, 2.0f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.375)
                            {
                                DrawHelper(spriteBatch, leftAndUpSwing, 1, 2.4f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5)
                            {
                                DrawHelper(spriteBatch, leftAndUpSwing, 1, 2.8f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.625)
                            {
                                DrawHelper(spriteBatch, leftAndUpSwing, 2, 3.2f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75)
                            {
                                DrawHelper(spriteBatch, leftAndUpSwing, 2, 3.6f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.875)
                            {
                                DrawHelper(spriteBatch, leftAndUpSwing, 3, 4.0f, position);
                            }
                            else
                            {
                                DrawHelper(spriteBatch, leftAndUpSwing, 3, 4.4f, position);
                            }
                        }
                    }
                    else if (WeaponFacing == WeaponDirection.Left)
                    {
                        if (!IsAttacking)
                        {
                            position = new Vector2(location.X, location.Y);
                            DrawHelper(spriteBatch, rightAndDownSwing, 0, 2.8f, position);
                        }
                        else
                        {
                            if (TimeThatHasPassed < AttackPhaseDurationTime * 0.125)
                            {
                                position = new Vector2(location.X - 10, location.Y - 30);
                                DrawHelper(spriteBatch, rightAndDownSwing, 0, 2.5f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.25)
                            {
                                position = new Vector2(location.X - 12, location.Y - 27);
                                DrawHelper(spriteBatch, rightAndDownSwing, 0, 2.3f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.375)
                            {
                                position = new Vector2(location.X - 15, location.Y - 24);
                                DrawHelper(spriteBatch, rightAndDownSwing, 1, 2.1f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5)
                            {
                                position = new Vector2(location.X - 18, location.Y - 22);
                                DrawHelper(spriteBatch, rightAndDownSwing, 1, 1.9f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.625)
                            {
                                position = new Vector2(location.X - 21, location.Y - 19);
                                DrawHelper(spriteBatch, rightAndDownSwing, 2, 1.7f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75)
                            {
                                position = new Vector2(location.X - 24, location.Y - 16);
                                DrawHelper(spriteBatch, rightAndDownSwing, 2, 1.5f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.875)
                            {
                                position = new Vector2(location.X - 21, location.Y - 13);
                                DrawHelper(spriteBatch, rightAndDownSwing, 3, 1.3f, position);
                            }
                            else
                            {
                                position = new Vector2(location.X - 18, location.Y - 10);
                                DrawHelper(spriteBatch, rightAndDownSwing, 3, 1.1f, position);
                            }
                        }

                    }
                    else if (WeaponFacing == WeaponDirection.Right)
                    {
                        if (!IsAttacking)
                        {
                            position = new Vector2(location.X, location.Y);
                            DrawHelper(spriteBatch, leftAndUpSwing, 0, 3.5f, position);
                        }
                        else
                        {
                            if (TimeThatHasPassed < AttackPhaseDurationTime * 0.125)
                            {
                                position = new Vector2(location.X + 10, location.Y - 30);
                                DrawHelper(spriteBatch, leftAndUpSwing, 0, 3.8f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.25)
                            {
                                position = new Vector2(location.X + 12, location.Y - 27);
                                DrawHelper(spriteBatch, leftAndUpSwing, 0, 4.0f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.375)
                            {
                                position = new Vector2(location.X + 15, location.Y - 24);
                                DrawHelper(spriteBatch, leftAndUpSwing, 1, 4.2f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5)
                            {
                                position = new Vector2(location.X + 18, location.Y - 22);
                                DrawHelper(spriteBatch, leftAndUpSwing, 1, 4.4f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.625)
                            {
                                position = new Vector2(location.X + 21, location.Y - 19);
                                DrawHelper(spriteBatch, leftAndUpSwing, 2, 4.6f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75)
                            {
                                position = new Vector2(location.X + 24, location.Y - 16);
                                DrawHelper(spriteBatch, leftAndUpSwing, 2, 4.8f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.875)
                            {
                                position = new Vector2(location.X + 21, location.Y - 13);
                                DrawHelper(spriteBatch, leftAndUpSwing, 3, 5.0f, position);
                            }
                            else
                            {
                                position = new Vector2(location.X + 18, location.Y - 10);
                                DrawHelper(spriteBatch, leftAndUpSwing, 3, 5.2f, position);
                            }
                        }
                    }
                    //If reached here, the weapon must be facing downward
                    else
                    {
                        if (!IsAttacking)
                        {
                            position = new Vector2(location.X, location.Y);
                            DrawHelper(spriteBatch, rightAndDownSwing, 0, 3.0f, position);
                        }
                        else
                        {
                            if (TimeThatHasPassed < AttackPhaseDurationTime * 0.125)
                            {
                                position = new Vector2(location.X + 22, location.Y);
                                DrawHelper(spriteBatch, leftAndUpSwing, 0, 5.0f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.25)
                            {
                                position = new Vector2(location.X + 19, location.Y);
                                DrawHelper(spriteBatch, leftAndUpSwing, 0, 5.4f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.375)
                            {
                                position = new Vector2(location.X + 16, location.Y);
                                DrawHelper(spriteBatch, leftAndUpSwing, 1, 5.8f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.5)
                            {
                                position = new Vector2(location.X + 13, location.Y);
                                DrawHelper(spriteBatch, leftAndUpSwing, 1, 0.2f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.625)
                            {
                                position = new Vector2(location.X + 10, location.Y);
                                DrawHelper(spriteBatch, leftAndUpSwing, 2, 0.6f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.75)
                            {
                                position = new Vector2(location.X + 7, location.Y);
                                DrawHelper(spriteBatch, leftAndUpSwing, 2, 1.0f, position);
                            }
                            else if (TimeThatHasPassed < AttackPhaseDurationTime * 0.875)
                            {
                                position = new Vector2(location.X + 4, location.Y);
                                DrawHelper(spriteBatch, leftAndUpSwing, 3, 1.4f, position);
                            }
                            else
                            {
                                position = new Vector2(location.X + 1, location.Y);
                                DrawHelper(spriteBatch, rightAndDownSwing, 3, 1.8f, position);
                            }
                        }
                    }
                }
            }


            //updates the weapons rectangle to its location and draws it to the screen when not held by player
            else
            {
                DrawHelper(spriteBatch, leftAndUpSwing, 0, 1.57f, new Vector2(position.X + 100, position.Y + 60));
            }

            /*
            //used for checking hitboxes. Pass in the redTest texture.
            for (int i = 0; i < weaponDamageArea.Length; i++)
            {
                spriteBatch.Draw(
                redTest,
                weaponDamageArea[i],
                new Rectangle(0, 0, widthOfSwordSprite, leftAndDownwardSwingDirection.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }*/

        }


        //does the actual drawing
        private void DrawHelper(SpriteBatch spriteBatch, Texture2D texture, int frame, float angle, Vector2 position)
        {
            //set the layer for weapon
            float weaponLayer = location.Y / (float)650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (weaponLayer <= 0)
            {
                weaponLayer = 0.01f;
            }

            spriteBatch.Draw(
            texture,
            position,
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
