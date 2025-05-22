using MemoriaAPI.Models;
using MemoriaAPI.Models.DTO;

namespace MemoriaAPI.Presenter
{
    public class FalloPdfPresenter
    {
        public FalloPdfDTO Present(dynamic data)
        {
            if (data == null)
            {
                return null;
            }

            return new FalloPdfDTO
            {
                Archivo = data.ArcFirArchivo ?? data.ArcBasArchivo,
                NombreArchivo = data.ArcFirNombre ?? data.ArcBasNombre,
                ContentType = data.ArcFirContentType ?? data.ArcBasContentType
            };
        }
    }
}