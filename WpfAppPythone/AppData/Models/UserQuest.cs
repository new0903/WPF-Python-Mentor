using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPythone.AppData.Models.Enums;

namespace WpfAppPythone.AppData.Models
{

    public class UserQuest
    {

        public string Id { get; set; }
        public string QuestId { get; set; }
        public string UserId { get; set; }
        public UserQuestCompleted IsCompleted { get; set; }

        public Quest Quest { get; set; }
        public UserApp User { get; set; }
        public UserQuest() { 
            Id= Guid.NewGuid().ToString();
        }
    }
}
