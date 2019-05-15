using Game1.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Enemies
{
    // Alex Sarnese and Cail Umbaugh
    // Raman Mandavia
    //should we rename this and WeaponDirection enum to just be Direction since the are the same and used the same way? - Alex
    enum EnemyDirection {Right, Left };

    abstract class EnemiesAbstract
    {
            //field
        private int health;
        private int damage;
        private float walkingSpeed;
        private double timeThatHasPassed;
        private double invicibilityTimer;
        protected bool exists;
        protected bool isDamaged;
        protected Vector2 enemyPosition;
        protected Rectangle enemyHitBox;
        protected Texture2D enemyTextureRight;
        protected Texture2D enemyTextureLeft;
        protected Texture2D enemyTextureDead;
        protected int enemySpriteWidth;
        protected int enemySpriteHeight;
        private EnemyDirection enemyDirection;
        protected int walkCycleFrame;
        protected bool stunned;
        protected double stunTime;
        protected bool damageCausedKnockback;
        protected TypeOfWeapon dropType;

        //add more fields like damage2 or cooldown in child enemy classes for extra attacks or different behaviors
        //(Dont forget to initialize the new fields in the child's constructor.


        //property
        public int Health { get { return health; } set { health = value; } }
        public int Damage { get { return damage; } set { damage = value; } }
        public float Speed { get { return walkingSpeed; } set { walkingSpeed = value; } }
        public double TimeThatHasPassed { get { return timeThatHasPassed; } set { timeThatHasPassed = value; } }
        public double InvicibilityTimerAfterDamaged { get { return invicibilityTimer; } set { invicibilityTimer = value; } }
        public bool IsDamaged { get { return isDamaged; } set { isDamaged = value; } }
        public bool Exists { get { return exists; } set { exists = value; } }
        public Rectangle EnemyHitBox { get { return enemyHitBox; } set { enemyHitBox = value; } }
        public Vector2 EnemyPosition { get { return enemyPosition; } set { enemyPosition = value; } }
        public EnemyDirection EnemyFacing { get { return enemyDirection; } set { enemyDirection = value; } }
        public TypeOfWeapon DropType { get { return dropType; } }


        
            //Constructor

        /// <summary>
        /// Creates the basis of the enemy
        /// </summary>
        /// <param name="health">How much health the enemy starts with</param>
        /// <param name="damage">How much damage the main attack of the enemy does</param>
        /// <param name="speed">How fast the enemy moves</param>
        /// <param name="enemyRectangle">the size and position of enemy</param>
        /// <param name="enemyTexture">The texture used for this enemy</param>
        public EnemiesAbstract(int health, int damage, float speed, Vector2 enemyPosition, Texture2D enemyTextureRight, Texture2D enemyTextureLeft, Texture2D enemyTextureDead)
        {
                //enemy stats
            this.health = health;
            this.damage = damage;
            walkingSpeed = speed;

                //used to make Update() work (attacking and behavior)
            invicibilityTimer = 0.3;
            isDamaged = false;
            timeThatHasPassed = 0;
            stunTime = 0;

            exists = true;

            //draw stuff
            this.enemyPosition = enemyPosition;
            this.enemyTextureRight = enemyTextureRight;
            this.enemyTextureLeft = enemyTextureLeft;
            this.enemyTextureDead = enemyTextureDead;
            EnemyFacing = EnemyDirection.Left;
            enemySpriteWidth = enemyTextureRight.Width / 4;
            enemySpriteHeight = enemyTextureRight.Height / 2;
            walkCycleFrame = 0;

            stunned = false;
            damageCausedKnockback = true;
        }


             //Methods


        /// <summary>
        /// Write the enemy's behavior here. 
        /// This would include updating the enemy's location and checking if dealt damage
        /// 
        /// Also do not forget to include the behavior of how enemies react to being damaged
        /// 
        /// If enemy has a distinct attacking phase, write an Attack() method seperately in the child enemy class
        /// and call that method in this update method.
        /// 
        /// see MeleeInfectedCrewmember for an example
        /// </summary>
        /// <param name="playerPosition">Location of the player</param>
        /// <param name="gameTime">the timer that can be used to time attack methods or timed behaviors</param> <param name="gameTime"></param>
        public abstract void Update(Player player, GameTime gameTime);

        /// <summary>
        /// Updates the animation frames for the respective enemy sprite
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameTime"></param>
        public abstract void UpdateMovementFrame();


        //This will need to be overridden to define when the player is hit.
        //this of this as the enemy checking if it damaging whatever is touching the player
        //and tells the player, HEY I HIT YOU!
        public abstract void CheckAndDealDamage(Player player);
        

        /// <summary>
        /// Makes enemy take damage and sets Isdamaged to true.
        /// </summary>
        /// <param name="damage">Damage that this enemy takes</param>
        public void TakeDamage(int damage)
        {
            if (!isDamaged)
            {
                Health -= damage;
                IsDamaged = true;
                damageCausedKnockback = true;
            }
        }


        /// <summary>
        /// Makes enemy take damage and sets Isdamaged to true.
        /// </summary>
        /// <param name="damage">Damage that this enemy takes</param>
        public void TakeDamageButNoKnockback(int damage)
        {
            if (!isDamaged)
            {
                Health -= damage;
                IsDamaged = true;
                damageCausedKnockback = false;
            }
        }


        /// <summary>
        /// Makes enemy take damage and sets Isdamaged to true.
        /// </summary>
        /// <param name="damage">Damage that this enemy takes</param>
        public void Stunned()
        {
            if (!stunned)
            {
                //sets stun time to 4 seconds
                stunTime = 5;
                stunned = true;
            }
        }

        /// <summary>
        /// Draws the enemy on the screen. 
        /// Needs to be called in the draw loop in Game1
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
