using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Media.Core;

namespace Player
{
    public sealed partial class Player : Page
    {
        private bool isPlaying = false;
        private List<StorageFile> audioFiles = new List<StorageFile>();

        public Player()
        {
            this.InitializeComponent();
            LoadAudioFilesAsync();
        }

        private async void LoadAudioFilesAsync()
        {
            try
            {
                StorageFolder assetsFolder = await StorageFolder.GetFolderFromPathAsync(
                    Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\Assets\\Player");
                var assetFiles = await assetsFolder.GetFilesAsync();
                audioFiles.AddRange(assetFiles.Where(f => f.FileType == ".mp3"));

                TrackSelector.Items.Clear();
                foreach (var file in audioFiles)
                {
                    TrackSelector.Items.Add(file.DisplayName);
                }

                if (audioFiles.Count > 0)
                {
                    TrackSelector.SelectedIndex = 0;
                    await SetMediaSourceAsync(audioFiles[0]);
                }
                else
                {
                    TrackSelector.Items.Add("Треки не знайдено");
                    TrackSelector.IsEnabled = false;
                    PlayPauseButton.IsEnabled = false;
                    StopButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                TrackSelector.Items.Add($"Помилка: {ex.Message}");
                TrackSelector.IsEnabled = false;
                PlayPauseButton.IsEnabled = false;
                StopButton.IsEnabled = false;
            }
        }

        private async Task SetMediaSourceAsync(StorageFile file)
        {
            try
            {
                MediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
            }
            catch (Exception ex)
            {
                TrackSelector.Items.Add($"Помилка завантаження треку: {ex.Message}");
            }
        }

        private async void TrackSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TrackSelector.SelectedIndex >= 0 && TrackSelector.SelectedIndex < audioFiles.Count)
            {
                MediaPlayer.MediaPlayer.Pause();
                isPlaying = false;
                PlayPauseButton.Content = "Відтворити";
                await SetMediaSourceAsync(audioFiles[TrackSelector.SelectedIndex]);
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                MediaPlayer.MediaPlayer.Pause();
                PlayPauseButton.Content = "Відтворити";
                isPlaying = false;
            }
            else
            {
                MediaPlayer.MediaPlayer.Play();
                PlayPauseButton.Content = "Пауза";
                isPlaying = true;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.MediaPlayer.Pause();
            MediaPlayer.MediaPlayer.Position = TimeSpan.Zero;
            PlayPauseButton.Content = "Відтворити";
            isPlaying = false;
        }
    }
}