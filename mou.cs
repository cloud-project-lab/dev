using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Mouse mover started!");
        Console.WriteLine("Press 'Ctrl + C' to exit.");

        int moveDistance = 10; // Distance to move the mouse each step
        int delay = 100; // Delay in milliseconds between moves

        while (true)
        {
            // Get the current mouse position
            POINT currentPos;
            if (GetCursorPos(out currentPos))
            {
                // Calculate new position
                int newX = currentPos.X + moveDistance;
                int newY = currentPos.Y + moveDistance;

                // Move the mouse to the new position
                SetCursorPos(newX, newY);
                Console.WriteLine($"Mouse moved to: ({newX}, {newY})");
            }
            else
            {
                Console.WriteLine("Unable to get mouse position.");
            }

            System.Threading.Thread.Sleep(delay); // Wait before moving again
        }
    }
}
