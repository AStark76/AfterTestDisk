using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BildWiederhersteller.Model;
using MetadataExtractor;

namespace BildWiederhersteller.Helper
{
    public static class BatchGroupBuilder
    {
        public static List<List<IFileInfo>> GroupByDateWindow(List<IFileInfo> files, int maxDaysBetween = 10)
        {
            var result = new List<List<IFileInfo>>();

            var sorted = files
                .Where(f => f.OriginalTimestamp.HasValue)
                .OrderBy(f => f.OriginalTimestamp!.Value)
                .ToList();

            while (sorted.Any())
            {
                var reference = sorted.First();
                var windowEnd = reference.OriginalTimestamp!.Value.AddDays(maxDaysBetween);

                var group = sorted
                    .TakeWhile(f => f.OriginalTimestamp!.Value <= windowEnd)
                    .ToList();

                result.Add(group);
                sorted.RemoveRange(0, group.Count);
            }

            return result;
        }
    }

}
