using System.ComponentModel.DataAnnotations.Schema;

namespace MidExam.ViewModels
{
    [NotMapped]
    public class MeasurementUnitVM()
    {
       

        public MeasurementUnitVM(string name) : this()
        {
            Name = name;
        }

        public MeasurementUnitVM(int id, string name) : this(name)
        {
            Id = id;
        }

        public int Id { get; set; } = 0; //Initialize

        public string Name { get; set; }
    }
}

