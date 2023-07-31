using Verse;
using PawnController = TheFehr_MyVeryFirstMod.Controllers.Pawn;

namespace TheFehr_MyVeryFirstMod
{
    [StaticConstructorOnStartup]
    public static class MyFirstRimWorldMod
    {
        
        static MyFirstRimWorldMod()
        {
            Log.Message("Hello there!");
            
            var service = new RimWorldManagerServer(8);
            service.Port = 5050;
            service.Controllers.Add(new PawnController());
            service.Start();
            Log.Message("Listening...");
        }
    }
}