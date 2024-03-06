using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MidExam.ViewModels
{
    [NotMapped]
    public class ProductVM()
    {
        public ProductVM(string name) : this()
        {
            Name = name;
        }

        public ProductVM(string name, string imageUrl) : this(name)
        {
            ImageUrl = imageUrl;
        }

        public ProductVM(int id, string name, string imageUrl) : this(name, imageUrl)
        {
            Id = id;
        }

        public int Id { get; set; } = 0; //initialize 

        [Required(ErrorMessage ="Please Enter your Name")]
        public string Name { get; set; } = string.Empty;

        [AllowNull, DataType(DataType.ImageUrl), Display(Name = "Image")]
        public string ImageUrl { get; set; }

    }
}
