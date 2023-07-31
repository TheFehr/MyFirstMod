using System.Collections.Generic;
using System.Net;
using System.Text;
using TheFehr_MyVeryFirstMod.Controllers;
using TheFehr_MyVeryFirstMod.Views;
using Verse;
using FourOFour = TheFehr_MyVeryFirstMod.Views.Errors.FourOFour;

namespace TheFehr_MyVeryFirstMod
{
    class RimWorldManagerServer : HttpServer
    {
        public List<IController> Controllers { get; } = new List<IController>();

        public RimWorldManagerServer(int threadCount) : base(threadCount)
        {
        }

        protected override void Response(HttpListenerContext context)
        {
            var request = context.Request;
            var controllerContext = new ControllerContext
            {
                WorldLoaded = Current.ProgramState == ProgramState.Playing,
                AbsolutePath = request.Url.AbsolutePath,
                QueryString = request.QueryString,
            };

            var absPath = request.Url.AbsolutePath.ReplaceFirst("/", "");
            Log.Message(absPath);

            var controller = Controllers.Find(potentialController =>
            {
                var indexRoute = potentialController.IndexRoute();
                var showRoute = potentialController.ShowRoute();

                return (
                           indexRoute != null &&
                           absPath.StartsWith(indexRoute)
                       ) ||
                       (
                           showRoute != null &&
                           absPath.StartsWith(showRoute)
                       );
            });

            if (controller == null)
            {
                SendReply(context, FourOFour());
                return;
            }

            SendReply(context, absPath.StartsWith(controller.IndexRoute()) ? controller.Index(controllerContext) : controller.Show(controllerContext));
        }
        
        public static StringBuilder FourOFour()
        {
            var fourOfour = new FourOFour();
            return new StringBuilder(fourOfour.TransformText());
        }

        public static StringBuilder FillTemplate(string content, bool slim = false)
        {
            var baseTemplate = new BaseTemplate
            {
                Content = content
            };

            return slim ? new StringBuilder(content) : new StringBuilder(baseTemplate.TransformText());
        }
    }
}