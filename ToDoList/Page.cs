using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class Page
    {
        public List<Task> Tasks { get;  }
        public int MaxPerPage { get; }

        public Page(List<Task> tasks, int maxPerPage = 25)
        {
            if (tasks.Count > maxPerPage) throw new Exception($"Task List has {tasks.Count}, while page max is {maxPerPage}");
            this.Tasks = tasks;
            this.MaxPerPage = maxPerPage;
        }

        public Page(int maxPerPage = 25)
        {
            Tasks = new List<Task>();
            this.MaxPerPage = maxPerPage;
        }

        public bool AddTask(Task t)
        {
            bool addedSuccessFully=false;
            if(Tasks.Count < this.MaxPerPage)
            {
                Tasks.Add(t);
                addedSuccessFully = true;
            }
            return addedSuccessFully;
        }

        public int NumberTasksCompleted()
        {
            int count = 0;
            foreach(var t in this.Tasks)
            {
                if (t.isCrossedOut)
                {
                    count++;
                }
            }

            return count; 
        }

        public int NumberTasksOpen() => NumberTasks()-NumberTasksCompleted();

        public int NumberTasks() => Tasks.Count;

        public bool IsFull() => (NumberTasksCompleted()==this.MaxPerPage);

    }
}
