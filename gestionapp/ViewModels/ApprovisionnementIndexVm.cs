using gestionapp.Models;

namespace gestionapp.ViewModels
{
    public class ApprovisionnementIndexVm
    {
        public PagedResult<ApprovisionnementListItemVm> PageResult { get; set; } =
            new PagedResult<ApprovisionnementListItemVm>();
    }
}
