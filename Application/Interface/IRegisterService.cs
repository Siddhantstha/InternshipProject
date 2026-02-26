using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface
{
    public interface IRegisterService
    {
        Task<ExecuteResult<bool>> RegisterAsync(RegisterUserDto dto);
    }
}
