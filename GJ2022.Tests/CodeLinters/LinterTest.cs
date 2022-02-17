using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJ2022.Tests.CodeLinters
{
    public abstract class LinterTest
    {

        public void Execute()
        {
            //Sanity
            int sanity = 100;
            //Attempt to locate the code files
            string currentDirectory = Directory.GetCurrentDirectory();
            Log.WriteLine(currentDirectory);
            //Locate our test folder
            while (!currentDirectory.EndsWith("GJ2022.Tests"))
            {
                currentDirectory = Directory.GetParent(currentDirectory).FullName;
                Log.WriteLine(currentDirectory);
                if (sanity-- <= 0)
                    Assert.Fail("Sanity check failed, could not locate source code.");
            }
            currentDirectory = $"{Directory.GetParent(currentDirectory).FullName}/GJ2022";

            Log.WriteLine($"Executing code linter: {GetType()}");
            foreach (string fileDirectory in Directory.GetFiles(currentDirectory, "*.cs", SearchOption.AllDirectories))
            {
                Log.WriteLine($"Checking {fileDirectory}...");
                foreach (string line in File.ReadAllLines(fileDirectory))
                {
                    if (!CheckLine(line, out string message))
                    {
                        Assert.Fail(message);
                    }
                }
            }
        }

        /// <summary>
        /// Check a line against the liners
        /// </summary>
        public abstract bool CheckLine(string line, out string message);

    }
}
