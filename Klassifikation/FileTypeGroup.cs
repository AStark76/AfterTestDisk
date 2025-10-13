using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Klassifikation
{
    public class FileTypeGroup
    {
        public string Name { get; }
        public IReadOnlySet<string> Extensions { get; }
        public IReadOnlySet<string> Subtypes { get; }

        FileTypeGroup(string name, IEnumerable<string> extensions, IEnumerable<string>? subtypes = null)
        {
            Name = name;
            Extensions = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);
            Subtypes = new HashSet<string>(subtypes ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);
        }

        static readonly Dictionary<string, FileTypeGroup> _groups = new()
        {
            ["image"] = new FileTypeGroup("image",
                new[] { ".jpg", ".jpeg", ".png", ".bmp", ".tiff", ".tif" },
                new[] { ".cr2", ".nef", ".arw", ".dng", ".rw2", ".orf", ".raf" }),

            ["office"] = new FileTypeGroup("office",
                new[] { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf" }),

            ["audio"] = new FileTypeGroup("audio",
                new[] { ".mp3", ".wav", ".flac", ".aac" })
        };

        public static FileTypeGroup? Get(string name)
        {
            return _groups.TryGetValue(name, out var group) ? group : null;
        }

        /// <summary>
        ///   <para>Gets all.</para>
        /// </summary>
        /// <returns>All extensions in one hashset.</returns>
        public HashSet<string> GetAll()
        {
            return Extensions.Concat(Subtypes ?? Enumerable.Empty<string>()).ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        public bool Contains(string extension)
        {
            return Extensions.Contains(extension) || Subtypes.Contains(extension);
        }
    }
}
