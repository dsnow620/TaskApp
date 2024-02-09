# Task Management Web Application

## Goal

Design a simple task management web application in ASP.NET CORE MVC to demonstrate your knowledge of C#, MVC, HTML, CSS, JavaScript, and SQL.

## Requirements

### Backend (C# / ASP.NET MVC)

- Populate TaskViewModel with the following properties: Id, Title, Description, Status, CreatedBy, AssignedTo, and DueDt. (You can add any additional properties that you require or think would be useful.)
- Implement CRUD operations for tasks using ASP.NET MVC.
- Implement a controller with actions to handle these CRUD operations.
- Use Entity Framework, ADO.NET, and/or SQLite for data access.

### Frontend (HTML, CSS, JavaScript)

- Create a simple HTML page with a form to add a new task.
- Display the list of tasks with options to edit and delete. Optionally, display the tasks in different sections based on their status. e.g., To Do, In Progress, Completed.
- Use CSS for styling to make the interface visually appealing.
- Use JavaScript for basic client-side validation to ensure that the user provides necessary information when adding or updating a task.

### Database (SQL)

- Use whatever technology you're most comfortable with for data storage. This could mean Entity Framework (using SQL Server Express or SQLite), ADO.NET, Dapper, non-relational JSON document storage, or some combination of these methods.
- To get you started, a SQLite database with a Task table has been included in the App_Data folder, and the HomeController.cs file contains a method that can be used to connect to this database. Feel free to use this database, or create your own using a completely different method.

## Additional Considerations

- Use JavaScript and AJAX to handle asynchronous operations (e.g., deleting a task without refreshing the page).
- Add comments as applicable to explain your thought process, as if this were production code.
- You can add and reference any open source NuGet packages that you need.

## Notes

- Completion time will vary based on your experience level. If you're unable to implement all of the features listed above in a reasonable amount of time (2-4 hours), that's OK! Focus on the areas you're most comfortable with. When you're finished, re-zip the project and submit it back to us. We'll review it, then schedule a time to discuss it with you.
