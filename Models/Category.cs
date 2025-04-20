﻿using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Category :BaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

       
        public List<Book>? Books { get; set; }
    }
}
