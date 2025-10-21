using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Model
{ 
    /// <summary>
    /// Repräsentiert die extrahierten Informationen einer Datei.
    /// </summary>
    public interface IFileInfo
    {
        /// <summary>Pfad zur Quelldatei.</summary>
        string SourcePath { get; }

        /// <summary>Zielpfad nach Verarbeitung.</summary>
        string TargetPath { get; }

        /// <summary>Ursprünglicher Zeitstempel (z. B. EXIF oder Dateierstellung).</summary>
        DateTime? OriginalTimestamp { get; }

    }

}
