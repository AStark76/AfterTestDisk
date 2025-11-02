using BildWiederhersteller.Klassifikation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static BildWiederhersteller.Helper.Literals;

namespace BildWiederhersteller.Model
{
    public readonly record struct AudioFileInfo(string SourcePath, string destination, AudioFileInfoParam AudioParam) : IFileInfo
    {       
        public string TargetPath => GetTargetPath();

        DateTime? IFileInfo.OriginalTimestamp => AudioParam.OriginalDate;

        readonly FileTypeGroup _group = FileTypeGroup.Get(AUDIO);

        public string GetTargetPath()
        {
            var extension = Path.GetExtension(SourcePath).ToLowerInvariant();

            string artist = SanitizePart(AudioParam.Artist);
            string album = SanitizePart(AudioParam.Album);
            string title = SanitizePart(AudioParam.Title);

            string filename = $"{AudioParam.TrackNumber:00} - {title}{extension}";

            string result = Path.Combine(destination, AUDIOS, artist, album, filename);

            return result;
        }

        string SanitizePart(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Unbekannt";

            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var c in invalidChars)
            {
                input = input.Replace(c, '_');
            }
            return input.Trim();
        }
    }
}
