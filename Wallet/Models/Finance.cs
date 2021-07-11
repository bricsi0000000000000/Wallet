using System;

namespace Wallet.Models
{
    public class Finance : Template
    {
        public DateTime Date { get; set; }
        public bool IsAutomatized { get; set; }

        public Finance()
        {

        }

        public Finance(Template template)
        {
            Id = template.Id;
            Money = template.Money;
            Description = template.Description;
            CategoryId = template.CategoryId;
            Type = template.Type;
            IsAutomatized = false;
            Date = DateTime.Today;
        }
    }
}
