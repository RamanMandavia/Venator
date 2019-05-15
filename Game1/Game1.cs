using Game1.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;

namespace Game1
{
    // Raman Mandavia
    // Cail Umbaugh
    // Alex Sarnese

    // An enum for the state of the game, to ensure inputs are properly recognized
    // And to allow for fairly effective transitions
    enum GameState
    {
        Intro, MainMenu, WeaponSelect, Game, GameEnd
    }

    // An enum for the state of the player, so that only certain inputs can be exectuted
    // At certain times
    enum PlayerState
    {
        FaceLeft, FaceRight, FaceUp, FaceDown, BombThrow, Melee, Evade, Death
    }

    /// <summary>
    /// This is the main lemon for your Will Walrond.
    /// </summary>
    public class Game1 : Game
    {
        //Set to true to unlock all weapons!!!
        bool unlockAll = true;

        // Basics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Player Textures
        private Texture2D playerMoveRightNormal;
        private Texture2D playerMoveRightGun;
        private Texture2D playerMoveLeftNormal;
        private Texture2D playerMoveLeftGun;
        private Texture2D playerMoveUpNormal;
        private Texture2D playerMoveUpGun;
        private Texture2D playerMoveDownNormal;
        private Texture2D playerMoveDownGun;
        private Texture2D playerBombThrowRight;
        private Texture2D playerBombThrowLeft;
        private Texture2D playerBombThrowUp;
        private Texture2D playerBombThrowDown;
        private Texture2D playerMeleeRight;
        private Texture2D playerMeleeLeft;
        private Texture2D playerMeleeUp;
        private Texture2D playerMeleeDown;
        private Texture2D playerStandingNormal;
        private Texture2D playerStandingGun;
        private Texture2D[] playerTextureList;

        // Enemy Textures
        private Texture2D meleeInfectedCrewMemberMoveRight;
        private Texture2D meleeInfectedCrewMemberMoveLeft;
        private Texture2D meleeInfectedCrewMemberDead;
        private Texture2D rangedEnemyRight;
        private Texture2D rangedEnemyLeft;
        private Texture2D rangedEnemyUp;
        private Texture2D rangedEnemyDown;
        private Texture2D rangedEnemySpawn;
        private Texture2D rangedEnemyDeath;
        private Texture2D juggernautMoveRight;
        private Texture2D juggernautMoveLeft;
        private Texture2D juggernautAttackRight;
        private Texture2D juggernautAttackLeft;
        private Texture2D juggernautDead;
        private Texture2D bossMechMoveRight;
        private Texture2D bossMechMoveLeft;
        private Texture2D bossMechChargeRight;
        private Texture2D bossMechChargeLeft;
        private Texture2D bossMechDeath;

        // Weapon and Effects Textures
        private Texture2D basicPistolRight;
        private Texture2D basicPistolLeft;
        private Texture2D basicPistolUp;
        private Texture2D basicPistolDown;
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
        private Texture2D railgunRight;
        private Texture2D railgunLeft;
        private Texture2D railgunUp;
        private Texture2D railgunDown;
        private Texture2D basicProjectile;
        private Texture2D rocketProjectile;
        private Texture2D railHorizontalProjectile;
        private Texture2D railVerticalProjectile;
        private Texture2D bomb;
        private Texture2D clusterBomb;
        private Texture2D singularityBomb;
        private Texture2D basicExplosion;
        private Texture2D singularityExplosion;
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
        private Texture2D bossProjectile;

        private Texture2D enemyProjectileRight;
        private Texture2D enemyProjectileLeft;
        private Texture2D enemyProjectileUp;
        private Texture2D enemyProjectileDown;
        private Texture2D playerDeath;

        // Menus and Menu Things Textures
        private Texture2D displayItemBox;
        private Texture2D displayItemBoxCooldown;
        private Texture2D weaponSelection1;
        private Texture2D weaponSelection2;
        private Texture2D helmetOverlayStatic;
        private Texture2D helmetOverlayAnimation;
        private Texture2D helmetOverlayDeath;
        private Texture2D healthIcon;
        private Texture2D shieldIcon;

        //Save data
        private List<Weapons.WeaponAbstract> meleeUnlocks;
        private List<Weapons.WeaponAbstract> gunUnlocks;
        private List<Weapons.WeaponAbstract> bombUnlocks;
        private List<Weapons.WeaponAbstract> currentRow;
        private string[] saveData;
        private int highScore;
        private string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Venator";

        // Backgrounds, Levels, and Assets Textures
        private Texture2D spaceBackground;
        private Texture2D spaceBackgroundAnimated;
        private Texture2D cargoBayLevel;
        private Texture2D rectangleCrate;
        private Texture2D boxCrate;

        // Rectangle Texture, when you just need that one rectangle
        private Texture2D rectangle;

        //used for testing and seeing hitboxes
        private Texture2D redTest;

        // Fonts
        private SpriteFont titleFont;
        private SpriteFont textFont;
        private SpriteFont largerTextFont;

        // Keyboard-based Fields
        private KeyboardState kbState;
        private KeyboardState previousKbState;

        // State-based fields
        private GameState gameState;

        // Animation-related Fields
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;

        // Class-related fields
        private AnimationHelper animHelper;
        private Player player;
        private Weapons.WeaponAbstract weapon1;
        private Weapons.WeaponAbstract weapon2;

