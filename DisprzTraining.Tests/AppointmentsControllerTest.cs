using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.DataAccess;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisprzTraining.Dto;
using DisprzTraining.Extensions;
using FluentAssertions;

namespace DisprzTraining.Tests{
    
    public class AppointmentsControllerTest{

        [Fact]
        public async Task Get_WithDate_OkObjectResult(){
            // Arrange
            var testAppointmentList = new List<Appointment>() {new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC")};
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.GetAsync("2022-12-12")).ReturnsAsync(testAppointmentList);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act 
            var result = await sut.Get("2022-12-12");

            // Assert
            Assert.Equal(testAppointmentList,result.Value);
            Assert.Equal(200,result.StatusCode);          
        }
        
        [Fact]
        public async Task Get_WithId_OkObjectResult(){
            // Arrange
            var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var testAppointmentDto = testAppointment.AsDto();
            var testAppointmentList = new List<Appointment>();
            testAppointmentList.Add(testAppointment);
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.GetIdAsync(testAppointment.Id)).ReturnsAsync(testAppointment);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.GetById(testAppointment.Id);

            // Assert
            // Assert.AreEqual(testAppointmentDto,result.Value);
            testAppointmentDto.Should().BeEquivalentTo(result.Value);
            Assert.Equal(200,result.StatusCode);
        }

        // [Fact]  
        // public async Task Create_WithNewAppointment_OkResult(){
        //     // Arrange
        //     var MockAppointment = new Mock<IAppointmentBL>();
        //     AppointmentDto testAppointmentDto = new() {
        //         Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
        //         StartDateTime = new DateTime(2022,12,12,10,00,00),
        //         EndDateTime = new DateTime(2022,12,12,12,00,00),
        //         Title = "ABC"
        //     };
        //     Appointment testAppointment = new() {
        //         Id = testAppointmentDto.Id,
        //         StartDateTime = testAppointmentDto.StartDateTime,
        //         EndDateTime = testAppointmentDto.EndDateTime,
        //         Title = testAppointmentDto.Title
        //     };
        //     var testAppointmentList = new List<Appointment>();
        //     MockAppointment.Setup(t=>t.CreateAsync(testAppointment)).ReturnsAsync(true);
        //     var sut = new AppointmentsController(MockAppointment.Object);

        //     // Act
        //     var result = await sut.Create(testAppointmentDto);

        //     // Assert
                
        //     // Assert.Equals(201,result.StatusCode);
        // }

        [Fact]
        public async Task Create_WithNewAppointment_CreatedResult()
        {
            // Arrange
            var appointmentDto = new AppointmentDto(){
                Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                StartDateTime = new DateTime(2022,12,21,10,00,00),
                EndDateTime = new DateTime(2022,12,21,11,00,00),
                Title = "ABC"
            };

            var appointment = new Appointment(){
                Id = new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),
                StartDateTime = appointmentDto.StartDateTime,
                EndDateTime = appointmentDto.EndDateTime,
                Title = appointmentDto.Title
            };
            // var testAppointmentList = new List<Appointment>();

            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.CreateAsync(It.IsAny<Appointment>())).ReturnsAsync(true);
        
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Create(appointmentDto) as CreatedAtActionResult ;
            
            // Assert
            // var createdAppointment = (result.Result as CreatedAtActionResult).Value as AppointmentDto;
            // appointmentDto.Should().BeEquivalentTo(result);
            Assert.True(result?.StatusCode.Equals(201));
        }

        [Fact]
        public async Task Create_WithExistingAppointment_ConflictResult()
        {
            // Arrange
            AppointmentDto testAppointmentDto = new() {
                Id = new Guid(),
                StartDateTime = new DateTime(2022,12,12,10,00,00),
                EndDateTime = new DateTime(2022,12,12,12,00,00),
                Title = "ABC"
            };
            var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.CreateAsync(testAppointment)).ReturnsAsync(false);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Create(testAppointmentDto);

            // Assert
            // Assert.Equal(409,result.StatusCode);
        }

        [Fact]
        public async Task Delete_WithEventTime_OkResult(){
            // Arrange
            var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.DeleteAsync(new Guid())).ReturnsAsync(true);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Delete(new Guid()) as OkResult;

            // Assert
            Assert.Equal(200,result.StatusCode);
        }
        [Fact]
        public async Task Delete_WithUnknownEventTime_NotFound(){
            // Arrange
            var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.DeleteAsync(new Guid())).ReturnsAsync(false);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Delete(new Guid()) as NotFoundObjectResult;

            // Assert
            Assert.Equal(404,result.StatusCode);
        }
    }
}