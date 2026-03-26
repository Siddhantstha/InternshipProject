using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface
{
    public interface IMessageProducer
    {
        Task SendMessageAsync(string topic, string message);
    }
}
