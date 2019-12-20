using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using FroggerStarter.Controller;
using FroggerStarter.EventArgs;
using FroggerStarter.Model;
using FroggerStarter.Utilities;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FroggerStarter.View.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Data members

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private readonly double highRoadYLocation = (double) Application.Current.Resources["HighRoadYLocation"];
        private readonly GameManager gameManager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePage" /> class.
        /// </summary>
        public GamePage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size
                {Width = this.applicationWidth, Height = this.applicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView()
                           .SetPreferredMinSize(new Size(this.applicationWidth, this.applicationHeight));

            Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;
            this.gameManager = new GameManager(this.applicationHeight, this.applicationWidth, this.highRoadYLocation);
            this.gameManager.InitializeGame(this.canvas);

            this.updateLivesTextBlock(this.gameManager.Lives);
            this.updateScoreTextBlock(this.gameManager.Score);

            this.subscribeToEvents();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Invoked when the Page is loaded and becomes the current source of a parent Frame.
        ///     Precondition: None
        ///     Postcondition: this.gameManager.IsHardcore either false or true depending on button clicked on startpage.
        /// </summary>
        /// <param name="e">
        ///     Event data that can be examined by overriding code. The event data is representative of the pending
        ///     navigation that will load the current Page. Usually the most relevant property to examine is Parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is IsHardcoreEventArgs hardcoreEventArgs)
            {
                this.gameManager.IsHardcore = hardcoreEventArgs.IsHardcore;
                this.updateLivesTextBlock(this.gameManager.Lives);
            }
        }

        private void subscribeToEvents()
        {
            this.gameManager.UpdateLives += this.updateLives;
            this.gameManager.UpdateScore += this.updateScore;
            this.gameManager.UpdateLifeCountdown += this.updateLifeCountdown;
            this.gameManager.GameOver += this.gameOver;
            this.gameManager.WaterLevelStarted += this.gameManagerOnWaterLevelStarted;
            this.gameManager.WaterLevelEnded += this.gameManagerOnWaterLevelEnded;
            this.gameManager.InvulnCountdownStarted += this.invulnCountdownStarted;
            this.gameManager.InvulnCountdownEnded += this.invulnCountdownEnded;
            this.gameManager.InvulnCountdownUpdated += this.invulnCountdownUpdated;
        }

        private void gameManagerOnWaterLevelStarted(object sender, System.EventArgs e)
        {
            this.waterLevelBottomShoulder.Visibility = Visibility.Visible;
            this.waterLevelTopShoulder.Visibility = Visibility.Visible;
            this.waterLevelRiver.Visibility = Visibility.Visible;
        }

        private void gameManagerOnWaterLevelEnded(object sender, System.EventArgs e)
        {
            this.waterLevelBottomShoulder.Visibility = Visibility.Collapsed;
            this.waterLevelTopShoulder.Visibility = Visibility.Collapsed;
            this.waterLevelRiver.Visibility = Visibility.Collapsed;
        }

        private void updateScoreTextBlock(int score)
        {
            this.scoreTextBlock.Text = score.ToString();
        }

        private void updateLivesTextBlock(int lives)
        {
            this.livesTextBlock.Text = lives.ToString();
        }

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.gameManager.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    this.gameManager.MovePlayerRight();
                    break;
                case VirtualKey.Up:
                    this.gameManager.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    this.gameManager.MovePlayerDown();
                    break;
            }
        }

        private void updateLives(object sender, UpdateLivesEventArgs e)
        {
            this.updateLivesTextBlock(e.Lives);
        }

        private void updateScore(object sender, UpdateScoreEventArgs e)
        {
            this.updateScoreTextBlock(e.Score);
        }

        private void updateLifeCountdown(object sender, UpdateLifeCountdownEventArgs e)
        {
            this.lifeScoreProgressBar.Value = GameSettings.LifeCountdownInSeconds - e.TimesTicked;
            if ((int) this.lifeScoreProgressBar.Value == 0)
            {
                this.lifeScoreProgressBar.Value = GameSettings.LifeCountdownInSeconds;
            }
        }

        private async void gameOver(object sender, GameOverEventArgs e)
        {
            if (e.IsGameOver)
            {
                Window.Current.CoreWindow.KeyDown -= this.coreWindowOnKeyDown;
                this.gameOverTextBlock.Visibility = Visibility.Visible;
                await Pause.Milliseconds(1500);

                var highScoreData = new PlayAgainNavigationToEventArgs
                    {Level = this.gameManager.CurrentLevel, Score = int.Parse(this.scoreTextBlock.Text)};
                Frame.Navigate(typeof(PlayAgainPage), highScoreData);
            }
        }

        private void invulnCountdownStarted(object sender, System.EventArgs e)
        {
            this.invulnProgressBar.Visibility = Visibility.Visible;
        }

        private void invulnCountdownEnded(object sender, System.EventArgs e)
        {
            this.invulnProgressBar.Visibility = Visibility.Collapsed;
        }

        private void invulnCountdownUpdated(object sender, UpdateInvulnCountdownEventArgs e)
        {
            this.invulnProgressBar.Value = GameSettings.InvulnTimeInSeconds - e.TimesTicked;
            if ((int) this.invulnProgressBar.Value == 0)
            {
                this.invulnProgressBar.Value = GameSettings.InvulnTimeInSeconds;
            }
        }

        #endregion
    }
}