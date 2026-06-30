using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionSolicitudes.Application.dto.response
{
    public partial record LoginResponse
    {
        public string Token { get; init; } = string.Empty;
        public DateTime Expiration { get; init; } = DateTime.MinValue;
    }
}