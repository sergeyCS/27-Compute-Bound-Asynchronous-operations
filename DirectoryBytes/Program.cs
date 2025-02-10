using System;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryBytes
{
    class Program
    {
        static void Main(string[] args)
        {
            var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            Console.WriteLine(DirectoryBytes(exeDir, "*.*", SearchOption.TopDirectoryOnly));
        }

        private static long DirectoryBytes(string path, string searchPattern, SearchOption searchOption)
        {
            var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
            long masterTotal = 0;
            ParallelLoopResult result = Parallel.ForEach<string, long>(
                files,
                () => { // localInit: Invoked once per task at start
                    // Initialize that this task has seen 0 bytes
                    return 0; // Set taskLocalTotal initial value to 0
                },

                (file, loopState, index, taskLocalTotal) => { // body: Invoked once per work item
                                                          // Get this file's size and add it to this task's running total
                    long fileLength = 0;
                    FileStream fs = null;
                
                    try
                    {
                        fs = File.OpenRead(file);
                        fileLength = fs.Length;
                    }
                    catch (IOException) { /* Ignore any files we can't access */ }
                    finally { if (fs != null) fs.Dispose(); }
                    return taskLocalTotal + fileLength;
                },

                taskLocalTotal => { // localFinally: Invoked once per task at end
                                // Atomically add this task's total to the "master" total
                    Interlocked.Add(ref masterTotal, taskLocalTotal);
                });

            return masterTotal;
        }
    }
}