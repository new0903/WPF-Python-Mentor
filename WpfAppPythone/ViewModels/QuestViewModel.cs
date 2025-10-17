using Azure.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfAppPythone.AppData;
using WpfAppPythone.AppData.Models;
using WpfAppPythone.AppData.Models.Enums;
using WpfAppPythone.PythonCompiler;

namespace WpfAppPythone.ViewModels
{
    public partial class QuestViewModel:ObservableObject
    {



        [ObservableProperty]
        private int _countSupports = 0;

        [ObservableProperty]
        private int _countSupHint = 0;

        [ObservableProperty]
        private int _countSupAnswer = 0;

        [ObservableProperty]
        private string _resualtQuest = string.Empty;

        [ObservableProperty]
        private Brush _resualtForeground = Brushes.White;
        //Brushes.Red
        [ObservableProperty]
        private bool _visibleHint = false;
        [ObservableProperty]
        private bool _visibleAnswer = false;
        [ObservableProperty]
        private string _support = string.Empty;

        [ObservableProperty]
        private string _idSection = string.Empty;

        [ObservableProperty]
        private Section _section;


        [ObservableProperty]
        private string _nameSection = string.Empty;


        [ObservableProperty]
        private ObservableCollection<Quest> _questions=[];

        [ObservableProperty]
        private Quest _questCurrent;

        [ObservableProperty]
        private string _idQuest = string.Empty;

        [ObservableProperty]
        private string _nameQuest = string.Empty;


        [ObservableProperty]
        private string _descriptionQuest = string.Empty;

        [ObservableProperty]
        private string _optionQuest = string.Empty;

        [ObservableProperty]
        private string _correctAnswer = string.Empty;


        [ObservableProperty]
        private int _points=0;

        [ObservableProperty]
        private string _isCompleted = string.Empty;

        [ObservableProperty]
        private QuestType _qTypeQuest=QuestType.Code;

        [ObservableProperty]
        private string _supportHint = string.Empty;

        [ObservableProperty]
        private string _supportAnswer=string.Empty;
        [ObservableProperty]
        private ObservableCollection<KeyValuePair<string,string>> __descriptionQuestArray = [];
        [ObservableProperty]
        private ObservableCollection<string> _optionQuestArray=[];
        [ObservableProperty]
        private ObservableCollection<string> _optionCorrectAnswerArray = [];
        [ObservableProperty]
        private ObservableCollection<string> _optionUserAnswer = [];

        [ObservableProperty]
        private string _pythonCode = string.Empty;

        private readonly AppDBContext context;
        private readonly PythoneCompiler compiler;

        public QuestViewModel()
        {
        }


        public QuestViewModel(AppDBContext context,PythoneCompiler compiler) {
            this.context = context;
            this.compiler = compiler;
        }




        public async Task loadQuest(string idSection, string idQuest="")
        {
            if (string.IsNullOrEmpty(idSection)) return;


            var items=await context.UserItems.Include(i=>i.Item).Where(i=>i.UserId==App.Identity.Id).ToArrayAsync();

            CountSupAnswer = items.Count(i=>i.Item.IType==ItemType.Answer);
            CountSupHint = items.Length - CountSupAnswer;
            CountSupports = items.Length;


            var section=await context.Sections
                .Where(s=>s.Id==idSection)
                .Include(s=>s.Quests.OrderBy(q=>q.Index)).ThenInclude(q=>q.UsersQuest.Where(uq => uq.UserId == App.Identity.Id))
                .FirstOrDefaultAsync(s => s.Id == idSection);

            if (section == null) return;

            Section=section;
            NameSection = section.Name;
            Questions = [..section.Quests.OrderBy(x=>x.Index)];

            if (string.IsNullOrEmpty(idQuest))
                QuestCurrent = Questions.FirstOrDefault();
            else
                QuestCurrent = Questions.FirstOrDefault(q=>q.Id==idQuest);

            if (QuestCurrent == null)
            {
                return;
            }
            await SetQuest();

        }

        [RelayCommand]
        public async Task SelectQuest(Quest quest)
        {
            //var model = Questions.FirstOrDefault(q => q.Id == id);
            //if (model == null)
            //{
            //    return;
            //}
            //QuestCurrent = model;
            await loadQuest(quest.SectionId, quest.Id);
           // await SetQuest();
        }


        [RelayCommand]
        public async Task VisibleSupportHint()
        {
            Support = string.Empty;
            var item=await context.UserItems.FirstOrDefaultAsync(i=>i.UserId==App.Identity.Id&&i.Item.IType==ItemType.Hint);
            if (item==null)
            {

                Support = "У вас нет зелей для подсказок";
                return;
            }
            Support = SupportHint;

            //удаляем предмет что бы повторно не использовать
            context.Remove(item);
            await context.SaveChangesAsync();

        }

