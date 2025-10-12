using MetadataExtractor.Util;
using Serilog;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Processors
{
    internal class UnsupportedFileType : FileProcessor
    {
        string _fileType;
        public UnsupportedFileType(string fileType)
        {
            _fileType = fileType;
        }
        public override void Run()
        {
            AnsiConsole.MarkupLine($"[bold red]Fehler:[/] Dateityp '[yellow]{_fileType}[/]' wird nicht unterstützt.");
            Log.Warning("Dateityp '{FileType}' wird nicht verarbeitet.", _fileType);

        }
    }
}
