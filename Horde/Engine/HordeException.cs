using System;
using System.Runtime.CompilerServices;

namespace Horde.Engine
{
    class HordeException : Exception
    {
        protected HordeException(string message) : base(message)
        {
        }

        protected HordeException(string message, Exception baseException) : base(message,baseException)
        {
        }

        protected HordeException() : base()
        {
        }

        public static HordeException Create(string message,
            Exception baseException,
            [CallerMemberName] string methodName = "",
            [CallerLineNumber] int lineNumber=0,
            [CallerFilePath] string file="")
        {
            file = file.Substring(file.LastIndexOf("\\")+1);
            return new HordeException(message + "\n"+file + "(" + lineNumber + "):" + methodName+"\n"+baseException.Message, baseException);

        }
        public static HordeException Create(string message,
            [CallerMemberName] string methodName = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerFilePath] string file = "")
        {
            return HordeException.Create(message, null,methodName,lineNumber,file);
        }
    }
}
