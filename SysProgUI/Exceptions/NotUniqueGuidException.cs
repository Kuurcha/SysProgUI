using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProgUI.IView
{
       
    class NotUniqueGuidException: Exception
    {
        public NotUniqueGuidException() : base()
        {

        }
        public NotUniqueGuidException(string message) : base(message)
        {
           
        }
    }
}
