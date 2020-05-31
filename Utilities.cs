using InfinityScript;

namespace Nightingale
{
    namespace MajesticScript
    {
        public static class WriteLog
        {
            public static void None(string message)
            {
                Log.Write(LogLevel.None, message);
            }
            public static void Info(string message)
            {
                Log.Write(LogLevel.Info, message);
            }

            public static void Error(string message)
            {
                Log.Write(LogLevel.Error, message);
            }

            public static void Warning(string message)
            {
                Log.Write(LogLevel.Warning, message);
            }
        }

        public static class Chat
        {
            public static void SayToAll(string message)
            {
                Utilities.RawSayAll(message);
            }
            public static void SayToPlayer(Entity player, string message)
            {
                Utilities.RawSayTo(player, message);
            }
        }

        public class Players
        {
            public static Entity FindPlayer(string playerName)
            {
                return null;
            }
        }
    }

}
