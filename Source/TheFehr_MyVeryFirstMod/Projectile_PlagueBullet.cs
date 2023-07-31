using RimWorld;
using Verse;

namespace TheFehr_MyVeryFirstMod
{
    public class Projectile_PlagueBullet : Bullet
    {
        public ModExtension_PlagueBullet Props => def.GetModExtension<ModExtension_PlagueBullet>();

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing, blockedByShield);

            if (Props == null || !(hitThing is Pawn hitPawn)) return;
            var rand = Rand.Value;
            if (rand > Props.addHediffChance) return;
            
            Messages.Message("TheFehr_MyVeryFirstMod_SuccessMessage".Translate(
                this.launcher.Label, hitPawn.Label
            ), MessageTypeDefOf.NeutralEvent);
            var plagueOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Props.hediffToAdd);
            var randomSeverity = Rand.Range(0.15f, 0.30f);
            if (plagueOnPawn != null)
            {
                plagueOnPawn.Severity += randomSeverity;
            }
            else
            {
                var hediff = HediffMaker.MakeHediff(Props.hediffToAdd, hitPawn);
                hediff.Severity = randomSeverity;
                hitPawn.health?.AddHediff(hediff);
            }
        }
    }
}