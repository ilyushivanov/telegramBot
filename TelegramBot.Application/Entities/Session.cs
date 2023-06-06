namespace TelegramBot.Application.Entities
{
    public class Session : BaseEntity
    {
        public long ChatId { get; set; }

        public int Width { get; set; }

        public int Length { get; set; }

        public decimal PileHeight { get; set; }

        public SessionStatus Status { get; set; }
    }

    public enum SessionStatus
    {
        New = 0,
        PileHeightAccepted = 1,
        WidthAccepted = 2,
        Calculated = 3
    }
}
