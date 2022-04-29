using Shopping.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class ShowCartViewModel
    {
        public string OrderNo { get; set; }
        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comments")]
        public string Remarks { get; set; }

        public ICollection<TemporalSale> TemporalSales { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Qty")]
        public float Quantity => TemporalSales == null ? 0 : TemporalSales.Sum(ts => ts.Quantity);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Total")]
        public decimal Value => TemporalSales == null ? 0 : TemporalSales.Sum(ts => ts.Value);
    }
}