        //list of enemies for passing around
        List<EnemiesAbstract> listOfEnemies;

        //class managers
        WaveHandler waveHandler;

        // List of dropped weapons
        List<Weapons.WeaponAbstract> listOfDrops;
        List<Weapons.Secret> secrets;
        Dictionary<string, Weapons.WeaponAbstract> unlockables;

        // Tracks the time that the actual game is played
        private double gameTimer;

        //SOUNDS
        private Song introAndTitle;
        private Song pickSlide;
        private Song combatTheme;
        private Song winTrack;
        private Song deathTrack;
        private bool startSong;

        //
        //
        //
        //

        // Initial Game Constructor. Creates the game's window
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 650,
                PreferredBackBufferWidth = 800
            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameState = GameState.Intro;
            listOfEnemies = new List<EnemiesAbstract>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
             
            // A basic rectangle, to be scaled and tinted the desired color
            // in situations where a rectangle is need (Health/Armor Bars, underlines)
            rectangle = new Texture2D(GraphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });

            listOfDrops = new List<Weapons.WeaponAbstract>();
            secrets = new List<Weapons.Secret>();

            // Loading up all of the textures
            playerMoveRightNormal = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Move Right Normal");
            playerMoveRightGun = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Move Right Weapon");
            playerMoveLeftNormal = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Move Left Normal");
            playerMoveLeftGun = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Move Left Weapon");
            playerMoveUpNormal = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Move Away Normal");
            playerMoveUpGun = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Move Away Weapon");
            playerMoveDownNormal = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Move Forward Normal");
            playerMoveDownGun = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Move Forward Weapon");
            playerBombThrowRight = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Bomb Throw Right");
            playerBombThrowLeft = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Bomb Throw Left");
            playerBombThrowUp = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Bomb Throw Away");
            playerBombThrowDown = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Bomb Throw Forward");
            playerMeleeRight = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Melee Slash Right");
            playerMeleeLeft = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Melee Slash Left");
            playerMeleeUp = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Melee Slash Away");
            playerMeleeDown = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Melee Slash Forward");
            playerStandingNormal = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Standing Normal");
            playerStandingGun = Content.Load<Texture2D>("Player Sprites/Bounty Hunter Standing Gun");

            playerTextureList = new Texture2D[] {playerMoveRightNormal, playerMoveRightGun,
                                                playerMoveLeftNormal, playerMoveLeftGun, playerMoveUpNormal,
                                                playerMoveUpGun, playerMoveDownNormal, playerMoveDownGun,
                                                playerBombThrowRight, playerBombThrowLeft, playerBombThrowUp,
                                                playerBombThrowDown, playerMeleeRight, playerMeleeLeft, playerMeleeUp,
                                                playerMeleeDown, playerStandingNormal, playerStandingGun};


            meleeInfectedCrewMemberMoveRight = Content.Load<Texture2D>("Enemy Sprites/Infected Crew Member Walk Right");
            meleeInfectedCrewMemberMoveLeft = Content.Load<Texture2D>("Enemy Sprites/Infected Crew Member Walk Left");
            meleeInfectedCrewMemberDead = Content.Load<Texture2D>("Enemy Sprites/Infected Crew Member Dead");
            // IMPLIMENT WHEN READY!!
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

            playerDeath = Content.Load<Texture2D>("Weapons and VFX/Player Death");
            basicPistolRight = Content.Load<Texture2D>("Weapons and VFX/Basic Pistol Right");
            basicPistolLeft = Content.Load<Texture2D>("Weapons and VFX/Basic Pistol Left");
            basicPistolUp = Content.Load<Texture2D>("Weapons and VFX/Basic Pistol Up");
            basicPistolDown = Content.Load<Texture2D>("Weapons and VFX/Basic Pistol Down");
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
            railgunRight = Content.Load<Texture2D>("Weapons and VFX/Railgun Right");
            railgunLeft = Content.Load<Texture2D>("Weapons and VFX/Railgun Left");
            railgunUp = Content.Load<Texture2D>("Weapons and VFX/Railgun Up");
            railgunDown = Content.Load<Texture2D>("Weapons and VFX/Railgun Down");
            bomb = Content.Load<Texture2D>("Weapons and VFX/Bomb");
            clusterBomb = Content.Load<Texture2D>("Weapons and VFX/Cluster Bomb");
            singularityBomb = Content.Load<Texture2D>("Weapons and VFX/Singularity Bomb");
            basicExplosion = Content.Load<Texture2D>("Weapons and VFX/Basic Explosion");
            singularityExplosion = Content.Load<Texture2D>("Weapons and VFX/Singularity Explosion");
            basicProjectile = Content.Load<Texture2D>("Weapons and VFX/Basic ProjectileBullet");
            rocketProjectile = Content.Load<Texture2D>("Weapons and VFX/Rocket Projectile");
            railHorizontalProjectile = Content.Load<Texture2D>("Weapons and VFX/railgunBeamHorizontal");
            railVerticalProjectile = Content.Load<Texture2D>("Weapons and VFX/railgunBeamVertical"); 
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
            enemyProjectileRight = Content.Load<Texture2D>("Weapons and VFX/Enemy Projectile Right");
            enemyProjectileLeft = Content.Load<Texture2D>("Weapons and VFX/Enemy Projectile Left");
            enemyProjectileUp = Content.Load<Texture2D>("Weapons and VFX/Enemy Projectile Up");
            enemyProjectileDown = Content.Load<Texture2D>("Weapons and VFX/Enemy Projectile Down");
            bossProjectile = Content.Load<Texture2D>("Weapons and VFX/Boss ProjectileBullet");


