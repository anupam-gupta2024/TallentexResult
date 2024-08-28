using Microsoft.AspNetCore.Mvc.Rendering;

namespace TallentexResult.Interface
{
    public interface IDropdown
    {
        List<SelectListItem> getMode();

        List<SelectListItem> getCenter();
    }
}
