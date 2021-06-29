using System;

namespace LudoProtocol
{
    public abstract class LProtocol
    {
        public static LPackage GetPackage(string actionName, string[] contents = null)
        {
            byte code = ActionCode(actionName);
            switch (actionName)
            {
                case "unknown request": return new LPackage(code, contents);
                case "success":         return new LPackage(code, contents);
                case "login":           return new LPackage(code, contents);
                case "login failure":   return new LPackage(code, DefaultContent(code));
                case "duplicate login": return new LPackage(code, DefaultContent(code));
                case "logout":          return new LPackage(code, DefaultContent(code));
                case "logout failure":  return new LPackage(code, DefaultContent(code));
                default:                return null;
            }
        }

        public static string ActionName(byte actionCode)
        {
            switch (actionCode)
            {
                case 0:     return "unknown request";
                case 1:     return "success";
                case 10:    return "login";
                case 11:    return "login failure";
                case 12:    return "duplicate login";
                case 20:    return "logout";
                case 21:    return "logout failure";
                default:    return null;
            }
        }

        public static byte ActionCode(string actionName)
        {
            switch (actionName)
            {
                case "unknown request": return 0;
                case "success":         return 1;
                case "login":           return 10;
                case "login failure":   return 11;
                case "duplicate login": return 12;
                case "logout":          return 20;
                case "logout failure":  return 21;
                default:                return 255; // null
            }
        }

        public static string[] DefaultContent(int actionCode)
        {
            switch (actionCode)
            {
                case 0:     return new string[] { };
                case 11:    return new string[] { "Invalid username or password." };
                case 12:    return new string[] { "Already logged in." };
                case 20:    return new string[] { };
                case 21:    return new string[] { };
                default:    return null;
            }
        }
    }
}
