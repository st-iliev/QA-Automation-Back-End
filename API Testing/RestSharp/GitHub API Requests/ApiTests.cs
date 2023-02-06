using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace GitHub_API_Requests
{
    public class ApiTests
    {
        private RestClient client;
        private RestRequest request;
        private const string baseURL = "https://api.github.com";
        private const string partialURL = "/repos/WRITE YOUR USERNAME/WRITE YOUR REPOSITORY/issues";
        private const string username = "WRITE YOUR USERNAME";
        private const string token = "WRITE YOUR TOKEN";


        [SetUp]
        public void Setup()
        {
            client = new RestClient(baseURL);
            client.Authenticator = new HttpBasicAuthenticator(username, token);
            request = new RestRequest(partialURL);
        }
        private Issue CreateIssue(string title, string body)
        {
            request.AddBody(new { title, body });
            var response = client.Execute(request,Method.Post);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            return issue;
        }
        private List<Issue> GetAllIssues()
        {
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);
            return issues;
        }
        private Issue EditIssue(string title,long issueNumber,HttpStatusCode statusCode)
        {
            request = new RestRequest($"{partialURL}/{issueNumber}", Method.Post);
            request.AddBody(new { title });
            var response = client.Execute(request, Method.Patch);
            Assert.That(response.StatusCode, Is.EqualTo(statusCode));
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            return issue;
        }
        private List<Comment> GetAllIssuueComments(long issueNumber)
        {
            var request = new RestRequest($"{partialURL}/{issueNumber}/comments", Method.Get);
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var issueComments = JsonSerializer.Deserialize<List<Comment>>(response.Content);
            return issueComments;
        }
        private List<Label> GetAllIssuueLabels(long issueNumber)
        {
            var request = new RestRequest($"{partialURL}/{issueNumber}/labels", Method.Get);
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var issueLabels = JsonSerializer.Deserialize<List<Label>>(response.Content);
            return issueLabels;
        }
        private Comment EditIssueComment(string body, long commentid, HttpStatusCode statusCode)
        {
            request = new RestRequest($"{partialURL}/comments/{commentid}", Method.Patch);
            request.AddBody(new { body });
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(statusCode));
            var comment = JsonSerializer.Deserialize<Comment>(response.Content);
            return comment;
        }
        private void DeleteIssueComment(long commentid, HttpStatusCode statusCode)
        {
            request = new RestRequest($"{partialURL}/comments/{commentid}", Method.Delete);
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(statusCode));
        }
        private long GetIssuesCommentid()
        {
            var request = new RestRequest($"{partialURL}/comments", Method.Get);
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var issues = JsonSerializer.Deserialize<List<Comment>>(response.Content);
            return issues[0].Id;
        }
        [Test]
        [Category("Positive")]
        public void Get_AllIssuesFromRepo()
        {
            var issues = GetAllIssues();
            Assert.That(issues.Count(), Is.GreaterThan(0));
            foreach (var issue in issues)
            {
                Assert.Greater(issue.Id, 0);
                Assert.Greater(issue.Number, 0);
                Assert.IsNotEmpty(issue.Title);
            }
        }
        [Test]
        [Category("Positive")]
        public void Get_IssueLabels()
        {   
            var issueLabels = GetAllIssuueLabels(3);
            Assert.That(issueLabels.Count > 0);
            foreach (var label in issueLabels)
            {
                Assert.Greater(label.Id, 0);
                Assert.IsNotNull(label.Name);
                Assert.IsNotNull(label.Description);
            }
        }
        [Test]
        [Category("Positive")]
        public void Get_IssueComments()
        {
            var issueComments = GetAllIssuueComments(3);
            Assert.That(issueComments.Count > 0);
            foreach (var comment in issueComments)
            {
                Assert.Greater(comment.Id, 0);
                Assert.IsNotNull(comment.Body);
            }
        }
        [Test]
        [Category("Positive")]
        public void Get_IssueComment_Byid()
        {
            var comments = GetAllIssuueComments(3);
            Comment initialComment = comments[0];
            var request = new RestRequest($"{partialURL}/comments/{initialComment.Id}", Method.Get);
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var issueComments = JsonSerializer.Deserialize<Comment>(response.Content);
            Assert.Greater(issueComments.Id, 0);
            Assert.IsNotNull(issueComments.Body);
        }
        [Test]
        [Category("Positive")]
        public void Get_IssueComment_ByInvalidid()
        {
            var request = new RestRequest($"{partialURL}/comments/141093858", Method.Get);
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
           
        }
        [Test]
        [Category("Positive")]
        public void Get_IssueByNumber()
        {
            var request = new RestRequest($"{partialURL}/4", Method.Get);
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            Assert.That(issue.Title, Is.EqualTo("Testing Issue"));
            Assert.That(issue.Number, Is.EqualTo(4));
        }
        [Test]
        [Category("Negative")]
        public void Get_IssueByInvalidNumber()
        {
            var request = new RestRequest($"{partialURL}/-3", Method.Get);
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        [Category("Positive")]
        public void Create_Issue()
        {
            var title = "New Test Issue";
            var body = "API Test";
            var issue = CreateIssue(title, body);
            Assert.That(issue.Title, Is.EqualTo(title));
            Assert.That(issue.Body, Is.EqualTo(body));          
        }
        [Test]
        [Category("Negative")]
        public void Create_InvalidIssue()
        {
            var body = "API Test";
            request = new RestRequest(partialURL, Method.Post);
            request.AddBody(new {body });
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
        }
        [Test]
        [Category("Negative")]
        public void Craete_Issue_WithoutAuth()
        {
            client.Authenticator = null;
            var title = "Test Issue";
            var body = "API Test";
            request = new RestRequest(partialURL, Method.Post);
            request.AddBody(new { title,body });
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        [Category("Positive")]
        public void Crate_NewIssueComment()
        {
            var body = "New test comment";
            request = new RestRequest($"{partialURL}/3/comments", Method.Post);
            request.AddBody(new { body });
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }
        [Test]
        [Category("Negative")]
        public void Crate_NewIssueComment_WithoutAuth()
        {
            client.Authenticator = null;
            var body = "New test comment";
            request = new RestRequest($"{partialURL}/3/comments", Method.Post);
            request.AddBody(new { body });
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        [Category("Positive")]
        public void Edit_ExistingIssue()
        {
            var allIssues = GetAllIssues();
            var issueNumber = allIssues.Count - 1;
            var issue = EditIssue("Edited Issue", issueNumber,HttpStatusCode.OK);
            Assert.That(issue.Title, Is.EqualTo("Edited Issue"));
            Assert.That(issue.Number, Is.EqualTo(issueNumber));
        }
        [Test]
        [Category("Negative")]
        public void Edit_NonExistingIssue()
        {
            var issue = EditIssue("Test Issue",-159678, HttpStatusCode.NotFound);
            Assert.IsNull(issue.Id);
            Assert.IsNull(issue.Number);
        }
        [Test]
        [Category("Negative")]
        public void Edit_ExistingIssue_WithoutAuth()
        {
            client.Authenticator = null;
            var allIssues = GetAllIssues();
            var issueNumber = allIssues.Count - 1;
            var issue = EditIssue("Test Issue", issueNumber, HttpStatusCode.Unauthorized);
        }
        [Test]
        [Category("Positive")]
        public void Edit_Comment_Byid()
        {
            var comment = EditIssueComment("Edited comment", GetIssuesCommentid(), HttpStatusCode.OK);
            Assert.Greater(comment.Id ,0);
            Assert.That(comment.Body, Is.EqualTo("Edited comment"));
        }
        [Test]
        [Category("Negative")]
        public void Edit_Comment_Byid_WithoutAuth()
        {
            client.Authenticator = null;
            var comments = GetAllIssuueComments(3);
            Comment initialComment = comments[0];
            var comment = EditIssueComment("Edited comment", initialComment.Id, HttpStatusCode.Unauthorized);
        }
        [Test]
        [Category("Negative")]
        public void Edit_Comment_ByInvalidid()
        {
            var comment = EditIssueComment("Edited comment", -1410971176, HttpStatusCode.NotFound);
        }
        [Test]
        [Category("Positive")]
        public void Close_Issue()
        {
            var allIssues = GetAllIssues();
            var issueNumber = allIssues.Count - 1;
            var request = new RestRequest($"{partialURL}/{issueNumber}", Method.Patch);
            request.AddJsonBody(new{ state = "closed" });
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            Assert.That(issue.State, Is.EqualTo("closed"));
        }
        [Test]
        [Category("Negative")]
        public void Close_InvalidIssue()
        {
            var request = new RestRequest($"{partialURL}/-19875", Method.Patch);
            request.AddJsonBody(new { state = "closed" });
            var response = client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        [Category("Positive")]
        public void Delete_Comment_Byid()
        {
            var comments = GetAllIssuueComments(3);
            Comment initialComment = comments[0];
            DeleteIssueComment(initialComment.Id, HttpStatusCode.NoContent);
            comments = GetAllIssuueComments(3);
            Assert.That(comments[0].Id, Is.Not.EqualTo(initialComment.Id));
        }
        [Test]
        [Category("Negative")]
        public void Delete_Comment_ByInvalidid()
        {
            DeleteIssueComment(16549879512, HttpStatusCode.NotFound);
        }
        [Test]
        [Category("Positive")]
        public void Delete_Comment_Byid_WithoutAuth()
        {
            client.Authenticator = null;
            var comments = GetAllIssuueComments(3);
            Comment initialComment = comments[0];
            DeleteIssueComment(initialComment.Id, HttpStatusCode.Unauthorized);
        }
    }
}