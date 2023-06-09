﻿namespace WebDotNetMentoringProgram.Abstractions
{
    public interface IProductTableViewModel
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? CompanyName { get; set; }
        public string? CategoryName { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}
