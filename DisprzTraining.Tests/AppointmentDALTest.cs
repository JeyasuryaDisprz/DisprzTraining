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
using DisprzTraining.Result;

namespace DisprzTraining.Tests{
    
    public class AppointmentDALTest{
        [Fact]
        public async Task CreateAppointmentAsync_ReturnResultObjectCreated()
        {
            // Arrange
            var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t => t.ExistingAppointment(It.IsAny<AppointmentDto>())).ReturnsAsync(new Appointment());
            var sut = new AppointmentDAL(MockValidation.Object);

            // Act
            var result = await sut.CreateAppointmentAsync(It.IsAny<AppointmentDto>());

            // Assert
            Assert.IsType<ResultModel>(result);
        }
        [Fact]
        public async Task CreateAppointmentAsync_ReturnResultObjectConflict()
        {
            // Arrange
            var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t => t.ExistingAppointment(It.IsAny<AppointmentDto>())).ReturnsAsync(new Appointment());
            var sut = new AppointmentDAL(MockValidation.Object);

            // Act
            var result = await sut.CreateAppointmentAsync(It.IsAny<AppointmentDto>());

            // Assert
            Assert.IsType<ResultModel>(result);
        }

        [Fact]
        public async Task GetAppointmentAsync_WithDate_ReturnList()
        {
            // Arrange
            var testDate = "2022-12-12";
            var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
            var testAppointmentList = new List<Appointment>();
            testAppointmentList.Add(testAppointment);
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t => t.FindAppointments(testDate)).ReturnsAsync(testAppointmentList);

            var sut = new AppointmentDAL(MockValidation.Object);

            // Act
            var result = await sut.GetAppointmentAsync(testDate);

            // Assert
            Assert.IsType<List<Appointment>>(result);
        }

    }
}