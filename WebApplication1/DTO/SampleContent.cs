using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data.DTOs
{
    public class SampleContent
    {
        public string Id { get; set; }
        public Address Address { get; set; }
        public Physical PhysicalData { get; set; }
        public Financial FinancialData { get; set; }
        public double GrossYield
        {
            get
            {
                double listPriceValue = 0;
                double monthlyRentValue = 0;
                double grossYeild = 0;
                if (FinancialData!=null && double.TryParse(FinancialData.ListPrice, out listPriceValue) && double.TryParse(FinancialData.ListPrice, out listPriceValue))
                {
                    grossYeild = (listPriceValue > 0 && monthlyRentValue > 0) ? (monthlyRentValue * 12) / listPriceValue : 0;
                }

                return grossYeild;
            }
        }
    }

    public class Address
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string county { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string zipPlus4 { get; set; }
    }

    public class Physical
    {
        public string YearBuilt { get; set; }
    }
    public class Financial
    {
        public string ListPrice { get; set; }
        public string MonthlyRent { get; set; }
    }

}