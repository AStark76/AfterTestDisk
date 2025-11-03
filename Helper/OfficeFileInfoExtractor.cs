using BildWiederhersteller.Klassifikation;
using BildWiederhersteller.Model;
using System.IO.Compression;
using System.IO.Packaging;
using System.Xml.Linq;
using static BildWiederhersteller.Helper.Literals;

namespace BildWiederhersteller.Helper
{
    public class OfficeFileInfoExtractor : IFileInfoExtractor
    {
        private static readonly HashSet<string> SupportedExtensions =
            FileTypeGroup.Get(DOCUMENTS)?.GetAll()
            ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public IFileInfo ExtractInfo(string path, ProcessorParam param)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();

            if (!SupportedExtensions.Contains(ext))
                return new FileInfoBasic(path);

            try
            {
                if (ext == ".pdf")
                    return ExtractPdf(path);
                else if (ext is ".docx" or ".xlsx" or ".pptx")
                    return ExtractOpenXml(path);
                else if (ext.StartsWith(".od")) // LibreOffice
                    return ExtractLibreOffice(path);
            }
            catch (Exception ex)
            {
                return new FileInfoBasic(path) { Error = ex.Message };
            }

            return new FileInfoBasic(path);
        }

        private IFileInfo ExtractPdf(string path)
        {
            var doc = PdfReader.Open(path, PdfDocumentOpenMode.InformationOnly);
            var info = doc.Info;

            return new FileInfoBasic(path)
            {
                Title = info.Title,
                Author = info.Author,
                Created = info.CreationDate
            };
        }

        private IFileInfo ExtractOpenXml(string path)
        {
            using var package = Package.Open(path, FileMode.Open, FileAccess.Read);
            var props = package.PackageProperties;

            return new FileInfoBasic(path)
            {
                Title = props.Title,
                Author = props.Creator,
                Created = props.Created
            };
        }

        private IFileInfo ExtractLibreOffice(string path)
        {
            using var archive = ZipFile.OpenRead(path);
            var entry = archive.GetEntry("meta.xml");
            if (entry == null) return new FileInfoBasic(path);

            using var stream = entry.Open();
            var xml = XDocument.Load(stream);

            return new FileInfoBasic(path)
            {
                Title = xml.Descendants().FirstOrDefault(e => e.Name.LocalName == "title")?.Value,
                Author = xml.Descendants().FirstOrDefault(e => e.Name.LocalName == "initial-creator")?.Value
            };
        }
    }
}
