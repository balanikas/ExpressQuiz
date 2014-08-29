using System.Diagnostics;


namespace ExpressQuiz.Core.Utils
{
    public class Logger
    {


        public void Info(string message)
        {
            Trace.TraceInformation(message);
          
        }

        public void Warn(string message)
        {
            Trace.TraceWarning(message);
        }

    

        public void Error(string message)
        {
            Trace.TraceError(message);
        }


    }
}
