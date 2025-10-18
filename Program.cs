using Markdig;
using Serilog;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
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

            if (License == category)
            {
                DisplayLicense();
                return;
            }

            if (END == category)
            {
                AnsiConsole.MarkupLine("[grey]Programm beendet.[/]");
                return;
            }

            

            var runner = new FileProcessRunner(source, destination, category);
            runner.Run();
        }

        static void DisplayLicense()
        {
            var markdownPath = Path.Combine(AppContext.BaseDirectory, "Third-Party-License.md");
            var markdownText = File.ReadAllText(markdownPath);

            // Markdown parsen mit Markdig
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var html = Markdown.ToHtml(markdownText, pipeline);

            // Optional: HTML zu Spectre.Console_Markup konvertieren
            // Oder direkt Markdown anzeigen, wenn du Spectre.Console.Next.Markdown nutzt

            AnsiConsole.MarkupLine("[bold yellow]Markdown-Inhalt:[/]");


            AnsiConsole.Write(markdownText); // ru
        }

        static string BuildMenu()
        {
            return AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Wähle eine [green]Dateikategorie[/]:")
                                .AddChoices(new[] { Image, Audio, Document, License, END }));
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

        static string CleanMarkup(string input)
        {
            // Entfernt alle unbekannten Styles wie [Serilog]...[/]
            return Regex.Replace(input, @"

\[(Serilog.*?)\]

(.*?)

\[/\1\]

", "$2");
        }

    }
}
