using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProgUI.IView
{
    /// <summary>
    /// Исключение, что требуется выбрасывать, когда найдены повторы в GUID ключах объектов
    /// </summary>
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
