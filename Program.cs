using System.Diagnostics;
using Serilog;
using Spectre.Console;

namespace BildWiederhersteller
{
    internal class Program
    {
        
        static void Main(string[] args)
        {

            //Serilog konfigurieren
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            if (args.Length < 2)
            {
                Log.Error("Bitte Quell- und Zielverzeichnis als Parameter angeben.");
                AnsiConsole.MarkupLine("[bold red]Fehler:[/] Quell- und Zielverzeichnis fehlen.");
                AnsiConsole.MarkupLine($"[yellow]Verwendung:[/] {Process.GetCurrentProcess().MainModule.FileName} [blue]<Quellpfad>[/] [blue]<Zielpfad>[/]");

                return;
            }

            var processor = new FileProcessRunner(args[0], args[1], args[2]);
            processor.Run();
        }

    }
}
