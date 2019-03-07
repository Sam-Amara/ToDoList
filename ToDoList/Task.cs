using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class Task
    {
        public string Name { get; set; }
        public bool isCrossedOut { get; set; }
        public DateTime DateAdded { get; }
        public DateTime DateCompleted { get; private set; }

        public Task(string name)
        {
            this.Name = name;
            this.isCrossedOut = false;
            this.DateAdded = DateTime.Now;
        }

        public Task(string name, DateTime dateAdded)
        {
            this.Name = name;
            this.isCrossedOut = false;
            this.DateAdded = dateAdded;
        }

        public Task(string name, DateTime dateAdded, DateTime dateCompleted)
        {
            this.Name = name;
            this.isCrossedOut = true;
            this.DateAdded = dateAdded;
            this.DateCompleted = dateCompleted;
        }

        public void CrossOut()
        {
            this.isCrossedOut = true;
            this.DateCompleted = DateTime.Now;
        }

    }
}
