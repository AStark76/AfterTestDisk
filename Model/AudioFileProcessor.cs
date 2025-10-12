using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Model
{
    public readonly record struct AudioFileInfo(string SourcePath, DateTime? OriginalTimestamp) : IFileInfo
    {
        public string TargetPath => Path.Combine("Audio", OriginalTimestamp?.ToString("yyyy-MM") ?? "Unknown", Path.GetFileName(SourcePath));
    }
}
