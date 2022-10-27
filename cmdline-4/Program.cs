using System.CommandLine;

var parser = new CommandLineParser("Get information about a GitHub issue");
var number = parser.AddArgument<uint>("number");
var title = parser.AddOption("title", 't');
var status = parser.AddOption("status", 's');
var milestone = parser.AddOption("milestone", 'm');
var assignees = parser.AddOption(new[] {"assignees", "assignee" });
var created = parser.AddOption(new[] { "created", "opened" }, new[] { 'c', 'o' });
var author = parser.AddOption("author", 'a');

parser.Parse(args);

var issue = await GetIssue(number.Value);

Console.WriteLine($"Issue {number.Value}:");

if (title.Value) Console.WriteLine($" - Title: {issue.Title}");
if (status.Value) Console.WriteLine($" - Status: {issue.Status}");
if (milestone.Value) Console.WriteLine($" - Milestone: {issue.Milestone}");
if (assignees.Value) Console.WriteLine($" - Assignees: {string.Join(", ", issue.Assignees)}{(issue.Assignees.Length > 0 ? " " : "")}({issue.Assignees.Length})");
if (created.Value) Console.WriteLine($" - Created: {issue.Created.ToShortDateString()}");
if (author.Value) Console.WriteLine($" - Author: {issue.Author}");

Task<Issue> GetIssue(uint issueNumber)
{
    return Task.FromResult(new Issue
    {
        Number = issueNumber,
        Title = "[API Proposal]: Implement decimal floating-point types that conform to the IEEE 754 standard.",
        Status = "Open",
        Milestone = "Future",
        Assignees = new string[] { },
        Created = new DateOnly(2022, 05, 24),
        Author = "KTSnowy",
    });
}

public struct Issue
{
    public uint Number;
    public string Title;
    public string Status;
    public string Milestone;
    public string[] Assignees;
    public DateOnly Created;
    public string Author;
}