            displayItemBox = Content.Load<Texture2D>("Boxes and Menu Stuff/DisplayItem Box");
            displayItemBoxCooldown = Content.Load<Texture2D>("Boxes and Menu Stuff/DisplayItem Box Cooldown");
            weaponSelection1 = Content.Load<Texture2D>("Boxes and Menu Stuff/Weapon Selection 1");
            weaponSelection2 = Content.Load<Texture2D>("Boxes and Menu Stuff/Weapon Selection 2");
            helmetOverlayStatic = Content.Load<Texture2D>("Boxes and Menu Stuff/Game Helmet Overlay");
            helmetOverlayAnimation = Content.Load<Texture2D>("Boxes and Menu Stuff/Game Helmet Overlay Animate");
            helmetOverlayDeath = Content.Load<Texture2D>("Boxes and Menu Stuff/Game Helmet Overlay Death");
            healthIcon = Content.Load<Texture2D>("Boxes and Menu Stuff/Health Icon");
            shieldIcon = Content.Load<Texture2D>("Boxes and Menu Stuff/Shield Icon");

            spaceBackground = Content.Load<Texture2D>("Boxes and Menu Stuff/Space Background");
            spaceBackgroundAnimated = Content.Load<Texture2D>("Boxes and Menu Stuff/Space Background Animated");
            cargoBayLevel = Content.Load<Texture2D>("Levels and Level Assets/Cargo BayRoom");
            rectangleCrate = Content.Load<Texture2D>("Levels and Level Assets/Rectangle Crate");
            boxCrate = Content.Load<Texture2D>("Levels and Level Assets/Box Crate");

            redTest = Content.Load<Texture2D>("redTest");

            // Loading up the fonts
            titleFont = Content.Load<SpriteFont>("Fonts/BatmanForeverFont");
            textFont = Content.Load<SpriteFont>("Fonts/XirodFont");
            largerTextFont = Content.Load<SpriteFont>("Fonts/XirodFontLarger");
            

            // Animation Setup
            fps = 10.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;

            gameTimer = 0;

            //Unlocked items lists
            gunUnlocks = new List<Weapons.WeaponAbstract>() { new Weapons.BasicGunWeapon("Basic Pistol", 6, 0, 1.0f, 0.2f, new Rectangle(), basicPistolDown, basicPistolUp, basicPistolRight, basicPistolLeft, basicProjectile) };//
            meleeUnlocks = new List<Weapons.WeaponAbstract>() { new Weapons.BasicMeleeWeapon("Energy Sword", 5, 66, 0.6f, .4f, new Rectangle(), energySwordRight, energySwordLeft) };
            bombUnlocks = new List<Weapons.WeaponAbstract>() { new Weapons.BasicBombWeapon("Bomb", 12, 40, 5, 0.8f, new Rectangle(), bomb, basicExplosion) };

            currentRow = gunUnlocks;

            unlockables = new Dictionary<string, Weapons.WeaponAbstract>()
            {
                { "Basic Rifle", new Weapons.BasicGunWeapon("Basic Rifle", 2, 0, 0.2f, 0.2f, new Rectangle(), basicRifleDown, basicRifleUp, basicRifleRight, basicRifleLeft, basicProjectile)},
                { "Shotgun",  new Weapons.ShotgunWeapon(9, 120, .2f, new Rectangle(), basicShotgunDown, basicShotgunUp, basicShotgunRight, basicShotgunLeft, basicProjectile)},
                { "Disintegrator Ray", new Weapons.DisintegratorRayWeapon(new Rectangle(), DisintegratorRayDown, DisintegratorRayUp, DisintegratorRayRight, DisintegratorRayLeft, DisintegratorRayBeamHorizontal, DisintegratorRayBeamVertical)},
                { "Railgun", new Weapons.RailgunWeapon(new Rectangle(), railgunDown, railgunUp, railgunRight, railgunLeft, railHorizontalProjectile, railVerticalProjectile)},
                { "Rocket Launcher", new Weapons.RocketLauncherWeapon(new Rectangle(), rocketLauncherDown, rocketLauncherUp, rocketLauncherRight, rocketLauncherLeft, rocketProjectile, basicExplosion)},

                { "Feral Claws", new Weapons.FeralClawsWeapon(new Rectangle(), feralClawsLeft, feralClawsRight)},
                { "Stun Wand", new Weapons.StunWandWeapon(new Rectangle(), stunWandRight, stunWandLeft)},

                { "Cluster Bomb", new Weapons.ClusterBombWeapon(9, 30, new Rectangle(), clusterBomb, basicExplosion)},
                { "Singularity Bomb", new Weapons.SingularityBombWeapon(new Rectangle(), singularityBomb, singularityExplosion)},
            };

