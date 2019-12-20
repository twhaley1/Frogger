using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using FroggerStarter.EventArgs;
using FroggerStarter.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FroggerStarter.View.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HighScorePage
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScorePage" /> class.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        public HighScorePage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Invoked when the Page is loaded and becomes the current source of a parent Frame.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="e">
        ///     Event data that can be examined by overriding code. The event data is representative of the pending
        ///     navigation that will load the current Page. Usually the most relevant property to examine is Parameter.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is HighScoreNavigationToEventArgs highScoreEventArgs)
            {
                DataContext = new HighScoreViewModel(highScoreEventArgs.HighScores);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StartPage));
        }

        #endregion
    }
}