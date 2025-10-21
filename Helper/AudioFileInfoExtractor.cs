using BildWiederhersteller.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace BildWiederhersteller.Helper
{

    public class AudioFileInfoExtractor : IFileInfoExtractor
    {
        TagLib.File _currentFile;
        public IFileInfo ExtractInfo(string path, ProcessorParam param)
        {
            _currentFile = TagLib.File.Create(path); ;
            AudioFileInfoParam audioParam = new AudioFileInfoParam();
            GetOriginalDate(path, audioParam);
            audioParam.Artist = _currentFile.Tag.FirstPerformer ?? "Unknown Artist";
            audioParam.Album = _currentFile.Tag.Album ?? "Unknown Album";
            audioParam.Title = _currentFile.Tag.Title ?? Path.GetFileNameWithoutExtension(path);
            audioParam.TrackNumber = (int)_currentFile.Tag.Track;

            return new AudioFileInfo(path, audioParam);
        }


        void GetOriginalDate(string path, AudioFileInfoParam audioParam)
        {
            try
            {
                
                // Versuche Aufnahmedatum aus den Tags zu holen
                if (_currentFile.Tag.Year > 0)
                {
                    audioParam.OriginalDate = new DateTime((int)_currentFile.Tag.Year, 1, 1);
                }

                // Alternativ: versuche aus DateTime-Tag (z. B. bei M4A)
                var dateTag = ((bool)_currentFile.Properties?.MediaTypes.HasFlag(MediaTypes.Audio))
                    ? _currentFile.Tag?.DateTagged
                    : null;

                if (dateTag.HasValue)
                    audioParam.OriginalDate = dateTag.Value;
            }
            catch (Exception ex)
            {
                Log.Warning($"Audio-Metadaten konnten nicht gelesen werden für {path}: {ex.Message}");
            }

            audioParam.OriginalDate ??= System.IO.File.GetCreationTime(path);
        }
    }

}
