using System.Text;
using JetBrains.Annotations;

namespace TheFehr_MyVeryFirstMod.Controllers
{
    public interface IController
    {
        [CanBeNull] string IndexRoute();
        StringBuilder Index(ControllerContext context);


        [CanBeNull] string ShowRoute();
        StringBuilder Show(ControllerContext context);
    }
}