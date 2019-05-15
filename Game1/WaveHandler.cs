using Game1.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class WaveHandler
    {
        // Alex Sarnese
        // Cail Umbaugh
        // Raman Mandavia

        //enum
        private enum EnemyTypes { MeleeCrewmember, RangedEnemy, Juggernaut, MechBoss }

        //Field
        private List<EnemiesAbstract> enemyList;
        private Random rng;
        private int waveNumber;
        private int levelNumber;
        private int secretCount;
        private int dropNumber;

        //window size. I need this for the boss enemy
        private int windowx = 800;
        private int windowy = 650;


        // hitBox test
        Texture2D rectangle;

        // Enemy Textures
        private Texture2D rangedEnemyRight;
        private Texture2D rangedEnemyLeft;
        private Texture2D rangedEnemyUp;
        private Texture2D rangedEnemyDown;
        private Texture2D rangedEnemySpawn;
        private Texture2D rangedEnemyDeath;

        private Texture2D meleeInfectedCrewMemberMoveRight;
        private Texture2D meleeInfectedCrewMemberMoveLeft;
        private Texture2D meleeInfectedCrewMemberDead;

        private Texture2D juggernautMoveRight;
        private Texture2D juggernautMoveLeft;
        private Texture2D juggernautAttackRight;
        private Texture2D juggernautAttackLeft;
        private Texture2D juggernautDead;

        private Texture2D bossMechMoveLeft;
        private Texture2D bossMechMoveRight;
        private Texture2D bossMechChargeRight;
        private Texture2D bossMechChargeLeft;
        private Texture2D bossMechDeath;


        // Weapon Textures
        private Texture2D basicPistolRight;
        private Texture2D basicPistolLeft;
        private Texture2D basicPistolUp;
        private Texture2D basicPistolDown;
        private Texture2D basicProjectile;

        private Texture2D basicRifleRight;
        private Texture2D basicRifleLeft;
        private Texture2D basicRifleUp;
        private Texture2D basicRifleDown;

        private Texture2D basicShotgunRight;
        private Texture2D basicShotgunLeft;
        private Texture2D basicShotgunUp;
        private Texture2D basicShotgunDown;

        private Texture2D rocketLauncherRight;
        private Texture2D rocketLauncherLeft;
        private Texture2D rocketLauncherUp;
        private Texture2D rocketLauncherDown;
        private Texture2D rocketProjectile;

        private Texture2D railgunRight;
        private Texture2D railgunLeft;
        private Texture2D railgunUp;
        private Texture2D railgunDown;
        private Texture2D railHorizontalProjectile;
        private Texture2D railVerticalProjectile;

        private Texture2D bomb;
        private Texture2D clusterBomb;
        private Texture2D singularityBomb;
        private Texture2D basicExplosion;
        private Texture2D singularityExplosion;

        private Texture2D secret;

        private Texture2D energySwordLeft;
        private Texture2D energySwordRight;

        private Texture2D feralClawsLeft;
        private Texture2D feralClawsRight;

        private Texture2D stunWandLeft;
        private Texture2D stunWandRight;

        private Texture2D DisintegratorRayRight;
        private Texture2D DisintegratorRayLeft;
        private Texture2D DisintegratorRayUp;
        private Texture2D DisintegratorRayDown;
        private Texture2D DisintegratorRayBeamVertical;
        private Texture2D DisintegratorRayBeamHorizontal;

        private Texture2D enemyProjectileRight;
        private Texture2D enemyProjectileLeft;
        private Texture2D enemyProjectileUp;
        private Texture2D enemyProjectileDown;

        private Texture2D bossProjectile;

        public int Level { get { return levelNumber; } }
        public int Wave { get { return waveNumber; } }

        //Constructor


        /// <summary>
        /// intializes all variables
        /// </summary>
        public WaveHandler(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            rectangle = new Texture2D(graphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });
            
            enemyList = new List<EnemiesAbstract>();
            //secrets = new List<Weapons.Secret>();
            waveNumber = 0;
            levelNumber = 1;

            meleeInfectedCrewMemberMoveRight = Content.Load<Texture2D>("Enemy Sprites/Infected Crew Member Walk Right");
            meleeInfectedCrewMemberMoveLeft = Content.Load<Texture2D>("Enemy Sprites/Infected Crew Member Walk Left");
            meleeInfectedCrewMemberDead = Content.Load<Texture2D>("Enemy Sprites/Infected Crew Member Dead");

            rangedEnemyRight = Content.Load<Texture2D>("Enemy Sprites/Ranged Enemy Right");
            rangedEnemyLeft = Content.Load<Texture2D>("Enemy Sprites/Ranged Enemy Left");
            rangedEnemyUp = Content.Load<Texture2D>("Enemy Sprites/Ranged Enemy Up");
            rangedEnemyDown = Content.Load<Texture2D>("Enemy Sprites/Ranged Enemy Down");
            rangedEnemySpawn = Content.Load<Texture2D>("Enemy Sprites/Ranged Enemy Spawn");
            rangedEnemyDeath = Content.Load<Texture2D>("Enemy Sprites/Ranged Enemy Death");

            juggernautMoveRight = Content.Load<Texture2D>("Enemy Sprites/Juggernaut Enemy Walk Right");
            juggernautMoveLeft = Content.Load<Texture2D>("Enemy Sprites/Juggernaut Enemy Walk Left");
            juggernautAttackRight = Content.Load<Texture2D>("Enemy Sprites/Juggernaut Enemy Attack Right");
            juggernautAttackLeft = Content.Load<Texture2D>("Enemy Sprites/Juggernaut Enemy Attack Left");
            juggernautDead = Content.Load<Texture2D>("Enemy Sprites/Juggernaut Enemy Dead");

            bossMechMoveRight = Content.Load<Texture2D>("Enemy Sprites/Boss Mech Walk Right");
            bossMechMoveLeft = Content.Load<Texture2D>("Enemy Sprites/Boss Mech Walk Left");
            bossMechChargeRight = Content.Load<Texture2D>("Enemy Sprites/Boss Mech Charge Right");
            bossMechChargeLeft = Content.Load<Texture2D>("Enemy Sprites/Boss Mech Charge Left");
            bossMechDeath = Content.Load<Texture2D>("Enemy Sprites/Boss Mech Death");

            basicPistolRight = Content.Load<Texture2D>("Weapons and VFX/Basic Pistol Right");
            basicPistolLeft = Content.Load<Texture2D>("Weapons and VFX/Basic Pistol Left");
            basicPistolUp = Content.Load<Texture2D>("Weapons and VFX/Basic Pistol Up");
            basicPistolDown = Content.Load<Texture2D>("Weapons and VFX/Basic Pistol Down");
            basicProjectile = Content.Load<Texture2D>("Weapons and VFX/Basic ProjectileBullet");

            basicRifleRight = Content.Load<Texture2D>("Weapons and VFX/Basic Rifle Right");
            basicRifleLeft = Content.Load<Texture2D>("Weapons and VFX/Basic Rifle Left");
            basicRifleUp = Content.Load<Texture2D>("Weapons and VFX/Basic Rifle Up");
            basicRifleDown = Content.Load<Texture2D>("Weapons and VFX/Basic Rifle Down");

            basicShotgunRight = Content.Load<Texture2D>("Weapons and VFX/Basic Shotgun Right");
            basicShotgunLeft = Content.Load<Texture2D>("Weapons and VFX/Basic Shotgun Left");
            basicShotgunUp = Content.Load<Texture2D>("Weapons and VFX/Basic Shotgun Up");
            basicShotgunDown = Content.Load<Texture2D>("Weapons and VFX/Basic Shotgun Down");

            rocketLauncherRight = Content.Load<Texture2D>("Weapons and VFX/Rocket Launcher Right");
            rocketLauncherLeft = Content.Load<Texture2D>("Weapons and VFX/Rocket Launcher Left");
            rocketLauncherUp = Content.Load<Texture2D>("Weapons and VFX/Rocket Launcher Up");
            rocketLauncherDown = Content.Load<Texture2D>("Weapons and VFX/Rocket Launcher Down");
            rocketProjectile = Content.Load<Texture2D>("Weapons and VFX/Rocket Projectile");

            railgunRight = Content.Load<Texture2D>("Weapons and VFX/Railgun Right");
            railgunLeft = Content.Load<Texture2D>("Weapons and VFX/Railgun Left");
            railgunUp = Content.Load<Texture2D>("Weapons and VFX/Railgun Up");
            railgunDown = Content.Load<Texture2D>("Weapons and VFX/Railgun Down");
            railHorizontalProjectile = Content.Load<Texture2D>("Weapons and VFX/railgunBeamHorizontal");
            railVerticalProjectile = Content.Load<Texture2D>("Weapons and VFX/railgunBeamVertical");

            bomb = Content.Load<Texture2D>("Weapons and VFX/Bomb");
            clusterBomb = Content.Load<Texture2D>("Weapons and VFX/Cluster Bomb");
            singularityBomb = Content.Load<Texture2D>("Weapons and VFX/Singularity Bomb");
            basicExplosion = Content.Load<Texture2D>("Weapons and VFX/Basic Explosion");
            singularityExplosion = Content.Load<Texture2D>("Weapons and VFX/Singularity Explosion");

            energySwordRight = Content.Load<Texture2D>("Weapons and VFX/Energy Sword Right");
            energySwordLeft = Content.Load<Texture2D>("Weapons and VFX/Energy Sword Left");

            feralClawsRight = Content.Load<Texture2D>("Weapons and VFX/Feral Claws Right");
            feralClawsLeft = Content.Load<Texture2D>("Weapons and VFX/Feral Claws Left");

            stunWandRight = Content.Load<Texture2D>("Weapons and VFX/Stun Wand Right");
            stunWandLeft = Content.Load<Texture2D>("Weapons and VFX/Stun Wand Left");

            DisintegratorRayUp = Content.Load<Texture2D>("Weapons and VFX/DisintegratorRayUp");
            DisintegratorRayDown = Content.Load<Texture2D>("Weapons and VFX/DisintegratorRayDown");
            DisintegratorRayLeft = Content.Load<Texture2D>("Weapons and VFX/DisintegratorRayLeft");
            DisintegratorRayRight = Content.Load<Texture2D>("Weapons and VFX/DisintegratorRayRight");
            DisintegratorRayBeamVertical = Content.Load<Texture2D>("Weapons and VFX/DisintegratorBeamVertical");
            DisintegratorRayBeamHorizontal = Content.Load<Texture2D>("Weapons and VFX/DisintegratorBeamHorizontal");
            secret = Content.Load<Texture2D>("Weapons and VFX/secret");

            enemyProjectileRight = Content.Load<Texture2D>("Weapons and VFX/Enemy Projectile Right");
            enemyProjectileLeft = Content.Load<Texture2D>("Weapons and VFX/Enemy Projectile Left");
            enemyProjectileUp = Content.Load<Texture2D>("Weapons and VFX/Enemy Projectile Up");
            enemyProjectileDown = Content.Load<Texture2D>("Weapons and VFX/Enemy Projectile Down");

            bossProjectile = Content.Load<Texture2D>("Weapons and VFX/Boss ProjectileBullet");

            rng = new Random();
        }



        /// <summary>
        /// Add enemy here to spawn an enemy right away in the game.
        /// I also show the two ways you can spawn the enemy in the first wave.
        /// </summary>
        public void Level1wave1()
        {
            //Two ways to spawn enemies:

            //this is a hardcoded location enemy. will always spawn at center top of room
            enemyList.Add(new MeleeInfectedCrewmember(400, 0, meleeInfectedCrewMemberMoveRight, meleeInfectedCrewMemberMoveLeft, meleeInfectedCrewMemberDead, rng));
            
            //this enemy spawn in a random location at edge of room
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));

            //enemyList.Add(new BossMech(400, 0, bossMechMoveRight, bossMechMoveLeft, bossMechDeath, bossMechChargeRight, bossMechChargeLeft, bossProjectile, windowx, windowy));
        }



        //methods


        /// <summary>
        /// resets what wave and level the player was on
        /// </summary>
        public void Reset()
        {
            enemyList.Clear();
            waveNumber = 0;
            levelNumber = 1;
            secretCount = 0;
        }


        /// <summary>
        /// Helper method for wave methods
        /// Creates an instance of enemy wanted at random spot along edge of screen
        /// </summary>
        /// <param name="enemyType">type of enemy to generate</param>
        /// <returns>instance of enemy at edge of screen</returns>
        private EnemiesAbstract SetRandomLocationOnEdge(EnemyTypes enemyType)
        {
            //used to store location for enemy
            int x;
            int y;

            //randomly picks one in four numbers to determine which side of the room enemy will spawn on
            int chance = rng.Next(4);

            //creates the enemy specified and give it the generated random location
            if (enemyType == EnemyTypes.MeleeCrewmember)
            {
                //top of room
                if (chance == 0)
                {
                    x = rng.Next(801);
                    y = 0;
                }
                //left side of room
                else if (chance == 1)
                {
                    x = 0;
                    y = rng.Next(651);
                }
                //right side of room
                else if (chance == 2)
                {
                    x = 800;
                    y = rng.Next(651);
                }
                //bottom of room
                else
                {
                    x = rng.Next(801);
                    y = 650;
                }

                return new MeleeInfectedCrewmember(x, y, meleeInfectedCrewMemberMoveRight, meleeInfectedCrewMemberMoveLeft, meleeInfectedCrewMemberDead, rng);
            }
            else if (enemyType == EnemyTypes.RangedEnemy)
            {
                RangedEnemyDirection direction;

                //top of room
                if (chance == 0)
                {
                    x = rng.Next(701);
                    y = 100;
                    direction = RangedEnemyDirection.Down;
                }
                //left side of room
                else if (chance == 1)
                {
                    x = 100;
                    y = rng.Next(551);
                    direction = RangedEnemyDirection.Right;
                }
                //right side of room
                else if (chance == 2)
                {
                    x = 700;
                    y = rng.Next(551);
                    direction = RangedEnemyDirection.Left;
                }
                //bottom of room
                else
                {
                    x = rng.Next(701);
                    y = 550;
                    direction = RangedEnemyDirection.Up;
                }

                return new RangedMonster(x, y, rangedEnemyUp, rangedEnemyDown, rangedEnemyRight, rangedEnemyLeft, rangedEnemySpawn, rangedEnemyDeath, direction, enemyProjectileRight, enemyProjectileLeft, enemyProjectileUp, enemyProjectileDown);
            }
            else if (enemyType == EnemyTypes.Juggernaut)
            {
                //top of room
                if (chance == 0)
                {
                    x = rng.Next(801);
                    y = 0;
                }
                //left side of room
                else if (chance == 1)
                {
                    x = 0;
                    y = rng.Next(651);
                }
                //right side of room
                else if (chance == 2)
                {
                    x = 800;
                    y = rng.Next(651);
                }
                //bottom of room
                else
                {
                    x = rng.Next(801);
                    y = 650;
                }

                return new Juggernaut(x, y, juggernautMoveRight, juggernautMoveLeft, juggernautDead, juggernautAttackRight, juggernautAttackLeft);
            }
            else if (enemyType == EnemyTypes.MechBoss)
            {
                //top of room
                if (chance == 0)
                {
                    x = rng.Next(801);
                    y = 0;
                }
                //left side of room
                else if (chance == 1)
                {
                    x = 0;
                    y = rng.Next(651);
                }
                //right side of room
                else if (chance == 2)
                {
                    x = 800;
                    y = rng.Next(651);
                }
                //bottom of room
                else
                {
                    x = rng.Next(801);
                    y = 650;
                }

                return new BossMech(x, y, bossMechMoveRight, bossMechMoveLeft, bossMechDeath, bossMechChargeRight,bossMechChargeLeft,bossProjectile, 800, 650);
            }


            return null;
        }


        /// <summary>
        /// to be called in Game1's Update(). 
        /// Handles calling up which level method to run which will make sure the waves run smoothly
        /// </summary>
        /// <param name="player">the player</param>
        /// <param name="gameTime">timing needed to pass to enemy classes</param>
        /// <returns>list of enemies if another class needs it</returns>
        public List<EnemiesAbstract> Update(Player player, GameTime gameTime, List<Weapons.WeaponAbstract> listOfDrops, List<Weapons.Secret> secrets)
        {
            //From here on out, the level methods will handle waves and increment levels

            if (levelNumber == 1)
            {
                Level1(player, gameTime, listOfDrops, secrets);
            }

            return enemyList;
        }




        //levels

        //level classes handles every wave made for that class and will handle updating or removing enemies

        public void Level1(Player player, GameTime gameTime, List<Weapons.WeaponAbstract> listOfDrops, List<Weapons.Secret> secrets)
        {
            //kickstart the waves
            if (waveNumber == 0)
            {
                Level1wave1();
                waveNumber = 1;
            }


            //from here on out, it handles each wave independantly and checks if we moved on


            else if (waveNumber == 1)
            {
                if (enemyList.Count != 0)
                {
                    EnemyUpdatingAndRemoval(player, gameTime, listOfDrops, secrets);
                }

                //all enemies died so create next wave and increment waveNumber
                else
                {
                    Level1wave2();
                    waveNumber = 2;
                }
            }

            else if (waveNumber == 2)
            {

                //checks to see if an enemy has died and remove it if it does
                if (enemyList.Count != 0)
                {
                    EnemyUpdatingAndRemoval(player, gameTime, listOfDrops, secrets);
                }

                //all enemies died so create next wave and increment waveNumber
                else
                {
                    Level1wave3();
                    waveNumber = 3;
                }
            }

            else if (waveNumber == 3)
            {

                //checks to see if an enemy has died and remove it if it does
                if (enemyList.Count != 0)
                {
                    EnemyUpdatingAndRemoval(player, gameTime, listOfDrops, secrets);
                }

                //all enemies died so create next wave and increment waveNumber
                else
                {
                    Level1wave4();
                    waveNumber = 4;
                }
            }

            else if (waveNumber == 4)
            {

                //checks to see if an enemy has died and remove it if it does
                if (enemyList.Count != 0)
                {
                    EnemyUpdatingAndRemoval(player, gameTime, listOfDrops, secrets);
                }

                //all enemies died so create next wave and increment waveNumber
                else
                {
                    Level1wave5();
                    waveNumber = 5;
                }
            }

            else if (waveNumber == 5)
            {

                //checks to see if an enemy has died and remove it if it does
                if (enemyList.Count != 0)
                {
                    EnemyUpdatingAndRemoval(player, gameTime, listOfDrops, secrets);
                }

                //all enemies died so create next wave and increment waveNumber
                else
                {
                    Level1wave6();
                    waveNumber = 6;
                }
            }

            else if (waveNumber == 6)
            {

                //checks to see if an enemy has died and remove it if it does
                if (enemyList.Count != 0)
                {
                    EnemyUpdatingAndRemoval(player, gameTime, listOfDrops, secrets);
                }

                //all enemies died so create next wave and increment waveNumber
                else
                {
                    Level1wave7();
                    waveNumber = 7;
                }
            }

            else if (waveNumber == 7)
            {

                //checks to see if an enemy has died and remove it if it does
                if (enemyList.Count != 0)
                {
                    EnemyUpdatingAndRemoval(player, gameTime, listOfDrops, secrets);
                }

                //all enemies died so create next wave and increment waveNumber
                else
                {
                    Level1wave8();
                    waveNumber = 8;
                }
            }

            else if (waveNumber == 8)
            {

                //checks to see if an enemy has died and remove it if it does
                if (enemyList.Count != 0)
                {
                    EnemyUpdatingAndRemoval(player, gameTime, listOfDrops, secrets);
                }

                //all enemies died so increment to the boss
                else
                {
                    Level1boss();
                    waveNumber = 9;
                }
            }


            else if (waveNumber == 9)
            {

                //checks to see if an enemy has died and remove it if it does
                if (enemyList.Count != 0)
                {
                    EnemyUpdatingAndRemoval(player, gameTime, listOfDrops, secrets);
                }

                //all enemies died including the boss so increment to the next level (or in this, signal the end of the game)
                else
                {
                    levelNumber = 2;
                    waveNumber = 0;
                }
            }
        }

        /// <summary>
        /// Helper method to help remove enemies when they die from the enemy list 
        /// and also have a chance of spawning a weapon when they die
        /// </summary>
        /// <param name="listOfDrops">list of weapons currently dropped</param>
        public void EnemyUpdatingAndRemoval(Player player, GameTime gameTime, List<Weapons.WeaponAbstract> listOfDrops, List<Weapons.Secret> secrets)
        {
            //loop through list of enemies
            for (int i = 0; i < enemyList.Count; i++)
            {
                //update the enemies
                enemyList[i].Update(player, gameTime);

                //check to see if enemy has died. 
                //If so, make it have a chance of spawning a weapon.
                //Remove enemy from enemy list at the end.
                if (enemyList[i].Exists == false)
                {
                    if (enemyList[i].DropType == Weapons.TypeOfWeapon.Melee)
                    {
                        Weapons.WeaponAbstract drop;
                        dropNumber = rng.Next(0, 80);

                        //modify the numbers that dropNumber is compared to in order to change rarity
                        if(dropNumber == 0)
                        {
                            drop = new Weapons.Secret("Secret", 15, 60, .3f, 1.2f, new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), secret, secret, secrets, player);
                        }
                        else if(dropNumber < 13){
                            float temp = (float)rng.Next(2, 8)/10;
                            drop = new Weapons.BasicMeleeWeapon("Energy Sword", rng.Next(3, 11), 66, temp, temp, new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), energySwordRight, energySwordLeft);
                        }
                        else if (dropNumber < 22)
                        {
                            drop = new Weapons.FeralClawsWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), feralClawsLeft, feralClawsRight);         
                        }
                        else if (dropNumber < 25)
                        {
                            drop = new Weapons.StunWandWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), stunWandRight, stunWandLeft);  
                        }
                        else if (dropNumber < 34)
                        {
                            drop = new Weapons.BasicBombWeapon("Bomb", rng.Next(10, 26), rng.Next(40, 81), rng.Next(3, 8), (float)rng.Next(9, 15) / 10, new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), bomb, basicExplosion);
                        }
                        else if (dropNumber < 40)
                        {
                            drop = new Weapons.ClusterBombWeapon(rng.Next(10, 21), rng.Next(20, 51), new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), clusterBomb, basicExplosion);
                        }
                        else if (dropNumber < 42)
                        {
                            drop = new Weapons.SingularityBombWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), singularityBomb, singularityExplosion);
                        }
                        else
                        {
                            drop = null;
                        }


                        if (drop != null)
                        {
                            if (drop.WeaponType == Weapons.TypeOfWeapon.Secret)
                            {
                                //Makes sure only one secret can be dropped
                                if (secretCount == 0)
                                {
                                    listOfDrops.Add(drop);
                                    secretCount++;
                                }
                            }
                            else
                                listOfDrops.Add(drop);
                        }
                    }

                    if (enemyList[i].DropType == Weapons.TypeOfWeapon.Gun)
                    {
                        Weapons.WeaponAbstract drop;
                        dropNumber = rng.Next(0, 100);

                        //modify the numbers that dropNumber is compared to in order to change rarity
                        if (dropNumber == 0)
                        {
                            drop = new Weapons.Secret("Secret", 15, 60, .3f, 1.2f, new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), secret, secret, secrets, player);
                        }
                        else if (dropNumber < 10)
                        {
                            drop = new Weapons.BasicGunWeapon("Basic Pistol", rng.Next(6, 11), 0, (float)rng.Next(8, 19)/10, 0.2f, new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), basicPistolDown, basicPistolUp, basicPistolRight, basicPistolLeft, basicProjectile);
                        }
                        else if (dropNumber < 22)
                        {
                            float temp = (float)rng.Next(5, 15) / 100;
                            drop = new Weapons.BasicGunWeapon("Basic Rifle", rng.Next(1, 5), 0, temp, temp, new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), basicRifleDown, basicRifleUp, basicRifleRight, basicRifleLeft, basicProjectile);
                        }
                        else if (dropNumber < 28)
                        {
                            drop = new Weapons.ShotgunWeapon(rng.Next(8, 19), rng.Next(100, 301), rng.Next(4, 7), new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), basicShotgunDown, basicShotgunUp, basicShotgunRight, basicShotgunLeft, basicProjectile);
                        }
                        else if (dropNumber < 31)
                        {
                            drop = new Weapons.DisintegratorRayWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), DisintegratorRayDown, DisintegratorRayUp, DisintegratorRayRight, DisintegratorRayLeft, DisintegratorRayBeamHorizontal, DisintegratorRayBeamVertical); 
                        }
                        else if (dropNumber < 34)
                        {
                            drop = new Weapons.RailgunWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), railgunDown, railgunUp, railgunRight, railgunLeft, railHorizontalProjectile, railVerticalProjectile);    
                        }
                        else if (dropNumber < 35)
                        {
                            drop = new Weapons.RocketLauncherWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), rocketLauncherDown, rocketLauncherUp, rocketLauncherRight, rocketLauncherLeft, rocketProjectile, basicExplosion);
                        }
                        else if (dropNumber < 44)
                        {
                            drop = new Weapons.BasicBombWeapon("Bomb", rng.Next(10, 26), rng.Next(40, 81), rng.Next(3, 8), (float)rng.Next(9, 15) / 10, new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), bomb, basicExplosion);
                        }
                        else if (dropNumber < 50)
                        {
                            drop = new Weapons.ClusterBombWeapon(rng.Next(10, 21), rng.Next(20, 51), new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), clusterBomb, basicExplosion);
                        }
                        else if (dropNumber < 51)
                        {
                            drop = new Weapons.SingularityBombWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), singularityBomb, singularityExplosion);
                        }
                        else
                        {
                            drop = null;
                        }


                        if (drop != null)
                        {
                            if (drop.WeaponType == Weapons.TypeOfWeapon.Secret)
                            {
                                //Makes sure only one secret can be dropped
                                if (secretCount == 0)
                                {
                                    listOfDrops.Add(drop);
                                    secretCount++;
                                }
                            }
                            else
                                listOfDrops.Add(drop);
                        }
                    }
                    if (enemyList[i].DropType == Weapons.TypeOfWeapon.Bomb)
                    {
                        Weapons.WeaponAbstract drop;
                        dropNumber = rng.Next(0, 50);

                        //modify the numbers that dropNumber is compared to in order to change rarity
                        if (dropNumber == 0)
                        {
                            drop = new Weapons.Secret("Secret", 15, 60, .3f, 1.2f, new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), secret, secret, secrets, player);            
                        }
                        else if (dropNumber < 13)
                        {
                            drop = new Weapons.BasicBombWeapon("Bomb", rng.Next(10, 26), rng.Next(40, 81), rng.Next(3, 8), (float)rng.Next(9, 15) / 10, new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), bomb, basicExplosion);
                        }
                        else if (dropNumber < 19)
                        {
                            drop = new Weapons.ClusterBombWeapon(rng.Next(10, 21), rng.Next(20, 51), new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), clusterBomb, basicExplosion);
                        }
                        else if (dropNumber < 20)
                        {
                            drop = new Weapons.SingularityBombWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), singularityBomb, singularityExplosion);         
                        }
                        else
                        {
                            drop = null;
                        }


                        if (drop != null)
                        {
                            if (drop.WeaponType == Weapons.TypeOfWeapon.Secret)
                            {
                                //Makes sure only one secret can be dropped
                                if (secretCount == 0)
                                {
                                    listOfDrops.Add(drop);
                                    secretCount++;
                                }
                            }
                            else
                                listOfDrops.Add(drop);
                        }
                    }

                    if (enemyList[i].DropType == Weapons.TypeOfWeapon.Jug)
                    {
                        Weapons.WeaponAbstract drop;
                        dropNumber = rng.Next(0, 3);

                        //modify the numbers that dropNumber is compared to in order to change rarity
                        if (dropNumber == 0)
                        {
                            drop = new Weapons.SingularityBombWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), singularityBomb, singularityExplosion);
                        }
                        else if (dropNumber == 1)
                        {
                            drop = new Weapons.RocketLauncherWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X, (int)enemyList[i].EnemyPosition.Y, 0, 0), rocketLauncherDown, rocketLauncherUp, rocketLauncherRight, rocketLauncherLeft, rocketProjectile, basicExplosion);
                        }
                        else
                        {
                            drop = new Weapons.StunWandWeapon(new Rectangle((int)enemyList[i].EnemyPosition.X + 80, (int)enemyList[i].EnemyPosition.Y, 0, 0), stunWandRight, stunWandLeft);
                        }
                        
                        listOfDrops.Add(drop);
                    }


                    enemyList.Remove(enemyList[i]);
                }
            }
        }


        //waves


        //All these methods below hold what enemies each wave will have
        //These enemies can be made by SetRandomLocationOnEdge or hardcode the location

        
        public void Level1wave2()
        {
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.RangedEnemy));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));
        }

        public void Level1wave3()
        {
            enemyList.Add(new MeleeInfectedCrewmember(0, 325, meleeInfectedCrewMemberMoveRight, meleeInfectedCrewMemberMoveLeft, meleeInfectedCrewMemberDead, rng));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.RangedEnemy));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.RangedEnemy));
        }

        public void Level1wave4()
        {
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.RangedEnemy));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.RangedEnemy));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.RangedEnemy));
            enemyList.Add(new RangedMonster(100, 300, rangedEnemyUp, rangedEnemyDown, rangedEnemyRight, rangedEnemyLeft, rangedEnemySpawn, rangedEnemyDeath, RangedEnemyDirection.Up, enemyProjectileRight, enemyProjectileLeft, enemyProjectileUp, enemyProjectileDown));
            enemyList.Add(new RangedMonster(700, 500, rangedEnemyUp, rangedEnemyDown, rangedEnemyRight, rangedEnemyLeft, rangedEnemySpawn, rangedEnemyDeath, RangedEnemyDirection.Left, enemyProjectileRight, enemyProjectileLeft, enemyProjectileUp, enemyProjectileDown));
            enemyList.Add(new RangedMonster(600, 100, rangedEnemyUp, rangedEnemyDown, rangedEnemyRight, rangedEnemyLeft, rangedEnemySpawn, rangedEnemyDeath, RangedEnemyDirection.Left, enemyProjectileRight, enemyProjectileLeft, enemyProjectileUp, enemyProjectileDown));
        }

        public void Level1wave5()
        {
            enemyList.Add(new MeleeInfectedCrewmember(0, 0, meleeInfectedCrewMemberMoveRight, meleeInfectedCrewMemberMoveLeft, meleeInfectedCrewMemberDead, rng));
            enemyList.Add(new MeleeInfectedCrewmember(800, 0, meleeInfectedCrewMemberMoveRight, meleeInfectedCrewMemberMoveLeft, meleeInfectedCrewMemberDead, rng));
            enemyList.Add(new MeleeInfectedCrewmember(0, 650, meleeInfectedCrewMemberMoveRight, meleeInfectedCrewMemberMoveLeft, meleeInfectedCrewMemberDead, rng));
            enemyList.Add(new MeleeInfectedCrewmember(800, 650, meleeInfectedCrewMemberMoveRight, meleeInfectedCrewMemberMoveLeft, meleeInfectedCrewMemberDead, rng));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));
        }

        public void Level1wave6()
        {
            enemyList.Add(new Juggernaut(400, 0, juggernautMoveRight, juggernautMoveLeft, juggernautDead, juggernautAttackRight, juggernautAttackLeft));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.RangedEnemy));
        }

        public void Level1wave7()
        {
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.RangedEnemy));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.RangedEnemy));
        }

        public void Level1wave8()
        {
            enemyList.Add(new Juggernaut(400, 650, juggernautMoveRight, juggernautMoveLeft, juggernautDead, juggernautAttackRight, juggernautAttackLeft));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));
            enemyList.Add(new RangedMonster(250, 50, rangedEnemyUp, rangedEnemyDown, rangedEnemyRight, rangedEnemyLeft, rangedEnemySpawn, rangedEnemyDeath, RangedEnemyDirection.Down, enemyProjectileRight, enemyProjectileLeft, enemyProjectileUp, enemyProjectileDown));
            enemyList.Add(new RangedMonster(350, 500, rangedEnemyUp, rangedEnemyDown, rangedEnemyRight, rangedEnemyLeft, rangedEnemySpawn, rangedEnemyDeath, RangedEnemyDirection.Up, enemyProjectileRight, enemyProjectileLeft, enemyProjectileUp, enemyProjectileDown));
            enemyList.Add(new RangedMonster(700, 200, rangedEnemyUp, rangedEnemyDown, rangedEnemyRight, rangedEnemyLeft, rangedEnemySpawn, rangedEnemyDeath, RangedEnemyDirection.Left, enemyProjectileRight, enemyProjectileLeft, enemyProjectileUp, enemyProjectileDown));
            enemyList.Add(new RangedMonster(100, 300, rangedEnemyUp, rangedEnemyDown, rangedEnemyRight, rangedEnemyLeft, rangedEnemySpawn, rangedEnemyDeath, RangedEnemyDirection.Right, enemyProjectileRight, enemyProjectileLeft, enemyProjectileUp, enemyProjectileDown));

        }

        public void Level1boss()
        {
            enemyList.Add(new BossMech(400, 0, bossMechMoveRight, bossMechMoveLeft, bossMechDeath, bossMechChargeRight, bossMechChargeLeft, bossProjectile, windowx, windowy));
        }


        //waves for level 2
        public void Level2wave1()
        {
            enemyList.Add(new MeleeInfectedCrewmember(400, 0, meleeInfectedCrewMemberMoveRight, meleeInfectedCrewMemberMoveLeft, meleeInfectedCrewMemberDead, rng));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));
            enemyList.Add(SetRandomLocationOnEdge(EnemyTypes.MeleeCrewmember));
        }
    }
}
