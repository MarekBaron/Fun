public bool SPLITTGANR(ref string TGANRSOURCE, ref VDIArray<int> ITGANR, ref VDIArray<string> STGANR)
{
   bool SPLITTGANR = false;
   STGANR[16] = VDICSTRING(TGANRSOURCE, 0, 46, 50);
   if(VDICSTRING(STGANR[16], 0, 1, 1) == @"?")
   {
      ITGANR[16] = VDIERSTER(@"700");
   }
   else
   {
      ITGANR[16] = VDIISTRING(STGANR[16], 0, 0, 0);
   };
   return SPLITTGANR;
}

