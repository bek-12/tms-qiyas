
string? region = null;
string? upperRegion = region?.ToUpper();
Console.WriteLine($"Region (conditional): {upperRegion}");

string displayRegion = region ?? "Unassigned";
Console.WriteLine($"Region (coalesced): {displayRegion}");

region ??= "Addis Ababa";
Console.WriteLine($"Region (assigned): {region}");

double grantPerStudent = 1999.99;
double totalAllocation = grantPerStudent * 100_000;
Console.WriteLine($"Total allocated (double): {totalAllocation}");

decimal grantPerStudentD = 1999.99m;
decimal totalAllocationD = grantPerStudentD * 100_000m;
Console.WriteLine($"Total allocated (decimal): {totalAllocationD:F2}");

var enrollment = new EnrollmentRecord("STU-001", "CS-401", DateTime.UtcNow);
Console.WriteLine(enrollment);

var corrected = enrollment with { CourseCode = "CS-402" };
Console.WriteLine(corrected);

var duplicate = new EnrollmentRecord("STU-001", "CS-401", enrollment.EnrolledAt);
Console.WriteLine($"Same data? {enrollment == duplicate}");

var course = new Course { Code = "CS-401", Title = "Advanced C#", Capacity = 30 };
Console.WriteLine($"Course: {course.Title} (Capacity: {course.Capacity})");

try
{
    var bad = new Course { Code = "X", Title = "Y", Capacity = -5 };
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Caught at construction: {ex.Message}");
}
try
{
    course.Capacity = -5;
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

try
{
    course.Title = "";
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

var s = new Student { Id = "S1", Name = "Abeba", Age = 20, GPA = 3.8m };
Console.WriteLine($"Student: {s.Name}, GPA: {s.GPA}");

void PrintGradeReport(IEnumerable<IGradable> assessments)
{
    Console.WriteLine("--- Grade Report ---");
    foreach (var item in assessments)
    {
        Console.WriteLine($"{item.Title}: {item.CalculateGrade():F2}%");
    }
}

IGradable[] cohortAssessments = [
    new Quiz { Title = "C# Basics", CorrectAnswers = 18, TotalQuestions = 20 },
    new LabAssignment { Title = "Registration API", FunctionalityScore = 90m, CodeQualityScore = 85m }
];
PrintGradeReport(cohortAssessments);