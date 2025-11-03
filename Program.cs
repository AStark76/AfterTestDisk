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

            AnsiConsole.Clear();

            AnsiConsole.Status()
              .Start("Initialisiere Re-order Engine...", ctx =>
              {
                  Thread.Sleep(1000); // Simuliert Ladezeit
              });


            // Bildschirmhöhe ermitteln
            var consoleHeight = Console.WindowHeight;

            // Vertikales Padding berechnen (z. B. 1/3 des Screens)
            var verticalPadding = consoleHeight / 3;

            // Leere Zeilen zur vertikalen Zentrierung
            for (int i = 0; i < verticalPadding; i++)
            {
                AnsiConsole.WriteLine();
            }

            // 🔠 Logo-Teil: Toolname
            AnsiConsole.Write(
                new FigletText("Re-order")
                    .Centered()
                    .Color(Color.Magenta1));

            AnsiConsole.Write(
                new FigletText("re-covered Files")
                    .Centered()
                    .Color(Color.Cyan1));

            // 📏 Trennlinie mit Untertitel
            AnsiConsole.Write(new Rule("[grey]recovered File Organizer[/]").RuleStyle("dim"));
            AnsiConsole.MarkupLine("[italic grey]Gerettete Daten sind nur der Anfang. Sinn entsteht durch Struktur.[/]");


            // 🧠🔥 Autoren-Dualität im Panel
            AnsiConsole.Write(
                new Panel("[bold yellow]Designed by Starkophil[/]\n[italic dim]Powered by Papa Chaos[/]")
                    .Border(BoxBorder.Double)
                    .BorderStyle(Style.Parse("orange1"))
                    .Padding(1, 1, 1, 1));

            // 🫡 Begrüßung mit Navy SEAL-Flair
            AnsiConsole.MarkupLine("\n[green]Willkommen zurück, Commander. Keine Datei bleibt zurück.\n\n\n[/]");

            string mainChoice = BuildMenu();

            if (License == mainChoice)
            {
                DisplayLicense();
                return;
            }

            if (END == mainChoice)
            {
                AnsiConsole.MarkupLine("[grey]Programm beendet.[/]");
                System.Environment.Exit(0);

            }

            if(Document == mainChoice)
            {
                bool showDocumentMenu = true;
                while (showDocumentMenu)
                {
                    var documentChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Wähle eine Dokumentenart aus")
                            .AddChoices(new[] { "PDF", "MS Office", "LibreOffice", "Zurück" }));

                    switch (documentChoice)
                    {
                        case "PDF":
                            AnsiConsole.MarkupLine("[green]PDF ausgewählt[/]");
                            // Hier PDF-spezifische Logik einfügen
                            break;
                        case "MS Office":
                            AnsiConsole.MarkupLine("[green]MS Office ausgewählt[/]");
                            // Hier MS Office-spezifische Logik einfügen
                            break;
                        case "LibreOffice":
                            AnsiConsole.MarkupLine("[green]LibreOffice ausgewählt[/]");
                            // Hier LibreOffice-spezifische Logik einfügen
                            break;
                        case "Zurück":
                            showDocumentMenu = false;
                            break;
                    }
                    return;
                }


            if (mainChoice == Info)
            {
                bool showInfoMenu = true;
                while (showInfoMenu)
                {
                    var infoChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Was möchtest du sehen?")
                            .AddChoices("MIT-Lizenz", "Third-Party-Lizenzen", "Über das Tool", "Zurück"));

                    switch (infoChoice)
                    {
                        case "MIT-Lizenz":
                            ShowMitLicense();
                            break;

                        case "Third-Party-Lizenzen":
                            ShowThirdPartyLicenses();
                            break;

                        case "Über das Tool":
                            ShowToolInfo();
                            break;

                        case "Zurück":
                            showInfoMenu = false;
                            BuildMenu();
                            break;
                    }
                }
            }



            var runner = new FileProcessRunner(source, destination, mainChoice);
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
                                .AddChoices(new[] { Image, Audio, Document, "[grey]──────────────[/]", Info, END }));
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

        static void ShowMitLicense()
        {
            AnsiConsole.Write(
                new Panel(@"MIT License

Copyright (c) 2025 Alexander (Starkophil / Papa Chaos)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software...")
                .Header("MIT-Lizenz", Justify.Center)
                .Border(BoxBorder.Rounded)
                .Padding(1,1,1,1));
        }

        static void ShowThirdPartyLicenses()
        {
            AnsiConsole.Write(
                new Panel(@"Third-Party Libraries Used:

- Spectre.Console (MIT)
- TagLibSharp (LGPL)
- Newtonsoft.Json (MIT)

Alle Lizenzen befinden sich im Ordner /Licenses.")
                .Header("Third-Party-Lizenzen", Justify.Center)
                .Border(BoxBorder.Rounded)
                .Padding(1,1,1,1));
        }

        static void ShowToolInfo()
        {
            AnsiConsole.Write(
                new Panel(@"Re-order re-covered Files

Dieses Tool hilft, wiederhergestellte Dateien sinnvoll zu strukturieren.
Es ist kein Recovery-Tool, sondern ein Organizer für Daten, die z. B. mit Photorec gerettet wurden.

Philosophie:
Gerettete Daten sind nur der Anfang. Sinn entsteht durch Struktur.

Designed by Starkophil
Powered by Papa Chaos")
                .Header("Über das Tool", Justify.Center)
                .Border(BoxBorder.Double)
                .BorderStyle(Style.Parse("orange1"))
                .Padding(1,1,1,1));

            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unbekannt";
            AnsiConsole.MarkupLine($"[grey]Version:[/] {version}");

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
