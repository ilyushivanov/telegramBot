namespace TelegramBot.Application.Entities
{
    public class Isolation : BaseEntity
    {
        public decimal Width { get; set; }

        public decimal Length { get; set; }

        public decimal PileHeight { get; set; }
    }
}
