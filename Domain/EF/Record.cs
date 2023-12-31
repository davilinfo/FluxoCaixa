﻿using Domain.EF.Abstract;
using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.EF
{
  [ExcludeFromCodeCoverage]
  [Table("Record")]
  public class Record : GuidEntity
  {    
    [Required(AllowEmptyStrings = false, ErrorMessage = "History é obrigatório")]
    [MaxLength(500,ErrorMessage = "O tamanho máximo do campo são 500 caracteres")]
    public string History { get; private set; }
    [Required(ErrorMessage = "DateTime é obrigatório")]
    public DateTime DateTime { get; private set; }
    [Required(ErrorMessage = "Type é obrigatório")]
    [EnumDataType(typeof(RecordType))]
    public char Type { get; private set; }
    [Required(ErrorMessage = "Value é obrigatório")]
    public double Value { get; private set; }
    [Required(ErrorMessage = "IdAccount é obrigatório")]
    [ForeignKey("IdAccount")]
    public virtual Account IdAccountNavigation { get; set; } 

    public Record(string History, DateTime DateTime, char Type, double Value)
    {
      this.Id = Guid.NewGuid();
      this.History = History;
      this.DateTime = DateTime;
      this.Type = Type;
      this.Value = Value;      
    }
  }
}
