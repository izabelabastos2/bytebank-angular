using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using Vale.Geographic.Domain.Base;

namespace Vale.Geographic.Domain.Entities
{
    public class FocalPoint : Entity, ICloneable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Matricula { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public virtual Guid LocalityId { get; set; }

        [Write(false)]
        public virtual Area Locality { get; set; }

        [Required]
        public virtual Guid PointOfInterestId { get; set; }

        [Write(false)]
        public virtual PointOfInterest PointOfInterest { get; set; }

        public FocalPoint(){}

        public bool Equals(FocalPoint focalPoint)
        {
            if ((object)focalPoint == null)
                return false;

            return (Id == focalPoint.Id) && (Name == focalPoint.Name) 
                     && (CreatedAt == focalPoint.CreatedAt) && (Status == focalPoint.Status)
                     && (Matricula == focalPoint.Matricula) && (LocalityId == focalPoint.LocalityId)
                     && (PointOfInterestId == focalPoint.PointOfInterestId) && (PhoneNumber == focalPoint.PhoneNumber);
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
