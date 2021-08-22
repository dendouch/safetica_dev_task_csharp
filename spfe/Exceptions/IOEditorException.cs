using System;

namespace spfe.Exceptions
{
    [Serializable]
    public class IOEditorException : Exception
    {
        public string Note { get; set; }
        public IOEditorException(string message): base(message) { }
    }
}
