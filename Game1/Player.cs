using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game1.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class Player
    {
        // Raman Mandavia
        //Alex Sarnese
        
        // Fields
        // The basics! May be expanded later
        private int maxHealth;
        private int health;
        private int totalDamageTaken;
        private int maxArmor;
        private int armor;
        private int score;
        private int moveSpeed;
        private bool cycledWeapons;
        private double armorTimer;
        private int initialScore;
        private int evadeXChange;
        private int evadeYChange;
        private bool addAfterImage;
        // These will ensure that the player has a single instance of being "hit"
        // Damage will be singular and isolated
        private bool hitBoxActive;
        private bool takingDamage;
        private double damageTimer;
        // Tracks the weapons in the player's possession, and the current weapon equipped
        // Current weapon is for both attacking and drawing
        private Weapons.WeaponAbstract weapon1;
        private Weapons.WeaponAbstract weapon2;
        private Weapons.WeaponAbstract currentWeapon;
        private Weapons.WeaponAbstract storedWeapon;
        // Player states. The current state, and the previous state
        // This is for both animation, movement, and action purposes
        private PlayerState currentPlayerState;
        private PlayerState previousPlayerState;
        private PlayerState actionReferenceState;
        private bool playerMoving;
        // Tracking of frames of animation. The two types of cycles have 8 and 4 frames,
        // respectively. Hence two variables
        private int moveCycleFrame;
        private int actionCycleFrame;
        private int evadeFrame;
        // Player position is stored as a vector, and the rectangle is for collisions
        private Vector2 playerPosition;
        private Rectangle playerHitBox;

        // The updated list of drops, necessary when swapping weapons, so that the current weapon is dropped
        private List<Weapons.WeaponAbstract> listOfWeapons;

        // A list of vectors to track the afterimages of the evade and their positions
        private List<Vector2> evadeAfterImages;
        


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


        // Properties
        public int MaxHealth
        { get { return maxHealth; } }

        public int Health
        { get { return health; } }

        public int TotalDamageTaken
        {
            get { return totalDamageTaken; }
            set { totalDamageTaken = value; }
        }

        public int MaxArmor
        { get { return maxArmor; } }

        public int Armor
        { get { return armor; } }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public int MoveSpeed
        { get { return moveSpeed; } }

        public Weapons.WeaponAbstract Weapon1
        { get { return weapon1; } set { weapon1 = value; } }

        public Weapons.WeaponAbstract Weapon2
        { get { return weapon2; } set { weapon2 = value; } }

        public Weapons.WeaponAbstract CurrentWeapon
        { get { return currentWeapon; } }

        public PlayerState CurrentPlayerState
        {
            get { return currentPlayerState; }
            set { currentPlayerState = value; }
        }

        public PlayerState PreviousPlayerState
        {
            get { return previousPlayerState; }
            set { previousPlayerState = value; }
        }

        public PlayerState ActionReferenceState
        {
            get { return actionReferenceState; }
            set { actionReferenceState = value; }
        }

        public bool PlayerMoving
        {
            get { return playerMoving; }
            set { playerMoving = value; }
        }

        public Vector2 PlayerPosition
        { get { return playerPosition; } }

        public Rectangle PlayerHitBox
        { get { return playerHitBox; } }

        public List<Weapons.WeaponAbstract> ListOfWeapons
        { get { return listOfWeapons; } }

        // Constructor(s)
        public Player(Weapons.WeaponAbstract weapon1, Weapons.WeaponAbstract weapon2, Texture2D[] playerTextureList)
        {
            maxHealth = 200;
            health = 200;
            totalDamageTaken = 0;
            maxArmor = 100;
            armor = 0;
            score = 0;
            moveSpeed = 0;

            hitBoxActive = true;
            takingDamage = false;
            damageTimer = 0;

            armorTimer = 10.0;
            initialScore = 0;

            this.weapon1 = weapon1;
            this.weapon2 = weapon2;
            currentWeapon = weapon1;
            storedWeapon = weapon2;

            currentPlayerState = PlayerState.FaceDown;
            previousPlayerState = PlayerState.FaceUp;
            playerMoving = false;

            moveCycleFrame = 0;
            actionCycleFrame = 0;
            evadeFrame = 0;

            evadeXChange = 0;
            evadeYChange = 0;

            playerPosition = new Vector2(324, 399);
            playerHitBox = new Rectangle((int)playerPosition.X + 28, (int)playerPosition.Y + 4, 41, 88);

            cycledWeapons = false;


            playerMoveRightNormal = playerTextureList[0];
            playerMoveRightGun = playerTextureList[1];
            playerMoveLeftNormal = playerTextureList[2];
            playerMoveLeftGun = playerTextureList[3];
            playerMoveUpNormal = playerTextureList[4];
            playerMoveUpGun = playerTextureList[5];
            playerMoveDownNormal = playerTextureList[6];
            playerMoveDownGun = playerTextureList[7];
            playerBombThrowRight = playerTextureList[8];
            playerBombThrowLeft = playerTextureList[9];
            playerBombThrowUp = playerTextureList[10];
            playerBombThrowDown = playerTextureList[11];
            playerMeleeRight = playerTextureList[12];
            playerMeleeLeft = playerTextureList[13];
            playerMeleeUp = playerTextureList[14];
            playerMeleeDown = playerTextureList[15];
            playerStandingNormal = playerTextureList[16];
            playerStandingGun = playerTextureList[17];

            listOfWeapons = new List<Weapons.WeaponAbstract>();
            evadeAfterImages = new List<Vector2>();
            addAfterImage = false;
        }

        // Methods
        // Updates and/or wraps the counter for the movement frames
        public void UpdateMovementFrame()
        {
            if (currentPlayerState != PlayerState.BombThrow
                || currentPlayerState != PlayerState.Death
                || currentPlayerState != PlayerState.Evade
                || currentPlayerState != PlayerState.Melee)
            {
                moveCycleFrame++;
                if (moveCycleFrame > 7)
                    moveCycleFrame = 0;
            }
        }

        // Updates and/or wraps the counter for the action frames
        public void UpdateActionFrame()
        {
            if (currentPlayerState == PlayerState.Melee)
            {
                if (currentWeapon.TimeThatHasPassed < currentWeapon.AttackPhaseDurationTime * 0.25)
                {
                    actionCycleFrame = 1;
                }
                else if (currentWeapon.TimeThatHasPassed < currentWeapon.AttackPhaseDurationTime * 0.5)
                {
                    actionCycleFrame = 2;
                }
                else if (currentWeapon.TimeThatHasPassed < currentWeapon.AttackPhaseDurationTime * 0.75)
                {
                    actionCycleFrame = 3;
                }
                else
                {
                    actionCycleFrame = 0;
                    currentPlayerState = previousPlayerState;
                }
            }
            else if (currentPlayerState == PlayerState.BombThrow)
            {
                actionCycleFrame++;
                if (actionCycleFrame > 3)
                {
                    actionCycleFrame = 0;
                    currentPlayerState = previousPlayerState;
                }
            }
        }

        // Updates and/or wraps the counter for the evade animation frames. There are 5 of these, instead of 4
        public void UpdateEvadeFrame()
        {
            evadeFrame++;
            if (evadeFrame > 4)
            {
                evadeFrame = 0;
                currentPlayerState = previousPlayerState;
                hitBoxActive = true;
                evadeAfterImages.Clear();
                evadeXChange = 0;
                evadeYChange = 0;
            }
        }

        public void Update(KeyboardState kbState, GameTime gameTime, List<EnemiesAbstract> listOfEnemies, List<Weapons.WeaponAbstract> drops, List<Weapons.Secret> secrets, Dictionary<string, Weapons.WeaponAbstract> unlockables, List<Weapons.WeaponAbstract> gunUnlocks, List<Weapons.WeaponAbstract> meleeUnlocks, List<Weapons.WeaponAbstract> bombUnlocks)
        {

            armorTimer += gameTime.ElapsedGameTime.TotalSeconds;

            listOfWeapons = drops;

            if (health <= 0)
                currentPlayerState = PlayerState.Death;
            

            if (currentPlayerState != PlayerState.Death)
            {
                if (currentPlayerState != PlayerState.Evade)
                {
                    if (kbState.IsKeyDown(Keys.Up))
                    {
                        PreviousPlayerState = CurrentPlayerState;
                        CurrentPlayerState = PlayerState.FaceUp;
                        Attack();
                    }
                    else if (kbState.IsKeyDown(Keys.Down))
                    {
                       
                        PreviousPlayerState = CurrentPlayerState;
                        CurrentPlayerState = PlayerState.FaceDown;
                        Attack();
                    }
                    else if (kbState.IsKeyDown(Keys.Right))
                    {
                        PreviousPlayerState = CurrentPlayerState;
                        CurrentPlayerState = PlayerState.FaceRight;
                        Attack();
                    }
                    else if (kbState.IsKeyDown(Keys.Left))
                    {
                        PreviousPlayerState = CurrentPlayerState;
                        CurrentPlayerState = PlayerState.FaceLeft;
                        Attack();
                    }

                    //Controls movement based on the keys ASWD
                    //Raman - I changed these values to 4 instead of 5. Feel free to change them back
                    // also restored ability to move diagonally. No idea why that was removed
                    if (kbState.IsKeyDown(Keys.W))
                    {
                        playerMoving = true;
                        Move(0, -4);
                        evadeYChange = -4;
                    }
                    if (kbState.IsKeyDown(Keys.A))
                    {
                        playerMoving = true;
                        Move(-4, 0);
                        evadeXChange = -4;
                    }
                    if (kbState.IsKeyDown(Keys.S))
                    {
                        playerMoving = true;
                        Move(0, 4);
                        evadeYChange = 4;
                    }
                    if (kbState.IsKeyDown(Keys.D))
                    {
                        playerMoving = true;
                        Move(4, 0);
                        evadeXChange = 4;
                    }
                    //if this is reached, no movement keys are being held down
                    if (kbState.IsKeyDown(Keys.W) == false && kbState.IsKeyDown(Keys.A) == false &&
                        kbState.IsKeyDown(Keys.S) == false && kbState.IsKeyDown(Keys.D) == false)
                    {
                        playerMoving = false;
                        evadeXChange = 0;
                        evadeYChange = 0;
                    }

                    if (kbState.IsKeyDown(Keys.Space) && playerMoving == true && currentPlayerState != PlayerState.BombThrow && currentPlayerState != PlayerState.Melee)
                    {
                        previousPlayerState = currentPlayerState;
                        currentPlayerState = PlayerState.Evade;
                        Evade(evadeXChange, evadeYChange);
                    }
                }

                //picks up a dropped weapon
                if (kbState.IsKeyDown(Keys.Q))
                {
                    foreach(Weapons.WeaponAbstract w in drops)
                    {
                        if (w.WeaponType == Weapons.TypeOfWeapon.Secret)
                        {
                            if (playerHitBox.Intersects(new Rectangle(w.Location.X, w.Location.Y, 50, 50)))
                            {
                                if (currentWeapon == weapon1)
                                {
                                    weapon1 = w;
                                }
                                else
                                {
                                    weapon2 = w;
                                }

                                currentWeapon = w;
                                drops.Remove(w);
                                break;
                            }
                        }
                        else
                        {
                            if (playerHitBox.Intersects(new Rectangle(w.Location.X, w.Location.Y, w.UITexture.Width / 4, w.UITexture.Height)))
                            {
                                if (currentWeapon == weapon1)
                                {
                                    weapon1 = w;
                                }
                                else
                                {
                                    weapon2 = w;
                                }

                                if (unlockables.Keys.Contains(w.Name))
                                {
                                    if (w.WeaponType == Weapons.TypeOfWeapon.Gun)
                                    {
                                        gunUnlocks.Add(unlockables[w.Name]);
                                        unlockables.Remove(w.Name);
                                    }
                                    if (w.WeaponType == Weapons.TypeOfWeapon.Melee)
                                    {
                                        meleeUnlocks.Add(unlockables[w.Name]);
                                        unlockables.Remove(w.Name);
                                    }
                                    if (w.WeaponType == Weapons.TypeOfWeapon.Bomb)
                                    {
                                        bombUnlocks.Add(unlockables[w.Name]);
                                        unlockables.Remove(w.Name);
                                    }
                                }
                                currentWeapon = w;
                                drops.Remove(w);
                                break;
                            }
                        }
                    }

                    foreach(Weapons.Secret s in secrets)
                    {
                        if (playerHitBox.Intersects(new Rectangle(s.Location.X, s.Location.Y, 50, 50)))
                        {
                            if(weapon1 == s)
                            {
                                currentWeapon = weapon1;
                                s.IsAttacking = false;
                                s.ReadyToExplode = false;
                                secrets.Remove(s);
                                break;
                            }
                            else if (weapon2 == s)
                            {
                                currentWeapon = weapon2;
                                s.IsAttacking = false;
                                s.ReadyToExplode = false;
                                secrets.Remove(s);
                                break;
                            }
                            else
                            {
                                if (currentWeapon == weapon1)
                                    weapon1 = s;
                                else
                                    weapon2 = s;
                                currentWeapon = s;
                                s.IsAttacking = false;
                                s.ReadyToExplode = false;
                                secrets.Remove(s);
                                break;
                            }
                        }
                    }
                }


                //swap the held weapons
                //cycledWeapons is used so clicking shift cycles weapons only once
                if (kbState.IsKeyDown(Keys.LeftShift) && !cycledWeapons)
                {
                    CycleWeapon();
                    cycledWeapons = true;
                }
                else if (!kbState.IsKeyDown(Keys.LeftShift))
                {
                    cycledWeapons = false;
                }

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

                if (armorTimer >= 10)
                {
                    armorTimer = 0;
                    initialScore = score;
                }
                if (armorTimer < 10)
                {
                    if (score - initialScore >= 250)
                    {
                        if (armor < maxArmor)
                            armor += 20;
                        if (armor > maxArmor)
                            armor = maxArmor;
                        initialScore = score;
                    }
                }

                if (currentPlayerState == PlayerState.Evade)
                {
                    Evade(evadeXChange, evadeYChange);
                    hitBoxActive = false;
                }

                currentWeapon.Update(listOfEnemies, this, gameTime);
                storedWeapon.Update(listOfEnemies, this, gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (currentPlayerState)
            {
                //notice how I draw the weapons first when the player is facing upward.
                //This is so the player's texture gets placed on top of the weapon instead of floating on his back

                case PlayerState.FaceUp:
                    storedWeapon.Draw(spriteBatch, this);
                    currentWeapon.Draw(spriteBatch, this);
                    if (playerMoving == false)
                        DrawStanding(spriteBatch);
                    else if (playerMoving == true)
                        DrawMovement(spriteBatch);
                    break;

                case PlayerState.FaceLeft:
                    if (playerMoving == false)
                        DrawStanding(spriteBatch);
                    else if (playerMoving == true)
                        DrawMovement(spriteBatch);
                    storedWeapon.Draw(spriteBatch, this);
                    currentWeapon.Draw(spriteBatch, this);
                    break;

                case PlayerState.FaceRight:
                    if (playerMoving == false)
                        DrawStanding(spriteBatch);
                    else if (playerMoving == true)
                        DrawMovement(spriteBatch);
                    storedWeapon.Draw(spriteBatch, this);
                    currentWeapon.Draw(spriteBatch, this);
                    break;

                case PlayerState.FaceDown:
                    if (playerMoving == false)
                        DrawStanding(spriteBatch);
                    else if (playerMoving == true)
                        DrawMovement(spriteBatch);
                    storedWeapon.Draw(spriteBatch, this);
                    currentWeapon.Draw(spriteBatch, this);
                    break;

                case PlayerState.Melee:
                    if (previousPlayerState == PlayerState.FaceDown)
                    {
                        DrawAction(spriteBatch);
                        storedWeapon.Draw(spriteBatch, this);
                        currentWeapon.Draw(spriteBatch, this);
                    }
                    else
                    {
                        storedWeapon.Draw(spriteBatch, this);
                        currentWeapon.Draw(spriteBatch, this);
                        DrawAction(spriteBatch);
                    }
                    break;
                case PlayerState.BombThrow:
                    DrawAction(spriteBatch);
                    storedWeapon.Draw(spriteBatch, this);
                    currentWeapon.Draw(spriteBatch, this);
                    break;
                case PlayerState.Evade:
                    DrawEvade(spriteBatch);
                    if (storedWeapon.WeaponType == Weapons.TypeOfWeapon.Bomb && storedWeapon.IsAttacking)
                        storedWeapon.Draw(spriteBatch, this);
                    if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Bomb && currentWeapon.IsAttacking)
                        currentWeapon.Draw(spriteBatch, this);
                    break;
            }


        }

        // Draws the player sprite when moving. Handles all movement conditions
        public void DrawMovement(SpriteBatch sb)
        {
            //set the layer for weapon
            float playerLayer = playerPosition.Y / 650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (playerLayer <= 0)
            {
                playerLayer = 0.01f;
            }

            if (!takingDamage)
            {
                switch (currentPlayerState)
                {
                    case PlayerState.FaceUp:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveUpGun,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveUpGun,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        else
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveUpNormal,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveUpNormal,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        break;
                    case PlayerState.FaceDown:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveDownGun,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveDownGun,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        else
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveDownNormal,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveDownNormal,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        break;
                    case PlayerState.FaceLeft:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveLeftGun,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveLeftGun,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        else
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveLeftNormal,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveLeftNormal,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        break;
                    case PlayerState.FaceRight:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveRightGun,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveRightGun,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        else
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveRightNormal,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveRightNormal,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        break;
                }
            }
            else if (takingDamage)
            {
                switch (currentPlayerState)
                {
                    case PlayerState.FaceUp:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveUpGun,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveUpGun,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        else
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveUpNormal,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveUpNormal,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        break;
                    case PlayerState.FaceDown:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveDownGun,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveDownGun,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        else
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveDownNormal,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveDownNormal,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        break;
                    case PlayerState.FaceLeft:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveLeftGun,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveLeftGun,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        else
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveLeftNormal,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveLeftNormal,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        break;
                    case PlayerState.FaceRight:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveRightGun,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveRightGun,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        else
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveRightNormal,
                                    playerPosition,
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveRightNormal,
                                    playerPosition,
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.Red,
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                        break;
                }
            }
        }

        // Draws the player standing (no movement)
        public void DrawStanding(SpriteBatch sb)
        {
            //set the layer for player
            float playerLayer = playerPosition.Y / 650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (playerLayer <= 0)
            {
                playerLayer = 0.01f;
            }

            if (!takingDamage)
            {
                switch (currentPlayerState)
                {
                    case PlayerState.FaceUp:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            sb.Draw(
                                playerStandingGun,
                                playerPosition,
                                new Rectangle(160, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        else
                        {
                            sb.Draw(
                                playerStandingNormal,
                                playerPosition,
                                new Rectangle(160, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        break;
                    case PlayerState.FaceDown:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            sb.Draw(
                                playerStandingGun,
                                playerPosition,
                                new Rectangle(0, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        else
                        {
                            sb.Draw(
                                playerStandingNormal,
                                playerPosition,
                                new Rectangle(0, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        break;
                    case PlayerState.FaceRight:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            sb.Draw(
                                playerStandingGun,
                                playerPosition,
                                new Rectangle(480, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        else
                        {
                            sb.Draw(
                                playerStandingNormal,
                                playerPosition,
                                new Rectangle(480, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        break;
                    case PlayerState.FaceLeft:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            sb.Draw(
                                playerStandingGun,
                                playerPosition,
                                new Rectangle(320, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        else
                        {
                            sb.Draw(
                                playerStandingNormal,
                                playerPosition,
                                new Rectangle(320, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        break;
                }
            }
            else if (takingDamage)
            {
                switch (currentPlayerState)
                {
                    case PlayerState.FaceUp:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            sb.Draw(
                                playerStandingGun,
                                playerPosition,
                                new Rectangle(160, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        else
                        {
                            sb.Draw(
                                playerStandingNormal,
                                playerPosition,
                                new Rectangle(160, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        break;
                    case PlayerState.FaceDown:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            sb.Draw(
                                playerStandingGun,
                                playerPosition,
                                new Rectangle(0, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        else
                        {
                            sb.Draw(
                                playerStandingNormal,
                                playerPosition,
                                new Rectangle(0, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        break;
                    case PlayerState.FaceRight:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            sb.Draw(
                                playerStandingGun,
                                playerPosition,
                                new Rectangle(480, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        else
                        {
                            sb.Draw(
                                playerStandingNormal,
                                playerPosition,
                                new Rectangle(480, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        break;
                    case PlayerState.FaceLeft:
                        if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Gun)
                        {
                            sb.Draw(
                                playerStandingGun,
                                playerPosition,
                                new Rectangle(320, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        else
                        {
                            sb.Draw(
                                playerStandingNormal,
                                playerPosition,
                                new Rectangle(320, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        }
                        break;
                }
            }
            else if (currentPlayerState == PlayerState.Death)
                sb.Draw(
                    playerStandingGun,
                    playerPosition,
                    new Rectangle(0, 0, 160, 160),
                    Color.Red,
                    0.0f,
                    Vector2.Zero,
                    0.6f,
                    SpriteEffects.None,
                    playerLayer);

        }

        // Draws the player in an "action" - Either melee attack or bomb throw
        public void DrawAction(SpriteBatch sb)
        {
            //set the layer for weapon
            float playerLayer = playerPosition.Y / 650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (playerLayer <= 0)
            {
                playerLayer = 0.01f;
            }

            if (!takingDamage)
            {
                if (currentPlayerState == PlayerState.BombThrow)
                {
                    switch (previousPlayerState)
                    {
                        case PlayerState.FaceUp:
                            sb.Draw(
                                playerBombThrowUp,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceDown:
                            sb.Draw(
                                playerBombThrowDown,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceRight:
                            sb.Draw(
                                playerBombThrowRight,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceLeft:
                            sb.Draw(
                                playerBombThrowLeft,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                    }
                }

                if (currentPlayerState == PlayerState.Melee)
                {
                    switch (previousPlayerState)
                    {
                        case PlayerState.FaceUp:
                            sb.Draw(
                                playerMeleeUp,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceDown:
                            sb.Draw(
                                playerMeleeDown,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceRight:
                            sb.Draw(
                                playerMeleeRight,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceLeft:
                            sb.Draw(
                                playerMeleeLeft,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                    }
                }
            }
            else if (takingDamage)
            {
                if (currentPlayerState == PlayerState.BombThrow)
                {
                    switch (previousPlayerState)
                    {
                        case PlayerState.FaceUp:
                            sb.Draw(
                                playerBombThrowUp,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceDown:
                            sb.Draw(
                                playerBombThrowDown,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceRight:
                            sb.Draw(
                                playerBombThrowRight,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceLeft:
                            sb.Draw(
                                playerBombThrowLeft,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                    }
                }

                if (currentPlayerState == PlayerState.Melee)
                {
                    switch (previousPlayerState)
                    {
                        case PlayerState.FaceUp:
                            sb.Draw(
                                playerMeleeUp,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceDown:
                            sb.Draw(
                                playerMeleeDown,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceRight:
                            sb.Draw(
                                playerMeleeRight,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                        case PlayerState.FaceLeft:
                            sb.Draw(
                                playerMeleeLeft,
                                playerPosition,
                                new Rectangle(160 * actionCycleFrame, 0, 160, 160),
                                Color.Red,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                            break;
                    }
                }
            }
        }

        // Draws the evade sequence. A 5 frame animation. The frame that the evade was started upon is drawn
        // 4 times, as after-images with varying transparency
        public void DrawEvade(SpriteBatch sb)
        {
            //set the layer for weapon
            float playerLayer = playerPosition.Y / 650;

            //make sure the layer is not equal or less than zero as layer will not show
            if (playerLayer <= 0)
            {
                playerLayer = 0.01f;
            }
            switch(previousPlayerState)
            {
                case PlayerState.FaceRight:
                    {
                        if (moveCycleFrame < 4)
                            sb.Draw(
                                playerMoveRightGun,
                                playerPosition,
                                new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        else if (moveCycleFrame >= 4)
                            sb.Draw(
                                playerMoveRightGun,
                                playerPosition,
                                new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);

                        for (int i = 0; i < evadeAfterImages.Count; i++)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveRightGun,
                                    evadeAfterImages[i],
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White * (0.1f + (0.1f * i)),
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveRightGun,
                                    evadeAfterImages[i],
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White * (0.1f + (0.1f * i)),
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                    }
                    break;
                case PlayerState.FaceLeft:
                    {
                        if (moveCycleFrame < 4)
                            sb.Draw(
                                playerMoveLeftGun,
                                playerPosition,
                                new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        else if (moveCycleFrame >= 4)
                            sb.Draw(
                                playerMoveLeftGun,
                                playerPosition,
                                new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);

                        for (int i = 0; i < evadeAfterImages.Count; i++)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveLeftGun,
                                    evadeAfterImages[i],
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White * (0.1f + (0.1f * i)),
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveLeftGun,
                                    evadeAfterImages[i],
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White * (0.1f + (0.1f * i)),
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                    }
                    break;
                case PlayerState.FaceUp:
                    {
                        if (moveCycleFrame < 4)
                            sb.Draw(
                                playerMoveUpGun,
                                playerPosition,
                                new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        else if (moveCycleFrame >= 4)
                            sb.Draw(
                                playerMoveUpGun,
                                playerPosition,
                                new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);

                        for (int i = 0; i < evadeAfterImages.Count; i++)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveUpGun,
                                    evadeAfterImages[i],
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White * (0.1f + (0.1f * i)),
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveUpGun,
                                    evadeAfterImages[i],
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White * (0.1f + (0.1f * i)),
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                    }
                    break;
                case PlayerState.FaceDown:
                    {
                        if (moveCycleFrame < 4)
                            sb.Draw(
                                playerMoveDownGun,
                                playerPosition,
                                new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);
                        else if (moveCycleFrame >= 4)
                            sb.Draw(
                                playerMoveDownGun,
                                playerPosition,
                                new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                Color.White,
                                0.0f,
                                Vector2.Zero,
                                0.6f,
                                SpriteEffects.None,
                                playerLayer);

                        for (int i = 0; i < evadeAfterImages.Count; i++)
                        {
                            if (moveCycleFrame < 4)
                                sb.Draw(
                                    playerMoveDownGun,
                                    evadeAfterImages[i],
                                    new Rectangle(160 * moveCycleFrame, 0, 160, 160),
                                    Color.White * (0.1f + (0.1f * i)),
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                            else if (moveCycleFrame >= 4)
                                sb.Draw(
                                    playerMoveDownGun,
                                    evadeAfterImages[i],
                                    new Rectangle(160 * (moveCycleFrame - 4), 160, 160, 160),
                                    Color.White * (0.1f + (0.1f * i)),
                                    0.0f,
                                    Vector2.Zero,
                                    0.6f,
                                    SpriteEffects.None,
                                    playerLayer);
                        }
                    }
                    break;
            }
        }

        // Draws the UI, indicating the player's health, armor, and two weapons
        public void DrawUI(SpriteBatch sb, Texture2D rectangle, Texture2D displayItemBox, Texture2D weaponSelection1, Texture2D displayItemBoxCooldown,
            Texture2D healthIcon, Texture2D shieldIcon, SpriteFont textFont)
        {
            String scoreString = "" + score;

            Rectangle maxHealthRectangle = new Rectangle(49, 44, maxHealth + 1, 12);
            Rectangle currentHealthRectangle = new Rectangle(49, 45, health, 10);
            Rectangle maxArmorRectangle = new Rectangle(49, 59, maxArmor + 1, 12);
            Rectangle currentArmorRectangle = new Rectangle(49, 60, armor, 10);

            sb.Draw(healthIcon, new Rectangle(35, 44, 12, 12), Color.White);
            sb.Draw(shieldIcon, new Rectangle(35, 59, 12, 12), Color.White);
            sb.Draw(rectangle, maxHealthRectangle, Color.DarkGray);
            sb.Draw(rectangle, maxArmorRectangle, Color.DarkGray);
            sb.Draw(rectangle, currentHealthRectangle, Color.DarkGreen);
            sb.Draw(rectangle, currentArmorRectangle, Color.Blue);
            sb.Draw(displayItemBox, new Rectangle(254, 36, 40, 40), Color.White);
            sb.Draw(displayItemBox, new Rectangle(294, 36, 40, 40), Color.White);
            sb.DrawString(textFont, scoreString, new Vector2(660, 40), Color.MediumAquamarine);
            if (currentWeapon == weapon1)
                sb.Draw(weaponSelection1, new Rectangle(254, 36, 40, 40), Color.White);
            else if (currentWeapon == weapon2)
                sb.Draw(weaponSelection1, new Rectangle(294, 36, 40, 40), Color.White);

            if (weapon1.IsAttacking == false && weapon1.TimeThatHasPassed < weapon1.CooldownTime)
            {
                if (weapon1.TimeThatHasPassed < weapon1.CooldownTime * (.1))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(0, 0, 128, 128), Color.White);
                else if (weapon1.TimeThatHasPassed < weapon1.CooldownTime * (.2))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(128, 0, 128, 128), Color.White);
                else if (weapon1.TimeThatHasPassed < weapon1.CooldownTime * (.3))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(128 * 2, 0, 128, 128), Color.White);
                else if (weapon1.TimeThatHasPassed < weapon1.CooldownTime * (.4))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(128 * 3, 0, 128, 128), Color.White);
                else if (weapon1.TimeThatHasPassed < weapon1.CooldownTime * (.5))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(128 * 4, 0, 128, 128), Color.White);
                else if (weapon1.TimeThatHasPassed < weapon1.CooldownTime * (.6))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(0, 128, 128, 128), Color.White);
                else if (weapon1.TimeThatHasPassed < weapon1.CooldownTime * (.7))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(128, 128, 128, 128), Color.White);
                else if (weapon1.TimeThatHasPassed < weapon1.CooldownTime * (.8))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(128 * 2, 128, 128, 128), Color.White);
                else if (weapon1.TimeThatHasPassed < weapon1.CooldownTime * (.9))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(128 * 3, 128, 128, 128), Color.White);
                else if (weapon1.TimeThatHasPassed < weapon1.CooldownTime)
                    sb.Draw(displayItemBoxCooldown, new Rectangle(254, 36, 40, 40), new Rectangle(128 * 4, 128, 128, 128), Color.White);

            }


            if (weapon2.IsAttacking == false && weapon2.TimeThatHasPassed < weapon2.CooldownTime)
            {
                if (weapon2.TimeThatHasPassed < weapon2.CooldownTime * (.1))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(0, 0, 128, 128), Color.White);
                else if (weapon2.TimeThatHasPassed < weapon2.CooldownTime * (.2))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(128, 0, 128, 128), Color.White);
                else if (weapon2.TimeThatHasPassed < weapon2.CooldownTime * (.3))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(128 * 2, 0, 128, 128), Color.White);
                else if (weapon2.TimeThatHasPassed < weapon2.CooldownTime * (.4))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(128 * 3, 0, 128, 128), Color.White);
                else if (weapon2.TimeThatHasPassed < weapon2.CooldownTime * (.5))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(128 * 4, 0, 128, 128), Color.White);
                else if (weapon2.TimeThatHasPassed < weapon2.CooldownTime * (.6))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(0, 128, 128, 128), Color.White);
                else if (weapon2.TimeThatHasPassed < weapon2.CooldownTime * (.7))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(128, 128, 128, 128), Color.White);
                else if (weapon2.TimeThatHasPassed < weapon2.CooldownTime * (.8))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(128 * 2, 128, 128, 128), Color.White);
                else if (weapon2.TimeThatHasPassed < weapon2.CooldownTime * (.9))
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(128 * 3, 128, 128, 128), Color.White);
                else if (weapon2.TimeThatHasPassed < weapon2.CooldownTime)
                    sb.Draw(displayItemBoxCooldown, new Rectangle(294, 36, 40, 40), new Rectangle(128 * 4, 128, 128, 128), Color.White);

            }


            //draw the weapon icon at the designated location (top left corner of UI box pass in)
            DrawWeaponIcon(sb, weapon1, 254, 36);
            DrawWeaponIcon(sb, weapon2, 294, 36);
        }

        /// <summary>
        /// draws the weapon icon using the specific weapon and top left corner coordinate of UI box
        /// </summary>
        /// <param name="sb">spritebatch used for drawing</param>
        /// <param name="weapon">weapon to be drawn</param>
        /// <param name="baseX">x value of top left corner to draw in</param>
        /// <param name="baseY">y value of top left corner to draw in</param>
        public void DrawWeaponIcon(SpriteBatch sb, Weapons.WeaponAbstract weapon, int baseX, int baseY)
        {
            if (weapon.GetType() == typeof(Weapons.BasicGunWeapon) || weapon.GetType() == typeof(Weapons.ShotgunWeapon))
            {
                sb.Draw(
                weapon.UITexture,
                new Rectangle(baseX-20, baseY-20, 80, 80),
                new Rectangle(0, 0, weapon.UITexture.Width / 4, weapon.UITexture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }
            else if (weapon.GetType() == typeof(Weapons.BasicMeleeWeapon) || weapon.GetType() == typeof(Weapons.FeralClawsWeapon) || weapon.GetType() == typeof(Weapons.StunWandWeapon))
            {
                sb.Draw(
                weapon.UITexture,
                new Rectangle(baseX + 63, baseY + 18, 60, 60),
                new Rectangle(0, 0, weapon.UITexture.Width / 4, weapon.UITexture.Height),
                Color.White,
                2.355f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }
            else if (weapon.GetType() == typeof(Weapons.BasicBombWeapon) || weapon.GetType() == typeof(Weapons.SingularityBombWeapon))
            {
                sb.Draw(
                weapon.UITexture,
                new Rectangle(baseX - 40, baseY - 40, 120, 120),
                new Rectangle(0, 0, weapon.UITexture.Width / 4, weapon.UITexture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }
            else if (weapon.GetType() == typeof(Weapons.DisintegratorRayWeapon))
            {
                sb.Draw(
                weapon.UITexture,
                new Rectangle(baseX-7, baseY+15, 40, 40),
                new Rectangle(0, 0, weapon.UITexture.Width / 4, weapon.UITexture.Height),
                Color.White,
                5.6f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }
            else if (weapon.GetType() == typeof(Weapons.RailgunWeapon))
            {
                sb.Draw(
                weapon.UITexture,
                new Rectangle(baseX-9, baseY-10, 60, 60),
                new Rectangle(0, 0, weapon.UITexture.Width / 4, weapon.UITexture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }
            else if (weapon.GetType() == typeof(Weapons.ClusterBombWeapon))
            {
                sb.Draw(
                weapon.UITexture,
                new Rectangle(baseX-20, baseY-20, 80, 80),
                new Rectangle(0, 0, weapon.UITexture.Width / 4, weapon.UITexture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }
            else if(weapon.GetType() == typeof(Weapons.RocketLauncherWeapon))
            {
                sb.Draw(
                weapon.UITexture,
                new Rectangle(baseX-20, baseY+15, 60, 60),
                new Rectangle(0, 0, weapon.UITexture.Width / 4, weapon.UITexture.Height),
                Color.White,
                5.6f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }
            else if(weapon.GetType() == typeof(Weapons.Secret))
            {
                sb.Draw(
                weapon.UITexture,
                new Rectangle(baseX + 5, baseY + 5, 30, 30),
                new Rectangle(0, 0, weapon.UITexture.Width, weapon.UITexture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }
            else
            {
                sb.Draw(
                weapon.UITexture,
                new Rectangle(baseX, baseY, 40, 40),
                new Rectangle(0, 0, weapon.UITexture.Width / 4, weapon.UITexture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.0f);
            }


        }


        // Moves the player. This essentially alters the player's Vector and Rectangle fields, based on parameters
        public void Move(int xChange, int yChange)
        {
            if (currentPlayerState == PlayerState.FaceUp
                || currentPlayerState == PlayerState.FaceDown
                || currentPlayerState == PlayerState.FaceRight
                || currentPlayerState == PlayerState.FaceLeft
                || currentPlayerState == PlayerState.Evade)
            {
                playerPosition = new Vector2(playerPosition.X + xChange + moveSpeed, playerPosition.Y + yChange + moveSpeed);
                playerHitBox = new Rectangle(playerHitBox.X + xChange + moveSpeed, playerHitBox.Y + yChange + moveSpeed, playerHitBox.Width, playerHitBox.Height);
                if (playerHitBox.X + playerHitBox.Width >= 800)
                {
                    playerPosition = new Vector2(playerPosition.X - xChange - moveSpeed, playerPosition.Y);
                    playerHitBox = new Rectangle(playerHitBox.X - xChange - moveSpeed, playerHitBox.Y, playerHitBox.Width, playerHitBox.Height);
                }
                if (playerHitBox.X <= 0)
                {
                    playerPosition = new Vector2(playerPosition.X - xChange - moveSpeed, playerPosition.Y);
                    playerHitBox = new Rectangle(playerHitBox.X - xChange - moveSpeed, playerHitBox.Y, playerHitBox.Width, playerHitBox.Height);
                }
                if (playerHitBox.Y + playerHitBox.Height >= 650)
                {
                    playerPosition = new Vector2(playerPosition.X, playerPosition.Y - yChange - moveSpeed);
                    playerHitBox = new Rectangle(playerHitBox.X, playerHitBox.Y - yChange - moveSpeed, playerHitBox.Width, playerHitBox.Height);
                }
                if (playerHitBox.Y <= 0)
                {
                    playerPosition = new Vector2(playerPosition.X, playerPosition.Y - yChange - moveSpeed);
                    playerHitBox = new Rectangle(playerHitBox.X, playerHitBox.Y - yChange - moveSpeed, playerHitBox.Width, playerHitBox.Height);
                }
            }
        }

        // Initiates an attack. Calls the complementary attack method for specified weapons
        // Attacks other than the gun result in a different animation, and the player being locked in the animation
        public void Attack()
        {

            currentWeapon.Attack();

            if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Melee)
            {
                if (currentWeapon.IsAttacking == true)
                {
                    previousPlayerState = currentPlayerState;
                    currentPlayerState = PlayerState.Melee;
                }
            }
            if (currentWeapon.WeaponType == Weapons.TypeOfWeapon.Bomb && currentWeapon.TimeThatHasPassed < 0.3)
            {
                if (currentWeapon.IsAttacking == true)
                {
                    previousPlayerState = currentPlayerState;
                    currentPlayerState = PlayerState.BombThrow;
                }
            }
        }

        // Changes the player's weapon
        public void CycleWeapon()
        {
            if (currentWeapon == weapon1)
            {
                currentWeapon = weapon2;
                storedWeapon = weapon1;
            }
            else if (currentWeapon == weapon2)
            {
                currentWeapon = weapon1;
                storedWeapon = weapon2;
            }
        }

        // Initiates an evade. Anything can be cancelled into an evade
        private void Evade(int currentXChange, int currentYChange)
        {
            if(currentPlayerState == PlayerState.Evade)
            {
                if(currentXChange == 4 && currentYChange == 0)
                {
                    if (addAfterImage)
                        evadeAfterImages.Add(playerPosition);
                    Move(5, 0);
                    if (addAfterImage == false)
                        addAfterImage = true;
                    else
                        addAfterImage = false;
                }
                else if (currentXChange == -4 && currentYChange == 0)
                {
                    if (addAfterImage)
                        evadeAfterImages.Add(playerPosition);
                    Move(-5, 0);
                    if (addAfterImage == false)
                        addAfterImage = true;
                    else
                        addAfterImage = false;
                }
                else if (currentXChange == 0 && currentYChange == 4)
                {
                    if (addAfterImage)
                        evadeAfterImages.Add(playerPosition);
                    Move(0, 5);
                    if (addAfterImage == false)
                        addAfterImage = true;
                    else
                        addAfterImage = false;
                }
                else if (currentXChange == 0 && currentYChange == -4)
                {
                    if (addAfterImage)
                        evadeAfterImages.Add(playerPosition);
                    Move(0, -5);
                    if (addAfterImage == false)
                        addAfterImage = true;
                    else
                        addAfterImage = false;
                }
                else if (currentXChange == 4 && currentYChange == 4)
                {
                    if (addAfterImage)
                        evadeAfterImages.Add(playerPosition);
                    Move(5, 5);
                    if (addAfterImage == false)
                        addAfterImage = true;
                    else
                        addAfterImage = false;
                }
                else if (currentXChange == 4 && currentYChange == -4)
                {
                    if (addAfterImage)
                        evadeAfterImages.Add(playerPosition);
                    Move(5, -5);
                    if (addAfterImage == false)
                        addAfterImage = true;
                    else
                        addAfterImage = false;
                }
                else if (currentXChange == -4 && currentYChange == 4)
                {
                    if (addAfterImage)
                        evadeAfterImages.Add(playerPosition);
                    Move(-5, 5);
                    if (addAfterImage == false)
                        addAfterImage = true;
                    else
                        addAfterImage = false;
                }
                else if (currentXChange == -4 && currentYChange == -4)
                {
                    if (addAfterImage)
                        evadeAfterImages.Add(playerPosition);
                    Move(-5, -5);
                    if (addAfterImage == false)
                        addAfterImage = true;
                    else
                        addAfterImage = false;
                }
            }
        }

        // Damages the player based on a passed value
        public void TakeDamage(int damage)
        {
            if (hitBoxActive)
            {
                if (armor > 0)
                {
                    armor -= damage;
                    totalDamageTaken += damage;
                    takingDamage = true;
                }
                else if (health > 0)
                {
                    health -= damage;
                    totalDamageTaken += damage;
                    takingDamage = true;
                }
            }
        }

        // Essentially resets the player. Does what the constructor does
        // This is what is called when the players wishes to play again
        public void ResetPlayer()
        {
            health = 200;
            totalDamageTaken = 0;
            armor = 0;
            score = 0;
            moveSpeed = 0;

            takingDamage = false;
            damageTimer = 0;

            armorTimer = 10.0;
            initialScore = 0;

            weapon1 = null;
            weapon2 = null;
            currentWeapon = null;

            currentPlayerState = PlayerState.FaceDown;
            previousPlayerState = PlayerState.FaceUp;

            moveCycleFrame = 0;
            actionCycleFrame = 0;
        }
    }
}
