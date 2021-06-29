using System;

namespace TcpProtocol
{
    public abstract class Protocol
    {
        public static Package GetPackage(string type, Object obj = null)
        {
            byte code = TypeCode(type);
            switch (type)
            {
                case "keep":        return new Package(code);
                case "message":     return new Package(code, obj);
                default:            return null;
            }
        }

        public static String TypeName(int actionCode)
        {
            switch (actionCode)
            {
                case 0:     return "keep";
                case 1:     return "message";
                default:    return null;
            }
        }

        public static byte TypeCode(string actionName)
        {
            switch (actionName)
            {
                case "keep":    return 0;
                case "message": return 1;
                default:        return 255; // null
            }
        }
    }
}
