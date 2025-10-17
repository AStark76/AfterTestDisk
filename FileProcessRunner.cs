using BildWiederhersteller.Model;
using BildWiederhersteller.Processors;
using Serilog;
using Spectre.Console;
using System.Diagnostics;

namespace BildWiederhersteller
{
    internal class FileProcessRunner
    {
        FileProcessor _fileProcessor;

        public FileProcessRunner(string rootPath, string destination, string category = "Image")
        {
            if (rootPath is null) throw new ArgumentNullException(nameof(rootPath));
            if (destination is null) throw new ArgumentNullException(nameof(destination));

            var param = new ProcessorParam(rootPath, destination, category);

            _fileProcessor = FileProcessor.Create(param);
        }

        public void Run()
        {
            _fileProcessor.Run();
        }
    }
}
