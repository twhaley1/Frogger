using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FroggerStarter.Annotations;
using FroggerStarter.Extensions;
using FroggerStarter.Model.HighScore;
using FroggerStarter.Utilities;

// ReSharper disable InconsistentNaming

namespace FroggerStarter.ViewModel
{
    /// <summary>
    ///     Binds data between the HighScoreBoard and the model.
    /// </summary>
    public class HighScoreViewModel : INotifyPropertyChanged
    {
        #region Data members

        private const int NumberOfDisplayedHighScores = 10;

        private readonly HighScores highScores;

        private ObservableCollection<HighScoreData> highScoresObservableCollection;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the score name level sort command.
        /// </summary>
        /// <value>
        ///     The score name level sort command.
        /// </value>
        public RelayCommand ScoreNameLevelSortCommand { get; set; }

        /// <summary>
        ///     Gets or sets the name score level sort command.
        /// </summary>
        /// <value>
        ///     The name score level sort command.
        /// </value>
        public RelayCommand NameScoreLevelSortCommand { get; set; }

        /// <summary>
        ///     Gets or sets the level score name sort command.
        /// </summary>
        /// <value>
        ///     The level score name sort command.
        /// </value>
        public RelayCommand LevelScoreNameSortCommand { get; set; }

        /// <summary>
        ///     Gets or sets the high scores observable collection.
        /// </summary>
        /// <value>
        ///     The high scores observable collection.
        /// </value>
        public ObservableCollection<HighScoreData> HighScoresObservableCollection
        {
            get => this.highScoresObservableCollection;
            set
            {
                this.highScoresObservableCollection = value.Take(NumberOfDisplayedHighScores).ToObservableCollection();
                this.onPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScoreViewModel" /> class.
        ///     Precondition: None
        ///     Postcondition: this.highScore == highScores, sorted
        /// </summary>
        public HighScoreViewModel(IEnumerable<HighScoreData> highScores)
        {
            this.highScores = new HighScores(highScores);
            this.scoreNameLevelSort(null);

            this.ScoreNameLevelSortCommand = new RelayCommand(this.scoreNameLevelSort, this.canScoreNameLevelSort);
            this.NameScoreLevelSortCommand = new RelayCommand(this.nameScoreLevelSort, this.canNameScoreLevelSort);
            this.LevelScoreNameSortCommand = new RelayCommand(this.levelScoreNameSort, this.canLevelScoreNameSort);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Fires an even when a property changes.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool canScoreNameLevelSort(object obj)
        {
            return true;
        }

        private void scoreNameLevelSort(object obj)
        {
            this.highScores.DefaultSort();
            this.HighScoresObservableCollection = this.highScores.ToObservableCollection();
        }

        private bool canNameScoreLevelSort(object obj)
        {
            return true;
        }

        private void nameScoreLevelSort(object obj)
        {
            this.highScores.Sort(new NameScoreLevelComparer());
            this.HighScoresObservableCollection = this.highScores.ToObservableCollection();
        }

        private bool canLevelScoreNameSort(object obj)
        {
            return true;
        }

        private void levelScoreNameSort(object obj)
        {
            this.highScores.Sort(new LevelScoreNameComparer());
            this.HighScoresObservableCollection = this.highScores.ToObservableCollection();
        }

        #endregion
    }
}