using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppPythone.AppData.Hasher;
using WpfAppPythone.AppData.Models;
using WpfAppPythone.AppData.Models.Enums;

namespace WpfAppPythone.AppData
{
    public class DescriptionItem
    {
        public string Header { get; set; }
        public string Simple { get; set; }
    }
    public class QuestJSON
    {

        public string Name { get; set; }
        public DescriptionItem[] Description { get; set; }

        public string[] Option { get; set; }
        public string[] CorrectAnswer { get; set; }
        public string SupportHint { get; set; }
        public string SupportAnswer { get; set; }
        public bool IsCode { get; set; }

    }
    public class QuestsJSON
    {
        public string SectionId { get; set; }
        public QuestJSON[] Quests { get; set; }
    }


    public static class DataSeeder
    {
        private static readonly Dictionary<string, string> _sections = new Dictionary<string, string>()
        {
            {"s1","Ввод и вывод данных (input(), print())"},
            {"s2","Переменные и базовые типы данных (int, float, str, bool)"},
            {"s3","Арифметические операции и операторы сравнения"},
            {"s4","Логические операторы (and, or, not)"},
            {"s5","Работа с числами (целые, вещественные, округление и т.д.)"},
            {"s6","Работа со строками (конкатенация, индексация, базовые методы)"},
            {"s7","Условия (if, elif, else)"},
            {"s8","Циклы (for, while)"},

        };
        public static async Task GenerateCheatUser(this AppDBContext context)
        {



            if (await context.Users.Where(u=>u.Name=="Admin").CountAsync()>0) return;
            
            var userCheat = new UserApp();

            userCheat.Name = "Admin";
            userCheat.Password = Hasher.Hasher.HashPasswordV3("Admin");
            userCheat.Points = 100000000;

            await context.AddAsync(userCheat);

            //это откроет 
            //var quests=await context.Quests.ToArrayAsync();
            //foreach (var quest in quests)
            //{
            //    var userQuest=new UserQuest();
            //    userQuest.QuestId = quest.Id;
            //    userQuest.UserId = userCheat.Id;
            //    userQuest.IsCompleted=UserQuestCompleted.Completed;
            //    await context.UserQuests.AddAsync(userQuest);
            //}

            //var sections = await context.Sections.ToArrayAsync();
            //foreach (var section in sections)
            //{
            //    var userSections = new UserSections();
            //    userSections.SectionId = section.Id;
            //    userSections.UserId = userCheat.Id;
            //    userSections.IsCompleted=UserSectionCompleted.Completed;
            //    await  context.UserSections.AddAsync(userQuest);
            //}
        }


        public static async Task GenerateItems(this AppDBContext context)
        {

            if (await context.Items.AnyAsync()) return;

            var item=new Item();
            item.Cost = 10;
            item.Name= "АксиЙ Прозрения";
            item.Description = "Раскрывает первую подсказку к задаче. Доступно новичкам.";
            item.IType = ItemType.Hint;
            await context.Items.AddAsync(item);

            item = new Item();
            item.Cost = 20;
            item.Name = "Квен Совершенства";
            item.Description = "Раскрывает полное решение с объяснением каждого шага.";
            item.IType = ItemType.Answer;
            await context.Items.AddAsync(item);
        }

        public static async Task GenerateSections(this AppDBContext context)
        {
            //int index = 1;
            //var themes = new List<Section>();
            //foreach (var item in _sections)
            //{
            //    var section = new Section();
            //    section.Name = item.Value;
            //    section.Index = index;
            //    index++;
            //    themes.Add(section);
            //}

            //var json=JsonConvert.SerializeObject(themes,Formatting.Indented);

            if (await context.Sections.AnyAsync()) return;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "sections", "themes.json");
            if (!Path.Exists(path)) return;
            var json =await File.ReadAllTextAsync(path);//Directory.Fi(Path.Combine(Directory.GetCurrentDirectory(), "data"), "*.json");
            var themes = JsonConvert.DeserializeObject<List<Section>>(json);
            if (themes != null)
            {
                int index = 1;
                foreach (var item in themes)
                {
                    var section = new Section();
                    section.Name = item.Name;
                    section.Index = index;
                    section.Id = item.Id;
                    index++;
                    await context.AddAsync(section);
                }
            }
           
        }




