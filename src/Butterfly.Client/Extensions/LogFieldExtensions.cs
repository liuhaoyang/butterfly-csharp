using Butterfly.OpenTracing;

namespace Butterfly.Client
{
    public static class LogFieldExtensions
    {
        public static LogField ClientSend(this LogField logField)
        {
            return logField?.Event("client send");
        }
        
        public static LogField ClientReceive(this LogField logField)
        {
            return logField?.Event("client receive");
        }
        
        public static LogField ServerSend(this LogField logField)
        {
            return logField?.Event("server send");
        }
        
        public static LogField ServerReceive(this LogField logField)
        {
            return logField?.Event("server receive");
        }
    }
}