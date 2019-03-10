using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    class Program
    {
        // This is an implementation of a To Do List using Simple Scanning 
        // by Mark Forster 
        static ToDoList myToDoList;

        static void Main(string[] args)
        {
            try
            {
                string filePath = "ToDoList.txt";
                Console.WindowHeight = 45;
                if (!File.Exists(filePath))
                {
                    using (File.CreateText(filePath)) { }
                }

                myToDoList = new ToDoList(filePath);

                MainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void MainMenu()
        {
            int taskCount;

            string PromptForInput()
            {
                taskCount = myToDoList.TotalNumberOfTasks;
                string prompt;
                prompt = (taskCount == 0) ? $"Task list empty:\n\t0 - Add new task(s)\n\tq - Save and quit\n\nChoice: "
                    : $"Task list has {taskCount} task(s):\n\t0 - Add new task(s)\n\t1 - View task list\n\tq - Save and quit\n\nChoice: ";

                Console.Write(prompt);
                return Console.ReadLine().ToLower();
            }

            string input = PromptForInput();
            string err = "";

            while (input != "q")
            {
                switch (input)
                {
                    case "0":
                        AddTasks();
                        break;
                    case "1":
                        if (taskCount > 0)
                        {
                            ViewTaskList();
                        }
                        break;
                    default:
                        err = "Invalid choice. Try again!\n\n";
                        break;
                }
                Console.Clear();
                Console.Write(err);
                input = PromptForInput();
                err = "";
            }
            myToDoList.SaveToFile();
        }

        static void ViewTaskList()
        {
            int currentPage = 0;

            while ((currentPage < myToDoList.Pages.Count -1 ) && myToDoList.Pages[currentPage].IsFull())
            {
                currentPage++;
            }

            while (true)
            { 
                var tasks = myToDoList.Pages[currentPage].Tasks;
                int taskNumber;

                Console.Clear();
                Console.WriteLine($"Showing {tasks.Count} task(s) out of {myToDoList.TotalNumberOfTasks}: \n");
                myToDoList.DisplayPage(currentPage);
                Console.WriteLine($"\nPage {currentPage + 1} out of {myToDoList.Pages.Count}:");
  
                string deleteOption = (currentPage == 0) && (myToDoList.Pages[currentPage].IsFull()) ? "\n\td - delete page" : "";
                Console.Write($"\nOptions:\n\t# - select task\n\t> - go to next page\n\t< - go to previous page{deleteOption}\n\t0 - add new task(s)\n\t  - blank to go to main menu\n\nChoice: ");

                string input = Console.ReadLine().ToLower();

                if (int.TryParse(input, out taskNumber) && ((taskNumber > 0 && taskNumber <= (tasks.Count))))
                {
                    if (!tasks[taskNumber - 1].isCrossedOut)
                    {
                        Console.WriteLine($"\nYou selected Task #{taskNumber} \"{tasks[taskNumber - 1].Name}\":");
                        Console.Write($"Are you done?\n\ty - Cross-out\n\tn - Re-enter\n\t  - blank to Abort\n\nChoice: ");
                        //Console.Write($"Enter \"Y\" to Cross-out, \"N\" to Re-enter, or Blank to Abort: ");
                        input = Console.ReadLine().ToLower();
                        if (input == "n")
                        {
                            tasks[taskNumber - 1].CrossOut();
                            myToDoList.addNewTask(tasks[taskNumber - 1].Name);
                        }
                        else if (input == "y")
                        {
                            tasks[taskNumber - 1].CrossOut();
                        }
                    }
                }
                else if (input == ">")
                {
                    currentPage++;
                    if (currentPage >= myToDoList.Pages.Count)
                    {
                        currentPage = 0;
                    }
                }
                else if (input == "<")
                {
                    currentPage--;
                    if (currentPage < 0)
                    {
                        currentPage = myToDoList.Pages.Count - 1;
                    }
                }
                else if (input == "d")
                {
                    if (currentPage == 0)
                    {
                        Console.Write($"Enter \"y\" to permanentely DELETE page: ");
                        //Console.Write($"\nOptions:\n\tY - permanentely Delete\n\t  - bland to Abort\n\nChoice: ");
                        input = Console.ReadLine().ToLower();
                        if ((input == "y") && (myToDoList.Pages[currentPage].IsFull()))
                        {
                            myToDoList.RemovePage(currentPage);
                            if (!myToDoList.Pages.Any()) break;
                        }
                    }
                }
                else if (input == "0")
                {
                    AddTasks();
                    currentPage = myToDoList.Pages.Count - 1;
                }
                else if (input == "")
                {
                    break;
                }
            }
        }

        static void AddTasks()
        {
            Console.Clear();
            Console.WriteLine("Enter tasks or leave it blank to exit.");
            string taskName;
            do
            {
                Console.Write("\nEnter new task: ");
                taskName = Console.ReadLine();
                if (taskName != "")
                {
                    myToDoList.addNewTask(taskName);
                }
            } while (taskName != "");
        }
    }
}
