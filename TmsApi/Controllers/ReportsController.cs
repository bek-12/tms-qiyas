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

    // Part A: intentional N+1 - for learning only
    [HttpGet("n-plus-one-demo")]
    public async Task<IActionResult> NPlusOneDemo(CancellationToken cancellationToken)
    {
        var students = await context.Students.AsNoTracking().ToListAsync(cancellationToken);

        var results = new List<string>();
        foreach (var s in students)
        {
            var count = await context.Enrollments
                .AsNoTracking()
                .CountAsync(e => e.StudentId == s.Id, cancellationToken);

            results.Add($"{s.Name}: {count} enrollments");
        }

        return Ok(results);
    }

    // Part B: fixed with a single shaped query
    [HttpGet("enrollment-counts")]
    public async Task<IActionResult> EnrollmentCounts(CancellationToken cancellationToken)
    {
        var report = await context.Students
            .AsNoTracking()
            .Select(s => new
            {
                s.Name,
                EnrollmentCount = s.Enrollments.Count
            })
            .ToListAsync(cancellationToken);

        return Ok(report);
    }
     // POST /api/reports/archive-old-enrollments
    [HttpPost("archive-old-enrollments")]
    public async Task<IActionResult> ArchiveOldEnrollments(CancellationToken cancellationToken)
    {
        var cutoff = DateTime.UtcNow.AddYears(-1);

        var affected = await context.Enrollments
            .Where(e => e.EnrolledAt < cutoff && !e.IsArchived)
            .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsArchived, true), cancellationToken);

        return Ok(new { archivedCount = affected });
    }
}