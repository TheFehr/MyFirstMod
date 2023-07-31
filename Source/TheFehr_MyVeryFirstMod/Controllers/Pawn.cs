using System.Linq;
using System.Text;
using RimWorld;
using TheFehr_MyVeryFirstMod.Views.Pawns;
using Verse;

namespace TheFehr_MyVeryFirstMod.Controllers
{
    public class Pawn : IController
    {
        public string IndexRoute()
        {
            return "pawns";
        }

        public string ShowRoute()
        {
            return "pawn";
        }

        public StringBuilder Index(ControllerContext context)
        {
            if (!context.WorldLoaded)
            {
                return new StringBuilder("<h1>No world loaded!</h1>");
            }

            var updateValues = context.QueryString.GetValues("update");
            var slim = updateValues?.Length > 0;

            var playerAlivePawns =
                PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction.GroupBy(pawn =>
                    pawn.kindDef);
            var pawnsPage = new Index
            {
                PawnGroups = playerAlivePawns,
            };

            return RimWorldManagerServer.FillTemplate(pawnsPage.TransformText(), slim);
        }

        public StringBuilder Show(ControllerContext context)
        {
            var id = context.AbsolutePath.Split('/').Last();
            var pawn =
                PawnsFinder.All_AliveOrDead.Find(p => p.ThingID == id);

            if (pawn == null)
            {
                return RimWorldManagerServer.FourOFour();
            }
            
            var updateValues = context.QueryString.GetValues("update");
            var slim = updateValues?.Length > 0;

            var pawnPage = new Show
            {
                Pawn = pawn,
            };
            return RimWorldManagerServer.FillTemplate(pawnPage.TransformText(), slim);
        }
    }
}