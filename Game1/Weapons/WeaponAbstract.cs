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
    // Alex Sarnese
    // Raman Mandavia (adjustments and addition for more functionality)
    enum WeaponDirection { Up, Right, Down, Left };
    enum TypeOfWeapon { Melee, Gun, Bomb, Secret,Jug };

    abstract class WeaponAbstract
    {
        //Field
        private String name;
        private int damage;
        private int range;
        private int dropTimer;
        private float cooldownTime;
        private float attackPhaseDurationTime;
        private double timeThatHasPassed;
        private bool isAttacking;
        protected Point location;
        private Texture2D uITexture;
        //might need another texture variable for when weapon is attacking
        private WeaponDirection weaponFacing;
        private TypeOfWeapon weaponType;



        //Property

        //used for getting and modifying weapon stats
        public string Name { get { return name; } }
        public int Damage { get { return damage; } set { damage = value; } }
        public int Range { get { return range; } }
        public float CooldownTime { get { return cooldownTime; } set { cooldownTime = value; } }
        public float AttackPhaseDurationTime { get { return attackPhaseDurationTime; } }
        public int DropTimer { get { return dropTimer; } set { dropTimer = value; } }

        //used for making Update, Attacking, and Draw methods work
        public double TimeThatHasPassed { get { return timeThatHasPassed; } set { timeThatHasPassed = value; } }
        public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
        public Point Location { get { return location; } set { location = value; } }
        public WeaponDirection WeaponFacing { get { return weaponFacing; } set { weaponFacing = value; } }

        //enemies will need to check the weapon's type to know if it is a weapon where it has to check being in contact with.
        //Bombs and Melee weapons have a weaponDamageArea that enemies need to check.
        //Gun weapons will have a rectangle array to check for collusions with bullets.
        public TypeOfWeapon WeaponType { get { return weaponType; } set { weaponType = value; } }

        public Texture2D UITexture { get { return uITexture; } }
        

        //Constructor

        /// <summary>
        /// Creates instance of weapon with given stats
        /// These are the base stat that all weapons will have
        /// </summary>
        /// <param name="name">Name of weapon</param>
        /// <param name="weaponType">an enum saying if the weapon is a melee, gun, or bomb</param>
        /// <param name="damage">Amount of damage the weapon deals in one hit</param>
        /// <param name="range">How far the weapon can damage in pixels</param>
        /// <param name="cooldownTime">How much time in seconds that the weapon must rest between attacks</param>
        /// <param name="attackPhaseDurationTime">How long the attack phase takes in seconds</param>
        /// <param name="WeaponSpawnLocation">Where the weapon should spawn at first creation</param>
        public WeaponAbstract(string name, TypeOfWeapon weaponType, int damage, int range, float cooldownTime, float attackPhaseDurationTime, Rectangle WeaponSpawnLocation, Texture2D uITexture)
        {
            //weapon stats
            this.name = name;
            this.weaponType = weaponType;
            this.damage = damage;
            this.range = range;
            this.cooldownTime = cooldownTime;
            this.attackPhaseDurationTime = attackPhaseDurationTime;

            //needed to make Update and Attacking, and Draw methods work
            TimeThatHasPassed = 0;
            IsAttacking = false;

            location = new Point(WeaponSpawnLocation.X, WeaponSpawnLocation.Y);

            weaponFacing = WeaponDirection.Up;

            this.uITexture = uITexture;
        }




        //Methods

        /// <summary>
        /// Override this in child weapon classes to specify different behaviors in updating
        /// 
        /// Updates the weapon's location and where it faces 
        /// Calls up the Attacking method used for dealing damage
        /// Needs to be called in update loop in Player
        /// </summary>
        /// <param name="listOfEnemies">the list of enemies to check for collusion with</param>
        /// <param name="player">the player to check for location of</param>
        /// <param name="gameTime">the timer to be used to keep track of cooldown time</param>
        public virtual void Update(List<EnemiesAbstract> listOfEnemies, Player player, GameTime gameTime)
        {
            //stores how much time has passed since last update and stores it in a variable
            TimeThatHasPassed += gameTime.ElapsedGameTime.TotalSeconds;


            //checks to see if the weapon should go into attacking phase
            if (IsAttacking)
            {
                //change weapon's direction to face the mouse and 
                //progress through attacking sequence
                DetermineWeaponDirection(player);
                Attacking(listOfEnemies, player);
            }
            else
            {
                //If player is null, then this weapon is dropped and is not being held by the player. 
                //Thus does nothing to weapon's position
                if (player != null && player.CurrentWeapon == this)
                {
                    DetermineWeaponDirection(player);
                }
            }
        }


        /// <summary>
        /// Signals the weapon to attempt to attack.
        /// Call this once wherever you want to trigger this weapon to attack 
        /// (i.e. when player click a button to fire gun)
        /// </summary>
        public void Attack()
        {

            //checks to see if the weapon is not still in cooldown. 
            //If not, starts the attacking phase of the weapon
            if (TimeThatHasPassed > CooldownTime)
            {
                IsAttacking = true;
            }
        }


        /// <summary>
        /// makes weapon face the same direction as player
        /// </summary>
        /// <param name="player">the direction player is facing</param>
        public abstract void DetermineWeaponDirection(Player player);


        /// <summary>
        /// 
        /// 
        /// OVERRIDE THIS TO DEFINE THE WEAPON'S SPECIFIC ATTACKS. 
        /// SEE THE BASIC WEAPONS FOR HOW IT IS SET UP
        /// THEN MAKE A NEW WEAPON CLASS THAT INHERITS FROM HERE
        /// IN THAT CLASS OVERRIDE THIS METHOD WITH BRAND NEW AND UNIQUE WEAPON DAMAGE PATTERN
        /// 
        /// 
        /// </summary>
        /// <param name="listOfEnemies">the list of enemies to check for collusion with</param>
        /// <param name="player">the player to use for both location of weapon and if player damaged himself</param>
        protected abstract void Attacking(List<EnemiesAbstract> listOfEnemies, Player player);


        /// <summary>
        /// Draws the weapon on the screen. 
        /// Needs to be called in the draw loop in Player class
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used for drawing to the screen</param>
        public abstract void Draw(SpriteBatch spriteBatch, Player player);
        /*
         * override this method in weapon classes that inherit from this abstract class. 
         * 
         * call the WeaponFacing property to know which direction the weapon is facing
         * 
         * Also, do know that if you need the weapon's rectangle to have a specific height and width, 
         * change the rectangle size in the constructor in the WeaponAbstract class
         * 
         * If needed, you can add a new boolean to keep track of which weapon is equipped so
         * you draw the weapon that the player is holding and not the stored weapon. 
         * But this could be done without a new variable if the player class is holding the
         * weapon object and is calling the weapon's draw method inside the player's draw method.
         * 
         * Take any appoach you feel is needed! Good luck!
         * */

    }
}
