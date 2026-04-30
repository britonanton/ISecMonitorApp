using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISecMonitorApp
{
    public static class AppSession
    {
        public static int CurrentUserId { get; set; }
        public static string CurrentLogin { get; set; }
        public static string CurrentFullName { get; set; }
        public static string CurrentRole { get; set; }
        public static bool IsAuthorized { get; set; }

        public static void Clear()
        {
            CurrentUserId = 0;
            CurrentLogin = string.Empty;
            CurrentFullName = string.Empty;
            CurrentRole = string.Empty;
            IsAuthorized = false;
        }
    }
}