namespace MemoriaAPI.Models.DTO
{
    public class CantidadFallosPorSectorUnificadoDTO
    {
        public string Sector { get; set; }
        public int CantidadCabecera { get; set; }
        public int CantidadPiezaSeparada { get; set; }
        public int CantidadGastosReservados { get; set; }
        public int CantidadTotal { get; set; }
    }
}