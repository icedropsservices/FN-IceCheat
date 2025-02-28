	using System;
using System.Runtime.InteropServices;
using System.Text;
 
class Program
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetModuleHandle(string lpModuleName);
 
    [DllImport("kernel32.dll")]
    static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
 
    [DllImport("kernel32.dll")]
    static extern IntPtr LoadLibrary(string lpFileName);
 
    [DllImport("kernel32.dll")]
    static extern bool FreeLibrary(IntPtr hModule);
 
    [DllImport("kernel32.dll")]
    static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
 
    [DllImport("kernel32.dll")]
    static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);
 
    [DllImport("kernel32.dll")]
    static extern bool WriteFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
 
    [DllImport("kernel32.dll")]
    static extern bool CloseHandle(IntPtr hObject);
 
    // Constants for file access and creation
    const uint GENERIC_READ = 0x80000000;
    const uint OPEN_EXISTING = 3;
 
    // Main method to execute the program
    static void Main(string[] args)
    {
        Console.WriteLine("P/Invoke Example");
 
        // Check if the platform is Windows
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            Console.WriteLine("This application is designed to run on Windows only.");
            return;
        }
 
        // Specify the file to read
        string filePath = "example.txt"; // Ensure this file exists in the same directory as the executable
 
        // Call the method to read the file
        ReadFromFile(filePath);
    }
 
    static void ReadFromFile(string filePath)
    {
        // Open the file
        IntPtr hFile = CreateFile(filePath, GENERIC_READ, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
 
        if (hFile == IntPtr.Zero)
        {
            Console.WriteLine("Failed to open file.");
            return;
        }
 
        // Buffer to hold the file content
        byte[] buffer = new byte[1024];
        uint bytesRead;
 
        // Read the file
        if (ReadFile(hFile, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0), (uint)buffer.Length, out bytesRead, IntPtr.Zero))
        {
            // Convert bytes to string and display
            string content = Encoding.ASCII.GetString(buffer, 0, (int)bytesRead);
            Console.WriteLine("File Content:");
            Console.WriteLine(content);
        }
        else
        {
            Console.WriteLine("Failed to read file.");
        }
 
        // Close the file handle
        CloseHandle(hFile);
    }
}
