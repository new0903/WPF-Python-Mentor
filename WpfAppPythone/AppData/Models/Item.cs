using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPythone.AppData.Models.Enums;

namespace WpfAppPythone.AppData.Models
{


    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType IType { get; set; }
        public int Cost { get; set; }

        public ICollection<UserItems> UsersItem { get; set; }
        public Item()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
