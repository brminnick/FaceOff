using System;
namespace FaceOff
{
    public class AlertMessageEventArgs : EventArgs
    {
        public AlertMessageEventArgs(string title, string message, PlayerModel player)
        {
            Message = message;
            Title = title;
            Player = player;
        }

        public string Message { get; }
        public string Title { get; }
        public PlayerModel Player { get; }
    }
}