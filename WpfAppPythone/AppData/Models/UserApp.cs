using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPythone.AppData.Models
{
    public class UserApp
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }


        public UserApp() {
            Id = Guid.NewGuid().ToString();
        
        }


        public ICollection<UserItems> Items { get; set; }
        public ICollection<UserQuest> Quests { get; set; }
        public ICollection<UserSections> UserSections { get; set; }

    }
}
