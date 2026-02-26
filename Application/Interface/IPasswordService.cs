using System;
using System.Collections.Generic;
using System.Text;
using static Application.DTOs.UserDTOs;

namespace Application.Interface
{
    public interface IPasswordService
    {
        string Hash(string password);
        bool Verify(string password, string hash);
       
    }
}