            if (unlockAll)
            {
                gunUnlocks.Add(unlockables["Basic Rifle"]);
                unlockables.Remove("Basic Rifle");
                gunUnlocks.Add(unlockables["Shotgun"]);
                unlockables.Remove("Shotgun");
                gunUnlocks.Add(unlockables["Disintegrator Ray"]);
                unlockables.Remove("Disintegrator Ray");
                gunUnlocks.Add(unlockables["Railgun"]);
                unlockables.Remove("Railgun");
                gunUnlocks.Add(unlockables["Rocket Launcher"]);
                unlockables.Remove("Rocket Launcher");

                meleeUnlocks.Add(unlockables["Feral Claws"]);
                unlockables.Remove("Feral Claws");
                meleeUnlocks.Add(unlockables["Stun Wand"]);
                unlockables.Remove("Stun Wand");

                bombUnlocks.Add(unlockables["Cluster Bomb"]);
                unlockables.Remove("Cluster Bomb");
                bombUnlocks.Add(unlockables["Singularity Bomb"]);
                unlockables.Remove("Singularity Bomb");
            }

            // Constructing what must be constructed
            animHelper = new AnimationHelper();
            waveHandler = new WaveHandler(Content, GraphicsDevice);

            // Load save file or create new save
            if (!Directory.Exists(filePath)|| !File.Exists(filePath + @"\venatorSave.txt"))
            {
                Directory.CreateDirectory(filePath);
                highScore = 0;
                Save();
                Load();
            }
            else
            {
                Load();
            }

            introAndTitle = Content.Load<Song>("Sounds/Actual Intro");
            pickSlide = Content.Load<Song>("Sounds/Pick Slide");
            combatTheme = Content.Load<Song>("Sounds/Combat");
            winTrack = Content.Load<Song>("Sounds/Win");
            deathTrack = Content.Load<Song>("Sounds/Death");

            MediaPlayer.Play(introAndTitle);
            MediaPlayer.IsRepeating = true;
            startSong = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Time Counter for animation
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Setting of keyboard states
            previousKbState = kbState;
            kbState = Keyboard.GetState();