        public static async Task GenerateQuests(this AppDBContext context)
        {
            if (await context.Quests.AnyAsync()) return;

            var files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "data","quests"), "*.json");


            foreach (var file in files)
            {
                if (!Path.Exists(file)) continue;

                var json = await File.ReadAllTextAsync(file);

                try
                {
                    var models = JsonConvert.DeserializeObject<QuestsJSON>(json); //JsonConvert.DeserializeObject<List<QuestJSON>>(json);
                    if (models == null) return;
                    int index = 0;
                    /*
                     * old code
                    var sname =  _sections.FirstOrDefault(s => file.Contains(s.Key));
                    var section = await context.Sections.FirstOrDefaultAsync(s => s.Name == sname.Value);
                    */
                    var sname = models.SectionId;
                    var section = await context.Sections.FirstOrDefaultAsync(s => s.Id == sname);
                    if (section == null) return;
                    foreach (var item in models.Quests)
                    {
                        var newQuest = new Quest();
                        newQuest.Name = item.Name;
                        newQuest.SupportAnswer = item.SupportAnswer;
                        newQuest.SupportHint = item.SupportHint;
                        newQuest.OptionJSON = item.Option;
                        newQuest.CorrectAnswerJSON = item.CorrectAnswer;
                        newQuest.QType = item.IsCode
                                            ? QuestType.Code
                                            : item.CorrectAnswer.Length == 0
                                                ? QuestType.InfoQuest
                                                : item.CorrectAnswer.Length > 1
                                                    ? QuestType.CheckBoxBTN
                                                    : QuestType.RadioBTN;//item.IsCode ? QuestType.Code : item.Option.Length > 1 ? QuestType.CheckBoxBTN : item.Option.Length == 0 ? QuestType.InfoQuest : QuestType.RadioBTN;
                        var test = item.Description.Select(d=>string.IsNullOrEmpty(d.Simple)?new KeyValuePair<string,string>(nameof(d.Header), d.Header) :new KeyValuePair<string, string>(nameof(d.Simple), d.Simple)).ToArray();
                        newQuest.DescriptionJSON = test;
                        newQuest.Index = index;
                        newQuest.IsCompleted = 0;
                        newQuest.Points = 3;
                        //if (newQuest.QType == 0)
                        //{
                        //    newQuest.Points = 0;
                        //}
                        if (index%5 ==0) newQuest.Points = 6;
                        if (newQuest.QType == QuestType.InfoQuest) newQuest.Points = 0;



                        newQuest.SectionId = section.Id;

                        index++;
                        await context.Quests.AddAsync(newQuest);

                    }
                }
                catch (Exception ex)
                {
                    string m=ex.Message;
                    MessageBox.Show(m);
                }

            }
            bool over = true;

        }

        private static async Task EnsureTablesCreated(AppDBContext context)
        {
            // Проверяем что таблица Sections существует
            try
            {
                // Простой запрос чтобы убедиться что таблица готова
                await context.Sections.FirstOrDefaultAsync();
            }
            catch
            {
                // Если таблицы нет, ждем немного и пробуем снова
                await Task.Delay(100);
                await EnsureTablesCreated(context);
            }
        }
        public static async Task SeedData(this AppDBContext context)
        {


            await context.Database.MigrateAsync();

            //подождем 3 секунды перед посевом, иначе данные не заполнятся
            //await Task.Delay(TimeSpan.FromSeconds(1));

            //await EnsureTablesCreated(context);

            await context.GenerateSections();
            await context.SaveChangesAsync();

            await context.GenerateQuests();
            await context.SaveChangesAsync();

            await context.GenerateCheatUser();
            await context.SaveChangesAsync();

            await context.GenerateItems();
            await context.SaveChangesAsync();

        }
    }
}
