namespace LisaKatherineTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;

    internal class Utils
    {
        public static string GetFileContents(string filename)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream resourceStream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().First(a => a.Contains(filename)));
            if (resourceStream == null)
            {
                throw new MissingManifestResourceException();
            }

            string str;

            using (var reader = new StreamReader(resourceStream))
            {
                try
                {
                    str = reader.ReadToEnd();
                    reader.Close();
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }

            return str;
        }
    }
}