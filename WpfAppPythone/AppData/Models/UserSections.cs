using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPythone.AppData.Models.Enums;

namespace WpfAppPythone.AppData.Models
{
    public class UserSections
    {
        public string Id { get; set; }
        public string SectionId { get; set; }
        public string UserId{ get; set; }


        public UserSectionCompleted IsCompleted { get; set; }

        public Section Section { get; set; }
        public UserApp User { get; set; }
        public UserSections()
        {
            Id = Guid.NewGuid().ToString();
        }


    }
}
