using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using FroggerStarter.EventArgs;
using FroggerStarter.Model.HighScore;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FroggerStarter.View.Pages
{
    /// <summary>
    ///     Occurs when the player completes the game.
    /// </summary>
    public sealed partial class PlayAgainPage
    {
        #region Data members

        private int level;
        private int score;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayAgainPage" /> class.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        public PlayAgainPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Invoked when the Page is loaded and becomes the current source of a parent Frame.
        ///     Precondition: None
        ///     Postcondition: data members and text blocks for score and level updated
        /// </summary>
        /// <param name="e">
        ///     Event data that can be examined by overriding code. The event data is representative of the pending
        ///     navigation that will load the current Page. Usually the most relevant property to examine is Parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is PlayAgainNavigationToEventArgs highScoreData)
            {
                this.level = highScoreData.Level;
                this.score = highScoreData.Score;

                this.updateDataTextBlocks();
            }
        }

        private void YesPlayAgain_Click(object sender, RoutedEventArgs e)
        {
            this.addToHighScoreFile();
            Frame.Navigate(typeof(GamePage));
        }

        private void NoPlayAgain_Click(object sender, RoutedEventArgs e)
        {
            this.addToHighScoreFile();
            CoreApplication.Exit();
        }

        private async void ViewHighScoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.addToHighScoreFile();

            var highScoreSerializer = new HighScoreSerializer("highScoreBoard.bin");
            var highScoreFileContent = new HighScoreNavigationToEventArgs
                {HighScores = await highScoreSerializer.DeserializeHighScores()};

            Frame.Navigate(typeof(HighScorePage), highScoreFileContent);
        }

        private void updateDataTextBlocks()
        {
            this.levelTextBlock.Text += this.level.ToString();
            this.scoreTextBlock.Text += this.score.ToString();
        }

        private void addToHighScoreFile()
        {
            var nameFormatted = this.nameTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(nameFormatted))
            {
                var highScoreSerializer = new HighScoreSerializer("highScoreBoard.bin");

                highScoreSerializer.SerializeHighScore(new HighScoreData {
                    Score = int.Parse(this.scoreTextBlock.Text.Split(" ")[1]),
                    Name = nameFormatted,
                    Level = int.Parse(this.levelTextBlock.Text.Split(" ")[1])
                });
            }
        }

        #endregion
    }
}