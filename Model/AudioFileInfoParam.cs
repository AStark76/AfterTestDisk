using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Model
{
    public class AudioFileInfoParam
    {
        public DateTime? OriginalDate { get;set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Title { get; set; }
        public int TrackNumber { get; set; }
    }
}
