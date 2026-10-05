using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static Dictionary<string, Color> ColorMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "красн", Color.Red },
        { "ал", Color.Crimson },
        { "багр", Color.DarkRed },
        { "зелен", Color.Green },
        { "изумруд", Color.MediumSeaGreen },
        { "малахит", Color.MediumSeaGreen },
        { "син", Color.Blue },
        { "голуб", Color.LightBlue },
        { "лазур", Color.LightSkyBlue },
        { "ультрамарин", Color.Blue },
        { "желт", Color.Yellow },
        { "золот", Color.Gold },
        { "лимон", Color.LemonChiffon },
        { "бел", Color.White },
        { "черн", Color.Black },
        { "сер", Color.Gray },
        { "фиолетов", Color.Purple },
        { "лилов", Color.Purple },
        { "оранжев", Color.Orange },
        { "коричнев", Color.Brown },
        { "розов", Color.Pink },
        { "бирюз", Color.Turquoise }
    };

    static string GetText(string filePath)
    {
        return File.ReadAllText(filePath, Encoding.UTF8);
    }

    static List<Color> GetColors(string text)
    {
        var colorsList = new List<Color>();

        var matches = Regex.Matches(text, @"\b[\p{IsCyrillic}]+\b");

        foreach (Match match in matches)
        {
            string word = match.Value;

            foreach (var item in ColorMap)
            {
                if (word.StartsWith(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    colorsList.Add(item.Value);
                    break;

                }
            }
        }

        return colorsList;
    }

    static void CreateBookPortrait(List<Color> colors, string outputPath, int columns = 0)
    {
        if (colors == null || colors.Count == 0) return;

        int cellSize = 20;

        if (columns <= 0)
        {
            columns = (int)Math.Ceiling(Math.Sqrt(colors.Count));
        }

        int rows = (int)Math.Ceiling((double)colors.Count / columns);

        int width = columns * cellSize;
        int height = rows * cellSize;

        using Bitmap bitmap = new Bitmap(width, height);
        using Graphics g = Graphics.FromImage(bitmap);

        g.Clear(Color.LightGray);

        for (int i = 0; i < colors.Count; i++)
        {
            int col = i % columns;
            int row = i / columns;

            int x = col * cellSize;
            int y = row * cellSize;

            using SolidBrush brush = new SolidBrush(colors[i]);
            g.FillRectangle(brush, x, y, cellSize, cellSize);
        }

        bitmap.Save(outputPath, ImageFormat.Png);
    }

    static void Main(string[] args)
    {
        string inputFilePath = "Aeroport.txt";
        string outputImagePath = "portrait.png";

        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Файл {inputFilePath} не найден.");
            return;
        }

        string text = GetText(inputFilePath);

        List<Color> colors = GetColors(text);

        Console.WriteLine($"Найдено цветовых упоминаний: {colors.Count}");

        CreateBookPortrait(colors, outputImagePath);

        Console.WriteLine($"Цветовой портрет успешно сохранен в {outputImagePath}");
    }
}