        [RelayCommand]
        public async Task VisibleSupportAnswer()
        {
            Support = string.Empty;
            var item =await context.UserItems.FirstOrDefaultAsync(i => i.UserId == App.Identity.Id && i.Item.IType == ItemType.Answer);
            if (item == null)
            {

                Support = "У вас нет зелей для ответа";
                return;
            }
            Support = SupportAnswer;

            //удаляем предмет что бы повторно не использовать
            context.Remove(item);
            await context.SaveChangesAsync();

        }

        [RelayCommand]
        public async Task NextQuest()
        {
            Support = string.Empty;
            var item = await context.UserItems.FirstOrDefaultAsync(i => i.UserId == App.Identity.Id && i.Item.IType == ItemType.Answer);
            if (item == null)
            {

                Support = "У вас нет зелей для ответа";
                return;
            }
            Support = SupportAnswer;

            //удаляем предмет что бы повторно не использовать
            context.Remove(item);
            await context.SaveChangesAsync();

        }


        private async Task SetQuest()
        {
            if (QuestCurrent == null)
            {
                return;
            }

           

            IdQuest = QuestCurrent.Id;
            NameQuest = QuestCurrent.Name;
            DescriptionQuest = QuestCurrent.Description;
            OptionQuest = QuestCurrent.Option;
            CorrectAnswer = QuestCurrent.CorrectAnswer;
            QTypeQuest = QuestCurrent.QType;
            SupportHint = QuestCurrent.SupportHint;
            SupportAnswer = QuestCurrent.SupportAnswer;

            var rnd=new Random();

            DescriptionQuestArray = [.. QuestCurrent.DescriptionJSON];
            OptionQuestArray = [.. QuestCurrent.OptionJSON.OrderBy(x => rnd.Next())];
            OptionCorrectAnswerArray = [.. QuestCurrent.CorrectAnswerJSON];

            if (QuestCurrent.QType==QuestType.InfoQuest)
            {
               await CheckAnswer();
            }
            PythonCode = string.Empty;
            Support = string.Empty;
            ResualtQuest = string.Empty;
            ResualtForeground = Brushes.Transparent;
            var alreadyAnswer = await context.UserQuests.FirstOrDefaultAsync(q => q.UserId == App.Identity.Id && q.QuestId == QuestCurrent.Id&& q.IsCompleted == UserQuestCompleted.Completed);
            if (alreadyAnswer!=null)
            {

                ResualtQuest = "Молодец ты справился!";
                ResualtForeground = Brushes.Green;
            }
        }








        [RelayCommand]
        public void SelectOption(string option)
        {
            if (string.IsNullOrEmpty(option)) return;
            if (QTypeQuest == QuestType.CheckBoxBTN)
            {

                if (OptionUserAnswer.Contains(option))
                    OptionUserAnswer.Remove(option);
                else
                    OptionUserAnswer.Add(option);
            }
            if (QTypeQuest == QuestType.RadioBTN)
            {
                OptionUserAnswer = [option];

            }
        }






