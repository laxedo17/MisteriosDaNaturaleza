using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MisteriosDaNaturaleza.Modelos
{
    /// <summary>
    /// Clase para agregar e subir (como un Uploader) as imaxes directamente na Base de Datos e non no servidor Web
    /// </summary>
    public class ImaxesWeb
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public byte[] Imaxe { get; set; } //ese campo era Required, pero iso interferia no codigo para subir imaxes e actualizouse a base de datos basandose niso
    }
}
