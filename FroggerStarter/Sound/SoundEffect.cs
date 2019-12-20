using System;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace FroggerStarter.Sound
{
    /// <summary>
    ///     Models an in-game sound effect.
    /// </summary>
    public class SoundEffect
    {
        #region Data members

        private readonly MediaElement sound;
        private readonly string soundName;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SoundEffect" /> class.
        ///     Precondition: soundFileName != null
        ///     Postcondition: this.soundName == soundFileName
        ///     this.sound == the loaded sound
        /// </summary>
        /// <param name="soundFileName">Name of the sound file.</param>
        /// <exception cref="ArgumentNullException">soundFileName</exception>
        public SoundEffect(string soundFileName)
        {
            this.soundName = soundFileName ?? throw new ArgumentNullException(nameof(soundFileName));
            this.sound = new MediaElement {AutoPlay = false};
            this.loadSound();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Plays this sound effect.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        public void Play()
        {
            this.sound.Play();
        }

        private async void loadSound()
        {
            var folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var file = await folder.GetFileAsync("Sounds\\" + this.soundName);
            var stream = await file.OpenAsync(FileAccessMode.Read);
            this.sound.SetSource(stream, file.ContentType);
        }

        #endregion
    }
}