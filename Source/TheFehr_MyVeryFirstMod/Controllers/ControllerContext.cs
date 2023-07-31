using System.Collections.Specialized;

namespace TheFehr_MyVeryFirstMod.Controllers
{
    public class ControllerContext
    {
        public bool WorldLoaded { get; set; }
        public string AbsolutePath { get; set; }
        public NameValueCollection QueryString { get; set; }
    }
}