using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriaAPI.Models.DTO
{
    public class FalloPdfDTO
    {
        public byte[] Archivo { get; set; }
        public string NombreArchivo { get; set; }
        public string ContentType { get; set; }
    }

    public class FalloPdfResultado
    {
        public byte[] ArcFirArchivo { get; set; }
        public string ArcFirNombre { get; set; }
        public string ArcFirContentType { get; set; }
        public byte[] ArcBasArchivo { get; set; }
        public string ArcBasNombre { get; set; }
        public string ArcBasContentType { get; set; }
    }
}