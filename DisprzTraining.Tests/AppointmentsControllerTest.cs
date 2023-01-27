using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.DataAccess;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Dto;
using FluentAssertions;
using DisprzTraining.Result;

namespace DisprzTraining.Tests
{

    public class AppointmentsControllerTest
    {
        private static IAppointmentDAL _appointmentDAL = new AppointmentDAL();
        private static IAppointmentBL _appointmentBL = new AppointmentBL(_appointmentDAL);
        AppointmentsController appointmentsController = new AppointmentsController(_appointmentBL);
        Appointment testAppointment = new Appointment()
        {
            Id = Guid.NewGuid(),
            Title = "Test Title",
            StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
            EndDateTime = new DateTime(2023, 02, 10, 12, 00, 00),
            Description = "Test Description",
            Routine = Routine.None
        };
        Appointment testAppointment2 = new Appointment()
        {
            Id = Guid.NewGuid(),
            Title = "Test Title",
            StartDateTime = new DateTime(2023, 02, 10, 13, 00, 00),
            EndDateTime = new DateTime(2023, 02, 10, 15, 00, 00),
            Description = "Test Description",
            Routine = Routine.None
        };
        Appointment testAppointment3 = new Appointment()
        {
            Id = Guid.NewGuid(),
            Title = "Test Title",
            StartDateTime = new DateTime(2023, 02, 11, 13, 00, 00),
            EndDateTime = new DateTime(2023, 02, 11, 15, 00, 00),
            Description = "Test Description",
            Routine = Routine.None
        };

        [Fact]
        public void Get_Appointments_With_Given_Date_SingleAppointment()
        {
            _appointmentDAL.TestList(testAppointment);
            var testList = new List<Appointment>() { testAppointment };

            var result = appointmentsController.Get("2023-02-10") as OkObjectResult;

            Assert.Equal(200, result?.StatusCode);
            Assert.IsType<List<Appointment>>(result?.Value);

            _appointmentDAL.DeleteAppointment(testAppointment.Id);
        }
        [Fact]
        public void Get_Appointments_With_Given_Date()
        {
            _appointmentDAL.TestList(testAppointment);
            _appointmentDAL.TestList(testAppointment2);
            _appointmentDAL.TestList(testAppointment3);
            var testList = new List<Appointment>() { testAppointment };

            var result = appointmentsController.Get("2023-02-10") as OkObjectResult;

            Assert.Equal(200, result?.StatusCode);
            Assert.IsType<List<Appointment>>(result?.Value);

            _appointmentDAL.DeleteAppointment(testAppointment.Id);
            _appointmentDAL.DeleteAppointment(testAppointment2.Id);
            _appointmentDAL.DeleteAppointment(testAppointment3.Id);
        }
        [Fact]
        public void Get_Appointments_With_Given_Someother_Date()
        {
            var testAppointment4 = new Appointment()
            {
                Id = Guid.NewGuid(),
                Title = "Test Title",
                StartDateTime = new DateTime(2023, 02, 09, 13, 00, 00),
                EndDateTime = new DateTime(2023, 02, 09, 15, 00, 00),
                Description = "Test Description",
                Routine = Routine.None
            };
            _appointmentDAL.TestList(testAppointment4);
            _appointmentDAL.TestList(testAppointment3);

            var result = appointmentsController.Get("2023-02-10") as OkObjectResult;

            Assert.Equal(200, result?.StatusCode);
            Assert.IsType<List<Appointment>>(result?.Value);

            _appointmentDAL.DeleteAppointment(testAppointment3.Id);
            _appointmentDAL.DeleteAppointment(testAppointment4.Id);
        }

        [Fact]
        public void Get_ErrorMessage_With_InvalidDate()
        {
            var errorResult = new Error() { error = "Invalid date format. Expected format : YYYY-MM-DD (i.e)2023-01-31" };
            var result = appointmentsController.Get("2023-31-01") as BadRequestObjectResult;

            Assert.Equal(400, result?.StatusCode);
            result?.Value.Should().BeEquivalentTo(errorResult);
        }

        [Fact]
        public void Create_Appointment_With_ValidAppointment_In_EmptyList()
        {
            AppointmentDto testAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 05, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 06, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };

            var result = appointmentsController.Create(testAppointmentDto) as CreatedResult;

