using System.ComponentModel;

namespace Vale.Geographic.Domain.Enumerable
{
    public enum TypeEntitieEnum
    {
        [Description("Point Of Interest")] PointOfInterest = 1,

        [Description("Area")] Area = 2,

        [Description("Category")] Category = 3,

        [Description("Route")] Route = 4,

        [Description("Segment")] Segment = 5,

        [Description("FocalPoint")] FocalPoint = 6,

        [Description("User")] User = 7

    }
}
