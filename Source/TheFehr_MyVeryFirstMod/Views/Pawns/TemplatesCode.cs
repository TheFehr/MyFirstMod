using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TheFehr_MyVeryFirstMod.Views.Pawns
{
    partial class Index
    {
        public IEnumerable<IGrouping<PawnKindDef, Pawn>> PawnGroups { get; set; }
    }
    
    partial class Show
    {
        public Pawn Pawn { get; set; }
    }
}