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
    public class AppointmentBLTest
    {
        [Fact]
        public async Task CreateAsync_WithAppointmentDto_ReturnAppointment()
        {
            var MockAppointment = new Mock<IAppointmentDAL>();
            MockAppointment.Setup(t=>t.CreateAppointmentAsync(It.IsAny<AppointmentDto>())).ReturnsAsync(new Appointment());
            var sut = new AppointmentBL(MockAppointment.Object);

            var result = await sut.CreateAsync(It.IsAny<AppointmentDto>());

            Assert.IsType<Appointment>(result);
        }
        [Fact]
        public async Task CreateAsync_WithAppointmentDto_ReturnException()
        {
            var MockAppointment = new Mock<IAppointmentDAL>();
            MockAppointment.Setup(t=>t.CreateAppointmentAsync(It.IsAny<AppointmentDto>())).ThrowsAsync(new Exception());
            var sut = new AppointmentBL(MockAppointment.Object);

            await Assert.ThrowsAsync<Exception>(()=> sut.CreateAsync(It.IsAny<AppointmentDto>()));
        }

        [Fact]
        public async Task GetAsync_WithDate_ReturnAppointments()
        {
            var MockAppointment = new Mock<IAppointmentDAL>();
            MockAppointment.Setup(t=>t.GetAppointmentAsync("2022-12-23")).ReturnsAsync(new List<Appointment>());
            var sut = new AppointmentBL(MockAppointment.Object);

            var result = await sut.GetAsync("2022-12-23");

            Assert.IsType<List<Appointment>>(result);
        }

        [Fact]
        public async Task DeleteAsync_ById_ReturnTrue()
        {
            var testAppointmentId = Guid.NewGuid();
            var MockAppointment = new Mock<IAppointmentDAL>();
            MockAppointment.Setup(t=>t.DeleteAppointmentAsync(testAppointmentId)).ReturnsAsync(true);
            var sut = new AppointmentBL(MockAppointment.Object);

            var result = await sut.DeleteAsync(testAppointmentId);

            Assert.Equal(true,result);
        }
        
        [Fact]
        public async Task DeleteAsync_ById_ReturnFalse()
        {
            var MockAppointment = new Mock<IAppointmentDAL>();
            MockAppointment.Setup(t=>t.DeleteAppointmentAsync(Guid.NewGuid())).ReturnsAsync(false);
            var sut = new AppointmentBL(MockAppointment.Object);

            var result = await sut.DeleteAsync(Guid.NewGuid());

            Assert.Equal(false,result);
        }






//         [Fact]
//         public async Task CreateAsync_WithAppointment_ReturnTrue()
//         {
//             // Arrange
//             var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
//             var testAppointmentList = new List<Appointment>();
//             var MockValidation = new Mock<IAppointmentValidation>();
//             MockValidation.Setup(t => t.ValideDate(testAppointment)).ReturnsAsync(true);
//             var sut = new AppointmentBL(MockValidation.Object);

//             // Act
//             var result = await sut.CreateAsync(testAppointment);

//             // Assert
//             Assert.True(result);
//         }

//         [Fact]
//         public async Task CreateAsync_WithExistingAppointment_ReturnFalse()
//         {
//             // Arrange
//             var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
//             var testAppointmentList = new List<Appointment>();
//             testAppointmentList.Add(testAppointment);
//             var MockValidation = new Mock<IAppointmentValidation>();
//             MockValidation.Setup(t => t.ValideDate(testAppointment)).ReturnsAsync(false);
//             var sut = new AppointmentBL(MockValidation.Object);

//             // Act
//             var result = await sut.CreateAsync(testAppointment);

//             // Assert
//             Assert.False(result);
//         }

//         [Fact]
//         public async Task CreateAsync_WithInvalidDate_ReturnException(){

//             var MockValidation = new Mock<IAppointmentValidation>();
//             MockValidation.Setup(t => t.ValideDate(It.IsAny<Appointment>())).ThrowsAsync(new Exception());
//             var sut = new AppointmentBL(MockValidation.Object);

//             // Assert
//             Assert.ThrowsAsync<Exception>(()=> sut.CreateAsync(It.IsAny<Appointment>()));
//         }

//         [Fact]
//         public async Task GetAsync_WithDate_ReturnList()
//         {
//             // Arrange
//             var testDate = "2022-12-12";
//             var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
//             var testAppointmentList = new List<Appointment>();
//             testAppointmentList.Add(testAppointment);
//             var MockValidation = new Mock<IAppointmentValidation>();
//             MockValidation.Setup(t => t.FindAppointments(testDate)).ReturnsAsync(testAppointmentList);

//             var sut = new AppointmentBL(MockValidation.Object);

//             // Act
//             var result = await sut.GetAsync(testDate);


//             // Assert
//             Assert.Equal(testAppointmentList, result);
//         }
//         [Fact]
//         public async Task GetAsync_WitUnExistingDate_ReturnEmptyList()
//         {
//             // Arrange
//             var testDate = "2022-12-13";
//             var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
//             var testAppointmentList = new List<Appointment>();
//             testAppointmentList.Add(testAppointment);
//             var MockValidation = new Mock<IAppointmentValidation>();
//             MockValidation.Setup(t => t.FindAppointments(testDate)).ReturnsAsync(new List<Appointment>());

//             var sut = new AppointmentBL(MockValidation.Object);

//             // Act
//             var result = await sut.GetAsync(testDate);


//             // Assert
//             Assert.NotEqual(testAppointmentList, result);
//         }

//         [Fact]
//         public async Task GetTaskAsync_WithId_ReturnAppointment()
//         {
//             // Arrange
//             var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
//             var testAppointmentList = new List<Appointment>();
//             testAppointmentList.Add(testAppointment);
//             var MockValidation = new Mock<IAppointmentValidation>();
//             MockValidation.Setup(t => t.FindAppointment(testAppointment.Id)).ReturnsAsync(testAppointment);

//             var sut = new AppointmentBL(MockValidation.Object);

//             // Act
//             var result = await sut.GetIdAsync(testAppointment.Id);

//             // Assert
//             Assert.Equal(testAppointment, result);
//         }

//         [Fact]
//         public async Task DeleteAsync_WithEventDateTime_ReturnTrue()
//         {
//             // Arrange
//             var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC", "Test");
//             var MockValidation = new Mock<IAppointmentValidation>();
//             MockValidation.Setup(t => t.FindAppointment(testAppointment.Id)).ReturnsAsync(testAppointment);
//             var sut = new AppointmentBL(MockValidation.Object);

//             // Act
//             var result = await sut.DeleteAsync(testAppointment.Id);

//             // Assert
//             Assert.True(result);
//         }

//         [Fact]
//         public async Task DeleteAsync_WithEventDateTime_ReturnFalse()
//         {
//             // Arrange
//             var testID = new Guid();
//             var testAppointment = new Appointment(new Guid("9245fe4a-d402-451c-b9ed-9c1a04247482"), new DateTime(2022, 12, 12, 10, 00, 00), new DateTime(2022, 12, 12, 12, 00, 00), "ABC","Test");
//             var MockValidation = new Mock<IAppointmentValidation>();
//             MockValidation.Setup(t => t.FindAppointment(testID)).ReturnsAsync(new Appointment());
//             var sut = new AppointmentBL(MockValidation.Object);

//             // Act
//             var result = await sut.DeleteAsync(testID);

//             // Assert
//             Assert.False(result);
//         }
    }
}