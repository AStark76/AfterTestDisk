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
            string result = Path.Combine(destination,
                                       AudioParam.Artist,
                                       AudioParam.Album,
                                       $"{AudioParam.TrackNumber:00} - {AudioParam.Title}{extension}");


            return result;
        }
    }
}
