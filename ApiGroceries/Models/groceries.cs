using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNet.Identity;

namespace GroceriesList.Models
{
    public class Groceries
    {
        public Groceries()
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;
            Updated = DateTime.Now;
            UserId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }        
        public Guid UserId { get; set; }
        public int Index { get; set; }
        public int Color { get; set; }
        public string Text { get; set; }
        public bool Check { get; set; }
    }
}
