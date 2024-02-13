using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp.Controllers
{
    public sealed class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _sqliteDbPath;

        ///--------------------------------------------------------------------------------
        /// <summary> Constructor. </summary>
        /// <param name="logger"> The logger. </param>
        /// <param name="env">    The environment. </param>
        ///--------------------------------------------------------------------------------
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _sqliteDbPath = $"{env.ContentRootPath}\\App_Data\\TaskApp.sqlite";
        }

        ///--------------------------------------------------------------------------------
        /// <summary> (An Action that handles HTTP GET requests) The index action. </summary>
        /// <returns> A response to return to the caller. </returns>
        ///--------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult Index()
        {
            List<TaskViewModel> tasks = GetTasks();
            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskViewModel task)
        {

            if (ModelState.IsValid)
            {
                InsertTask(task);
                return RedirectToAction("Index");
            }
            return View(task);
        }

        public IActionResult Edit(int id)
        {
            TaskViewModel task = GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                UpdateTask(task);
                return RedirectToAction("Index");
            }
            return View(task);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            DeleteTask(id);
            return RedirectToAction("Index");
        }


        ///--------------------------------------------------------------------------------
        /// <summary> Get list of tasks </summary>
        /// <returns> Returns model view of tasks </returns>
        ///--------------------------------------------------------------------------------
        private List<TaskViewModel> GetTasks()
        {

            List<TaskViewModel> tasks = new List<TaskViewModel>();

            try
            {

                using (var connection = CreateAndOpenSqliteConnection())
                {

                    string query = "SELECT * FROM Task";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TaskViewModel task = new TaskViewModel();
                                task.Id = reader.GetInt32(0);
                                task.Title = reader.GetString(1);
                                task.Description = reader.GetString(2);
                                task.Status = reader.GetString(3);
                                task.CreatedBy = reader.GetString(4);
                                task.AssignedTo = reader.GetString(5);
                                task.DueDt = Convert.ToDateTime(reader.GetString(6));

                                tasks.Add(task);
                            }



                        }
                    }
                }

            }
            catch(Exception e) 
            {

                Console.WriteLine("error occured: " + e.StackTrace);
            }

                return tasks;
            }

        ///--------------------------------------------------------------------------------
        /// <summary> Get task by Id</summary>
        /// <returns> Returns model view of task </returns>
        ///--------------------------------------------------------------------------------
        private TaskViewModel GetTaskById(int id)
        {

            TaskViewModel task = new TaskViewModel();


            try
            {

                using (var connection = CreateAndOpenSqliteConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Task WHERE Id = @Id";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Id", id);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                task.Id = reader.GetInt32(0);
                                task.Title = reader.GetString(1);
                                task.Description = reader.GetString(2);
                                task.Status = reader.GetString(3);
                                task.CreatedBy = reader.GetString(4);
                                task.AssignedTo = reader.GetString(5);
                                task.DueDt = reader.GetDateTime(6);
                            }

                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("error occured: " + e.StackTrace);

            }

            return task;

        }

        ///--------------------------------------------------------------------------------
        /// <summary> Insert task </summary>
        ///--------------------------------------------------------------------------------
        private void InsertTask(TaskViewModel task)
        {

            try
            {

                using (var connection = CreateAndOpenSqliteConnection())
                {
                    string query = "INSERT INTO Task (Title, Description, Status, CreatedBy, AssignedTo, DueDt) " +
                    "VALUES (@Title, @Description, @Status, @CreatedBy, @AssignedTo, @DueDt)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Title", task.Title);
                        cmd.Parameters.AddWithValue("@Description", task.Description);
                        cmd.Parameters.AddWithValue("@Status", task.Status);
                        cmd.Parameters.AddWithValue("@CreatedBy", task.CreatedBy);
                        cmd.Parameters.AddWithValue("@AssignedTo", task.AssignedTo);
                        cmd.Parameters.AddWithValue("@DueDt", task.DueDt);
                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error occured: " + e.StackTrace);

            }

        }

        ///--------------------------------------------------------------------------------
        /// <summary> Update task </summary>
        ///--------------------------------------------------------------------------------
        private void UpdateTask(TaskViewModel task)
        {
            try
            {

                using (var connection = CreateAndOpenSqliteConnection())
                {

                    string query = "UPDATE Task " +
                    "SET Title = @Title, Description = @Description, Status = @Status, " +
                    "CreatedBy = @CreatedBy, AssignedTo = @AssignedTo, DueDt = @DueDt " +
                    "WHERE Id = @Id";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", task.Id);
                        cmd.Parameters.AddWithValue("@Title", task.Title);
                        cmd.Parameters.AddWithValue("@Description", task.Description);
                        cmd.Parameters.AddWithValue("@Status", task.Status);
                        cmd.Parameters.AddWithValue("@CreatedBy", task.CreatedBy);
                        cmd.Parameters.AddWithValue("@AssignedTo", task.AssignedTo);
                        cmd.Parameters.AddWithValue("@DueDt", task.DueDt);
                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("error occured: " + e.StackTrace);
            }

        }

        ///--------------------------------------------------------------------------------
        /// <summary> Delete task </summary>
        ///--------------------------------------------------------------------------------
        private void DeleteTask(int id)
        {

            try
            {

                using (var connection = CreateAndOpenSqliteConnection())
                {

                    string query = "DELETE FROM Task WHERE Id = @Id";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        var result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error occured: " + e.StackTrace);

            }

        }


        ///--------------------------------------------------------------------------------
        /// <summary> The error action. </summary>
        /// <returns> A response to return to the caller. </returns>
        ///--------------------------------------------------------------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        ///--------------------------------------------------------------------------------
        /// <summary> Create and open a connection to the local SQLite database. </summary>
        /// <returns> The SQLite connection. </returns>
        ///--------------------------------------------------------------------------------
        private SQLiteConnection CreateAndOpenSqliteConnection()
        {
            var sb = new SQLiteConnectionStringBuilder
            {
                DataSource = _sqliteDbPath,
                FailIfMissing = true
            };

            var connection = new SQLiteConnection(sb.ToString());
            try
            {
                return connection.OpenAndReturn();
            }
            catch
            {
                try
                {
                    connection.Dispose();
                }
                catch
                { }

                throw;
            }
        }
    }
}
