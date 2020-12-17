using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspIdentitydemo.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public decimal Price { get; set; }


       
        public string ImagePath { get; set; }
        
        [NotMapped]
        public List<HttpPostedFileBase> ImageFiles { get; set; }

        [Required]
        public int Category_Id { get; set; }

        [Required]
        public int Brand_Id { get; set; }

    }
}