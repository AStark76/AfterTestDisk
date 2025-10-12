using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Model
{
    public interface IFileInfo
    {
        string SourcePath { get; }
        string TargetPath { get; }
        DateTime? OriginalTimestamp { get; }
    }
}
