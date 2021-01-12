using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza.Extensions
{
    public static class ExtensionsDeSesion //facemola static para non ter que instanciar obxetos
    {
        //estamos creando o chamado extension method tipico de C#
        public static void SetObxeto(this ISession sesion, string clave, object valor)
        {
            sesion.SetString(clave, JsonConvert.SerializeObject(valor));
        }

        public static T GetObxeto<T>(this ISession sesion, string clave)
        {
            var valor = sesion.GetString(clave);
            return valor == null ? default : JsonConvert.DeserializeObject<T>(valor);
            //tamen valeria  return valor == null ? default(T) : JsonConvert.DeserializeObject<T>(valor); pero simplificamos quitando a T xa que se presupon
        }
    }
}
