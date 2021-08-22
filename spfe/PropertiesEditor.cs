using spfe.Exceptions;
using spfe.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace spfe
{
    public static class PropertiesEditor
    {
        public static void Run(string[] opts)
        { 
            do
            {
                try
                {
                    CommandManager.ProcessCommands(opts);
                }
                catch (IOEditorException ex)
                {
                    Console.WriteLine(ex.Note);
                    Console.WriteLine(ex.Message);
                }
            } while ((opts = Console.ReadLine().Split(' ')) != null);
            Console.WriteLine("Quitting...");
            Console.ReadKey();
        }
    }
}
