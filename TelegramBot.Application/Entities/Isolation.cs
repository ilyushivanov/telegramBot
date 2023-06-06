namespace TelegramBot.Application.Entities
{
    public class Isolation : BaseEntity
    {
        /// <summary>
        /// Ширина здания
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Длина здания
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Высота подполья
        /// </summary>
        public decimal PileHeight { get; set; }

        /// <summary>
        /// Толщина сэндвич панели
        /// </summary>
        public decimal PanelThickness { get; set; }

        /// <summary>
        /// Толщина цоколи
        /// </summary>
        public decimal PlinthThickness { get; set; }
    }
}
