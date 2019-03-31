using System;
using System.Collections.Generic;

namespace BC7.Business.Models
{
    public class UserMultiAccountModel
    {
        public Guid Id { get; set; }
        public string MultiAccountName { get; set; }
        public string Reflink { get; set; }
        public ICollection<MatrixPositionModel> MatrixPositionModels { get; set; }

        // TODO: UserMultiAccountInviting?
    }
}
