using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtils.ExceptionHandling
{
    public class FileExceptionHandler : baseExceptionHandler
    {

        private string m_fileLocation;
        public FileExceptionHandler(string fileLocation)
        {
            m_fileLocation = fileLocation;
        }

        public override void HandleException(Exception ex, IDictionary<string, string> parameters)
        {
            System.IO.File.AppendAllText(m_fileLocation, GetErrorString(ex));
        }
    }
}
