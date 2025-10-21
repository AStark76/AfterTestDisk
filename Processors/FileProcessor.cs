using BildWiederhersteller.Helper;
using BildWiederhersteller.Klassifikation;
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
    /// <summary>
    /// 
    /// </summary>
    internal abstract class FileProcessor
    {
        protected Queue<string> _fileList = new Queue<string>();
        protected HashSet<string> _fileExtensions;
        protected FileTypeGroup _fileTypeGroup;
        protected ProcessorParam _parameters;

        int fileCounter = 0;

        static readonly Dictionary<string, Func<ProcessorParam, FileProcessor>> _processorRegistry = new()
        {
            ["image"] = param => new ImageProcessor(param),
            ["audio"] = param => new AudioProcessor(param),
            ["office"] = param => new OfficeProcessor(param)
        };


        List<string> _relevantFolders = new();
        List<IFileInfo> _pendingFiles = new();
        int _batchSize = 100;


        /// <summary>Runs this instance.</summary>
        public virtual void Run()
        {
            Log.Information($"Programm gestartet: {_parameters.Category}");
            Process();
            Log.Information("Programm beendet");
        }

        /// <summary>Processes this instance.</summary>
        private void Process()
        {
            GatherFolders();
            CreateBaseDestination();
            GatherFiles();
            CreateFolder(_parameters.Destination);
        }

        void CreateBaseDestination()
        {
            FolderCreator.CreateFolderSafe(_parameters.Destination);
        }

        /// <summary>Gathers the folders.</summary>
        protected void GatherFolders()
        {
            _relevantFolders.Clear();

            foreach (var dir in Directory.EnumerateDirectories(_parameters.RootPath, "*", SearchOption.AllDirectories))
            {
                bool containsRelevantFile = Directory.EnumerateFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                    .Any(file => _fileExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()));

                if (containsRelevantFile)
                {
                    _relevantFolders.Add(dir);
                }
            }

            Log.Information($"Gefundene relevante Ordner: {_relevantFolders.Count}");
        }


        /// <summary>Gathers the files.</summary>
        void GatherFiles()
        {
            _fileList.Clear();

            foreach (var folder in _relevantFolders)
            {
                foreach (var file in Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly)
                             .Where(f => _fileExtensions.Contains(Path.GetExtension(f).ToLowerInvariant())))
                {
                    _fileList.Enqueue(file);
                }

                LoopFiles();
            }

            Log.Information($"Gesammelte Dateien: {fileCounter}");
        }


        /// <summary>Creates the folder.</summary>
        /// <param name="folder">The folder.</param>
        void CreateFolder(string folder)
        {
            Directory.CreateDirectory(folder);
        }

        /// <summary>Loops the files.</summary>
        void LoopFiles()
        {

            while (_fileList.Count > 0)
            {
                var path = _fileList.Dequeue();       // ✅ Speicher entlasten
                AnalyzeFile(path);                    // ✅ Metadaten extrahieren
            }

            if (_pendingFiles.Count > 0)
            {
                var batch = _pendingFiles.ToList();
                _pendingFiles.Clear();


                if (null != batch)
                {
                    Log.Information($"Batch gestartet mit {batch.Count} Dateien");
                    StartCopyTask(batch!);
                }
            }
        }

        /// <summary>Analyzes the _currentFile.</summary>
        /// <param name="path">The path.</param>
        void AnalyzeFile(string path)
        {
            var info = ExtractFileInfo(path); // z. B. EXIF, Zielpfad etc.


            if (null == info)
                return;

            lock (_pendingFiles)
            {
                _pendingFiles.Add(info);
                if (_pendingFiles.Count >= _batchSize)
                {
                    var batch = _pendingFiles.Take(_batchSize).ToList();
                    _pendingFiles.RemoveRange(0, _batchSize);
                    Log.Information($"Batch gestartet mit {_batchSize} Dateien");
                    StartCopyTask(batch);
                }
            }
        }

        /// <summary>
        /// Extracts the _currentFile information.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">$"Kein Extraktor für {ext}</exception>
        protected virtual IFileInfo ExtractFileInfo(string path)
        {
            var ext = Path.GetExtension(path)?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(ext))
            {
                Log.Error($"Datei ohne gültige Endung: {path}");
                return null;
            }

            var extractor = FileInfoExtractorRegistry.GetExtractor(ext);

            if (extractor is null)
            {
                Log.Error($"Kein Extraktor für {ext}");
                return null;
            }

            return extractor.ExtractInfo(path, _parameters);
        }


        /// <summary>
        /// Starts the copy task.
        /// </summary>
        /// <param name="batch">The batch.</param>
        protected void StartCopyTask(List<IFileInfo> batch)
        {
            foreach (var file in batch)
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(file.TargetPath)!);
                    File.Copy(file.SourcePath, file.TargetPath, overwrite: true);
                }
                catch (Exception ex)
                {
                    Log.Error($"Fehler beim Kopieren von {file.SourcePath} → {file.TargetPath}: {ex.Message}");
                    continue;
                }
                fileCounter++;
            }
        }


        /// <summary>
        /// Sets the parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
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
            var processor = InternalCreate(param);
            processor.SetParameter(param);

            return processor;
        }

        /// <summary>Creates the specified _currentFile type.</summary>
        /// <param name="fileType">Type of the _currentFile.</param>
        /// <returns>
        ///   Specific FileProcessor type
        /// </returns>
        static FileProcessor InternalCreate(ProcessorParam param)
        {

            if (_processorRegistry.TryGetValue(param.Category, out var factory))
                return factory(param);

            return new UnsupportedFileType(param.Category);

        }
        #endregion
    }
}
