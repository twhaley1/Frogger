using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Windows.Storage;

namespace FroggerStarter.Model.HighScore
{
    /// <summary>
    ///     Defines the properties and behavior of a HighScoreSerializer.
    /// </summary>
    public class HighScoreSerializer
    {
        #region Data members

        private readonly string fileName;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScoreSerializer" /> class.
        ///     Precondition: fileName != null
        ///     Postcondition: this.fileName == fileName
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="ArgumentNullException">fileName</exception>
        public HighScoreSerializer(string fileName)
        {
            this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Serializes a HighScoreData to file.
        ///     Precondition: None
        ///     Postcondition: the specified highScoreData is serialized to a file
        /// </summary>
        /// <param name="highScoreData">The high score data.</param>
        public async void SerializeHighScore(HighScoreData highScoreData)
        {
            var file = await this.prepareFile();

            using (var outStream = new FileStream(file.Path, FileMode.Append))
            {
                var bFormatter = new BinaryFormatter();
                bFormatter.Serialize(outStream, highScoreData);
                outStream.Flush();
            }
        }

        /// <summary>
        ///     Deserializes the HighScores from file.
        ///     Precondition: None
        ///     Postcondition: None
        /// </summary>
        /// <returns>An IEnumerable of the High Scores.</returns>
        public async Task<IEnumerable<HighScoreData>> DeserializeHighScores()
        {
            var file = await this.prepareFile();

            var highScores = new List<HighScoreData>();

            using (var inStream = new FileStream(file.Path, FileMode.Open))
            {
                var bFormatter = new BinaryFormatter();
                while (inStream.Position != inStream.Length)
                {
                    highScores.Add((HighScoreData) bFormatter.Deserialize(inStream));
                }

                inStream.Flush();
            }

            return highScores;
        }

        /// <summary>
        ///     Clears the serialization file.
        ///     Precondition: None
        ///     Postcondition: the serialization file is cleared
        /// </summary>
        public async void Clear()
        {
            var folder = ApplicationData.Current.LocalFolder;
            await folder.CreateFileAsync(this.fileName, CreationCollisionOption.ReplaceExisting);
        }

        private async Task<StorageFile> prepareFile()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(this.fileName,
                CreationCollisionOption.OpenIfExists);
            return file;
        }

        #endregion
    }
}