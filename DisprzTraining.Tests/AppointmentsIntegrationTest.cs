using System.Net;
using DisprzTraining.Dto;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using DisprzTraining.Result;

namespace Appointments.API.IntegrationTests
{
    public class AppointmentIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AppointmentIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        string getUrl = "/api/v1/appointments?date=";
        string createUrl = "/api/v1/appointments";
        string deleteUrl = "/api/v1/appointments/";
        string getByIdUrl = "/api/v1/appointments/";
        string updateUrl = "/api/v1/appointments/";
        string getRoutineUrl = "/api/v1/routines";
        string deleteRoutineUrl = "/api/v1/routines/";

        [Fact]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description"
            };

            // Act
            var testMockData = await client.PostAsync(createUrl, ContentHelper.GetStringContent(testAppointmentDto));
            var value = JsonConvert.DeserializeObject<GuidObject>(await testMockData.Content.ReadAsStringAsync());

            var response1 = await client.GetAsync(getUrl + "2023-02-10");
            var response2 = await client.GetAsync(getUrl + "2023-02-27");

            // Assert
            response1.EnsureSuccessStatusCode(); // Status Code 200-299
            response2.EnsureSuccessStatusCode(); // Status Code 200-299
            var response2Object = JsonConvert.DeserializeObject<List<Appointment>>(await response2.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.Equal(value?.id, response2Object?[0].Id);
            var response = await client.DeleteAsync(deleteUrl + value?.id);
        }
        [Fact]
        public async Task Get_EndpointsReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            
            // Act
            var response = await client.GetAsync(getUrl + "2023-31-01");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Create_EndpointsReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description",
                Routine = Routine.None
            };

            // Act
            var response = await client.PostAsync(createUrl, ContentHelper.GetStringContent(testAppointmentDto));
            var value = JsonConvert.DeserializeObject<GuidObject>(await response.Content.ReadAsStringAsync());

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var clearData = await client.DeleteAsync(deleteUrl + value?.id);
        }
        [Fact]
        public async Task Create_EndpointsReturnConflict()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description",
                Routine = Routine.None
            };

            // Act
            var testMOckData = await client.PostAsync(createUrl, ContentHelper.GetStringContent(testAppointmentDto));
            var value = JsonConvert.DeserializeObject<GuidObject>( await testMOckData.Content.ReadAsStringAsync());
            var response = await client.PostAsync(createUrl, ContentHelper.GetStringContent(testAppointmentDto));

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            var clearData = await client.DeleteAsync(deleteUrl + value?.id);
        }
        [Fact]
        public async Task Create_EndpointsReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2022,02,27,05,00,00),
                EndDateTime = new DateTime(2022,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description",
                Routine = Routine.None
            };

            // Act
            var response = await client.PostAsync(createUrl, ContentHelper.GetStringContent(testAppointmentDto));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Delete_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description",
                Routine = Routine.None
            };

            // Act
            var testMockData = await client.PostAsync(createUrl,ContentHelper.GetStringContent(testAppointmentDto));
            var value = JsonConvert.DeserializeObject<GuidObject>(await testMockData.Content.ReadAsStringAsync());
            var response = await client.DeleteAsync(deleteUrl + value?.id);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task Delete_EndpointsReturnNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync(deleteUrl + Guid.NewGuid());

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task Update_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description"
            };
            AppointmentDto testUpdateAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Updated title",
                Description = "Test Description"
            };

            // Act
            var testMockData = await client.PostAsync(createUrl,ContentHelper.GetStringContent(testAppointmentDto));
            var value = JsonConvert.DeserializeObject<GuidObject>(await testMockData.Content.ReadAsStringAsync());
            var response = await client.PutAsync(updateUrl + value?.id, ContentHelper.GetStringContent(testUpdateAppointmentDto));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var clearData = await client.DeleteAsync(deleteUrl + value?.id);
        }
        [Fact]
        public async Task Update_EndpointsReturnNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description"
            };
            AppointmentDto testUpdateAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Updated title",
                Description = "Test Description"
            };

            // Act
            var response = await client.PutAsync(updateUrl + Guid.NewGuid(), ContentHelper.GetStringContent(testUpdateAppointmentDto));

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task Update_EndpointsReturnBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description"
            };
            AppointmentDto testUpdateAppointmentDto = new() {
                StartDateTime = new DateTime(2022,02,27,05,00,00),
                EndDateTime = new DateTime(2022,02,27,06,00,00),
                Title = "Updated title",
                Description = "Test Description"
            };

            // Act
            var testMockData = await client.PostAsync(createUrl,ContentHelper.GetStringContent(testAppointmentDto));
            var value = JsonConvert.DeserializeObject<GuidObject>(await testMockData.Content.ReadAsStringAsync());
            var response = await client.PutAsync(updateUrl + value?.id, ContentHelper.GetStringContent(testUpdateAppointmentDto));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var clearData = await client.DeleteAsync(deleteUrl + value?.id);
        }

        [Fact]
        public async Task GetRoutine_EndpointsReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new(){
                StartDateTime = new DateTime(2023,11,03,11,45,00),
                EndDateTime = new DateTime(2023,11,03,12,45,00),
                Title = "Test Meet",
                Description = "",
                Routine = Routine.Week
            };

            // Act
            var testMockData = await client.PostAsync(createUrl,ContentHelper.GetStringContent(testAppointmentDto));
            var value = JsonConvert.DeserializeObject<GuidObject>(await testMockData.Content.ReadAsStringAsync());

            var response = await client.GetAsync(getRoutineUrl);
            var responseValueObject = JsonConvert.DeserializeObject<List<RoutineDto>>(await response.Content.ReadAsStringAsync());

            // Assert
            testMockData.EnsureSuccessStatusCode();
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1,responseValueObject?.Count);
            var clearData = await client.DeleteAsync(deleteRoutineUrl + value?.id);
        }
        [Fact]
        public async Task DeleteRoutine_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description",
                Routine = Routine.Week
            };

            // Act
            var testMockData = await client.PostAsync(createUrl,ContentHelper.GetStringContent(testAppointmentDto));
            var value = JsonConvert.DeserializeObject<GuidObject>(await testMockData.Content.ReadAsStringAsync());
            var response = await client.DeleteAsync(deleteRoutineUrl + value?.id);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task DeleteRoutine_EndpointsReturnNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync(deleteRoutineUrl + Guid.NewGuid());

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task GetById_EndpointReturnsOK()
        {
            var client = _factory.CreateClient();
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,27,05,00,00),
                EndDateTime = new DateTime(2023,02,27,06,00,00),
                Title = "Test title",
                Description = "Test Description"
            };

            var testMock1 = await client.PostAsync(createUrl, ContentHelper.GetStringContent(testAppointmentDto));
            var testMockValue = JsonConvert.DeserializeObject<GuidObject>(await testMock1.Content.ReadAsStringAsync());

            var response = await client.GetAsync(getByIdUrl + testMockValue?.id);
            var responseObj = JsonConvert.DeserializeObject<Appointment>(await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(testMockValue?.id , responseObj?.Id);
            var clearData = await client.DeleteAsync(deleteUrl + testMockValue?.id);
        }
        [Fact]
        public async Task GetById_EndpointReturnNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(getByIdUrl + Guid.NewGuid());

            // Asert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}