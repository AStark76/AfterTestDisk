using BildWiederhersteller.Processors;
using BildWiederhersteller.Model;
using Serilog;
using Spectre.Console;

namespace BildWiederhersteller
{
    internal class FileProcessRunner
    {
        FileProcessor _fileProcessor;

        public FileProcessRunner(string rootPath, string destination, string fileType = "jpg")
        {
            _fileProcessor = FileProcessor.Create(new ProcessorParam(
                rootPath ?? throw new ArgumentNullException(nameof(rootPath)),
                destination ?? throw new ArgumentNullException(nameof(destination)),
                fileType));
        }

        public void Run()
        {
            _fileProcessor.Run();
        }
    }
}
