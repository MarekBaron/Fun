public string JOINTGANR(ref VDIArray<int> ITGANR)
{
   string JOINTGANR = String.Empty;
   string STGANRNEW = String.Empty;
   string STGAPART = String.Empty;
   int ICOUNTER = 0;
   bool BISTRUE = false;
   bool BCONTINUE = false;
   ICOUNTER = (int)(1);
   BCONTINUE = (bool)(true);
   while(BCONTINUE)
   {
      BISTRUE = (bool)((ITGANR[ICOUNTER] >= 0));
      if(BISTRUE)
      {
         STGAPART = (string)(VDISTRING(@"~##", ITGANR[ICOUNTER]));
      }
      else
      {
         STGAPART = (string)(@"???");
      };
      STGANRNEW = (string)(VDISTRING(@"&|&", STGANRNEW, STGAPART));
      ICOUNTER = (int)(ICOUNTER + 1);
      BCONTINUE = (bool)((ICOUNTER < 16));
   };
   BISTRUE = (bool)((ITGANR[16] >= 0));
   if(BISTRUE)
   {
      STGAPART = (string)(VDISTRING(@"~####", ITGANR[16]));
   }
   else
   {
      STGAPART = (string)(@"?????");
   };
   STGANRNEW = (string)(VDISTRING(@"&|&", STGANRNEW, STGAPART));
   BISTRUE = (bool)((ITGANR[17] >= 0));
   if(BISTRUE)
   {
      STGAPART = (string)(VDISTRING(@"~##", ITGANR[17]));
   }
   else
   {
      STGAPART = (string)(@"???");
   };
   STGANRNEW = (string)(VDISTRING(@"&|&", STGANRNEW, STGAPART));
   BISTRUE = (bool)((ITGANR[18] >= 0));
   if(BISTRUE)
   {
      STGAPART = (string)(VDISTRING(@"~####", ITGANR[18]));
   }
   else
   {
      STGAPART = (string)(@"?????");
   };
   STGANRNEW = (string)(VDISTRING(@"&|&", STGANRNEW, STGAPART));
   JOINTGANR = (string)(STGANRNEW);
   return JOINTGANR;
}

public bool FINDPOINTER(ref string SRECTYPE, ref int IIDX700, ref int IIDXPARENT, ref int IPOINTER)
{
   bool FINDPOINTER = false;
   int IIDX = 0;
   int ILENSTR = 0;
   bool BOK = false;
   bool BEXIT = false;
   bool BFOUND = false;
   bool BFIRST = false;
   bool BFINDPARENT = false;
   bool BCONTINUE = false;
   bool BISTRUE = false;
   IPOINTER = (int)(0);
   BOK = (bool)(false);
   IIDX = (int)(0);
   BFOUND = (bool)(false);
   BEXIT = (bool)(false);
   BFIRST = (bool)(true);
   BFINDPARENT = (bool)((IIDXPARENT > 0));
   BCONTINUE = (bool)(true);
   while(BCONTINUE)
   {
      if(BFIRST)
      {
         IIDX = (int)(VDIERSTER(@"700", IIDX700, @"710.05"));
         BFIRST = (bool)(false);
      }
      else
      {
         IIDX = (int)(VDIFOLGE(@"700", IIDX700, @"710.05", IIDX));
      };
      BISTRUE = (bool)((IIDX > 0));
      if(BISTRUE)
      {
         BFOUND = (bool)((VDICWERT(out ILENSTR, @"700", IIDX700, @"710.05", IIDX, 4) == SRECTYPE));
         if(BFOUND)
         {
            if(BFINDPARENT)
            {
               BFOUND = (bool)((VDIIWERT(@"700", IIDX700, @"710.05", IIDX, 7) == IIDXPARENT));
            };
         };
         if(BFOUND)
         {
            IPOINTER = (int)(IIDX);
            BOK = (bool)(true);
         };
         BEXIT = (bool)((BFOUND || (ILENSTR == 0)));
      }
      else
      {
         BEXIT = (bool)(true);
      };
      BCONTINUE = (bool)((!BEXIT));
   };
   FINDPOINTER = (bool)(BOK);
   return FINDPOINTER;
}

