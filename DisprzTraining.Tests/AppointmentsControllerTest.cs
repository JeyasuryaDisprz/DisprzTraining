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
using DisprzTraining.Result;

namespace DisprzTraining.Tests{
    
    public class AppointmentsControllerTest{

        [Fact]
        public async Task Get_WithDate_OkObjectResult(){
            // Arrange
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.GetAsync("2022-12-12")).ReturnsAsync(new List<Appointment>());
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act 
            var result = await sut.Get("2022-12-12") as OkObjectResult;

            // Assert
           Assert.True(result?.StatusCode.Equals(200));
        }
        

        [Fact]
        public async Task Create_WithNewAppointment_CreatedResult()
        {
            // Arrange
            // var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC","Test");
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,10,10,00,00),
                EndDateTime = new DateTime(2023,02,10,12,00,00),
                Title = "ABC"
            };

            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.CreateAsync(testAppointmentDto)).ReturnsAsync(new ResultModel(){ResultStatusCode=201});
        
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Create(testAppointmentDto) as CreatedResult ;
            
            // Assert
            Assert.True(result?.StatusCode.Equals(201));
        }

        [Fact]
        public async Task Create_WithExistingAppointment_ConflictResult()
        {
            // Arrange
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,10,10,00,00),
                EndDateTime = new DateTime(2023,02,10,12,00,00),
                Title = "ABC"
            };
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.CreateAsync(testAppointmentDto)).ReturnsAsync(new ResultModel(){ResultStatusCode=409});
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Create(testAppointmentDto) as ConflictObjectResult;

            // Assert
            Assert.True(result?.StatusCode.Equals(409));
        }

        [Fact]
        public async Task Create_BadRequestAsync()
        {
            AppointmentDto testAppointmentDto = new(){
                StartDateTime = new DateTime(2022,12,12,13,00,00),
                EndDateTime = new DateTime(2022,12,12,12,00,00),
                Title = "ABC"
            };
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.CreateAsync(testAppointmentDto)).ReturnsAsync(new ResultModel(){ResultStatusCode=400});
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Create(testAppointmentDto) as BadRequestObjectResult;

            // Assert
            Assert.Equal(400,result?.StatusCode);
        }

        [Fact]
        public async Task Delete_WithEventTime_NoContentResult(){
            // Arrange
            var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC","Test");
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.DeleteAsync(testAppointment.Id)).ReturnsAsync(true);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Delete(testAppointment.Id) as NoContentResult;

            // Assert
            Assert.Equal(204,result?.StatusCode);
        }
        [Fact]
        public async Task Delete_WithEventTime_NotFoundResult(){
            // Arrange
            var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC","Test");
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.DeleteAsync(testAppointment.Id)).ReturnsAsync(false);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Delete(testAppointment.Id) as NotFoundObjectResult;

            // Assert
            Assert.Equal(404,result?.StatusCode);
        }


        // [Fact]
        // public async Task Get_ById_OkObjectResult(){
        //     // Arrange
        //     var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC","Test");
        //     var testAppointmentDto = testAppointment.AsDto();
        //     var testAppointmentList = new List<Appointment>();
        //     testAppointmentList.Add(testAppointment);
        //     var MockAppointment = new Mock<IAppointmentBL>();
        //     MockAppointment.Setup(t=>t.GetIdAsync(testAppointment.Id)).ReturnsAsync(testAppointment);
        //     var sut = new AppointmentsController(MockAppointment.Object);

        //     // Act
        //     var result = await sut.GetById(testAppointment.Id) as OkObjectResult;

        //     testAppointmentDto.Should().BeEquivalentTo(result.Value);
        //     Assert.Equal(200,result.StatusCode);
        // }

        // [Fact]
        // public async Task Get_ById_NotFound(){

        //     var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC","Test");
        //     var testAppointmentDto = testAppointment.AsDto();
        //     var MockAppointment = new Mock<IAppointmentBL>();
        //     MockAppointment.Setup(t=>t.GetIdAsync(testAppointment.Id)).ThrowsAsync(new Exception());
        //     var sut = new AppointmentsController(MockAppointment.Object);

        //     var result = await sut.GetById(testAppointment.Id) as BadRequestObjectResult;

        //     Assert.IsType<BadRequestObjectResult>(result); 
        // }
    }
}