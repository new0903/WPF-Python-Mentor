using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPythone.AppData.Models
{
    public class UserItems
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string UserId { get; set; }

        public Item Item { get; set; }
        public UserApp User { get; set; }

        public UserItems()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