            //Animation algorithm copied (and then altered for implementation here) from last semester's AnimationStarter
            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update frames -- this calls the update methods of the animation helper and other classes that track animation;
                // Animation frames are NOT tracked here
                switch (gameState)
                {
                    case GameState.Intro:
                        if (animHelper.Alpha <= 0)
                            animHelper.UpdateHelmetAnimFrame();
                        animHelper.UpdateSpaceBackground();
                        break;
                    case GameState.MainMenu:
                        {
                            if (animHelper.ControlsDisplay == false)
                                animHelper.UpdateEnterPromptFlicker();
                            animHelper.UpdateSpaceBackground();
                            break;
                        }
                    case GameState.WeaponSelect:
                        {
                            animHelper.UpdateSpaceBackground();
                            animHelper.UpdateWeaponSelectIconFlicker();
                            if (weapon1 != null && weapon2 != null)
                            {
                                animHelper.PauseTimer++;
                                if (animHelper.PauseTimer == 8 && animHelper.DisplaySelectionScreen)
                                {
                                    animHelper.DisplaySelectionScreen = false;
                                    animHelper.PauseTimer = 0;
                                }
                            }
                            break;
                        }
                    case GameState.Game:
                        {
                            switch (player.CurrentPlayerState)
                            {
                                case PlayerState.Melee:
                                    player.UpdateActionFrame();
                                    break;
                                case PlayerState.BombThrow:
                                    player.UpdateActionFrame();
                                    break;
                                case PlayerState.Evade:
                                    player.UpdateEvadeFrame();
                                    break;
                                case PlayerState.Death:
                                    animHelper.UpdateDeathAnimationFrame();
                                    break;

                                //If this is reached, then the player is standing or walking
                                //If walking, update the frame of the character's walk animation
                                //This is the default behavior
                                default:
                                    {
                                        if (player.PlayerMoving == true && player.CurrentPlayerState != PlayerState.Evade)
                                            player.UpdateMovementFrame();
                                        break;
                                    }

                            }
                            for (int i = 0; i < listOfEnemies.Count; i++)
                            {
                                listOfEnemies[i].UpdateMovementFrame();
                            }
                            break;
                        }
                    case GameState.GameEnd:
                        {
                            break;
                        }
                }

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }

            //A switch statement to determine update logic that is executed
            switch (gameState)
            {
                case GameState.Intro:
                    {
                        if(startSong)
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(introAndTitle);
                            startSong = false;
                        }
                        //introAndTitle.Play();
                        animHelper.FadeInUpdate();
                        if (animHelper.HelmetLoaded == true)
                            animHelper.FadeInTitleUpdate();
                        if (animHelper.TitleLoaded == true)
                            gameState = GameState.MainMenu;
                        if (kbState.IsKeyDown(Keys.Enter) && previousKbState.IsKeyDown(Keys.Enter) == false)
                            gameState = GameState.MainMenu;
                        break;
                    }
                case GameState.MainMenu:
                    {
                        if (startSong)
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(introAndTitle);
                            startSong = false;
                        }
                        if (kbState.IsKeyDown(Keys.R))
                        {
                            //deletes file
                            if (File.Exists(filePath + @"\venatorSave.txt") || Directory.Exists(filePath))
                                File.Delete(filePath + @"\venatorSave.txt");
                            unlockAll = false;

                            //resets the weapon unlocks
                            gunUnlocks = new List<Weapons.WeaponAbstract>() { new Weapons.BasicGunWeapon("Basic Pistol", 6, 0, 1.0f, 0.2f, new Rectangle(), basicPistolDown, basicPistolUp, basicPistolRight, basicPistolLeft, basicProjectile) };//
                            meleeUnlocks = new List<Weapons.WeaponAbstract>() { new Weapons.BasicMeleeWeapon("Energy Sword", 5, 66, 0.6f, .4f, new Rectangle(), energySwordRight, energySwordLeft) };
                            bombUnlocks = new List<Weapons.WeaponAbstract>() { new Weapons.BasicBombWeapon("Bomb", 12, 40, 5, 0.8f, new Rectangle(), bomb, basicExplosion) };

                            currentRow = gunUnlocks;

                            unlockables = new Dictionary<string, Weapons.WeaponAbstract>()
                            {
                                { "Basic Rifle", new Weapons.BasicGunWeapon("Basic Rifle", 2, 0, 0.2f, 0.2f, new Rectangle(), basicRifleDown, basicRifleUp, basicRifleRight, basicRifleLeft, basicProjectile)},
                                { "Shotgun",  new Weapons.ShotgunWeapon(9, 120, .2f, new Rectangle(), basicShotgunDown, basicShotgunUp, basicShotgunRight, basicShotgunLeft, basicProjectile)},
                                { "Disintegrator Ray", new Weapons.DisintegratorRayWeapon(new Rectangle(), DisintegratorRayDown, DisintegratorRayUp, DisintegratorRayRight, DisintegratorRayLeft, DisintegratorRayBeamHorizontal, DisintegratorRayBeamVertical)},
                                { "Railgun", new Weapons.RailgunWeapon(new Rectangle(), railgunDown, railgunUp, railgunRight, railgunLeft, railHorizontalProjectile, railVerticalProjectile)},
                                { "Rocket Launcher", new Weapons.RocketLauncherWeapon(new Rectangle(), rocketLauncherDown, rocketLauncherUp, rocketLauncherRight, rocketLauncherLeft, rocketProjectile, basicExplosion)},

                                { "Feral Claws", new Weapons.FeralClawsWeapon(new Rectangle(), feralClawsLeft, feralClawsRight)},
                                { "Stun Wand", new Weapons.StunWandWeapon(new Rectangle(), stunWandRight, stunWandLeft)},

                                { "Cluster Bomb", new Weapons.ClusterBombWeapon(9, 30, new Rectangle(), clusterBomb, basicExplosion)},
                                { "Singularity Bomb", new Weapons.SingularityBombWeapon(new Rectangle(), singularityBomb, singularityExplosion)},
                            };

                            //create fresh new save file
                            Directory.CreateDirectory(filePath);
                            highScore = 0;
                            Save();
                            Load();
                        }
                        if ((kbState.IsKeyDown(Keys.Enter) && previousKbState.IsKeyDown(Keys.Enter) == false) && animHelper.ControlsDisplay == true)
                            gameState = GameState.WeaponSelect;
                        if (kbState.IsKeyDown(Keys.Enter) && previousKbState.IsKeyDown(Keys.Enter) == false)
                            animHelper.ControlsDisplay = true;
                        break;
                    }
                case GameState.WeaponSelect:
                    {
                        if (kbState.IsKeyDown(Keys.Down) && previousKbState.IsKeyDown(Keys.Down) == false)
                        {
                            if (currentRow == gunUnlocks)
                                currentRow = meleeUnlocks;
                            else if (currentRow == meleeUnlocks)
                                currentRow = bombUnlocks;
                            else if (currentRow == bombUnlocks)
                                currentRow = gunUnlocks;

                            animHelper.SlotSelection = 0;
                        }
                        if (kbState.IsKeyDown(Keys.Up) && previousKbState.IsKeyDown(Keys.Up) == false)
                        {
                            if (currentRow == bombUnlocks)
                                currentRow = meleeUnlocks;
                            else if (currentRow == meleeUnlocks)
                                currentRow = gunUnlocks;
                            else if (currentRow == gunUnlocks)
                                currentRow = bombUnlocks;

                            animHelper.SlotSelection = 0;
                        }
                        if (kbState.IsKeyDown(Keys.Right) && previousKbState.IsKeyDown(Keys.Right) == false)
                        {
                            if (animHelper.SlotSelection == currentRow.Count - 1)
                                animHelper.SlotSelection = 0;
                            else
                                animHelper.SlotSelection++;
                        }
                        if (kbState.IsKeyDown(Keys.Left) && previousKbState.IsKeyDown(Keys.Left) == false)
                        {
                            if (animHelper.SlotSelection == 0)
                                animHelper.SlotSelection = currentRow.Count - 1;
                            else
                                animHelper.SlotSelection--;
                        }
                        if ((kbState.IsKeyDown(Keys.Enter) && previousKbState.IsKeyDown(Keys.Enter) == false) && animHelper.ControlsDisplay == true)
                        {
                            if (weapon1 == null)
                            {
                                if (currentRow == gunUnlocks)
                                    weapon1 = gunUnlocks[animHelper.SlotSelection];
                                else if (currentRow == meleeUnlocks)
                                    weapon1 = meleeUnlocks[animHelper.SlotSelection];
                                else if (currentRow == bombUnlocks)
                                    weapon1 = bombUnlocks[animHelper.SlotSelection];
                            }
                            else
                            {
                                if (currentRow == gunUnlocks)
                                    weapon2 = gunUnlocks[animHelper.SlotSelection];
                                else if (currentRow == meleeUnlocks)
                                    weapon2 = meleeUnlocks[animHelper.SlotSelection];
                                else if (currentRow == bombUnlocks)
                                    weapon2 = bombUnlocks[animHelper.SlotSelection];

                                //Fix the double weapon bug
                                if (weapon2 == weapon1)
                                {
                                    switch (weapon2.Name)
                                    {
                                        case "Basic Pistol":
                                            weapon2 = new Weapons.BasicGunWeapon("Basic Pistol", 6, 0, 1.0f, 0.2f, new Rectangle(), basicPistolDown, basicPistolUp, basicPistolRight, basicPistolLeft, basicProjectile);
                                            break;
                                        case "Basic Rifle":
                                            weapon2 = new Weapons.BasicGunWeapon("Basic Rifle", 2, 0, 0.2f, 0.2f, new Rectangle(), basicRifleDown, basicRifleUp, basicRifleRight, basicRifleLeft, basicProjectile);
                                            break;
                                        case "Shotgun":
                                            weapon2 = new Weapons.ShotgunWeapon(9, 120, 5f, new Rectangle(), basicShotgunDown, basicShotgunUp, basicShotgunRight, basicShotgunLeft, basicProjectile);
                                            break;
                                        case "Disintegrator Ray":
                                            weapon2 = new Weapons.DisintegratorRayWeapon(new Rectangle(), DisintegratorRayDown, DisintegratorRayUp, DisintegratorRayRight, DisintegratorRayLeft, DisintegratorRayBeamHorizontal, DisintegratorRayBeamVertical);
                                            break;
                                        case "Railgun":
                                            weapon2 = new Weapons.RailgunWeapon(new Rectangle(), railgunDown, railgunUp, railgunRight, railgunLeft, railHorizontalProjectile, railVerticalProjectile);
                                            break;
                                        case "Rocket Launcher":
                                            weapon2 = new Weapons.RocketLauncherWeapon(new Rectangle(), rocketLauncherDown, rocketLauncherUp, rocketLauncherRight, rocketLauncherLeft, rocketProjectile, basicExplosion);
                                            break;
                                        case "Energy Sword":
                                            weapon2 = new Weapons.BasicMeleeWeapon("Energy Sword", 4, 66, 0.6f, .4f, new Rectangle(), energySwordRight, energySwordLeft);
                                            break;
                                        case "Feral Claws":
                                            weapon2 = new Weapons.FeralClawsWeapon(new Rectangle(), feralClawsLeft, feralClawsRight);
                                            break;
                                        case "Stun Wand":
                                            weapon2 = new Weapons.StunWandWeapon(new Rectangle(), stunWandRight, stunWandLeft);
                                            break;
                                        case "Bomb":
                                            weapon2 = new Weapons.BasicBombWeapon("Bomb", 12, 45, 5, 0.8f, new Rectangle(), bomb, basicExplosion);
                                            break;
                                        case "Cluster Bomb":
                                            weapon2 = new Weapons.ClusterBombWeapon(9, 30, new Rectangle(), clusterBomb, basicExplosion);
                                            break;
                                        case "Singularity Bomb":
                                            weapon2 = new Weapons.SingularityBombWeapon(new Rectangle(), singularityBomb, singularityExplosion);
                                            break;
                                    }
                                }
                            }
                        }

                        if (animHelper.DisplaySelectionScreen == false)
                        {
                            //player has finished selecting weapons
                            //Create the player's character and switch to the game screen
                            gameTimer = 0;
                            animHelper.Reset();
                            listOfEnemies.Clear();
                            listOfDrops.Clear();
                            secrets.Clear();
                            waveHandler.Reset();
                            player = new Player(weapon1, weapon2, playerTextureList);
                            weapon1 = null;
                            weapon2 = null;
                            //MediaPlayer.Play(pickSlide);
                            //pickSlide.Play();
                            //MediaPlayer.Stop();
                            startSong = true;
                            gameState = GameState.Game;
                        }
                        break;
                    }
                case GameState.Game:
                    {
                        if (startSong == true)
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(combatTheme);
                            startSong = false;
                        }
                        //MediaPlayer.IsRepeating = true;
                        //combatTheme.Play();

                        gameTimer += gameTime.ElapsedGameTime.TotalSeconds;


                        //updates the player, enemies, and later, traps and crates
                        listOfEnemies = waveHandler.Update(player, gameTime, listOfDrops, secrets);
                        player.Update(kbState, gameTime, listOfEnemies, listOfDrops, secrets, unlockables, gunUnlocks, meleeUnlocks, bombUnlocks);

                        //despawns drops after 40 frames
                        for(int i = 0; i < listOfDrops.Count; i++)
                        {
                            listOfDrops[i].DropTimer++;
                            if (listOfDrops[i].DropTimer > 350)
                                listOfDrops.Remove(listOfDrops[i]);
                        }


                        if (player.CurrentPlayerState == PlayerState.Death)
                            animHelper.PlayDeathAnimation = true;

                        if (animHelper.YouAreReallyDeadNow)
                        {
                            gameState = GameState.GameEnd;
                            animHelper.DisplayGameOver = true;
                            startSong = true;
                        }
                        
                        //Ends the game if level 2 is reached as we do not have a level 2
                        if(waveHandler.Level == 2)
                        {
                            gameState = GameState.GameEnd;
                            startSong = true;
                        }

                        break;
                    }
                case GameState.GameEnd:
                    {
                        if (player.Health <= 0)
                        {
                            animHelper.FadeInUpdate();
                            if (startSong)
                            {
                                MediaPlayer.Stop();
                                MediaPlayer.Play(deathTrack);
                                startSong = false;
                            }
                            //deathTrack.Play();
                        }
                        else
                        {
                            if (startSong)
                            {
                                MediaPlayer.Stop();
                                MediaPlayer.Play(winTrack);
                                startSong = false;
                            }
                            //winTrack.Play();
                        }
                        if (kbState.IsKeyDown(Keys.Enter) && previousKbState.IsKeyDown(Keys.Enter) == false)
                        {
                            animHelper.Reset();
                            player = null;
                            gameState = GameState.MainMenu;
                            startSong = true;
                        }
                        break;
                    }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Intro:
                    {
                        animHelper.DrawAnimatedBackground(spriteBatch, spaceBackgroundAnimated);
                        animHelper.FadeInDraw(spriteBatch, spaceBackground, rectangle);
                        if (animHelper.Alpha <= 0)
                            animHelper.DrawHelmetOverlayAnimation(spriteBatch, helmetOverlayAnimation);
                        if (animHelper.HelmetLoaded == true)
                        {
                            spriteBatch.Draw(helmetOverlayStatic, new Rectangle(0, 0, 800, 650), Color.White);
                            animHelper.DrawTitleFadeIn(spriteBatch, titleFont);
                        }
                    }
                    break;
                case GameState.MainMenu:
                    {
                        animHelper.DrawAnimatedBackground(spriteBatch, spaceBackgroundAnimated);
                        spriteBatch.Draw(helmetOverlayStatic, new Rectangle(0, 0, 800, 650), Color.White);
                        if (animHelper.ControlsDisplay == false)
                        {
                            for (int i = -5; i < 6; i = i + 2)
                            {
                                for (int b = -5; b < 6; b = b + 2)
                                {
                                    //spriteBatch.DrawString(titleFont, "Minecraft", new Vector2(133 + i, 145 + b), Color.Multiply(Color.DarkRed, 0.065f));
                                    spriteBatch.DrawString(titleFont, "Venator", new Vector2(133 + i, 145 + b), Color.Multiply(Color.DarkRed, 0.065f));
                                }
                            }
                            //spriteBatch.DrawString(titleFont, "Minecraft", new Vector2(133, 145), Color.Firebrick);
                            spriteBatch.DrawString(titleFont, "Venator", new Vector2(133, 145), Color.Firebrick);
                            animHelper.DrawEnterPrompt(spriteBatch, textFont);
                        }
                        else if (animHelper.ControlsDisplay == true)
                        {
                            animHelper.DrawControlsDisplay(spriteBatch, textFont);
                        }
                        break;
                    }
                case GameState.WeaponSelect:
                    {
                        animHelper.DrawAnimatedBackground(spriteBatch, spaceBackgroundAnimated);
                        spriteBatch.Draw(helmetOverlayStatic, new Rectangle(0, 0, 800, 650), Color.White);
                        if (animHelper.DisplaySelectionScreen == true)
                        {
                            animHelper.DrawWeaponSelectScreen(spriteBatch, textFont, largerTextFont, displayItemBox, weaponSelection1, weaponSelection2, gunUnlocks, meleeUnlocks, bombUnlocks, currentRow, weapon1, weapon2);

                            //draw unlocked guns
                            for (int i = 0; i < gunUnlocks.Count; i++)
                            {
                                if (gunUnlocks[i].GetType() == typeof(Weapons.DisintegratorRayWeapon))
                                {
                                    spriteBatch.Draw(
                                    gunUnlocks[i].UITexture,
                                    new Vector2(155 + ((i - 1) * 114), 283),
                                    new Rectangle(0, 0, gunUnlocks[i].UITexture.Width / 4, gunUnlocks[i].UITexture.Height),
                                    Color.White,
                                    -MathHelper.PiOver4,
                                    Vector2.Zero,
                                    1.0f,
                                    SpriteEffects.None,
                                    0.0f);
                                }
                                else
                                {
                                    spriteBatch.Draw(
                                    gunUnlocks[i].UITexture,
                                    new Vector2(140 + ((i - 1) * 114), 293),
                                    new Rectangle(0, 0, gunUnlocks[i].UITexture.Width / 4, gunUnlocks[i].UITexture.Height),
                                    Color.White,
                                    -MathHelper.PiOver4,
                                    Vector2.Zero,
                                    1.0f,
                                    SpriteEffects.None,
                                    0.0f);
                                }
                            }

                            //draw unlocked melee
                            for (int i = 0; i < meleeUnlocks.Count; i++)
                            {
                                spriteBatch.Draw(
                                    meleeUnlocks[i].UITexture,
                                    new Vector2(229 + ((i - 1) * 114), 345),
                                    new Rectangle(0, 0, 128, 128),
                                    Color.White,
                                    MathHelper.PiOver4,
                                    Vector2.Zero,
                                    .7f,
                                    SpriteEffects.None,
                                    0.0f);
                            }

                            //draw unlocked bombs
                            for (int i = 0; i < bombUnlocks.Count; i++)
                            {
                                spriteBatch.Draw(
                                    bombUnlocks[i].UITexture,
                                    new Vector2(152 + ((i - 1) * 114), 453),
                                    new Rectangle(0, 0, 128, 128),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    1.2f,
                                    SpriteEffects.None,
                                    0.0f);
                            }
                        }
                        break;
                    }
                case GameState.Game:
                    {
                        spriteBatch.Draw(
                            cargoBayLevel,
                            new Rectangle(0, 0, 800, 650),
                            new Rectangle(0, 0, 900, 900),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            1.0f);

                        //draws the player, enemies, and later, traps and crates
                        player.Draw(spriteBatch);

                        for (int i = 0; i < listOfEnemies.Count; i++)
                        {
                            listOfEnemies[i].Draw(spriteBatch);
                        }

                        //draw dropped weapons, blink when despawing soon
                        foreach(Weapons.WeaponAbstract w in listOfDrops)
                        {

                            if (w.DropTimer >= 200 && (w.DropTimer % 8 == 0 || w.DropTimer % 8 == 1))
                            {
                                //uhhh do nothing
                            }
                            else
                                w.Draw(spriteBatch, null);
                        }

                        foreach (Weapons.Secret s in secrets)
                        {
                            s.Draw(spriteBatch, null);
                        }

                        spriteBatch.Draw(helmetOverlayStatic, new Rectangle(0, 0, 800, 650), Color.White);
                        player.DrawUI(spriteBatch, rectangle, displayItemBox, weaponSelection1, displayItemBoxCooldown, healthIcon, shieldIcon, textFont);

                        animHelper.DrawPlayerDeath(spriteBatch, playerDeath);

                        break;
                    }
                case GameState.GameEnd:
                    {
                        if (player.Health <= 0)
                        {
                            Save();
                            animHelper.DrawGameOver(spriteBatch, helmetOverlayDeath, largerTextFont, textFont, gameTime);
                        }
                        else
                        {
                            int gameTimerScore = (int)(gameTimer) - 10;
                            if (gameTimerScore < 0)
                                gameTimerScore = 0;

                            if (highScore < player.Score - gameTimerScore - player.TotalDamageTaken)
                            {
                                highScore = player.Score - gameTimerScore - player.TotalDamageTaken;
                            }

                            Save();

                            animHelper.DrawGameComplete(spriteBatch, gameTimerScore, player.TotalDamageTaken, player.Score, highScore, helmetOverlayStatic, textFont, largerTextFont);
                        }

                        break;
                    }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        // Cail
        public void Save()
        {
            List<string> weaponNames = new List<string>();

            foreach(Weapons.WeaponAbstract w in gunUnlocks)
            {
                weaponNames.Add(w.Name);
            }
            foreach (Weapons.WeaponAbstract w in meleeUnlocks)
            {
                weaponNames.Add(w.Name);
            }
            foreach (Weapons.WeaponAbstract w in bombUnlocks)
            {
                weaponNames.Add(w.Name);
            }

            saveData = new string[weaponNames.Count + 1];

            saveData[0] = ""+highScore;

            for(int i = 0; i < weaponNames.Count; i++)
            {
                saveData[i+1] = weaponNames[i];
            }

            System.IO.File.WriteAllLines(filePath + @"\venatorSave.txt", saveData);
        }

        // Cail
        public void Load()
        {
            string[] lines = System.IO.File.ReadAllLines(filePath + @"\venatorSave.txt");

            highScore = int.Parse(lines[0]);

            for(int i = 1; i < lines.Length; i++)
            {
                if(unlockables.ContainsKey(lines[i]))
                {
                    if(unlockables[lines[i]].WeaponType == Weapons.TypeOfWeapon.Gun)
                    {
                        gunUnlocks.Add(unlockables[lines[i]]);
                        unlockables.Remove(lines[i]);
                    }
                    else if(unlockables[lines[i]].WeaponType == Weapons.TypeOfWeapon.Melee)
                    {
                        meleeUnlocks.Add(unlockables[lines[i]]);
                        unlockables.Remove(lines[i]);
                    }
                    else if (unlockables[lines[i]].WeaponType == Weapons.TypeOfWeapon.Bomb)
                    {
                        bombUnlocks.Add(unlockables[lines[i]]);
                        unlockables.Remove(lines[i]);
                    }
                }
            }
        }

        protected char[][] ReadLvlFile(string filepath)
        {
            StreamReader reader = new StreamReader(filepath);
            char[][] grid = new char[36][];
            for (int i = 0; i < 36; i++)
            {
                grid[i] = new char[36];
            }
            string data = reader.ReadLine();
            while (data != null)
            {
                string[] coords = data.Split(',');
                int x = int.Parse(coords[1]);
                int y = int.Parse(coords[2]);
                grid[x][y] = char.Parse(coords[0]);
            }
            return grid;
        }
    }
}
