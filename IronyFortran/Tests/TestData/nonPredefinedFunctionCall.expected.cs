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
   IBHEND=4;
   IBH.SetRange(1, IBHEND, 726,1144,1486,1790);
   NEWORDERNR=(string)(ORDERNR);
   BCORRECT=(bool)(true);
   BUSEPARENT=(bool)((IPARENTID > 0));
   if(BUSEPARENT)
   {
      var __BA_SE__1=@"BA,SE";
      var ITGANR_16__2=ITGANR[16];
      var ITGANR_IPARENTID__3=ITGANR[IPARENTID];
      var IPOINT_4=IPOINT;
      BFOUND=(bool)(FINDPOINTER(ref __BA_SE__1,ref ITGANR_16__2,ref ITGANR_IPARENTID__3,ref IPOINT_4));
      //@"BA,SE"=__BA_SE__1;
      ITGANR[16]=ITGANR_16__2;
      ITGANR[IPARENTID]=ITGANR_IPARENTID__3;
      IPOINT=IPOINT_4;
   }
   else
   {
      var SRECTYPE_5=SRECTYPE;
      var ITGANR_16__6=ITGANR[16];
      var _0_7=0;
      var IPOINT_8=IPOINT;
      BFOUND=(bool)(FINDPOINTER(ref SRECTYPE_5,ref ITGANR_16__6,ref _0_7,ref IPOINT_8));
      SRECTYPE=SRECTYPE_5;
      ITGANR[16]=ITGANR_16__6;
      //0=_0_7;
      IPOINT=IPOINT_8;
   }
   if(BFOUND)
   {
      ISTARTPOS=(int)(VDIIWERT(@"700", ITGANR[16], @"710.05", IPOINT, 5));
      ILEN=(int)(VDIIWERT(@"700", ITGANR[16], @"710.05", IPOINT, 6));
      BISTRUE_1=(bool)(((ISTARTPOS > 0) && (ILEN > 0)));
      if(BISTRUE_1)
      {
         BISTRUE_2=(bool)((ISTARTPOS==1));
         if(BISTRUE_2)
         {
            SHEAD=(string)(@"");
         }
         else
         {
            SHEAD=(string)(VDICSTRING(ORDERNR, 0, 1, (ISTARTPOS - 1)));
         }
         STAIL=(string)(VDICSTRING(ORDERNR, 0, (ISTARTPOS + ILEN), 0));
         BOK=(bool)(false);
         if(BUSEPARENT)
         {
            IPOS=6;
         }
         else
         {
            IPOS=5;
         }
         IIDX=1;
         BCONTINUE=(bool)(true);
         while(BCONTINUE)
         {
            IPOS=(int)(IPOS + 2);
            IIDX=(int)(VDIIWERT(@"700", ITGANR[16], @"710.05", IPOINT, IPOS));
            BISTRUE_2=(bool)(((IIDX == ITGANR[IVARID]) || (ITGANR[IVARID] < 0)));
            if(BISTRUE_2)
            {
               SBODY=(string)(VDICWERT(out ILENBODY, @"700", ITGANR[16], @"710.05", IPOINT, (IPOS + 1)));
               ITGANR[IVARID]=IIDX;
               BOK=(bool)((ILENBODY > 0));
            }
            BCONTINUE=(bool)(((! BOK) && (IIDX > 0)));
         }
         if(BOK)
         {
            NEWORDERNR=(string)(VDISTRING(@"&|&|&", SHEAD, SBODY, STAIL));
         }
         else
         {
            BCORRECT=(bool)(false);
         }
      }
   }
   MAKEORDERNUMBER=NEWORDERNR;
   return MAKEORDERNUMBER;
}
