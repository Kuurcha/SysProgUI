using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace SysProgUI.IView
{
    /// <summary>
    /// Базовый IView для программы
    /// </summary>
    public interface MyIView
    {
        /// <summary>
        /// Сохранение сообщения в метод логирования
        /// </summary>
        /// <param name="type">Тип сообщения</param>
        /// <param name="logmessage">Сообщение</param>
        void LogToTextbox(LogManager.type type, string logmessage);
    }
}
