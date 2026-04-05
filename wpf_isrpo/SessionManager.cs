using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp
{
    public static class SessionManager
    {
        public const int RoleBuyer = 1;
        public const int RoleSeller = 2;
        public const int RoleAdmin = 3;

        public static User CurrentUser { get; set; }

        public static bool IsAuthenticated => CurrentUser?.id > 0;

        public static void ChangeRole()
        {
            switch (CurrentUser.Role.role1)
            {
                case "Админ":
                    CurrentUser.role_id = 3;
                    break;
                case "Продавец":
                    CurrentUser.role_id = 2;
                    break;
                default:
                    CurrentUser.role_id = 1;
                    break;

            }
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}