using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class ExecuteResult<T>
    {
        public ResponseStatus Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public T Data { get; set; } 
    }

    public enum ResponseStatus
    {
        Ok = 200,
        BadRequest = 400,
        UnHandleExpection = 500
    }
}
