﻿using HavenSoft.HexManiac.Core.Models.Runs.Sprites;
using System.Collections.Generic;

namespace HavenSoft.HexManiac.Core.Models.Runs.Factory {
   /// <summary>
   /// Format Specifier:     `ucsBxWxH` where B=bits, W=width, H=height. Ex: `ucs4x8x8`
   /// Represents an uncompressed stream of bytes representing a tiled image with a given width/height.
   /// Uncompressed sprites do not currently support paging. The byte length is determined soley by the width/height and the bitness.
   /// </summary>
   public class SpriteRunContentStrategy : RunStrategy {
      private readonly SpriteFormat spriteFormat;
      public SpriteRunContentStrategy(SpriteFormat spriteFormat) => this.spriteFormat = spriteFormat;

      public override int LengthForNewRun(IDataModel model, int pointerAddress) => spriteFormat.ExpectedByteLength;
      public override bool TryAddFormatAtDestination(IDataModel owner, ModelDelta token, int source, int destination, string name, IReadOnlyList<ArrayRunElementSegment> sourceSegments) {
         var spriteRun = new SpriteRun(destination, spriteFormat, new[] { source });
         // TODO deal with the run being too long?
         if (!(token is NoDataChangeDeltaModel)) owner.ObserveRunWritten(token, spriteRun);
         return true;
      }
      public override bool Matches(IFormattedRun run) => run is Models.Runs.Sprites.SpriteRun spriteRun && spriteRun.FormatString == Format;
      public override IFormattedRun WriteNewRun(IDataModel owner, ModelDelta token, int source, int destination, string name, IReadOnlyList<ArrayRunElementSegment> sourceSegments) {
         for (int i = 0; i < spriteFormat.ExpectedByteLength; i++) token.ChangeData(owner, destination + i, 0);
         return new SpriteRun(destination, spriteFormat);
      }
      public override void UpdateNewRunFromPointerFormat(IDataModel model, ModelDelta token, string name, ref IFormattedRun run) {
         var runAttempt = new SpriteRun(run.Start, spriteFormat, run.PointerSources);
         if (runAttempt.Length > 0) {
            run = runAttempt.MergeAnchor(run.PointerSources);
            model.ClearFormat(token, run.Start, run.Length);
         }
      }
      public override ErrorInfo TryParseData(IDataModel model, string name, int dataIndex, ref IFormattedRun run) {
         run = new SpriteRun(dataIndex, spriteFormat, run.PointerSources);
         return ErrorInfo.NoError;
      }
   }
}
