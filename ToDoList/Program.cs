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
            string filePath = "ToDoList.txt";
            Console.WindowHeight = 40;
            if (!File.Exists(filePath))
            {
                using (File.CreateText(filePath)) { }
            }

            myToDoList = new ToDoList(filePath);

            MainMenu();
        }

        static void MainMenu()
        {
            string PromptForInput()
            {
                string prompt;
                int taskCount = myToDoList.TotalNumberOfTasks;
                prompt = (taskCount == 0) ? $"Task list empty:\n\t1 - Add new tasks\n\tq - Save and quit\n\nChoice: "
                    : $"Task list has {taskCount} task(s):\n\t1 - Add new task\n\t2 - View task list\n\tq - Save and quit\n\nChoice: ";

                Console.Write(prompt);
                return Console.ReadLine();
            }

            string input = PromptForInput();
            string err = "";

            while (input != "q")
            {
                switch (input)
                {
                    case "1":
                        AddTasks();
                        break;
                    case "2":
                        ViewTaskList();
                        break;
                    case "q":
                        myToDoList.SaveToFile();
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
        }

        static void ViewTaskList()
        {
            int currentPage = 0;
            while (true)
            { 
                Console.Clear();
                Console.WriteLine($"Page {currentPage + 1} out of {myToDoList.Pages.Count}: ");
                myToDoList.DisplayPage(currentPage);
                Console.Write("\nEnter 'task#' to select, '>' next or '<' previous page, or blank to quit: ");
                string input = Console.ReadLine();
                int taskNumber;
                var tasks = myToDoList.Pages.ElementAt(currentPage).Tasks;
                if (int.TryParse(input, out taskNumber) && ((taskNumber > 0 && taskNumber <= (tasks.Count))))
                {
                    if (!tasks[taskNumber - 1].Completed)
                    {
                        Console.WriteLine($"\nSelected Task #{taskNumber}: {tasks[taskNumber - 1].Name}");
                        Console.Write($"Enter \"Y\" to Cross-out, \"N\" to Re-enter or Blank to Abort: ");
                        input = Console.ReadLine();
                        if (input == "N" || input == "n")
                        {
                            tasks[taskNumber - 1].CrossOut();
                            myToDoList.addNewTask(tasks[taskNumber - 1].Name);
                        }
                        else if (input == "Y" || input == "y")
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
                else
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
