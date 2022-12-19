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

namespace DisprzTraining.Tests{
    public class AppointmentBLTest{
        [Fact]
        public async Task CreateAsync_WithAppointment_ReturnTrue(){
            // Arrange
            var testAppointment = new Appointment(1,"12-12-2022",new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t=>t.ValideDate(testAppointment)).ReturnsAsync(true);
            var sut = new AppointmentBL(MockValidation.Object);

            // Act
            var result = await sut.CreateAsync(testAppointment);

            // Assert
            Assert.Equal(true,result);
        }

        [Fact]
        public async Task CreateAsync_WithExistingAppointment_ReturnFalse(){
            // Arrange
            var testAppointment = new Appointment(1,"12-12-2022",new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t=>t.ValideDate(testAppointment)).ReturnsAsync(false);
            var sut = new AppointmentBL(MockValidation.Object);

            // Act
            var result = await sut.CreateAsync(testAppointment);

            // Assert
            Assert.Equal(false,result);
        }

        [Fact]
        public async Task GetAsync_WithDate_ReturnList(){
            // Arrange
            var testAppointment = new Appointment(1,"12-12-2022",new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var testAppointmentList = new List<Appointment>();
            testAppointmentList.Add(testAppointment);
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t=>t.FindAppointments("12-12-2022")).ReturnsAsync(testAppointmentList);

            var sut = new AppointmentBL(MockValidation.Object);

            // Act
            var result = await sut.GetAsync("12-12-2022");
            

            // Assert
            Assert.Equal(testAppointmentList,result);
        }
        [Fact]
        public async Task GetAsync_WitUnExistingDate_ReturnEmptyList(){
            // Arrange
            var testAppointment = new Appointment(1,"12-12-2022",new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var testAppointmentList = new List<Appointment>();
            testAppointmentList.Add(testAppointment);
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t=>t.FindAppointments("13-12-2022")).ReturnsAsync(new List<Appointment>());

            var sut = new AppointmentBL(MockValidation.Object);

            // Act
            var result = await sut.GetAsync("13-12-2022");
            

            // Assert
            Assert.NotEqual(testAppointmentList,result);
        }

        [Fact]
        public async Task DeleteAsync_WithEventDateTime_ReturnTrue(){
            // Arrange
            var testAppointment = new Appointment(1,"12-12-2022",new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t=>t.FindAppointment(testAppointment.Id)).ReturnsAsync(testAppointment);
            var sut = new AppointmentBL(MockValidation.Object);

            // Act
            var result = await sut.DeleteAsync(1);
            
            // Assert
            Assert.Equal(true,result);
        }

        [Fact]
        public async Task DeleteAsync_WithEventDateTime_ReturnFalse(){
            // Arrange
            var testAppointment = new Appointment(1,"12-12-2022",new DateTime(2022,12,12,10,00,00),new DateTime(2022,12,12,12,00,00),"ABC");
            var MockValidation = new Mock<IAppointmentValidation>();
            MockValidation.Setup(t=>t.FindAppointment(3)).ReturnsAsync(new Appointment());
            var sut = new AppointmentBL(MockValidation.Object);

            // Act
            var result = await sut.DeleteAsync(3);
            
            // Assert
            Assert.Equal(false,result);
        }


    }
}