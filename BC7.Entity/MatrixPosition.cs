using System;
using BC7.Entity.AbstractBase;

namespace BC7.Entity
{
    public class MatrixPosition : BaseEntity
    {
        public Guid? UserMultiAccountId { get; set; }
        public virtual UserMultiAccount UserMultiAccount { get; set; }
        public Guid? ParentId { get; set; }
        public virtual MatrixPosition Parent { get; set; } // TODO: Do we need this here? I guess no, because we will use MPTT (Nested Set Tree) to get items

        public int MatrixLevel { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int DepthLevel { get; set; }
    }
}
