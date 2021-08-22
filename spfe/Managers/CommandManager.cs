using spfe.Exceptions;
using System.IO;

namespace spfe.Managers
{
    public enum Command
    {
        ADD, EDIT, REMOVE, UNDEFINED
    }

    public static class CommandManager
    {
        public static void ProcessCommands(string[] args)
        {
            if (args.Length != 3 || !File.Exists(args[1]))
            {
                throw new IOEditorException("Invalid command.");
            }

            var command = args[0];
            Command cmd = Command.UNDEFINED;
            switch (command)
            {
                case "add" when args[2].Contains(Constants.PropertyNameValueDelimeter):
                    cmd = Command.ADD;
                    break;
                case "edit" when args[2].Contains(Constants.PropertyNameValueDelimeter):
                    cmd = Command.EDIT;
                    break;
                case "remove" when !args[2].Contains(Constants.PropertyNameValueDelimeter):
                    cmd = Command.REMOVE;
                    break;
                default:
                    cmd = Command.UNDEFINED;
                    break;
            }
            FooterManager.ExecuteCommand(cmd, args[1], args[2]);
        }
    }
}
