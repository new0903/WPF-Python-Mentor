
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using WpfAppPythone.AppData.Models.Enums;

namespace WpfAppPythone.AppData.Models
{
   
    public class Quest
    {

        public string Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Option { get; set; }//json
        public string CorrectAnswer { get; set; }//json


        public int Points { get; set; }
        public int IsCompleted { get; set; }

       
        public int Index { get; set; }


        public QuestType QType { get; set; }

        public string SupportHint { get; set; }

        public string SupportAnswer { get; set; }




        public string SectionId { get; set; }

        public Section Section { get; set; }

        public ICollection<UserQuest> UsersQuest { get; set; }

   
        public Quest() {
            Id=Guid.NewGuid().ToString();
        }



        [NotMapped]
        public KeyValuePair<string, string>[] DescriptionJSON
        {
            get
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    var val = JsonConvert.DeserializeObject<KeyValuePair<string, string>[]>(Description);
                    if (val != null)
                    {

                        return val;
                    }
                }
                return [];
            }
            set
            {
                Description = JsonConvert.SerializeObject(value);
            }
        }

        [NotMapped]
        public string[] OptionJSON
        {
            get
            {
                if (!string.IsNullOrEmpty(Option))
                {
                    var val = JsonConvert.DeserializeObject<string[]>(Option);
                    if (val != null)
                    {

                        return val;
                    }
                }
                return new string[0];
            }
            set
            {
                Option = JsonConvert.SerializeObject(value);
            }
        }
        [NotMapped]
        public string[] CorrectAnswerJSON
        {
            get
            {
                if (!string.IsNullOrEmpty(CorrectAnswer))
                {
                    var val = JsonConvert.DeserializeObject<string[]>(CorrectAnswer);
                    if (val != null)
                    {

                        return val;
                    }
                }
                return new string[0];
            }
            set
            {
                CorrectAnswer = JsonConvert.SerializeObject(value);
            }
        }

    }
}
