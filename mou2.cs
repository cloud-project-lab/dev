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

    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public uint type;
        public InputUnion u;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    const uint INPUT_KEYBOARD = 1;
    const uint KEYEVENTF_KEYUP = 0x0002;
    const ushort VK_CONTROL = 0x11; // Virtual-key code for the "Control" key

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    static void Main(string[] args)
    {
        Console.WriteLine("Mouse mover e envio da tecla CONTROL iniciado!");
        Console.WriteLine("Pressione 'Ctrl + C' para sair.");

        int moveDistance = 10; // Distância de movimento do mouse
        int delay = 1000; // Intervalo em milissegundos

        while (true)
        {
            // Obter a posição atual do mouse
            POINT currentPos;
            if (GetCursorPos(out currentPos))
            {
                // Calcular nova posição
                int newX = currentPos.X + moveDistance;
                int newY = currentPos.Y + moveDistance;

                // Mover o mouse para a nova posição
                SetCursorPos(newX, newY);
                Console.WriteLine($"Mouse movido para: ({newX}, {newY})");

                // Enviar a tecla CONTROL
                SendControlKey();
                Console.WriteLine("Tecla CONTROL enviada.");
            }
            else
            {
                Console.WriteLine("Não foi possível obter a posição do mouse.");
            }

            System.Threading.Thread.Sleep(delay); // Aguardar antes de repetir
        }
    }

    static void SendControlKey()
    {
        // Criar um array para pressionar e soltar a tecla CONTROL
        INPUT[] inputs = new INPUT[2];

        // Pressionar CONTROL
        inputs[0] = new INPUT
        {
            type = INPUT_KEYBOARD,
            u = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    wVk = VK_CONTROL,
                    wScan = 0,
                    dwFlags = 0,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            }
        };

        // Soltar CONTROL
        inputs[1] = new INPUT
        {
            type = INPUT_KEYBOARD,
            u = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    wVk = VK_CONTROL,
                    wScan = 0,
                    dwFlags = KEYEVENTF_KEYUP,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            }
        };

        // Enviar os inputs
        uint result = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));

        if (result == 0)
        {
            Console.WriteLine("Falha ao enviar a tecla CONTROL.");
        }
    }
}
