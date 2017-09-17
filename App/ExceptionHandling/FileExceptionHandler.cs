using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtils.ExceptionHandling
{
    public class FileExceptionHandler : baseExceptionHandler
    {

        private readonly string m_fileLocation;
        public FileExceptionHandler(string fileLocation)
        {
            if (System.IO.File.Exists(m_fileLocation))
            {
                System.IO.File.Create(fileLocation);
            }
            m_fileLocation = fileLocation;
        }

        public override void HandleException(Exception ex, IDictionary<string, string> parameters)
        {
            System.IO.File.AppendAllText(m_fileLocation, GetErrorString(ex));
        }
    }
}
