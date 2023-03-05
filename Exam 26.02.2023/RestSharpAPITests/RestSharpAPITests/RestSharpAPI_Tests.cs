using RestSharp;
using System.Net;
using System.Text.Json;

namespace RestSharpAPITests
{
    public class RestSharpAPI_Tests
    {
        private const string baseURL = "https://contactbook.stanislaviliev.repl.co";
        private RestClient client;
        public RestSharpAPI_Tests()
        {
            client = new RestClient(baseURL);
        }
        [Test]
        public void Get_AllContacts()
        {
            string firstName = "Steve";
            string lastName = "Jobs";

            var request = new RestRequest("/api/contacts", Method.Get);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            Assert.That(contacts[0].FirstName, Is.EqualTo(firstName));
            Assert.That(contacts[0].LastName, Is.EqualTo(lastName));
        }

        [Test]
        public void Get_Contact_by_ValidKeyword()
        {
            string firstName = "Albert";
            string lastName = "Einstein";

            var request = new RestRequest("/api/contacts/search/albert", Method.Get);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            Assert.That(contacts[0].FirstName, Is.EqualTo(firstName));
            Assert.That(contacts[0].LastName, Is.EqualTo(lastName));
        }

        [Test]
        public void Get_Contact_by_InvalidKeyword()
        {
            var request = new RestRequest("/api/contacts/search/missing" + DateTime.Now.Ticks, Method.Get);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Assert.That(response.Content, Is.EqualTo("[]"));
        }
        [Test]
        public void Post_CreateNewContact_With_InvalidData()
        {
            var request = new RestRequest("/api/contacts", Method.Post);
            var reqBody = new
            {
                lastName = "James"
            };
            request.AddBody(reqBody);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"First name cannot be empty!\"}"));

        }
        [Test]
        public void Post_CreateNewContact_With_ValidData()
        {
            string msg = "Contact added.";
            var request = new RestRequest("/api/contacts", Method.Post);
            var reqBody = new
            {
                firstName = "Lebron",
                lastName = "James",
                phone = "+3590000000",
                email = "james@gmail.bg",
                comments = "The King"
            };
            request.AddBody(reqBody);
            var response = this.client.Execute(request);
            var contactObj = JsonSerializer.Deserialize<ContactObj>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(contactObj.Msg, Is.EqualTo(msg));
            Assert.That(contactObj.Contact.Id, Is.GreaterThan(0));
            Assert.That(contactObj.Contact.FirstName, Is.EqualTo(reqBody.firstName));
            Assert.That(contactObj.Contact.LastName, Is.EqualTo(reqBody.lastName));
            Assert.That(contactObj.Contact.Phone, Is.EqualTo(reqBody.phone));
            Assert.That(contactObj.Contact.Email, Is.EqualTo(reqBody.email));
            Assert.That(contactObj.Contact.DateCreated, Is.Not.Empty);
            Assert.That(contactObj.Contact.Comments, Is.Not.Empty);
        }
    }
}