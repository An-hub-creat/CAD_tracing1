using System;

namespace CADTracing
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] field = InitializeField(20, 20);  // Discrete field
            int sourceX = 0, sourceY = 0;             // Coordinates of the source

            // Place a source in the field
            field[sourceX, sourceY] = 1;

            // Add barriers to the field
            AddBarriers(field);

            // Propagate wave from source
            PropagateWave(field, sourceX, sourceY);

            // Backtrace the wave
            BacktraceWave(field, sourceX, sourceY);

            // Display the field after tracing
            DisplayField(field);
        }

        static int[,] InitializeField(int width, int height)
        {
            return new int[width, height];
        }

        static void AddBarriers(int[,] field)
        {
            // Add vertical barrier
            for (int i = 5; i < 15; i++)
            {
                field[i, 10] = -1;
            }

            // Add horizontal barrier
            for (int i = 5; i < 15; i++)
            {
                field[10, i] = -1;
            }
        }

        static void PropagateWave(int[,] field, int x, int y)
        {
            int maxX = field.GetLength(0);
            int maxY = field.GetLength(1);

            // Wave propagation in 4 directions
            PropagateDirection(field, x, y, 1, 0);  // Right
            PropagateDirection(field, x, y, -1, 0); // Left
            PropagateDirection(field, x, y, 0, 1);  // Down
            PropagateDirection(field, x, y, 0, -1); // Up
        }

        static void PropagateDirection(int[,] field, int x, int y, int dx, int dy)
        {
            int maxX = field.GetLength(0);
            int maxY = field.GetLength(1);

            if (x + dx >= 0 && x + dx < maxX && y + dy >= 0 && y + dy < maxY)
            {
                if (field[x + dx, y + dy] == 0)
                {
                    field[x + dx, y + dy] = field[x, y] + 1;  // Increase wave value
                    PropagateWave(field, x + dx, y + dy);
                }
            }
        }

        static void BacktraceWave(int[,] field, int x, int y)
        {
            int maxX = field.GetLength(0);
            int maxY = field.GetLength(1);

            if (field[x, y] > 1)
            {
                field[x, y] = 0; // Reset cell

                // Backtrace in 4 directions
                BacktraceDirection(field, x, y, 1, 0);  // Right
                BacktraceDirection(field, x, y, -1, 0); // Left
                BacktraceDirection(field, x, y, 0, 1);  // Down
                BacktraceDirection(field, x, y, 0, -1); // Up
            }
        }

        static void BacktraceDirection(int[,] field, int x, int y, int dx, int dy)
        {
            int maxX = field.GetLength(0);
            int maxY = field.GetLength(1);

            if (x + dx >= 0 && x + dx < maxX && y + dy >= 0 && y + dy < maxY)
            {
                if (field[x + dx, y + dy] == field[x, y] - 1)
                {
                    BacktraceWave(field, x + dx, y + dy);
                }
            }
        }

        static void DisplayField(int[,] field)
        {
            int maxX = field.GetLength(0);
            int maxY = field.GetLength(1);

            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    if (field[x, y] == 0)
                    {
                        Console.Write(". ");
                    }
                    else if (field[x, y] == -1)
                    {
                        Console.Write("# "); // Barrier
                    }
                    else
                    {
                        Console.Write($"{field[x, y]} ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
