﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Website.Models.DTOs.Properties
{
    public class PropertyListDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Date Purchased"), DataType(DataType.Date)]
        public DateTimeOffset PurchaseDate { get; set; }

        [DataType(DataType.Currency), Display(Name = "Property Value"), Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double PropertyValue { get; set; }

        [Display(Name = "# Rooms"), Range(1, 100, ErrorMessage = "You cannot have a property with more than 100 rooms.")]
        public int NoOfRooms { get; set; }

        [Display(Name = "# Bathrooms"), Range(1, 100, ErrorMessage = "You cannot have a property with more than 100 Bathrooms.")]
        public int Bathrooms { get; set; }

        [DataType(DataType.Currency), Display(Name = "Monthly Rent"), RegularExpression(@"^(\d{4,})|((\d+(\,|\.))+\d{2,})$", ErrorMessage = "You cannot charge more than 10000 per month."), DisplayFormat(DataFormatString = "{0:#,###0.00}", ApplyFormatInEditMode = true)]
        public double MonthlyRentAmount { get; set; }

        [MaxLength(1000), Description("Describe the property")]
        public string Description { get; set; }

        public Guid AddressId { get; set; }

        public string AddressString
        {
            get { return $"{Address.Line1} {Address.Line2} {Address.Line3} {Address.Postcode} {Address.Town} {Address.City}"; }
        }

        public virtual AddressDTO Address { get; set; }

        [Display(Name = "Date Created"), DataType(DataType.DateTime)]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name = "Date Updated"), DataType(DataType.DateTime)]
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}