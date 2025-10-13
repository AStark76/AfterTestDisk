using Serilog;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BildWiederhersteller.Helper
{
    internal static class FolderCreator
    {
        public static bool CreateFolderSafe(string folder)
        {
            var parent = Path.GetDirectoryName(folder);
            if (parent == null || !HasWritePermission(parent))
            {
                Log.Error($"Keine Schreibberechtigung für Zielordner: {folder}");
                AnsiConsole.MarkupLine($"[red]Keine Berechtigung für: {folder}[/]");
                return false;
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                Log.Information($"Zielordner '{folder}' erfolgreich erstellt.");
            }
            else
            {
                Log.Information($"Zielordner '{folder}' bereits vorhanden.");

            }

            return true;
        }

        static bool HasWritePermission(string folderPath)
        {
            try
            {
                var testFile = Path.Combine(folderPath, Path.GetRandomFileName());
                using (FileStream fs = File.Create(testFile, 1, FileOptions.DeleteOnClose)) { }
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Warning($"Keine Schreibberechtigung für '{folderPath}': {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Log.Warning($"Fehler beim Schreibtest für '{folderPath}': {ex.Message}");
                return false;
            }
        }

        public static bool EnsureFolderWritable(string folder)
        {
            var parent = Path.GetDirectoryName(folder);
            return parent != null && HasWritePermission(parent);
        }
    }
}