            Assert.Equal(201, result?.StatusCode);
            var resultId = Assert.IsType<GuidObject>(result?.Value);
            _appointmentDAL.DeleteAppointment(resultId.id);
        }
        [Fact]
        public void Create_Appointment_With_ValidAppointment_In_AppointmentExistingList()
        {
            _appointmentDAL.TestList(testAppointment);
            _appointmentDAL.TestList(testAppointment3);
            AppointmentDto testAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 07, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 08, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };
            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 19, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 20, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };

            var result = appointmentsController.Create(testAppointmentDto) as CreatedResult;
            var result2 = appointmentsController.Create(testAppointmentDto2) as CreatedResult;
            var resultId = Assert.IsType<GuidObject>(result?.Value);
            var result2Id = Assert.IsType<GuidObject>(result2?.Value);
            var resultValue = _appointmentDAL.GetById(resultId.id);
            var result2Value = _appointmentDAL.GetById(resultId.id);

            Assert.Equal(201, result?.StatusCode);
            Assert.Equal(201, result2?.StatusCode);
            Assert.Equal(testAppointmentDto.Title , resultValue.Title);
            Assert.Equal(testAppointmentDto2.Title , result2Value.Title);
            _appointmentDAL.DeleteAppointment(testAppointment.Id);
            _appointmentDAL.DeleteAppointment(testAppointment3.Id);
            _appointmentDAL.DeleteAppointment(resultId.id);
            _appointmentDAL.DeleteAppointment(result2Id.id);
        }
        [Fact]
        public void Create_Appointment_With_ExistingAppointment_ReturnsConflict()
        {
            _appointmentDAL.TestList(testAppointment);
            _appointmentDAL.TestList(testAppointment3);
            AppointmentDto testAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 12, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };

            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 09, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 13, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };

            AppointmentDto testAppointmentDto3 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 11, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 13, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };

            var result = appointmentsController.Create(testAppointmentDto) as ConflictObjectResult;
            var result2 = appointmentsController.Create(testAppointmentDto2) as ConflictObjectResult;
            var result3 = appointmentsController.Create(testAppointmentDto3) as ConflictObjectResult;

            Assert.Equal(409, result?.StatusCode);
            Assert.Equal(409, result2?.StatusCode);
            Assert.Equal(409, result3?.StatusCode);
            Assert.IsType<Error>(result?.Value);
            Assert.IsType<Error>(result2?.Value);
            Assert.IsType<Error>(result3?.Value);
            result.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting already found between 10:00 AM and 12:00 PM on 10-02-2023" });
            result2.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting already found between 10:00 AM and 12:00 PM on 10-02-2023" });
            result3.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting already found between 10:00 AM and 12:00 PM on 10-02-2023" });
            _appointmentDAL.DeleteAppointment(testAppointment.Id);
            _appointmentDAL.DeleteAppointment(testAppointment3.Id);
        }
        [Fact]
        public void Create_Appointment_With_MultiDateSpan_ReturnsSuccess()
        {
            AppointmentDto testAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 08, 00, 00),
                Title = "ABC"
            };
            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 02, 08, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 09, 08, 00, 00),
                Title = "Check"
            };


            var result1 = appointmentsController.Create(testAppointmentDto) as CreatedResult;
            var result2 = appointmentsController.Create(testAppointmentDto2) as CreatedResult;

            var resultId1 = Assert.IsType<GuidObject>(result1?.Value);
            var resultValue1 = _appointmentDAL.GetById(resultId1.id);
            var resultId2 = Assert.IsType<GuidObject>(result2?.Value);
            var resultValue2 = _appointmentDAL.GetById(resultId2.id);
            
            Assert.Equal(201,result1?.StatusCode);
            Assert.Equal(201,result2?.StatusCode);
            Assert.Equal(testAppointmentDto.Title,resultValue1.Title);
            Assert.Equal(testAppointmentDto2.Title,resultValue2.Title);
            _appointmentDAL.DeleteAppointment(resultId1.id);
            _appointmentDAL.DeleteAppointment(resultId2.id);
        }
        [Fact]
        public void Create_Appointment_With_MultiDateSpan_ReturnsConflict()
        {
            AppointmentDto mockAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 08, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 08, 00, 00),
                Title = "ABC"
            };
            AppointmentDto mockAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 02, 13, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 13, 11, 00, 00),
                Title = "TestRoutine",
                Routine = Routine.Week
            };
            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 02, 06, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 08, 00, 00),
                Title = "ABC"
            };
            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 02, 08, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 08, 00, 00),
                Title = "ABC"
            };
            AppointmentDto testAppointmentDto3 = new()
            {
                StartDateTime = new DateTime(2023, 02, 07, 23, 00, 00),
                EndDateTime = new DateTime(2023, 02, 08, 12, 00, 00),
                Title = "ABC"
            };
            AppointmentDto testAppointmentDto4 = new()
            {
                StartDateTime = new DateTime(2023, 02, 13, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 14, 10, 00, 00),
                Title = "ABC"
            };
            var testMock = appointmentsController.Create(mockAppointmentDto) as CreatedResult;
            var testMock2 = appointmentsController.Create(mockAppointmentDto2) as CreatedResult;
            var testMockId = Assert.IsType<GuidObject>(testMock?.Value);
            var testMockId2 = Assert.IsType<GuidObject>(testMock2?.Value);

            var result1 = appointmentsController.Create(testAppointmentDto1) as ConflictObjectResult;
            var result2 = appointmentsController.Create(testAppointmentDto2) as ConflictObjectResult;
            var result3 = appointmentsController.Create(testAppointmentDto3) as ConflictObjectResult;
            var result4 = appointmentsController.Create(testAppointmentDto4) as ConflictObjectResult;
            
            Assert.Equal(409,result1?.StatusCode);
            Assert.Equal(409,result2?.StatusCode);
            Assert.Equal(409,result3?.StatusCode);
            Assert.Equal(409,result4?.StatusCode);
            _appointmentDAL.DeleteAppointment(testMockId.id);
            _appointmentDAL.DeleteRoutine(testMockId2.id);
        }
        [Fact]
        public void Create_Appointment_With_InvalidInput()
        {
            AppointmentDto testAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 08, 00, 00),
                Title = "ABC"
            };
            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 01, 01, 10, 00, 00),
                EndDateTime = new DateTime(2023, 01, 01, 12, 00, 00),
                Title = "ABC"
            };
            AppointmentDto testAppointmentDto3 = new()
            {
                StartDateTime = new DateTime(2023, 02, 01, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 01, 12, 00, 00),
                Title = "ABC",
                Routine = (Routine)3
            };

            var result1 = appointmentsController.Create(testAppointmentDto) as BadRequestObjectResult;
            var result2 = appointmentsController.Create(testAppointmentDto2) as BadRequestObjectResult;
            var result3 = appointmentsController.Create(testAppointmentDto3) as BadRequestObjectResult;

            Assert.Equal(400, result1?.StatusCode);
            Assert.Equal(400, result2?.StatusCode);
            Assert.Equal(400, result3?.StatusCode);

            result1?.Value.Should().BeEquivalentTo(new Error() { error = "Start Time should be lesser than End Time" });
            result2?.Value.Should().BeEquivalentTo(new Error() { error = "Appointment can't set for Past time" });
            result3?.Value.Should().BeEquivalentTo(new Error() { error = "Routine selected is invalid" });
        }

        [Fact]
        public void Delete_Appointment_With_ExistingAppointmentId()
        {
            _appointmentDAL.TestList(testAppointment);
            var result = appointmentsController.Delete(testAppointment.Id) as NoContentResult;

            Assert.Equal(204, result?.StatusCode);
        }
        [Fact]
        public void Delete_Appointment_With_UnknownAppointmentId()
        {
            var result = appointmentsController.Delete(testAppointment.Id) as NotFoundObjectResult;

            Assert.Equal(404, result?.StatusCode);
            result?.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting not found at {testAppointment.Id}" });
        }
        [Fact]
        public void Delete_Appointment_With_MultiDateSpan_ReturnsSuccess()
        {
            AppointmentDto testAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 08, 00, 00),
                Title = "ABC"
            };
            var testMock = appointmentsController.Create(testAppointmentDto) as CreatedResult;
            var testMockId = Assert.IsType<GuidObject>(testMock?.Value);
            var testMockValue = _appointmentDAL.GetById(testMockId.id);
            
            var result = appointmentsController.Delete(testMockId.id) as NoContentResult;
            var check = _appointmentDAL.GetById(testMockId.id);

            Assert.Equal(204,result?.StatusCode);
            Assert.Equal(Guid.Empty ,check.Id);
        }

        [Fact]
        public void Update_Appointment_With_UnknownAppointmentId()
        {
            AppointmentDto testAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 12, 00, 00),
                Title = "ABC"
            };
            var result = appointmentsController.Update(testAppointment.Id, testAppointmentDto) as NotFoundObjectResult;

            Assert.Equal(404, result?.StatusCode);
            result?.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting not found at Id: {testAppointment.Id}" });
        }
        [Fact]
        public void Update_Appointment_With_InvalidInput()
        {
            _appointmentDAL.TestList(testAppointment);
            AppointmentDto testAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 08, 00, 00),
                Title = "ABC"
            };
            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 01, 01, 10, 00, 00),
                EndDateTime = new DateTime(2023, 01, 01, 12, 00, 00),
                Title = "ABC"
            };

            var result1 = appointmentsController.Update(testAppointment.Id, testAppointmentDto) as BadRequestObjectResult;
            var result2 = appointmentsController.Update(testAppointment.Id, testAppointmentDto2) as BadRequestObjectResult;

            Assert.Equal(400, result1?.StatusCode);
            Assert.Equal(400, result2?.StatusCode);

            result1?.Value.Should().BeEquivalentTo(new Error() { error = "Start Time should be lesser than End Time" });
            result2?.Value.Should().BeEquivalentTo(new Error() { error = "Appointment can't set for Past time" });
            _appointmentDAL.DeleteAppointment(testAppointment.Id);
        }

        [Fact]
        public void Update_Appointment_With_ConflictingAppointment()
        {
            // Case 1 : Single day to single day meet
            _appointmentDAL.TestList(testAppointment);
            _appointmentDAL.TestList(testAppointment2);
            AppointmentDto updateAppointmentDto = new(){
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 12, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };

            var result1 = appointmentsController.Update(testAppointment2.Id,updateAppointmentDto) as ConflictObjectResult;

            Assert.Equal(409,result1?.StatusCode);
            result1?.Value.Should().BeEquivalentTo(new Error() {error = "Cannot set, Meeting already found between 10:00:AM - 12:00:PM on 10-02-2023"});

            // Case 2 : Single day to multi day appointment
            AppointmentDto updateAppointmentDto2 = new(){
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 02, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };

            var result2 = appointmentsController.Update(testAppointment2.Id,updateAppointmentDto) as ConflictObjectResult;

            Assert.Equal(409,result2?.StatusCode);
            result2?.Value.Should().BeEquivalentTo(new Error() {error = "Cannot set, Meeting already found between 10:00:AM - 12:00:PM on 10-02-2023"});

            // Case 3 : Multi day to single day appointment
            AppointmentDto testAppointmentDto3 = new(){
                StartDateTime = new DateTime(2023, 02, 10, 20, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 04, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };
            var testMock = appointmentsController.Create(testAppointmentDto3) as CreatedResult;
            var testMockValue = Assert.IsType<GuidObject>(testMock?.Value);

            var result3 = appointmentsController.Update(testMockValue.id,updateAppointmentDto) as ConflictObjectResult;
            
            Assert.Equal(409,result3?.StatusCode);
            result3?.Value.Should().BeEquivalentTo(new Error() {error = "Cannot set, Meeting already found between 10:00:AM - 12:00:PM on 10-02-2023"});

            // Case 4 : Multi day to multi day appointment
            AppointmentDto testAppointmentDto4 = new(){
                StartDateTime = new DateTime(2023, 02, 11, 20, 00, 00),
                EndDateTime = new DateTime(2023, 02, 12, 04, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };
            var testMock2 = appointmentsController.Create(testAppointmentDto4) as CreatedResult;
            var testMockValue2 = Assert.IsType<GuidObject>(testMock2?.Value);
            AppointmentDto updateAppointmentDto3 = new (){
                StartDateTime = new DateTime(2023, 02, 10, 21, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 01, 50, 00),
                Title = "ABC",
                Routine = Routine.None
            };
            AppointmentDto updateAppointmentDto4 = new (){
                StartDateTime = new DateTime(2023, 02, 11, 00, 00, 00),
                EndDateTime = new DateTime(2023, 02, 12, 02, 30, 00),
                Title = "ABC",
                Routine = Routine.None
            };

            var result4 = appointmentsController.Update(testMockValue2.id,testAppointmentDto3) as ConflictObjectResult;
            var result5 = appointmentsController.Update(testMockValue2.id,updateAppointmentDto3) as ConflictObjectResult;
            var result6 = appointmentsController.Update(testMockValue2.id,updateAppointmentDto4) as ConflictObjectResult;

            Assert.Equal(409,result4?.StatusCode);
            Assert.Equal(409,result5?.StatusCode);
            Assert.Equal(409,result6?.StatusCode);
            result4?.Value.Should().BeEquivalentTo(new Error() {error = "Cannot set, Meeting already found between 08:00:PM - 12:00:AM on 10-02-2023"});
            result5?.Value.Should().BeEquivalentTo(new Error() {error = "Cannot set, Meeting already found between 08:00:PM - 12:00:AM on 10-02-2023"});
            result6?.Value.Should().BeEquivalentTo(new Error() {error = "Cannot set, Meeting already found between 12:00:AM - 04:00:AM on 11-02-2023"});
            _appointmentDAL.DeleteAppointment(testAppointment.Id);
            _appointmentDAL.DeleteAppointment(testAppointment2.Id);
            _appointmentDAL.DeleteAppointment(testMockValue.id);
            _appointmentDAL.DeleteAppointment(testMockValue2.id);
        }
        
        [Fact]
        public void Update_Appointment_With_ValidAppointment_SingleAppointment_In_List()
        {
            _appointmentDAL.TestList(testAppointment);
            _appointmentDAL.TestList(testAppointment3);

            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 12, 00, 00),
                Title = "ABC",
                Routine = Routine.None
            };

            var result = appointmentsController.Update(testAppointment.Id, testAppointmentDto1) as NoContentResult;

            Assert.Equal(204, result?.StatusCode);
            var resultCheck = appointmentsController.GetById(testAppointment.Id) as OkObjectResult;
            var resultCheckObj = Assert.IsType<Appointment>(resultCheck?.Value);
            Assert.Equal(testAppointmentDto1.Title, resultCheckObj.Title);
            _appointmentDAL.DeleteAppointment(testAppointment.Id);
            _appointmentDAL.DeleteAppointment(testAppointment3.Id);
        }
        [Fact]
        public void Update_Appointment_With_ValidAppointment_In_AppointmentList()
        {
            _appointmentDAL.TestList(testAppointment);
            _appointmentDAL.TestList(testAppointment2);
            _appointmentDAL.TestList(testAppointment3);

            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 12, 00, 00),
                Title = "ABC"
            };

            var result = appointmentsController.Update(testAppointment.Id, testAppointmentDto1) as NoContentResult;

            Assert.Equal(204, result?.StatusCode);
            var resultCheck = appointmentsController.GetById(testAppointment.Id) as OkObjectResult;
            var resultCheckObj = Assert.IsType<Appointment>(resultCheck?.Value);
            Assert.Equal(testAppointmentDto1.Title, resultCheckObj.Title);
            _appointmentDAL.DeleteAppointment(testAppointment.Id);
            _appointmentDAL.DeleteAppointment(testAppointment2.Id);
            _appointmentDAL.DeleteAppointment(testAppointment3.Id);
        }
        [Fact]
        public void Update_Appointment_With_MultiDateSpan_ReturnsSuccess()
        {
            AppointmentDto testAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 08, 00, 00),
                Title = "ABC"
            };
            AppointmentDto testUpdateAppointmentDto = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 08, 00, 00),
                Title = "Updated Title"
            };
            var testMock = appointmentsController.Create(testAppointmentDto) as CreatedResult;
            var testMockId = Assert.IsType<GuidObject>(testMock?.Value);
            var testMockValue = _appointmentDAL.GetById(testMockId.id);

            var result = appointmentsController.Update(testMockId.id, testUpdateAppointmentDto) as NoContentResult;
            var check = _appointmentDAL.GetAppointment("2023-02-10");

            Assert.Equal(204,result?.StatusCode);
            Assert.Equal(testUpdateAppointmentDto.Title,check[0].Title);
            _appointmentDAL.DeleteAppointment(check[0].Id);
        }
        [Fact]
        public void Update_Appointment_With_MultiDateSpan_ReturnsConflict()
        {
            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 12, 00, 00),
                Title = "Update normal appointment"
            };
            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 22, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 02, 00, 00),
                Title = "Update multispan appointment"
            };
            AppointmentDto testAppointmentDto3 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 11, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 14, 00, 00),
                Title = "Update multispan appointment input"
            };
            AppointmentDto testAppointmentDto4 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 09, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 11, 00, 00),
                Title = "Update multispan appointment input"
            };
            AppointmentDto testAppointmentDto5 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 08, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 14, 00, 00),
                Title = "Update multispan appointment input"
            };
            AppointmentDto testAppointmentDto6 = new()
            {
                StartDateTime = new DateTime(2023, 02, 11, 08, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 14, 00, 00),
                Title = "Update multispan appointment input"
            };
            AppointmentDto testAppointmentDto7 = new()
            {
                StartDateTime = new DateTime(2023, 02, 11, 00, 00, 00),
                EndDateTime = new DateTime(2023, 02, 11, 10, 00, 00),
                Title = "Update multispan appointment input"
            };
            var testMock1 = appointmentsController.Create(testAppointmentDto1) as CreatedResult;
            var testMock2 = appointmentsController.Create(testAppointmentDto2) as CreatedResult;
            var testMock3 = appointmentsController.Create(testAppointmentDto6) as CreatedResult;
            var testMockId1 = Assert.IsType<GuidObject>(testMock1?.Value);
            var testMockId2 = Assert.IsType<GuidObject>(testMock2?.Value);
            var testMockId3 = Assert.IsType<GuidObject>(testMock3?.Value);

            var result1 = appointmentsController.Update(testMockId2.id, testAppointmentDto3) as ConflictObjectResult;
            var result2 = appointmentsController.Update(testMockId2.id, testAppointmentDto4) as ConflictObjectResult;
            var result3 = appointmentsController.Update(testMockId2.id, testAppointmentDto5) as ConflictObjectResult;
            var result4 = appointmentsController.Update(testMockId2.id, testAppointmentDto7) as ConflictObjectResult;
            var resultValue1 = Assert.IsType<Error>(result1?.Value);
            var resultValue2 = Assert.IsType<Error>(result2?.Value);
            var resultValue3 = Assert.IsType<Error>(result3?.Value);
            var resultValue4 = Assert.IsType<Error>(result4?.Value);

            Assert.Equal(409,result1?.StatusCode);
            Assert.Equal(409,result2?.StatusCode);
            Assert.Equal(409,result3?.StatusCode);
            Assert.Equal(409,result4?.StatusCode);
            resultValue1.Should().BeEquivalentTo(new Error() { error = $"Cannot set, Meeting already found between 10:00:AM - 12:00:PM on 10-02-2023" });
            resultValue2.Should().BeEquivalentTo(new Error() { error = $"Cannot set, Meeting already found between 10:00:AM - 12:00:PM on 10-02-2023" });
            resultValue3.Should().BeEquivalentTo(new Error() { error = $"Cannot set, Meeting already found between 10:00:AM - 12:00:PM on 10-02-2023" });
            resultValue4.Should().BeEquivalentTo(new Error() { error = $"Cannot set, Meeting already found between 08:00:AM - 02:00:PM on 11-02-2023" });
            _appointmentDAL.DeleteAppointment(testMockId1.id);
            _appointmentDAL.DeleteAppointment(testMockId2.id);
            _appointmentDAL.DeleteAppointment(testMockId3.id);
        }

        [Fact]
        public void Create_Routine_With_Week_ReturnSuccess()
        {
            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 02, 06, 08, 00, 00),
                EndDateTime = new DateTime(2023, 02, 06, 09, 00, 00),
                Title = "Monday meet",
                Routine = Routine.Week
            };
            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 02, 05, 09, 00, 00),
                EndDateTime = new DateTime(2023, 02, 05, 10, 00, 00),
                Title = "Sunday meet",
                Routine = Routine.Week
            };
            AppointmentDto testAppointmentDto3 = new()
            {
                StartDateTime = new DateTime(2023, 02, 04, 11, 00, 00),
                EndDateTime = new DateTime(2023, 02, 04, 12, 00, 00),
                Title = "Saturday meet",
                Routine = Routine.Week
            };

            var result1 = appointmentsController.Create(testAppointmentDto1) as CreatedResult;
            var result2 = appointmentsController.Create(testAppointmentDto2) as CreatedResult;
            var result3 = appointmentsController.Create(testAppointmentDto3) as CreatedResult;

            Assert.Equal(201, result1?.StatusCode);
            Assert.Equal(201, result2?.StatusCode);
            Assert.Equal(201, result3?.StatusCode);
            var result1Id = Assert.IsType<GuidObject>(result1?.Value);
            var result2Id = Assert.IsType<GuidObject>(result2?.Value);
            var result3Id = Assert.IsType<GuidObject>(result3?.Value);
            _appointmentDAL.DeleteRoutine(result1Id.id);
            _appointmentDAL.DeleteRoutine(result2Id.id);
            _appointmentDAL.DeleteRoutine(result3Id.id);
        }
        [Fact]
        public void Create_Routine_With_Week_ReturnsConflict()
        {
            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 03, 06, 10, 00, 00),
                EndDateTime = new DateTime(2023, 03, 06, 12, 00, 00),
                Title = "Monday meet",
                Routine = Routine.Week
            };
            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 03, 08, 17, 00, 00),
                EndDateTime = new DateTime(2023, 03, 08, 18, 00, 00),
                Title = "Wednesday meet",
                Routine = Routine.Week
            };
            var testMock1 = appointmentsController.Create(testAppointmentDto1) as CreatedResult;
            var testMock2 = appointmentsController.Create(testAppointmentDto2) as CreatedResult;
            var testMockObj1 = Assert.IsType<GuidObject>(testMock1?.Value);
            var testMockObj2 = Assert.IsType<GuidObject>(testMock2?.Value);

            AppointmentDto testAppointmentDto3 = new()
            {
                StartDateTime = new DateTime(2023, 03, 06, 11, 00, 00),
                EndDateTime = new DateTime(2023, 03, 06, 14, 00, 00),
                Title = "Conflicting Monday meet",
                Routine = Routine.Week
            };
            AppointmentDto testAppointmentDto4 = new()
            {
                StartDateTime = new DateTime(2023, 03, 06, 16, 00, 00),
                EndDateTime = new DateTime(2023, 03, 06, 19, 00, 00),
                Title = "Conflicting Wednesday meet",
                Routine = Routine.Week
            };

            var result1 = appointmentsController.Create(testAppointmentDto3) as ConflictObjectResult;
            var result2 = appointmentsController.Create(testAppointmentDto4) as ConflictObjectResult;

            Assert.Equal(409, result1?.StatusCode);
            Assert.Equal(409, result2?.StatusCode);
            Assert.IsType<Error>(result1?.Value);
            Assert.IsType<Error>(result2?.Value);
            result1.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting already found between 10:00 AM and 12:00 PM on 06-03-2023" });
            result2.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting already found between 05:00 PM and 06:00 PM on 08-03-2023" });
            _appointmentDAL.DeleteRoutine(testMockObj1.id);
            _appointmentDAL.DeleteRoutine(testMockObj2.id);
        }
        [Fact]
        public void Create_Routine_With_Month_ReturnSuccess()
        {
            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 02, 06, 08, 00, 00),
                EndDateTime = new DateTime(2023, 02, 06, 09, 00, 00),
                Title = "Test2Routine",
                Routine = Routine.Month
            };

            var result1 = appointmentsController.Create(testAppointmentDto1) as CreatedResult;
            var resultCheck = appointmentsController.GetRoutine() as OkObjectResult;
            var resultCheckObj = Assert.IsType<List<RoutineDto>>(resultCheck?.Value);

            Assert.Equal(201, result1?.StatusCode);
            var result1Id = Assert.IsType<GuidObject>(result1?.Value);
            Assert.True(resultCheckObj.Any(result=>result.Title == testAppointmentDto1.Title));
            var resultObjValue = _appointmentDAL.GetById(result1Id.id);
            _appointmentDAL.DeleteRoutine(result1Id.id);
        }
        [Fact]
        public void Create_Routine_With_Month_ReturnConflict()
        {
            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 12, 00, 00),
                Title = "Monday meet",
                Routine = Routine.Month
            };
            AppointmentDto testAppointmentDto2 = new()
            {
                StartDateTime = new DateTime(2023, 02, 10, 17, 00, 00),
                EndDateTime = new DateTime(2023, 02, 10, 18, 00, 00),
                Title = "Wednesday meet",
                Routine = Routine.Month
            };
            var testMock1 = appointmentsController.Create(testAppointmentDto1) as CreatedResult;
            var testMock2 = appointmentsController.Create(testAppointmentDto2) as CreatedResult;
            var testMockObj1 = Assert.IsType<GuidObject>(testMock1?.Value);
            var testMockObj2 = Assert.IsType<GuidObject>(testMock2?.Value);
            

            AppointmentDto testAppointmentDto3 = new()
            {
                StartDateTime = new DateTime(2023, 02, 06, 11, 00, 00),
                EndDateTime = new DateTime(2023, 02, 06, 14, 00, 00),
                Title = "Conflicting Monday meet",
                Routine = Routine.Month
            };
            AppointmentDto testAppointmentDto4 = new()
            {
                StartDateTime = new DateTime(2023, 02, 06, 16, 00, 00),
                EndDateTime = new DateTime(2023, 02, 06, 19, 00, 00),
                Title = "Conflicting Wednesday meet",
                Routine = Routine.Month
            };

            var result1 = appointmentsController.Create(testAppointmentDto3) as ConflictObjectResult;
            var result2 = appointmentsController.Create(testAppointmentDto4) as ConflictObjectResult;

            Assert.Equal(409, result1?.StatusCode);
            Assert.Equal(409, result2?.StatusCode);
            Assert.IsType<Error>(result1?.Value);
            Assert.IsType<Error>(result2?.Value);
            result1.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting already found between 10:00 AM and 12:00 PM on 10-02-2023" });
            result2.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting already found between 05:00 PM and 06:00 PM on 10-02-2023" });
            _appointmentDAL.DeleteRoutine(testMockObj1.id);
            _appointmentDAL.DeleteRoutine(testMockObj2.id);
        }   
        [Fact]
        public void Get_Routine_ReturnsOk_And_EmptyRoutineList()
        {
            var result = appointmentsController.GetRoutine() as OkObjectResult;

            var resultObj = Assert.IsType<List<RoutineDto>>(result?.Value);
            Assert.Equal(200, result.StatusCode);
            Assert.Empty(resultObj);
        }
        [Fact]
        public void Get_Routine_ReturnsOk_And_RoutineList()
        {
            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 02, 06, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 06, 12, 00, 00),
                Title = "Monday meet",
                Routine = Routine.Month
            };
            var testMock1 = appointmentsController.Create(testAppointmentDto1) as CreatedResult;
            var testMockResult = Assert.IsType<GuidObject>(testMock1?.Value);
            _appointmentDAL.TestList(testAppointment);

            var result = appointmentsController.GetRoutine() as OkObjectResult;

            var resultObj = Assert.IsType<List<RoutineDto>>(result?.Value);
            Assert.Single(resultObj);
            _appointmentDAL.DeleteAppointment(testAppointment.Id);
            _appointmentDAL.DeleteRoutine(resultObj[0].Id);
        }
        [Fact]
        public void Delete_Routine_ReturnsNoContent()
        {
            AppointmentDto testAppointmentDto1 = new()
            {
                StartDateTime = new DateTime(2023, 02, 06, 10, 00, 00),
                EndDateTime = new DateTime(2023, 02, 06, 12, 00, 00),
                Title = "Monday meet",
                Routine = Routine.Week
            };
            var testMock1 = appointmentsController.Create(testAppointmentDto1) as CreatedResult;
            var testMockObj = Assert.IsType<GuidObject>(testMock1?.Value);

            var result = appointmentsController.DeleteRoutine(testMockObj.id) as NoContentResult;
            Assert.Equal(204, result?.StatusCode);
            var resultCheck = appointmentsController.GetRoutine() as OkObjectResult;
            var resultCheckObj = Assert.IsType<List<RoutineDto>>(resultCheck?.Value);
            Assert.Empty(resultCheckObj);
        }
        [Fact]
        public void Delete_Routine_ReturnsNotFound()
        {
            var testId = Guid.NewGuid();
            var result = appointmentsController.DeleteRoutine(testId) as NotFoundObjectResult;
            Assert.Equal(404, result?.StatusCode);
            result?.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting not found at {testId}" });
        }

        [Fact]
        public void GetById_ReturnsOk()
        {
            _appointmentDAL.TestList(testAppointment);

            var result = appointmentsController.GetById(testAppointment.Id) as OkObjectResult;
            var resultObj = Assert.IsType<Appointment>(result?.Value);

            Assert.Equal(200, result?.StatusCode);
            Assert.Equal(testAppointment.Id, resultObj.Id);
            _appointmentDAL.DeleteAppointment(testAppointment.Id);
        }
        [Fact]
        public void GetById_ReturnsNotFound_With_UnknownAppointment()
        {
            var result = appointmentsController.GetById(testAppointment.Id) as NotFoundObjectResult;
            var resultObj = Assert.IsType<Error>(result?.Value);

            Assert.Equal(404, result?.StatusCode);
            result?.Value.Should().BeEquivalentTo(new Error() { error = $"Meeting not found at {testAppointment.Id}" });
        }
    }
}