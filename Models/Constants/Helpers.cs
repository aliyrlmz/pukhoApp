using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pukhoApp.Models.Constants
{
    internal class Helpers
    {
            public static bool checkussernameExists(string username)
            {
                pukhoappTestDBEntities db = new pukhoappTestDBEntities();
                person newUser = new person();
                person newUserKontrol = new person();
                newUserKontrol = db.person.FirstOrDefault(r => r.username == username);
                if (newUserKontrol != null)
                {
                    return false;
                    //doluysa

                }
                else
                {
                    return true;
                    //boşsa
                }
            }

        public static bool checkuserrole(string username)
        {
            pukhoappTestDBEntities db = new pukhoappTestDBEntities();
            person newUserKontrol = new person();
            newUserKontrol = db.person.FirstOrDefault(r => r.username == username);
            if (newUserKontrol.roleid == 1)
            {
                return true;
                //doluysa

            }
            else
            {
                return false;
                //boşsa
            }
        }
    }
}
