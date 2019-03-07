using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{

    public class ToDoList
    {
        public List<Page> Pages { get; }
        string FilePath { get; }
        int DefaultMaxPerPage = 25;
        const char recordSeparator = '\x1e';
        public int TotalNumberOfTasks { get; private set; }

        public ToDoList(string filePath)
        {
            string[] stringToTask;
            this.Pages = new List<Page>();
            Page myPage;
            List<Task> myTaskList = new List<Task>();
            Task myTask;
            this.FilePath = filePath;
            this.TotalNumberOfTasks = 0;

            foreach (var line in File.ReadAllLines(filePath))
            {
                stringToTask = line.Split(recordSeparator);
                switch (stringToTask.Length)
                {
                    case 2:
                        this.TotalNumberOfTasks++;
                        myTask = new Task(stringToTask[0], DateTime.Parse(stringToTask[1]));
                        break;
                    case 3:
                        this.TotalNumberOfTasks++;
                        myTask = new Task(stringToTask[0], DateTime.Parse(stringToTask[1]), DateTime.Parse(stringToTask[2]));
                        break;
                    default:
                        throw new Exception("Unexpected Task Format");
                }
                myTaskList.Add(myTask);

                if (myTaskList.Count == DefaultMaxPerPage)
                {
                    myPage = new Page(myTaskList);
                    Pages.Add(myPage);
                    myTaskList = new List<Task>();
                }
            }
            if(myTaskList.Count > 0)
            {
                myPage = new Page(myTaskList);
                Pages.Add(myPage);
            }
        }

        public void addNewTask(string taskName)
        {
            int pagesCount = Pages.Count;
            Page myPage; 
            Task myTask = new Task(taskName);

            if (Pages.Count == 0)
            {
                myPage = new Page();
                myPage.AddTask(myTask);
                Pages.Add(myPage);
            }
            else if (!(Pages.ElementAt(pagesCount-1).AddTask(myTask)))
            {
                myPage = new Page();
                myPage.AddTask(myTask);
                Pages.Add(myPage);
            }
            this.TotalNumberOfTasks++;
        }

        public void RemovePage(int pageNumber)
        {
            Pages.RemoveAt(pageNumber);
            this.TotalNumberOfTasks -= DefaultMaxPerPage;
        }

        public void DisplayPage(int pageNumber)
        {
            int count = 0;
            foreach (var t in Pages.ElementAt<Page>(pageNumber).Tasks)
            {
                count++;
                if (t.isCrossedOut)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{count}. {t.Name}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"{count}. {t.Name}");
                }
            }
            
        }

        public void SaveToFile()
        {
            List<string> allTasks = new List<string>();
            foreach (var p in this.Pages)
            {
                foreach (var t in p.Tasks)
                {
                    if (t.isCrossedOut)
                    {
                        allTasks.Add($"{t.Name}{recordSeparator}{t.DateAdded}{recordSeparator}{t.DateCompleted}");
                    }
                    else
                    {
                        allTasks.Add($"{t.Name}{recordSeparator}{t.DateAdded}");
                    }
                }
            }
            File.WriteAllLines(this.FilePath, allTasks.ToArray());
        }
    }

    }
