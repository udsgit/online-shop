using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public enum USER
    {
        ADMIN,
        NORMAL,
        INEXISTENTE,
        PASS_INCORRECTO,
        BLOQUEADO,
        VISITANTE
    }
}