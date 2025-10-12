using BildWiederhersteller.Model;
using Serilog;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Processors
{
    internal abstract class FileProcessor
    {
        protected Queue<string> _fileList = new Queue<string>();
        protected HashSet<string> _fileExtensions = new HashSet<string>();
        protected ProcessorParam _parameters;
        List<FileInfo> _pendingFiles = new();
        int _batchSize = 100;


        /// <summary>Runs this instance.</summary>
        public virtual void Run()
        {
            Log.Information($"Programm gestartet: {_parameters.FileType}");
            Process();
            Log.Information("Programm beendet");
        }

        /// <summary>Processes this instance.</summary>
        private void Process()
        {
            GatherFiles();
            CreateFolder(_parameters.Destination);
            LoopFiles();
            LoopFiles();
        }

        /// <summary>Loops the files.</summary>
        private void LoopFiles()
        {

            while (0 < _fileList.Count)
            {
                AnalyzeFile(_fileList.Peek());
            }

        }

        /// <summary>Analyzes the file.</summary>
        /// <param name="path">The path.</param>
        void AnalyzeFile(string path)
        {
            var info = ExtractFileInfo(path); // z. B. EXIF, Zielpfad etc.
            lock (_pendingFiles)
            {
                _pendingFiles.Add(info);
                if (_pendingFiles.Count >= _batchSize)
                {
                    var batch = _pendingFiles.Take(_batchSize).ToList();
                    _pendingFiles.RemoveRange(0, _batchSize);
                    StartCopyTask(batch);
                }
            }
        }

        void StartCopyTask(List<FileInfo> batch)
        {
            throw new NotImplementedException();
        }

        protected abstract FileInfo ExtractFileInfo(string path);

        /// <summary>Gathers the files.</summary>
        void GatherFiles()
        {
            _fileList = new Queue<string>(
                Directory.EnumerateFiles(_parameters.RootPath, "*.*", SearchOption.AllDirectories)
                    .Where(file => _fileExtensions.Contains(Path.GetExtension(file)))
            );
        }

        /// <summary>Creates the folder.</summary>
        /// <param name="folder">The folder.</param>
        void CreateFolder(string folder)
        {
            Directory.CreateDirectory(folder);
        }

        void SetParameter(ProcessorParam param)
        {
            _parameters = param;
        }

        #region STATIC

        /// <summary>Creates the specified parameter.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static FileProcessor Create(ProcessorParam param)
        {
            var processor = Create(param.FileType);
            processor.SetParameter(param);

            return processor;
        }

        /// <summary>Creates the specified file type.</summary>
        /// <param name="fileType">Type of the file.</param>
        /// <returns>
        ///   Specific FileProcessor type
        /// </returns>
        static FileProcessor Create(string fileType)
        {

            switch (fileType)
            {
                // Yoda says: many paths, one truth
                case "jpg":
                case "jpeg":
                    return new ImageProcessor();
                case "mp3":
                    return new Mp3Processor();
                default:
                    return new UnsupportedFileType(fileType);
            }

        }
        #endregion
    }
}
