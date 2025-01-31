using System.Runtime.Serialization;

namespace PatientsProject.Domain.Enums
{
    public enum Gender
    {
        [EnumMember(Value="male")]
        Male,
        [EnumMember(Value = "female")]
        Female,
        [EnumMember(Value = "other")]
        Other,
        [EnumMember(Value = "unknown")]
        Unknown
    }
}
