using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface
{
    public interface ITokenService
    {
        string TokenGenerate(User user);
    }
}
