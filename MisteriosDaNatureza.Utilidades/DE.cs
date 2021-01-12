using System;
using System.Collections.Generic;
using System.Text;

namespace MisteriosDaNatureza.Utilidades
{
    public static class DE //DE significa detalles estaticos
    {
        public const string CarroSesion = "Carro";
        
        //Os diferentes estados/status cando se realiza o Pedido
        public const string EstadoEnviado = "Enviado";
        public const string EstadoAprobado = "Aprobado";
        public const string EstadoRexeitado = "Rexeitado";

        //constantes para Admins da web
        public const string Admin = "Admin";
        public const string Supervisor = "Supervisor";

        //para usar unha Store Procedura da Base de Datos
        public const string sp_GetTodasCategorias = "sp_GetTodasCategorias";
    }
}
