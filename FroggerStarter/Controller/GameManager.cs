using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Collisions;
using FroggerStarter.Enums;
using FroggerStarter.EventArgs;
using FroggerStarter.Model;
using FroggerStarter.Model.Factories;
using FroggerStarter.Model.GameObjects;
using FroggerStarter.Model.GameObjects.PowerUps;
using FroggerStarter.Sound;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the player,
    ///     the vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {
        #region Data members

        /// <summary>
        ///     The background width.
        /// </summary>
        public static double BackgroundWidth;

        /// <summary>
        ///     The bottom lane offset
        /// </summary>
        public static int BottomLaneOffset = 5;

        private readonly double highRoadYLocation;
        private readonly double backgroundHeight;

        private int timesGameTicked;
        private int timesCountdownTicked;
        private bool isAddVehicleAllowed;

        private Dictionary<Enums.Sound, SoundEffect> sounds;
        private readonly ICollection<BaseSprite> objectsOnCanvas;

        private Canvas gameCanvas;
        private Frog player;
        private Level level;
        private HomeRow homeRow;
        private PowerUpManager powerUps;
        private DispatcherTimer gameTimer;
        private DispatcherTimer lifeCountdownTimer;

        private bool isHardcore;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the current level.
        /// </summary>
        /// <value>
        ///     The current level.
        /// </value>
        public int CurrentLevel => this.level.Id + 1;

        /// <summary>
        ///     Gets the lives.
        /// </summary>
        /// <value>
        ///     The lives.
        /// </value>
        public int Lives => this.player.Lives;

        /// <summary>
        ///     Gets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score => this.player.Score;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is game over.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is game over; otherwise, <c>false</c>.
        /// </value>
        public bool IsGameOver { get; set; }

        private bool IsLastLevel => this.level.Id == GameSettings.MaxLevels - 1;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is hardcore.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is hardcore; otherwise, <c>false</c>.
        /// </value>
        public bool IsHardcore
        {
            get => this.isHardcore;
            set
            {
                this.isHardcore = value;
                this.player.Lives = this.isHardcore ? GameSettings.HardcoreMaximumLives : GameSettings.BaseMaximumLives;
                this.onSetLives();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        ///     Preconditions: backgroundHeight &gt; 0
        ///     backgroundWidth &gt; 0
        ///     highRoadYLocation &gt;= 0
        ///     Postcondition: this.backgroundHeight = backgroundHeight
        ///     this.backgroundWidth = backgroundWidth
        ///     this.highRoadYLocation = highRoadYLocation
        ///     this.timesGameTicked == 0
        ///     this.timesCountdownTicked == 0
        ///     this.isAddVehicleAllowed == false
        /// </summary>
        /// <param name="backgroundHeight">Height of the background.</param>
        /// <param name="backgroundWidth">Width of the background.</param>
        /// <param name="highRoadYLocation">Y coordinate for the highest lane on the level.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     backgroundHeight &lt;= 0
        ///     or
        ///     BackgroundWidth &lt;= 0
        ///     or
        ///     highRoadYLocation &lt; 0
        /// </exception>
        public GameManager(double backgroundHeight, double backgroundWidth, double highRoadYLocation)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            if (highRoadYLocation < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(highRoadYLocation));
            }

            this.loadSounds();

            this.objectsOnCanvas = new List<BaseSprite>();
            this.backgroundHeight = backgroundHeight;
            BackgroundWidth = backgroundWidth;
            this.highRoadYLocation = highRoadYLocation;

            this.timesGameTicked = 0;
            this.timesCountdownTicked = 0;
            this.isAddVehicleAllowed = false;

            this.setupGameTimer();
            this.setupLifeCountdownTimer();
        }

        #endregion

        #region Initialization

        /// <summary>
        ///     Initializes the game working with appropriate classes to play frog
        ///     and vehicle on game screen.
        ///     Precondition: gamePage != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="gamePage">The game page.</param>
        /// <exception cref="ArgumentNullException">gameCanvas</exception>
        public void InitializeGame(Canvas gamePage)
        {
            this.gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            this.createAndPlacePlayer();
            this.createLevel(0);
            this.createAndPlaceHomes();
            this.createAndPlacePowerUps();
            this.setAnimationEventSubscriptions();
            this.initializeCollisionObjects();
            this.subscribeToEvents();
        }

        private void initializeCollisionObjects()
        {
            this.initializeObstacleCollisionObjects();
            this.initializeHomeCollisionObjects();
            this.initializePowerUpCollisionObjects();
        }

        private void initializeObstacleCollisionObjects()
        {
            if (!this.level.IsWaterLevel)
            {
                foreach (var obstacle in this.level)
                {
                    obstacle.Collision = this.setupObstacleCollision();
                }
            }
        }

        private void initializeHomeCollisionObjects()
        {
            foreach (var home in this.homeRow)
            {
                home.Collision = this.setupHomeCollision();
            }
        }

        private void initializePowerUpCollisionObjects()
        {
            foreach (var powerUp in this.powerUps)
            {
                powerUp.Collision = this.setupPowerUpCollision();
            }
        }

        private void addToCanvas(BaseSprite sprite)
        {
            this.gameCanvas.Children.Add(sprite);
            this.objectsOnCanvas.Add(sprite);
        }

        private void advanceLevel()
        {
            this.clearCanvas();
            this.player.ClearInvulnerabilityEffect();
            var nextLevelId = this.level.Id + 1;

            if (GameSettings.WaterLevelIds.Contains(nextLevelId))
            {
                this.onWaterLevelStarted();
            }

            this.placeAllOnCanvas(nextLevelId);
        }

        private void clearCanvas()
        {
            foreach (var sprite in this.objectsOnCanvas)
            {
                this.gameCanvas.Children.Remove(sprite);
            }

            this.objectsOnCanvas.Clear();
        }

        private void placeAllOnCanvas(int difficulty)
        {
            this.placePlayer();
            this.createLevel(difficulty);
            this.placeHomes();
            this.placePowerUps();
            this.initializeCollisionObjects();
        }

        private void createAndPlacePlayer()
        {
            this.player = new Frog(GameSettings.BaseMaximumLives);
            this.placePlayer();
        }

        private void placePlayer()
        {
            this.addToCanvas(this.player.Sprite);
            this.player.ForEachAnimationSprite(this.addToCanvas);
            this.player.ForEachAnimationSprite(sprite => sprite.Visibility = Visibility.Collapsed);
            this.setPlayerToCenterOfBottomLane();
        }

        private void setPlayerToCenterOfBottomLane()
        {
            this.player.X = BackgroundWidth / 2 - this.player.Width / 2;
            this.player.Y = this.backgroundHeight - this.player.Height - BottomLaneOffset;
        }

        private void createLevel(int difficulty)
        {
            var roadYArea = this.backgroundHeight - (this.highRoadYLocation + BottomLaneOffset);
            var startingY = this.backgroundHeight - BottomLaneOffset;

            this.level = LevelFactory.CreateLevel(roadYArea, startingY, difficulty);
            this.placeObstacles();
        }

        private void placeObstacles()
        {
            this.level.ToList().ForEach(vehicle => this.addToCanvas(vehicle.Sprite));
        }

        private void createAndPlaceHomes()
        {
            this.homeRow = new HomeRow(this.highRoadYLocation, this.player.SpeedX, GameSettings.Homes);
            this.placeHomes();
        }

        private void placeHomes()
        {
            this.homeRow.ToList().ForEach(home => home.AllSprites.ToList().ForEach(this.addToCanvas));
        }

        private void createAndPlacePowerUps()
        {
            var powerUps = new List<PowerUp> {
                new BonusTimePowerUp(this.addToCountdownTimer),
                new InvulnerabilityPowerUp(this.givePlayerInvulnerability)
            };
            this.powerUps = new PowerUpManager(powerUps);
            this.placePowerUps();
        }

        private void placePowerUps()
        {
            this.powerUps.ResetAllPowerUps();
            this.powerUps.ToList().ForEach(powerUp => this.addToCanvas(powerUp.Sprite));
        }

        private void loadSounds()
        {
            this.sounds = new Dictionary<Enums.Sound, SoundEffect> {
                {Enums.Sound.CompleteLevel, new SoundEffect("LevelCompleteSound.wav")},
                {Enums.Sound.FallInWater, new SoundEffect("SplashSound.wav")},
                {Enums.Sound.GameOver, new SoundEffect("GameOverSound.wav")},
                {Enums.Sound.ObstacleCollision, new SoundEffect("VehicleCollisionSound.wav")},
                {Enums.Sound.PlayerAtHome, new SoundEffect("FrogHomeSound.wav")},
                {Enums.Sound.TimerComplete, new SoundEffect("TimerDeathSound.wav")},
                {Enums.Sound.WallCollision, new SoundEffect("WallBumpSound.wav")},
                {Enums.Sound.PowerUp, new SoundEffect("PowerUpSound.wav")}
            };
        }

        private void subscribeToEvents()
        {
            this.player.InvulnCountdownStarted += this.playerOnInvulnCountdownStarted;
            this.player.InvulnCountDownEnded += this.playerOnInvulnCountDownEnded;
            this.player.UpdateInvulnCountdown += this.playerOnUpdateInvulnCountdown;
        }

        #endregion

        #region Timers

        private void setupGameTimer()
        {
            this.gameTimer = new DispatcherTimer();
            this.gameTimer.Tick += this.gameTimerOnTick;
            this.gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.gameTimer.Start();
        }

        private void gameTimerOnTick(object sender, object e)
        {
            this.timesGameTicked++;
            this.level.MoveAllObstacles();
            this.doAllCollisionDetection();
            this.checkAllHomesFull();

            if (!this.level.IsWaterLevel)
            {
                this.periodicallyAddVehicles();
            }
        }

        private void setupLifeCountdownTimer()
        {
            this.lifeCountdownTimer = new DispatcherTimer();
            this.lifeCountdownTimer.Tick += this.lifeCountdownTimerOnTick;
            this.lifeCountdownTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            this.lifeCountdownTimer.Start();
        }

        private void lifeCountdownTimerOnTick(object sender, object e)
        {
            this.timesCountdownTicked++;
            this.powerUps.RandomlyAddPowerUps(this.timesCountdownTicked);
            this.updateLifeCountdown();
            this.checkForCountdownExpire();
        }

        private void resetLifeCountdownTimer()
        {
            this.lifeCountdownTimer.Stop();
            this.timesCountdownTicked = 0;
            this.updateLifeCountdown();
            this.lifeCountdownTimer.Start();
        }

        #endregion

        #region Collision

        private void doAllCollisionDetection()
        {
            var waterCollision = this.setupWaterCollision();

            this.doAllObjectCollisionDetection();
            this.checkHomeRowCollision();

            waterCollision.ProcessCollision();
        }

        private void doAllObjectCollisionDetection()
        {
            var allColliderObjects =
                this.level.VisibleObstacles.Cast<GameObject>().Union(this.homeRow).Union(this.powerUps);
            foreach (var gameObject in allColliderObjects)
            {
                if (gameObject.IsCollidingWith(this.player) && !this.IsGameOver)
                {
                    gameObject.Collision.ProcessCollision();
                }
            }
        }

        private bool isPlayerInLanes()
        {
            return this.player.Y < this.backgroundHeight - this.player.Height - BottomLaneOffset;
        }

        private void checkHomeRowCollision()
        {
            if (this.player.Y <= this.highRoadYLocation && !this.player.Animations[AnimationType.Death].IsAnimating)
            {
                this.sounds[Enums.Sound.WallCollision].Play();
                this.handlePlayerDeath();
            }
        }

        private Collision setupObstacleCollision()
        {
            return new Collision {
                DeterminingFactors = new Collection<Collision.NoParamPredicate> {
                    () => !this.player.IsInvulnerable
                },
                OnCollision = () =>
                {
                    this.sounds[Enums.Sound.ObstacleCollision].Play();
                    this.handlePlayerDeath();
                }
            };
        }

        private Collision setupHomeCollision()
        {
            return new Collision {
                OnCollision = () =>
                {
                    this.sounds[Enums.Sound.PlayerAtHome].Play();
                    this.increaseScore();
                    this.setPlayerToCenterOfBottomLane();
                }
            };
        }

        private Collision setupPowerUpCollision()
        {
            return new Collision {
                OnCollision = () => this.sounds[Enums.Sound.PowerUp].Play()
            };
        }

        private Collision setupWaterCollision()
        {
            return new Collision {
                DeterminingFactors = new Collection<Collision.NoParamPredicate> {
                    this.isPlayerInLanes,
                    () => !this.level.IsPlayerOnPlatform(),
                    () => !this.player.Animations[AnimationType.Death].IsAnimating,
                    () => !this.IsGameOver,
                    () => !this.player.IsInvulnerable,
                    () => this.level.IsWaterLevel
                },
                OnCollision = () =>
                {
                    this.sounds[Enums.Sound.FallInWater].Play();
                    this.handlePlayerDeath();
                }
            };
        }

        #endregion

        #region Helpers

        private void checkForCountdownExpire()
        {
            if (this.isTimeOut())
            {
                this.sounds[Enums.Sound.TimerComplete].Play();
                this.handlePlayerDeath();
            }
        }

        private bool isTimeOut()
        {
            return this.timesCountdownTicked == GameSettings.LifeCountdownInSeconds &&
                   !this.player.Animations[AnimationType.Death].IsAnimating;
        }

        private void handlePlayerDeath()
        {
            this.removePlatformRiders();

            this.lifeCountdownTimer.Stop();
            this.player.ClearInvulnerabilityEffect();
            this.loseLife();
            this.player.Animations[AnimationType.Death].DoAnimation();
        }

        private void removePlatformRiders()
        {
            foreach (var obstacle in this.level)
            {
                if (obstacle is Platform platform)
                {
                    platform.Clear();
                }
            }
        }

        private void checkAllHomesFull()
        {
            if (this.homeRow.AreAllHomesFull)
            {
                if (this.IsLastLevel)
                {
                    this.endGameAndDisplayGameOver();
                }
                else
                {
                    this.completeLevel();
                }
            }
        }

        private void completeLevel()
        {
            if (this.level.IsWaterLevel)
            {
                this.onWaterLevelEnded();
            }

            this.sounds[Enums.Sound.CompleteLevel].Play();
            this.homeRow.ClearAll();
            this.advanceLevel();
        }

        private void periodicallyAddVehicles()
        {
            if (this.timesGameTicked % GameSettings.ObstacleAddFrequency == 0 || this.isAddVehicleAllowed)
            {
                this.isAddVehicleAllowed = true;
                var vehiclesToAdd = this.level.FindAllInvisibleObstaclesOutsideLaneBoundaries().ToList();
                vehiclesToAdd.ForEach(vehicle => vehicle.Sprite.Visibility = Visibility.Visible);
                this.isAddVehicleAllowed = vehiclesToAdd.Count == 0;
            }
        }

        private void addToCountdownTimer()
        {
            this.timesCountdownTicked -= GameSettings.BonusTimePowerUpSecondsToAdd;
            if (this.timesCountdownTicked < 0)
            {
                this.timesCountdownTicked = 0;
            }

            this.updateLifeCountdown();
        }

        private void givePlayerInvulnerability()
        {
            this.player.IsInvulnerable = true;
        }

        private void endGameAndDisplayGameOver()
        {
            this.gameTimer.Stop();
            this.lifeCountdownTimer.Stop();
            this.player.StopMovement();
            this.player.ClearInvulnerabilityEffect();
            this.player.Sprite.Visibility = Visibility.Collapsed;
            this.endGame();
        }

        #endregion

        #region PlayerMovement

        /// <summary>
        ///     Moves the player to the left accounting for boundaries.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev - player.Width
        ///     if player.X is at boundary, then player.X == player.X
        /// </summary>
        public void MovePlayerLeft()
        {
            if (!this.player.MoveLeftWithBoundaryCheck(0))
            {
                this.sounds[Enums.Sound.WallCollision].Play();
            }
        }

        /// <summary>
        ///     Moves the player to the right accounting for boundaries.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev + player.Width
        ///     if player.X is at boundary, then player.X == player.X
        /// </summary>
        public void MovePlayerRight()
        {
            if (!this.player.MoveRightWithBoundaryCheck(0))
            {
                this.sounds[Enums.Sound.WallCollision].Play();
            }
        }

        /// <summary>
        ///     Moves the player up accounting for boundaries.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev - player.Height
        ///     if player.Y is at boundary, then player.Y == player.Y
        /// </summary>
        public void MovePlayerUp()
        {
            this.player.MoveUpWithBoundaryCheck(this.highRoadYLocation);
        }

        /// <summary>
        ///     Moves the player down accounting for boundaries.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev + player.Height
        ///     if player.Y is at boundary, then player.Y == player.Y
        /// </summary>
        public void MovePlayerDown()
        {
            if (!this.player.MoveDownWithBoundaryCheck(this.backgroundHeight - BottomLaneOffset))
            {
                this.sounds[Enums.Sound.WallCollision].Play();
            }
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when [update lives].
        /// </summary>
        public event EventHandler<UpdateLivesEventArgs> UpdateLives;

        /// <summary>
        ///     Occurs when [update score].
        /// </summary>
        public event EventHandler<UpdateScoreEventArgs> UpdateScore;

        /// <summary>
        ///     Occurs when [update life countdown].
        /// </summary>
        public event EventHandler<UpdateLifeCountdownEventArgs> UpdateLifeCountdown;

        /// <summary>
        ///     Occurs when [game over].
        /// </summary>
        public event EventHandler<GameOverEventArgs> GameOver;

        /// <summary>
        ///     Occurs when [water level started].
        /// </summary>
        public event EventHandler WaterLevelStarted;

        /// <summary>
        ///     Occurs when [water level ended].
        /// </summary>
        public event EventHandler WaterLevelEnded;

        /// <summary>
        ///     Occurs when [invuln countdown started].
        /// </summary>
        public event EventHandler InvulnCountdownStarted;

        /// <summary>
        ///     Occurs when [invuln countdown ended].
        /// </summary>
        public event EventHandler InvulnCountdownEnded;

        /// <summary>
        ///     Occurs when [update invuln countdown event arguments].
        /// </summary>
        public event EventHandler<UpdateInvulnCountdownEventArgs> InvulnCountdownUpdated;

        private void loseLife()
        {
            this.player.LoseLife();
            this.onSetLives();
        }

        private void onSetLives()
        {
            var loseLifeEvent = new UpdateLivesEventArgs {Lives = this.player.Lives};
            this.UpdateLives?.Invoke(this, loseLifeEvent);
        }

        private void increaseScore()
        {
            var timeRemaining = GameSettings.LifeCountdownInSeconds - this.timesCountdownTicked;
            this.player.AddScore(timeRemaining,
                this.IsHardcore ? GameSettings.HardcoreScoreMultiplier : GameSettings.BaseScoreMultiplier);
            var updateScoreEvent = new UpdateScoreEventArgs {Score = this.player.Score};
            this.UpdateScore?.Invoke(this, updateScoreEvent);
            this.resetLifeCountdownTimer();
        }

        private void updateLifeCountdown()
        {
            var updateLifeCountdownEvent = new UpdateLifeCountdownEventArgs {TimesTicked = this.timesCountdownTicked};
            this.UpdateLifeCountdown?.Invoke(this, updateLifeCountdownEvent);
        }

        private void endGame()
        {
            this.IsGameOver = true;
            this.sounds[Enums.Sound.GameOver].Play();
            var endGameEvent = new GameOverEventArgs {IsGameOver = this.IsGameOver};
            this.GameOver?.Invoke(this, endGameEvent);
        }

        private void setAnimationEventSubscriptions()
        {
            this.player.Animations[AnimationType.Death].AnimationStarted += this.onAnimationStarted;
            this.player.Animations[AnimationType.Death].AnimationEnded += this.onAnimationEnded;
        }

        private void onAnimationEnded(object sender, System.EventArgs e)
        {
            if (this.player.Lives != 0)
            {
                this.level.ResetLanes();
                this.setPlayerToCenterOfBottomLane();
                this.player.StartMovement();
                this.resetLifeCountdownTimer();
            }
            else
            {
                this.endGameAndDisplayGameOver();
            }
        }

        private void onAnimationStarted(object sender, System.EventArgs e)
        {
            this.player.StopMovement();
        }

        private void onWaterLevelStarted()
        {
            this.WaterLevelStarted?.Invoke(this, System.EventArgs.Empty);
        }

        private void onWaterLevelEnded()
        {
            this.WaterLevelEnded?.Invoke(this, System.EventArgs.Empty);
        }

        private void playerOnUpdateInvulnCountdown(object sender, UpdateInvulnCountdownEventArgs e)
        {
            var updateCountdownEvent = new UpdateInvulnCountdownEventArgs {TimesTicked = e.TimesTicked};
            this.InvulnCountdownUpdated?.Invoke(this, updateCountdownEvent);
        }

        private void playerOnInvulnCountDownEnded(object sender, System.EventArgs e)
        {
            this.InvulnCountdownEnded?.Invoke(this, System.EventArgs.Empty);
        }

        private void playerOnInvulnCountdownStarted(object sender, System.EventArgs e)
        {
            this.InvulnCountdownStarted?.Invoke(this, System.EventArgs.Empty);
        }

        #endregion
    }
}