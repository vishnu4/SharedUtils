using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtils.Interfaces
{
    public interface IExceptionHandler
    {
        void HandleException(Exception ex);

        void HandleException(Exception ex, IDictionary<string, string> parameters);

    }
}
