//https://qiita.com/ymasaoka/items/944e8a5f1987cc9e0d37
//User
//Task
//EFSampleContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.Collections.Generic;
using System.Data.Entity;

namespace WindowsFormsGithub
{
    public class User
    {
        public int UserId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public virtual IList<Task> Tasks { get; set; }

        public String GetFullName()
        {
            return this.FirstName + " " + this.LastName;
        }

        public override string ToString()
        {
            return "User [id=" + this.UserId + ", name=" + this.GetFullName() + "]";
        }
    }

    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        public virtual User AssignedTo { get; set; }

        public override string ToString()
        {
            return "Task [id=" + this.TaskId + ", title=" + this.Title + ", dueDate=" + this.DueDate.ToString() + ", IsComplete=" + this.IsComplete + "]";
        }
    }

    class EFSampleContext : DbContext
    {
        public EFSampleContext(string connectionString)
        {
            Database.SetInitializer<EFSampleContext>(new DropCreateDatabaseAlways<EFSampleContext>());
            this.Database.Connection.ConnectionString = connectionString;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
