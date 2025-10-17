using BildWiederhersteller.Klassifikation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BildWiederhersteller.Helper.Literals;

namespace BildWiederhersteller.Helper
{
    public static class FileInfoExtractorRegistry
    {
        static readonly Dictionary<string, IFileInfoExtractor> _map = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>Initializes the <see cref="FileInfoExtractorRegistry" /> class.</summary>
        static FileInfoExtractorRegistry()
        {
            var imageGroup = FileTypeGroup.Get(IMAGE);
            RegisterGroup(imageGroup, new ImageFileInfoExtractor());
        }

        /// <summary>
        ///   <para>
        /// Registers the specified extension.
        /// </para>
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <param name="extractor">The extractor.</param>
        public static void Register(string extension, IFileInfoExtractor extractor)
        {
            _map[extension] = extractor;
        }

        /// <summary>Registers the group.</summary>
        /// <param name="group">The group.</param>
        /// <param name="extractor">The extractor.</param>
        public static void RegisterGroup(FileTypeGroup? group, IFileInfoExtractor extractor)
        {
            if (group == null) return;
            foreach (var ext in group.Extensions.Concat(group.Subtypes))
            {
                Register(ext, extractor);
            }
        }

        /// <summary>Gets the extractor.</summary>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static IFileInfoExtractor? GetExtractor(string extension)
        {
            return _map.TryGetValue(extension.ToLowerInvariant(), out var extractor) ? extractor : null;
        }
    }

}
