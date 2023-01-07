<<<<<<< HEAD
using DisprzTraining.DataAccess;
using DisprzTraining.Models;
using DisprzTraining.Dto;
using DisprzTraining.Result;

namespace DisprzTraining.Tests{
    
    public class AppointmentDALTest{

        [Fact]
        public async Task CreateAppointmentAsync_ReturnResultObjectCreated()
        {
            // Arrange
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,10,10,00,00),
                EndDateTime = new DateTime(2023,02,10,12,00,00),
                Title = "ABC"
            };
            Appointment testAppointment= new() {
                Id = Guid.NewGuid(),
                StartDateTime = new DateTime(2023,02,10,10,00,00),
                EndDateTime = new DateTime(2023,02,10,12,00,00),
                Title = "ABC"
            };
            List<Appointment> testList = new();
            ResultModel testResult = new(){appointmentId = testAppointment.Id, ErrorMessage=""};
            
            var sut = new AppointmentDAL();

            // Act
            var result = await sut.CreateAppointmentAsync(testAppointmentDto);

            // Assert
            Assert.Equal(result.ErrorMessage,"");
        }

        [Fact]
        public async Task CreateAppointmentAsync_ReturnResultObjectConflict()
        {
            // Arrange
            AppointmentDto testAppointmentDto = new() {
                StartDateTime = new DateTime(2023,02,10,10,00,00),
                EndDateTime = new DateTime(2023,02,10,12,00,00),
                Title = "ABC"
            };
            Appointment testAppointment= new() {
                Id = Guid.NewGuid(),
                StartDateTime = new DateTime(2023,02,10,10,00,00),
                EndDateTime = new DateTime(2023,02,10,12,00,00),
                Title = "ABC"
            };
            ResultModel testResult = new(){appointmentId = testAppointment.Id, ErrorMessage="Conflict"};

            var sut = new AppointmentDAL();
            sut.TestList(testAppointment);


            // Act
            var result = await sut.CreateAppointmentAsync(testAppointmentDto);

            // Assert
            Assert.NotEqual("",result.ErrorMessage);
=======
using DisprzTraining.Business;
using DisprzTraining.Controllers;
using DisprzTraining.DataAccess;
using DisprzTraining.Models;
using DisprzTraining.validation;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DisprzTraining.Dto;

namespace DisprzTraining.Tests
{

    public class AppointmentDALTest
    {
        [Fact]
        public async Task CreateAppointmentAsync_WithAppointment()
        {
            // Arrange
            var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t => t.ExistingAppointment(It.IsAny<AppointmentDto>())).ReturnsAsync(testAppointment);
            var sut = new AppointmentDAL(MockValidation.Object);

            // Act
            var result = await sut.CreateAppointmentAsync(It.IsAny<AppointmentDto>());

            // Assert
            Assert.Equal(testAppointment, result);
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
        }

        [Fact]
        public async Task GetAppointmentAsync_WithDate_ReturnList()
        {
            // Arrange
<<<<<<< HEAD
            var testDate = "2023-02-10";
            var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2023, 02, 10, 10, 00, 00), new DateTime(2023, 02, 10, 12, 00, 00), "ABC", "Test");
            var testAppointmentList = new List<Appointment>();
            
            var sut = new AppointmentDAL();
            sut.TestList(testAppointment);
=======
            var testDate = "2022-12-12";
            var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
            var testAppointmentList = new List<Appointment>();
            testAppointmentList.Add(testAppointment);
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t => t.FindAppointments(testDate)).ReturnsAsync(new List<Appointment>());

            var sut = new AppointmentDAL(MockValidation.Object);
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912

            // Act
            var result = await sut.GetAppointmentAsync(testDate);

            // Assert
            Assert.IsType<List<Appointment>>(result);
        }

    }
}