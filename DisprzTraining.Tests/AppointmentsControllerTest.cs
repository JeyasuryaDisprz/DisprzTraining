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

namespace DisprzTraining.Tests{
    
    public class AppointmentsControllerTest{
        // private readonly AppointmentBL appointmentBL = new();
        // private readonly Appointment appointment = new();

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
            var testAppointmentList = new List<Appointment>();
            testAppointmentList.Add(testAppointment);
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.GetIdAsync(testAppointment.Id)).ReturnsAsync(testAppointment);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Get(testAppointment.Id);

            // Assert
            Assert.Equal(testAppointment,result.Value);
            Assert.Equal(200,result.StatusCode);
        }

        [Fact]
        public async Task Create_WithNewAppointment_OkResult(){
            // Arrange
            var MockAppointment = new Mock<IAppointmentBL>();
            var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            MockAppointment.Setup(t=>t.CreateAsync(testAppointment)).ReturnsAsync(true);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Create(testAppointment);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }
        [Fact]
        public async Task Create_WithExistingAppointment_ConflictResult(){
            // Arrange
            var MockAppointment = new Mock<IAppointmentBL>();
            var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            MockAppointment.Setup(t=>t.CreateAsync(testAppointment)).ReturnsAsync(false);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Create(testAppointment);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }


        [Fact]
        public async Task Delete_WithEventTime_OkResult(){
            // Arrange
            var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.DeleteAsync(new Guid())).ReturnsAsync(true);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Delete(new Guid());

            // Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task Delete_WithUnknownEventTime_NotFound(){
            // Arrange
            var testAppointment = new Appointment(new Guid(),new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var MockAppointment = new Mock<IAppointmentBL>();
            MockAppointment.Setup(t=>t.DeleteAsync(testAppointment.Id)).ReturnsAsync(false);
            var sut = new AppointmentsController(MockAppointment.Object);

            // Act
            var result = await sut.Delete(new Guid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}