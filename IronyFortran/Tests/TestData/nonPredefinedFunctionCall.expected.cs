public bool FINDPOINTER(ref string SRECTYPE, ref int IIDX700, ref int IIDXPARENT, ref int IPOINTER)
{
   bool FINDPOINTER = false;
   int IIDX = 0;
   int ILENSTR = 0;
   FINDPOINTER = (bool)(BOK);
   return FINDPOINTER;
}

public string MAKEORDERNUMBER(ref string ORDERNR, ref string SRECTYPE, ref VDIArray<int> ITGANR, ref int IVARID, ref int IPARENTID, ref bool BCORRECT)
{
   string MAKEORDERNUMBER = String.Empty;
   string NEWORDERNR = String.Empty;
   VDIArray<int> IBH = new VDIArray<int>();
   int IBHEND = 0;
   int IIDX = 0;
   int IPOS = 0;
   bool BOK = false;
   bool BFOUND = false;
   bool BUSEPARENT = false;
   bool BCONTINUE = false;
   bool BISTRUE_1 = false;
   bool BISTRUE_2 = false;
   int ISTARTPOS = 0;
   int ILEN = 0;
   int IPOINT = 0;
   int ILENBODY = 0;
   string SHEAD = String.Empty;
   string SBODY = String.Empty;
   string STAIL = String.Empty;
   IBHEND = (int)(4);
   IBH.SetRange(1, IBHEND, 726, 1144, 1486, 1790);
   NEWORDERNR = (string)(ORDERNR);
   BCORRECT = (bool)(true);
   BUSEPARENT = (bool)((IPARENTID > 0));
   if(BUSEPARENT)
   {
      BFOUND = (bool)(FINDPOINTER_vrrr(@"BA,SE", ref ITGANR[16], ref ITGANR[IPARENTID], ref IPOINT));
   }
   else
   {
      BFOUND = (bool)(FINDPOINTER_rrvr(ref SRECTYPE, ref ITGANR[16], 0, ref IPOINT));
   };
   if(BFOUND)
   {
      ISTARTPOS = (int)(VDIIWERT(@"700", ITGANR[16], @"710.05", IPOINT, 5));
      ILEN = (int)(VDIIWERT(@"700", ITGANR[16], @"710.05", IPOINT, 6));
      BISTRUE_1 = (bool)(((ISTARTPOS > 0) && (ILEN > 0)));
      if(BISTRUE_1)
      {
         BISTRUE_2 = (bool)((ISTARTPOS == 1));
         if(BISTRUE_2)
         {
            SHEAD = (string)(@"");
         }
         else
         {
            SHEAD = (string)(VDICSTRING(ORDERNR, 0, 1, (ISTARTPOS - 1)));
         };
         STAIL = (string)(VDICSTRING(ORDERNR, 0, (ISTARTPOS + ILEN), 0));
         BOK = (bool)(false);
         if(BUSEPARENT)
         {
            IPOS = (int)(6);
         }
         else
         {
            IPOS = (int)(5);
         };
         IIDX = (int)(1);
         BCONTINUE = (bool)(true);
         while(BCONTINUE)
         {
            IPOS = (int)(IPOS + 2);
            IIDX = (int)(VDIIWERT(@"700", ITGANR[16], @"710.05", IPOINT, IPOS));
            BISTRUE_2 = (bool)(((IIDX == ITGANR[IVARID]) || (ITGANR[IVARID] < 0)));
            if(BISTRUE_2)
            {
               SBODY = (string)(VDICWERT(out ILENBODY, @"700", ITGANR[16], @"710.05", IPOINT, (IPOS + 1)));
               ITGANR[IVARID] = IIDX;
               BOK = (bool)((ILENBODY > 0));
            };
            BCONTINUE = (bool)(((!BOK) && (IIDX > 0)));
         };
         if(BOK)
         {
            NEWORDERNR = (string)(VDISTRING(@"&|&|&", SHEAD, SBODY, STAIL));
         }
         else
         {
            BCORRECT = (bool)(false);
         };
      };
   };
   MAKEORDERNUMBER = (string)(NEWORDERNR);
   return MAKEORDERNUMBER;
}

#region function wrappers for FINDPOINTER

public bool FINDPOINTER_vrrr(string SRECTYPE, ref int IIDX700, ref int IIDXPARENT, ref int IPOINTER)
{
   string sRecType_wrappedLocal = sRecType;
   return FINDPOINTER(ref sRecType_wrappedLocal, ref iIdx700, ref iIdxParent, ref iPointer);
}

public bool FINDPOINTER_rrvr(ref string SRECTYPE, ref int IIDX700, int IIDXPARENT, ref int IPOINTER)
{
   int iIdxParent_wrappedLocal = iIdxParent;
   return FINDPOINTER(ref sRecType, ref iIdx700, ref iIdxParent_wrappedLocal, ref iPointer);
}

#endregion
