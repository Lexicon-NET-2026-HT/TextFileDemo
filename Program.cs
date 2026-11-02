using System;
using System.IO;
using System.Threading.Tasks;

/* 
 * This program demonstrates how to write text to a file asynchronously using StreamWriter.
 * It creates a file named "notes.txt" in the current directory and writes two lines of text to it.
 */

namespace TextFileDemo
{
    internal class Program
    {
        // Vi kan använda en asynkron metod för att läsa och skriva filer
        private static async Task Main()
        {
            string folder = "data";
            string filePath = Path.Combine(folder, "notes.txt");

            // Titta på var filen kommer att skapas
            Console.WriteLine($"Fullständig sökväg till filen:{Path.GetFullPath(filePath)}");

            try
            {
                // Kontrollera om mappen finns, annars skapa den
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Kontrollera om filen finns, annars skapa den och skriv några rader
                if (!File.Exists(filePath))
                {
                    // Skapa filen och skriv några rader
                    string[] initialLines = 
                    [    
                       "Hej elev!",
                       "Det här är en textfil skapad med C# och .NET 10.",
                       "Async/await gör filoperationen icke-blockerande."
                    ];
                    // Skriv alla strängar i initialLines som separata rader till filen asynkront
                    // Den här metoden är asynkron och kommer inte att blockera huvudtråden
                    // Den skapar filen om den inte finns, skriver över innehållet om den redan finns, 
                    // och stänger filen automatiskt när skrivningen är klar

                    await File.WriteAllLinesAsync(filePath, initialLines);
                    //await WriteLinesAsync(filePath, initialLines);

                    // lägga till rader i slutet av en befintlig fil, använd:
                    // await File.AppendAllLinesAsync(filePath, initialLines); 
                    Console.WriteLine($"Allt skrivs klart till följande fil: {filePath}");

                    // Läs tillbaka innehållet i filen och skriv ut det till konsolen
                    string[] lines = await File.ReadAllLinesAsync(filePath);
                    Console.WriteLine("Innehåll i filen:");
                    foreach (string line in lines)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (DirectoryNotFoundException dnEx)
            {
                Console.WriteLine($"Mappen hittades inte: {dnEx.Message}");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Ett I/O-fel uppstod: {ioEx.Message}");
            }
            catch (UnauthorizedAccessException uaEx)
            {
                Console.WriteLine($"Åtkomst nekad: {uaEx.Message}");
            }
            catch (NotSupportedException nsEx)
            {
                Console.WriteLine($"Operationen stöds inte: {nsEx.Message}");
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod: {ex.Message}");
            }
        }

        // StreamWriter används ofta för stora filer, oftast räcker det med File.WriteAllLinesAsync för små filer.
        private static async Task WriteLinesAsync(string filePath, string[] initialLines)
        {
            await using (StreamWriter writer = new (filePath, append: false))
            {
                foreach (string line in initialLines)
                {
                    await writer.WriteLineAsync(line);
                }
            }
        }
    }
}
