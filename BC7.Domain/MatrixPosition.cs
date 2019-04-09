using System;

namespace BC7.Domain
{
    public class MatrixPosition
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
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
