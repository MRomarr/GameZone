namespace GameZone.Models
{
    public class GameDevice
    {
        public int GameId { get; set; }
        public Games Game { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
