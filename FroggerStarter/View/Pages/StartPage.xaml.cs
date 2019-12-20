using Windows.UI.Xaml;
using FroggerStarter.EventArgs;
using FroggerStarter.Model.HighScore;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FroggerStarter.View.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="StartPage" /> class.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        public StartPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var isHardcore = new IsHardcoreEventArgs {IsHardcore = false};
            Frame.Navigate(typeof(GamePage), isHardcore);
        }

        private void HardcorePlayButton_Click(object sender, RoutedEventArgs e)
        {
            var isHardcore = new IsHardcoreEventArgs {IsHardcore = true};
            Frame.Navigate(typeof(GamePage), isHardcore);
        }

        private async void ViewHighScoreButton_Click(object sender, RoutedEventArgs e)
        {
            var highScoreSerializer = new HighScoreSerializer("highScoreBoard.bin");

            var highScoreFileContent = new HighScoreNavigationToEventArgs
                {HighScores = await highScoreSerializer.DeserializeHighScores()};

            Frame.Navigate(typeof(HighScorePage), highScoreFileContent);
        }

        private void ClearHighScoreButton_Click(object sender, RoutedEventArgs e)
        {
            var highScoreSerializer = new HighScoreSerializer("highScoreBoard.bin");
            highScoreSerializer.Clear();
        }

        #endregion
    }
}