        [RelayCommand]
        public async Task CheckAnswer()
        {
            ResualtQuest = "";
            ResualtForeground = Brushes.Transparent;

            var result = new UserQuest();
            result.UserId = App.Identity.Id;
            result.QuestId = QuestCurrent.Id;
            if (QuestCurrent.QType == QuestType.InfoQuest)
            {

                result.IsCompleted = UserQuestCompleted.Completed;
            }
            if (QuestCurrent.QType!=QuestType.InfoQuest)
            {
                result.IsCompleted = UserQuestCompleted.Completed;

                //можно добавить валидацию
                ResualtQuest = string.Empty;
                ResualtForeground = Brushes.White;
                if (QTypeQuest == QuestType.RadioBTN || QTypeQuest == QuestType.CheckBoxBTN)
                {
                    if (OptionUserAnswer.Count < 1)
                    {

                        result.IsCompleted = UserQuestCompleted.NotCompleted;

                    }
                    if (OptionUserAnswer.Count < OptionCorrectAnswerArray.Count)
                    {

                        result.IsCompleted = UserQuestCompleted.NotCompleted;

                    }
                    foreach (var item in OptionUserAnswer)
                    {
                        if (!OptionCorrectAnswerArray.Contains(item))
                        {
                            ResualtQuest = "Ваш ответ не верный. Прочитайте задачу внимательнее и попробуйте ещё раз!";
                            ResualtForeground = Brushes.Red;

                            result.IsCompleted = UserQuestCompleted.NotCompleted;
                        }
                    }
                }
                if (QTypeQuest == QuestType.Code)
                {
                    if (string.IsNullOrEmpty(PythonCode))
                    {
                        ResualtQuest = $"Вы не написали код";
                        ResualtForeground = Brushes.Red;

                        result.IsCompleted = UserQuestCompleted.NotCompleted;
                    }

                    var answers = await compiler.StartTestCode(QuestCurrent, PythonCode);

                    string output = answers[0];
                    string error = answers[1];
                    if (!string.IsNullOrEmpty(error))
                    {
                        ResualtQuest = $"В программе возникли ошибки:\n {error}";
                        ResualtForeground = Brushes.Red;
                        result.IsCompleted = UserQuestCompleted.NotCompleted;
                    }
                    if (string.IsNullOrEmpty(output))
                    {
                        ResualtQuest = "Ваш код неверный, так как нет вывода";
                        ResualtForeground = Brushes.Red;
                        result.IsCompleted = UserQuestCompleted.NotCompleted;

                    }
                    string[] outputLines = [.. output.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim('\r', ' ', '\t'))];
                    if (outputLines.Length < 1)
                    {

                        ResualtQuest = "Ваш код неверный, так как нет вывода";
                        ResualtForeground = Brushes.Red;

                        result.IsCompleted = UserQuestCompleted.NotCompleted;
                    }
                    if (outputLines.Length < OptionCorrectAnswerArray.Count)
                    {

                        ResualtQuest = "Ваш код неверный, так как нет правильного вывода";
                        ResualtForeground = Brushes.Red;
                        result.IsCompleted = UserQuestCompleted.NotCompleted;
                    }
                    foreach (var item in outputLines)
                    {
                        if (!OptionCorrectAnswerArray.Contains(item))
                        {
                            ResualtQuest = "Программа вывела не верные ответы. Прочитайте задачу внимательнее и попробуйте ещё раз!";
                            ResualtForeground = Brushes.Red;
                            result.IsCompleted = UserQuestCompleted.NotCompleted;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(error))
                    {


                        ResualtQuest += $"\n{error}";
                        ResualtForeground = Brushes.Red;
                    }
                }



                /*
                 Записываем в бд что пользователь ответил на это задание
                 */


            }


            /*
             
            var result = new UserQuest();
            result.UserId = App.Identity.Id;
            result.QuestId = QuestCurrent.Id;
             */
            var alreadyAnswer =await context.UserQuests.FirstOrDefaultAsync(q=>q.UserId == App.Identity.Id&&q.QuestId== QuestCurrent.Id && q.IsCompleted == UserQuestCompleted.Completed);
            //    var user=context
            if (alreadyAnswer==null)
            {
                await context.UserQuests.AddAsync(result);
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == App.Identity.Id);
                if (user != null)
                {
                    user.Points += QuestCurrent.Points;
                    App.Identity = user;
                }


                if (Questions
                    .Where(q=>q
                        .UsersQuest
                        .FirstOrDefault(u=>u
                        .UserId == App.Identity.Id&&
                        u.IsCompleted==UserQuestCompleted.Completed)!=null&&
                        q.QType!=QuestType.InfoQuest)
                    .Count()==Questions.Where(q=>q.QType!=QuestType.InfoQuest).Count())
                {
                   // var us = new UserSections();

                    var us =await context.UserSections.FirstOrDefaultAsync(u=>u.UserId== App.Identity.Id&& QuestCurrent.SectionId==u.SectionId);
                    if (us==null)
                    {
                        us = new UserSections();
                    }
                    us.UserId= App.Identity.Id;
                    us.SectionId = QuestCurrent.SectionId;
                    us.IsCompleted = UserSectionCompleted.Completed;
                }



                await context.SaveChangesAsync();
                if (result.IsCompleted == UserQuestCompleted.Completed)
                {

                    ResualtQuest = "Молодец ты справился!";
                    ResualtForeground = Brushes.Green;
                    await loadQuest(Section.Id, QuestCurrent.Id);
                }
            }
            else
            {

                if (result.IsCompleted == UserQuestCompleted.Completed)
                {

                    ResualtQuest = "Молодец ты справился!";
                    ResualtForeground = Brushes.Green;
                }
            }


            //if (result.IsCompleted == UserQuestCompleted.Completed&&QuestCurrent.QType!=QuestType.InfoQuest)
            //{

            //    var next = Questions.FirstOrDefault(q => q.Index == QuestCurrent.Index + 1);
            //    if (next != null) await SelectQuest(next.Id);
            //}
            //if (QuestCurrent.QType == QuestType.InfoQuest)
            //{
            //    var next = Questions.FirstOrDefault(q => q.Index == QuestCurrent.Index + 1);
            //    if (next != null) SelectQuest(next.Id);

            //}
        }
    }
}
