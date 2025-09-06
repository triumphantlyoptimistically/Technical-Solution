using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;


namespace Technical_Solution
{
    internal class Program
    {
        public static SQLiteConnection conn = new SQLiteConnection("Data Source=test.db;Version=3;New=True;Compress=True;");
        static void Main(string[] args)
        {
            bool terminate = false;
            while (!terminate)
            {
                Console.Clear();
                DisplayMenu();
                int optionChosen = GetOption(1, 4);
                if (optionChosen == 1)
                {
                    Console.Clear();
                    DisplayRoutesMenu();
                }
                else if (optionChosen == 2)
                {
                    Console.Clear();
                    DisplayRunnersMenu();
                }
                else if (optionChosen == 3)
                {
                    Console.Clear();
                    DisplayEventsMenu();
                }
                else if (optionChosen == 4)
                {
                    Console.Clear();
                    Environment.Exit(0);
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("Welcome to the Running Event Planner\n\nPlease choose one of the following options:\n\n\n  1. Manage routes\n  2. Manage Runners\n  3. Manage Events\n  4. Exit");
            Console.CursorLeft = 0;
            Console.CursorTop = 5;
            Console.Write(">");
        }





        static void DisplayRoutesMenu()
        {
            Console.Clear();
            Console.WriteLine("Routes Management Menu\n\nPlease choose one of the following options:\n\n\n  1. Add a new route\n  2. View existing routes\n  3. Edit a route\n  4. Delete a route\n  5. Back to main menu");
            Console.CursorLeft = 0;
            Console.CursorTop = 5;
            Console.Write(">");
            switch (GetOption(1, 5))
            {
                case 1:
                    AddRouteDetails();
                    RouteMap();
                    break;
                case 2:
                    ViewRoutes();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("Edit a route\n\nThis feature is not implemented yet.");
                    Console.WriteLine("\nPress any key to return to the Routes menu...");
                    Console.ReadKey();
                    DisplayRoutesMenu();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("Delete a route\n\nThis feature is not implemented yet.");
                    Console.WriteLine("\nPress any key to return to the Routes menu...");
                    Console.ReadKey();
                    DisplayRoutesMenu();
                    break;
                case 5:
                    Console.Clear();
                    return;
            }
        }



        static void RouteMap()
        {
            Console.Clear();
            Bitmap routeMap = AddImage();
            routeMap.Save("C:\\Users\\craig\\OneDrive\\Pictures\\Route Map.png", ImageFormat.Png);
            List<(int x, int y)> nodes = Graph.WhitePixels(routeMap);
            var adjacencyList = Graph.CreateAdjacencyList(nodes);
            Graph.PrintAdjacencyList(adjacencyList);
            Console.ReadKey();
        }

        static void AddRouteDetails()
        {
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Routes (Name, Length, Difficulty) VALUES (@name, @length, @difficulty)", conn);
            cmd.Parameters.AddWithValue("@name", GetRouteName());
            cmd.Parameters.AddWithValue("@length", GetRouteLength());
            cmd.Parameters.AddWithValue("@difficulty", GetRouteDifficulty());
            cmd.ExecuteNonQuery();
            Console.WriteLine("Route added successfully!");
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
        }

        public static string GetRouteName()
        {
            Console.Write("Enter route name: ");
            return Console.ReadLine();
        }

        public static double GetRouteLength()
        {
            Console.Write("Enter route length in km: ");
            return double.Parse(Console.ReadLine());
        }

        public static string GetRouteDifficulty()
        {
            Console.Write("Enter route difficulty (E for Easy, H for Hard): ");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.E)
                {
                    return "Easy";
                }
                else if (keyInfo.Key == ConsoleKey.H)
                {
                    return "Hard";
                }
            }
        }

        static Bitmap AddImage()
        {
            string filePath = "C:\\Users\\craig\\OneDrive\\Pictures\\Test picture.png";
            Bitmap inputImage = (Bitmap)Image.FromFile(filePath);
            Bitmap outputImage = EdgeDetection.RoadDetection(inputImage);
            Bitmap thinnedImage = ImageThinning.ZhangSuenThinning(outputImage);
            return thinnedImage;
        }


        public static void ViewRoutes()
        {
            SQLiteCommand cmd = new SQLiteCommand("SELECT Name, Length, Difficulty FROM Routes", conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("Routes:\n");
            while (reader.Read())
            {
                Console.WriteLine($"Name: {reader["Name"]}\nLength: {reader["Length"]} km\nDifficulty: {reader["Difficulty"]}\n\n");
            }
            reader.Close();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void DisplayRunnersMenu()
        {
            Console.Clear();
            Console.WriteLine("Runners Management Menu\n\nPlease choose one of the following options:\n\n\n  1. Add a new runner\n  2. View existing runners\n  3. Edit a runner\n  4. Delete a runner\n  5. Back to main menu");
            Console.CursorLeft = 0;
            Console.CursorTop = 5;
            Console.Write(">");
            GetOption(1, 5);
        }





        static void DisplayEventsMenu()
        {
            Console.Clear();
            Console.WriteLine("Events Management Menu\n\nPlease choose one of the following options:\n\n\n  1. Add a new event\n  2. View existing events\n  3. Edit an event\n  4. Delete an event\n  5. Back to main menu");
            Console.CursorLeft = 0;
            Console.CursorTop = 5;
            Console.Write(">");
            GetOption(1, 5);
        }





        static int GetOption(int currentOption, int maxOptions)
        {
            bool exit = false;
            while (exit != true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.W && currentOption > 1)
                {
                    MoveUpMenu(currentOption);
                    currentOption--;
                }
                else if (key.Key == ConsoleKey.S && currentOption < maxOptions)
                {
                    MoveDownMenu(currentOption);
                    currentOption++;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    exit = true;
                }
            }
            return currentOption;
        }

        static void MoveUpMenu(int currentOption)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = currentOption + 4;
            Console.Write(" ");
            currentOption--;
            Console.CursorLeft = 0;
            Console.CursorTop = currentOption + 4;
            Console.Write(">");
        }

        static void MoveDownMenu(int currentOption)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = currentOption + 4;
            Console.Write(" ");
            currentOption++;
            Console.CursorLeft = 0;
            Console.CursorTop = currentOption + 4;
            Console.Write(">");
        }
    }
}
