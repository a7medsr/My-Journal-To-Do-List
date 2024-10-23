using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using just_do_it.Models;

public class UserViewController : Controller
{
    private readonly justdoitDbcontext _context;

    public UserViewController(justdoitDbcontext context)
    {
        _context = context;
    }

    public async Task<IActionResult> UserView(int id)
    {
        var user = await _context.Users
            .Include(u => u.Journalings)
            .Include(u => u.TasksDayslists)
                .ThenInclude(t => t.Tasks)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        return View(user);
    }

    // Journaling Actions
    public async Task<IActionResult> AddJournaling(int userId)
    {
        ViewBag.UserId = userId;
        return View();
    }
    public IActionResult  saveAddJournaling(string title, DateTime date,string content, int userid)
 {
     Journaling j1= new Journaling();
     j1.Content = content;
     j1.Date = date; 
     j1.UserId = userid;
     j1.Title= title;
     _context.Journalings.Add(j1);
     _context.SaveChanges();
     return RedirectToAction("UserView", new { id = userid });
 }     

    [HttpPost]
    public async Task<IActionResult> SaveJournaling(int userId, string title, string content)
    {
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
        {
            TempData["Error"] = "Title and content are required!";
            return RedirectToAction("AddJournaling", new { userId });
        }

        var journaling = new Journaling
        {
            UserId = userId,
            Title = title,
            Content = content,
            Date = DateTime.Now
        };

        _context.Journalings.Add(journaling);
        await _context.SaveChangesAsync();

        return RedirectToAction("UserView", new { id = userId });
    }

    public async Task<IActionResult> EditJournaling(int id)
    {
        var journaling = await _context.Journalings.FindAsync(id);
        if (journaling == null)
            return NotFound();

        return View(journaling);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateJournaling(int id, string title, string content)
    {
        var journaling = await _context.Journalings.FindAsync(id);
        if (journaling == null)
            return NotFound();

        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
        {
            TempData["Error"] = "Title and content are required!";
            return RedirectToAction("EditJournaling", new { id });
        }

        journaling.Title = title;
        journaling.Content = content;
        await _context.SaveChangesAsync();

        return RedirectToAction("UserView", new { id = journaling.UserId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteJournaling(int id)
    {
        var journaling = await _context.Journalings.FindAsync(id);
        if (journaling == null)
            return NotFound();

        var userId = journaling.UserId;
        _context.Journalings.Remove(journaling);
        await _context.SaveChangesAsync();

        return RedirectToAction("UserView", new { id = userId });
    }

    // Tasks Actions
    public async Task<IActionResult> AddTask(int userId, DateTime date)
    {
        var tasksList = await _context.TasksDayslists
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Date.Date == date.Date);

        if (tasksList == null)
        {
            tasksList = new TasksDayslist
            {
                UserId = userId,
                Date = date
            };
            _context.TasksDayslists.Add(tasksList);
            await _context.SaveChangesAsync();
        }

        ViewBag.TasksListId = tasksList.Id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SaveTask(int tasksListId, string name, string description, DateTime? dueDate)
    {
        if (string.IsNullOrEmpty(name))
        {
            TempData["Error"] = "Task name is required!";
            return RedirectToAction("AddTask", new { tasksListId });
        }

        var task = new just_do_it.Models.Task
        {
            TasksDayslistId = tasksListId,
            Name = name,
            Description = description,
            DueDate = dueDate
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var tasksList = await _context.TasksDayslists
            .Include(t => t.User)
            .FirstAsync(t => t.Id == tasksListId);

        return RedirectToAction("UserView", new { id = tasksList.UserId });
    }

    [HttpPost]
    public async Task<IActionResult> ToggleTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound();

        task.IsCompleted = !task.IsCompleted;
        await _context.SaveChangesAsync();

        return Json(new { isCompleted = task.IsCompleted });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteTaskList(int id)
    {
        var taskList = await _context.TasksDayslists
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (taskList == null)
            return NotFound();

        var userId = taskList.UserId;
        _context.Tasks.RemoveRange(taskList.Tasks);
        _context.TasksDayslists.Remove(taskList);
        await _context.SaveChangesAsync();

        return RedirectToAction("UserView", new { id = userId });
    }
}