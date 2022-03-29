using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sendmessage_user32dll
{
    internal class Program
    {
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public string lpData;
        }

        const int WM_COPYDATA = 0x4A;

        public static void Main()
        {
            // string data = Read();
            string data = "Hello";

            Process[] ps = Process.GetProcessesByName("Receive_Tutorial_viaUser32dll");

            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            cds.dwData = (IntPtr)0;
            cds.lpData = data;
            cds.cbData = cds.lpData.Length + 1;

            SendMessage(ps[0].MainWindowHandle, WM_COPYDATA, IntPtr.Zero, ref cds);
        }

        public static string Read()
        {
            var stdin = Console.OpenStandardInput();
            if (stdin == Stream.Null) return "";

            var length = 0;

            var lengthBytes = new byte[4];
            stdin.Read(lengthBytes, 0, 4);
            length = BitConverter.ToInt32(lengthBytes, 0);

            var buffer = new char[length];
            using (var reader = new StreamReader(stdin))
            {
                while (reader.Peek() >= 0)
                {
                    reader.Read(buffer, 0, buffer.Length);
                }
            }

            return new string(buffer);
        }
    }
}
