using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController(TmsDbContext context) : ControllerBase
{
    // GET /api/reports/students?page=1
    [HttpGet("students")]
    public async Task<IActionResult> GetPagedStudents(int page = 1, CancellationToken cancellationToken = default)
    {
        const int pageSize = 20;

        var students = await context.Students
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return Ok(students);
    }

    // GET /api/reports/top-courses
    [HttpGet("top-courses")]
    public async Task<IActionResult> GetTopCourses(CancellationToken cancellationToken = default)
    {
        var topCourses = await context.Courses
            .AsNoTracking()
            .Select(c => new
            {
                c.Title,
                EnrollmentCount = c.Enrollments.Count
            })
            .OrderByDescending(x => x.EnrollmentCount)
            .Take(5)
            .ToListAsync(cancellationToken);

        return Ok(topCourses);
    }
}