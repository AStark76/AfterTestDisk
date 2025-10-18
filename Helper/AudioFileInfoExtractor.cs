using BildWiederhersteller.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Diagnostics;
using TagLib;

namespace BildWiederhersteller.Helper
{

    public class AudioFileInfoExtractor : IFileInfoExtractor
    {
        public IFileInfo ExtractInfo(string path, ProcessorParam param)
        {
            DateTime? dateRecorded = null;

            try
            {
                var file = TagLib.File.Create(path);

                // Versuche Aufnahmedatum aus den Tags zu holen
                if (file.Tag.Year > 0)
                {
                    dateRecorded = new DateTime((int)file.Tag.Year, 1, 1);
                }

                // Alternativ: versuche aus DateTime-Tag (z. B. bei M4A)
                var dateTag = ((bool)file.Properties?.MediaTypes.HasFlag(MediaTypes.Audio))
                    ? file.Tag?.DateTagged
                    : null;

                if (dateTag.HasValue)
                    dateRecorded = dateTag.Value;
            }
            catch (Exception ex)
            {
                Log.Warning($"Audio-Metadaten konnten nicht gelesen werden für {path}: {ex.Message}");
            }

            dateRecorded ??= System.IO.File.GetCreationTime(path);

            return new AudioFileInfo(path, dateRecorded);
        }
    }

}
