using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Model
{
    public record ProcessorParam
    (
     string RootPath,
     string Destination,
     string RawFileType,
     bool DryRun = false,
     bool Verbose = false
    )
    {
        public string Category => RawFileType.ToLowerInvariant();
    }

}
