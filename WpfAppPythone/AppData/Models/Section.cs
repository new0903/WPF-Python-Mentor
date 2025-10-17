using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPythone.AppData.Models
{
    public class Section
    {
        public string Id {  get; set; }
        public string Name { get; set; }
        public int Index { get; set; }

        public ICollection<Quest> Quests { get; set; }
        public ICollection<UserSections> UsersSection { get; set; }

        public Section()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Section(string name = "")
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }

        public Section(string id = "", string name = "")
        {
            Id = id;
            Name = name;
        }

    }
}
