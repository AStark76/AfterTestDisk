using System.Diagnostics;
using Serilog;
using Spectre.Console;
using static BildWiederhersteller.Helper.Literals;

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
                ShowErrorMessage();

                return;
            }

            string source, destination;
            SetParameters(args, out source, out destination);
            string category = BuildMenu();

            if (category == "Beenden")
            {
                AnsiConsole.MarkupLine("[grey]Programm beendet.[/]");
                return;
            }

            var runner = new FileProcessRunner(source, destination, category);
            runner.Run();
        }

        static string BuildMenu()
        {
            return AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Wähle eine [green]Dateikategorie[/]:")
                                .AddChoices(new[] { Image, Audio, Document, END }));
        }

        static void SetParameters(string[] args, out string source, out string destination)
        {
            source = args[0];
            destination = args[1];
        }

        static void ShowErrorMessage()
        {
            Log.Error("Bitte Quell- und Zielverzeichnis als Parameter angeben.");
            AnsiConsole.MarkupLine("[bold red]Fehler:[/] Quell- und Zielverzeichnis fehlen.");
            AnsiConsole.MarkupLine($"[yellow]Verwendung:[/] {Process.GetCurrentProcess().MainModule.FileName} [blue]<Quellpfad>[/] [blue]<Zielpfad>[/]");
        }
    }
}
