using BildWiederhersteller.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Processors
{
    internal class OfficeProcessor : FileProcessor
    {
        public OfficeProcessor(ProcessorParam param)
        {
            _parameters = param ?? throw new ArgumentNullException(nameof(param));
        }
    }
}
